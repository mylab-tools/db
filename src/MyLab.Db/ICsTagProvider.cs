using System;
using Microsoft.Extensions.Configuration;

namespace MyLab.Db
{
    internal interface ICsTagProvider
    {
        string ProvideTag(string name);
    }

    class ConfigurationSectionCsTagProvider : ICsTagProvider
    {
        private readonly IConfigurationSection _configurationSection;

        public ConfigurationSectionCsTagProvider(IConfigurationSection configurationSection)
        {
            _configurationSection = configurationSection ?? throw new ArgumentNullException(nameof(configurationSection));
        }

        public string ProvideTag(string name)
        {
            return _configurationSection[name];
        }
    }
}