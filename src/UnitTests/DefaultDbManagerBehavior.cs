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
            IDbManager dbManager = CreateDbManager();

            //Act
            await using var dc = dbManager.Use();
            var res = await dc
                    .GetTable<TestDbEntity>()
                    .FirstOrDefaultAsync(e => e.Id == 0);

            //Assert
            Assert.Equal("foo", res.Value);
        }

        [Fact]
        public async Task ShouldDoOnceRequest()
        {
            //Arrange
            IDbManager dbManager = CreateDbManager();

            //Act
            var res = await dbManager.DoOnce()
                .GetTable<TestDbEntity>()
                .FirstOrDefaultAsync(e => e.Id == 0);

            //Assert
            Assert.Equal("foo", res.Value);
        }

        IDbManager CreateDbManager()
        {
            var csProvider = new TestSingleCsProvider(_connectionString);
            var dbProviderSource = new SingleDbProviderSource(new SQLiteDataProvider(ProviderName.SQLite));
            return new DefaultDbManager(csProvider, dbProviderSource);
        }
    }
}
