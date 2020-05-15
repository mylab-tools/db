using MyLab.Db;

namespace UnitTests
{
    class TestSingleCsProvider : IConnectionStringProvider
    {
        private readonly string _connectionString;

        public TestSingleCsProvider(string connectionString)
        {
            _connectionString = connectionString;
        }
        public string GetConnectionString(string name = null)
        {
            return _connectionString;
        }
    }
}