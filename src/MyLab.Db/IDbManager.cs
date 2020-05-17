using LinqToDB;
using LinqToDB.Async;
using LinqToDB.Data;
using LinqToDB.SqlQuery;

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
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly IDbProviderSource _providerSource;

        public DefaultDbManager(IConnectionStringProvider connectionStringProvider, IDbProviderSource providerSource)
        {
            _connectionStringProvider = connectionStringProvider;
            _providerSource = providerSource;
        }

        public DataConnection Use(string connectionStringName = null)
        {
            var dbProvider = _providerSource.Provide(connectionStringName);
            var cs = _connectionStringProvider.GetConnectionString(connectionStringName);

            return new DataConnection(dbProvider, cs);
        }

        public DataContext DoOnce(string connectionStringName = null)
        {
            var dbProvider = _providerSource.Provide(connectionStringName);
            var cs = _connectionStringProvider.GetConnectionString(connectionStringName);

            return new DataContext(dbProvider, cs);
        }
    }
}
