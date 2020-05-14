using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using MyLab.Db;
using Xunit;

namespace UnitTests
{
    public class ConnectionStringProviderBehavior
    {
        [Fact]
        public void ShouldProvideClassicDefaultConnectionString()
        {
            //Arrange
            var csProvider = CreateProvider("classic.json");

            //Act
            var classicDefaultCs = csProvider.GetConnectionString();

            //Assert
            Assert.Equal("foo", classicDefaultCs);
        }

        [Fact]
        public void ShouldProvideClassicNamedConnectionString()
        {
            //Arrange
            var csProvider = CreateProvider("classic.json");

            //Act
            var classicNamedCs = csProvider.GetConnectionString("Custom");

            //Assert
            Assert.Equal("bar", classicNamedCs);
        }

        [Fact]
        public void ShouldThrowExceptionIfNotFound()
        {
            //Arrange
            var csProvider = CreateProvider("classic.json");

            //Act & Assert
            Assert.Throws<ConnectionStringNotFoundException>(()=>csProvider.GetConnectionString("not-exists"));
        }

        [Theory]
        [InlineData("default-detailed.json")]
        [InlineData("default-simple.json")]
        public void ShouldProvideDefaultConnectionString(string filename)
        {
            //Arrange
            var csProvider = CreateProvider(filename);

            //Act
            var defaultCs = csProvider.GetConnectionString();

            //Assert
            Assert.Equal("baz;User=foo;Password=bar", defaultCs);
        }

        [Theory]
        [InlineData("named-detailed.json")]
        [InlineData("named-simple.json")]
        public void ShouldProvideNamedConnectionString(string filename)
        {
            //Arrange
            var csProvider = CreateProvider(filename);

            //Act
            var namedCs = csProvider.GetConnectionString("Cs1");

            //Assert
            Assert.Equal("baz1;User=foo1;Password=bar1", namedCs);
        }

        ConnectionStringProvider CreateProvider(string filename) =>
            new ConnectionStringProvider(new ConfigurationBuilder()
                .AddJsonFile("config\\" + filename)
                .Build());
    }
}
