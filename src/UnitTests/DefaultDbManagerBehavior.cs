using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using MyLab.Db;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests
{
    public class DefaultDbManagerBehavior :IClassFixture<TestDbFixture>
    {
        private readonly string _connectionString;

        public DefaultDbManagerBehavior(TestDbFixture dbFixture, ITestOutputHelper output)
        {
            dbFixture.Output = output;
            _connectionString = dbFixture.ConnectionString;
        }

        [Fact]
        public async Task ShouldUseConnection()
        {
            //Arrange
            var csProvider = new TestSingleCsProvider(_connectionString);
            var dbProviderSource = new SingleDbProviderSource(new SQLiteDataProvider());
            IDbManager dbManager = new DefaultDbManager(csProvider, dbProviderSource);

            //Act
            await using var dc = dbManager.Connect();
            var res = await dc
                    .GetTable<TestDbEntity>()
                    .Where(e => e.Id == 0)
                    .FirstOrDefaultAsync();

            //Assert
            Assert.Equal("foo", res.Value);
        }
    }
}
