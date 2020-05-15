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
        DataConnection Connect(string connectionStringName = null);
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

        public DataConnection Connect(string connectionStringName = null)
        {
            var dbProvider = _providerSource.Provide(connectionStringName);
            var cs = _connectionStringProvider.GetConnectionString(connectionStringName);

            return new DataConnection(dbProvider, cs);
        }
    }
}
