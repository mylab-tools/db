using MyLab.Db;

namespace UnitTests
{
    class TestMemoryDbCsProvider : IConnectionStringProvider
    {
        public string GetConnectionString(string name = null)
        {
            return "Data Source=:memory:";
        }
    }
}