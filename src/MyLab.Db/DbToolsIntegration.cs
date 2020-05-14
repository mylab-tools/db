using System;
using LinqToDB.DataProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyLab.Db
{
    /// <summary>
    /// Contains extension methods to integrate tools into application
    /// </summary>
    public static class DbToolsIntegration
    {
        public static IServiceCollection AddDbTools(
            this IServiceCollection services,
            IConnectionStringProvider csProvider,
            IDbProviderSource dbProviderSource)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (dbProviderSource == null) throw new ArgumentNullException(nameof(dbProviderSource));

            services.AddSingleton(csProvider);
            services.AddSingleton(dbProviderSource);

            return services;
        }

        public static IServiceCollection AddDbTools(
            this IServiceCollection services,
            IConfiguration configuration,
            IDataProvider defaultDataProvider)
        {
            return AddDbTools(services,
                new DefaultConnectionStringProvider(configuration),
                new SingleDbProviderSource(defaultDataProvider));
        }
    }
}
