using System;
using Microsoft.Extensions.Configuration;

namespace MyLab.Db
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString(string name = null);
    }

    class DefaultConnectionStringProvider : IConnectionStringProvider
    {
        public const string ClassicDefaultCsName = "Default";
        public const string CsSectionName = "DB";
        public const string CsNodeName = "ConnectionString";

        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultConnectionStringProvider"/>
        /// </summary>
        public DefaultConnectionStringProvider(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public string GetConnectionString(string name = null)
        {
            var classicCs = _config.GetConnectionString(name ?? ClassicDefaultCsName);
            if (classicCs != null)
                return classicCs;

            var dbSection = _config.GetSection(
                name != null 
                    ? $"{CsSectionName}:{name}"
                    : CsSectionName);

            if (dbSection.Exists())
            {
                var csSection = dbSection.GetSection(CsNodeName);

                if (!csSection.Exists())
                {
                    return dbSection.Value;
                }

                if (csSection.Value != null)
                {
                    var csTagProvider = new ConfigurationSectionCsTagProvider(dbSection);
                    var csBuilder = new ConnectionStringBuilder(csSection.Value);
                    return csBuilder.Build(csTagProvider);
                }
            }

            throw new ConnectionStringNotFoundException(name);
        }
    }
}
