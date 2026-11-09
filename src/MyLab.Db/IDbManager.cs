using System.Diagnostics;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.Extensions.Logging;
using MyLab.Log.Dsl;

namespace MyLab.Db
{
    /// <summary>
    /// Provides DB tools
    /// </summary>
    public interface IDbManager
    {
        /// <summary>
        /// Gets <see cref="DataConnection"/> to db with specified connection string name.
        /// </summary>
        /// <remarks>Use this to get connection to do several requests to db in 'using' scope</remarks>
        DataConnection Use(string connectionStringName = null);
        /// <summary>
        /// Gets connection to db with specified connection string name
        /// </summary>
        /// <remarks>Use this to do one request to db without 'using' scope</remarks>
        DataContext DoOnce(string connectionStringName = null);
    }

    class DefaultDbManager : IDbManager
    {
        readonly IConnectionStringProvider _connectionStringProvider;
        readonly IDbProviderSource _providerSource;
        readonly IDslLogger _log;

        public DefaultDbManager(
            IConnectionStringProvider connectionStringProvider, 
            IDbProviderSource providerSource,
            ILogger<DefaultDbManager> logger = null)
        {
            _connectionStringProvider = connectionStringProvider;
            _providerSource = providerSource;
            _log = logger?.Dsl();

            InitLogging();
        }

        void InitLogging()
        {
            DataConnection.TurnTraceSwitchOn();
        }

        public DataConnection Use(string connectionStringName = null)
        {
            var dbProvider = _providerSource.Provide(connectionStringName);
            var cs = _connectionStringProvider.GetConnectionString(connectionStringName);
            var dOpts = AdviceTracing(new DataOptions().UseConnectionString(dbProvider, cs));
            
            return new DataConnection(dOpts);
        }

        public DataContext DoOnce(string connectionStringName = null)
        {
            var dbProvider = _providerSource.Provide(connectionStringName);
            var cs = _connectionStringProvider.GetConnectionString(connectionStringName);
            var dOpts = AdviceTracing(new DataOptions().UseConnectionString(dbProvider, cs));
            
            return new DataContext(dOpts);
        }

        DataOptions AdviceTracing(DataOptions dOpt)
        {
            return dOpt.UseTracing(ti =>
            {
                if (ti.TraceInfoStep != TraceInfoStep.AfterExecute)
                    return;

                DslExpression logRecord;
                if (ti.TraceLevel == TraceLevel.Error)
                {
                    logRecord = ti.Exception != null
                        ? _log?.Error("DB error", ti.Exception)
                        : _log?.Error("DB error");
                }
                else
                {
                    logRecord = _log?.Debug("DB query");
                }

                if (logRecord != null)
                {
                    if (ti.ExecutionTime.HasValue)
                        logRecord = logRecord.AndFactIs("ExecutionTime", ti.ExecutionTime);

                    if (ti.RecordsAffected.HasValue)
                        logRecord = logRecord.AndFactIs("RecordsAffected", ti.RecordsAffected);

                    logRecord
                        .AndFactIs("SqlText", ti.SqlText)
                        .Write();
                }
            });
        }
    }
}
