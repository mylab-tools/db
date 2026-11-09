using System.Linq;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Internal.DataProvider.SQLite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyLab.Db;
using Xunit;

namespace UnitTests
{
    public class DbToolsIntegrationBehavior
    {
        [Fact]
        public void ShouldProvideConnectionTools()
        {
            //Arrange
            var config = new ConfigurationBuilder()
                .AddJsonFile("config\\sqlite.json")
                .Build();
            var serviceProvider = new ServiceCollection()
                .AddDbTools(new DefaultSQLiteProvider())
                .ConfigureDbTools(config)
                .BuildServiceProvider();

            var serviceConsumer = ActivatorUtilities.CreateInstance<TestService>(serviceProvider);

            //Act
            var value = serviceConsumer.GetValue("foo");

            //Assert
            Assert.Equal("foo", value);
        }

        class TestService 
        {
            private readonly IConnectionStringProvider _connectionStringProvider;
            private readonly IDbProviderSource _providerSource;

            public TestService(IConnectionStringProvider connectionStringProvider, IDbProviderSource providerSource)
            {
                _connectionStringProvider = connectionStringProvider;
                _providerSource = providerSource;
            }

            public string GetValue(string value)
            {
                var dbProvider = _providerSource.Provide(null);
                var cs = _connectionStringProvider.GetConnectionString();

                using var c= new DataConnection(dbProvider, cs);

                return c.Query<string>($"select \"{value}\";").First();
            }
        }
    }
}
