using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using MyLab.Db;
using Xunit;

namespace UnitTests
{
    public class DefaultDbManagerBehavior
    {
        [Fact]
        public async Task ShouldUseConnection()
        {
            //Arrange
            var csProvider = new TestCsProvider();
            var dbProviderSource = new SingleDbProviderSource(new SQLiteDataProvider());
            IDbManager dbManager = new DefaultDbManager(csProvider, dbProviderSource);

            //Act
            var res = await dbManager.FirstOrDefaultAsync(async dc =>
            {
                return await dc.QueryToArrayAsync<string>("select \"foo\";");
            });

            //Assert
            Assert.Equal("foo", res);
        }

        class TestCsProvider : IConnectionStringProvider
        {
            public string GetConnectionString(string name = null)
            {
                return "Data Source=:memory:";
            }
        }
    }
}
