using System;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace UnitTests
{
    public class FrameworkConfigurationBehavior
    {
        [Fact]
        public void ShouldLoadConnectionString()
        {
            //Arrange
            var config = new ConfigurationBuilder()
                .AddJsonFile("config-classic.json")
                .Build();

            //Act
            var cs = config.GetConnectionString("Default");

            //Assert
            Assert.Equal("connection-string", cs);
        }
    }
}
