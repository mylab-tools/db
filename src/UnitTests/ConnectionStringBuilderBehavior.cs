using MyLab.Db;
using Xunit;

namespace UnitTests
{
    public class ConnectionStringBuilderBehavior
    {
        private const string CsPattern = "Host=host.com;UserId={User};Pass={Password}";

        [Fact]
        public void ShouldThrowIfPatternsNotFound()
        {
            //Arrange
            var builder = new ConnectionStringBuilder(CsPattern);
            var tagProvider = new NullCsTagProvider();

            //Act
            ConnectionStringTagsNotFoundException caught = null;

            try
            {
                builder.Build(tagProvider);
            }
            catch (ConnectionStringTagsNotFoundException exception)
            {
                caught = exception;
            }

            //Assert
            Assert.NotNull(caught);
            Assert.Equal(new []{"User", "Password"}, caught.AbsentTags);
        }

        [Fact]
        public void ShouldBuildConnectionString()
        {
            //Arrange
            var builder = new ConnectionStringBuilder(CsPattern);
            var tagProvider = new TestCsTagProvider("foo", "bar");

            //Act
            var cs = builder.Build(tagProvider);

            //Assert
            Assert.Equal("Host=host.com;UserId=foo;Pass=bar", cs);
        }

        class NullCsTagProvider : ICsTagProvider
        {
            public string ProvideTag(string name)
            {
                return null;
            }
        }

        class TestCsTagProvider : ICsTagProvider
        {
            private readonly string _user;
            private readonly string _password;

            public TestCsTagProvider(string user, string password)
            {
                _user = user;
                _password = password;
            }

            public string ProvideTag(string name)
            {
                switch (name)
                {
                    case "User": return _user;
                    case "Password": return _password;
                    default: return null;
                }
            }
        }
    }
}
