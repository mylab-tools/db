using System;
using LinqToDB.Configuration;
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
        [Obsolete("Use 'AddDbTools<TDbProviderSource>(IServiceCollection)' instead")]
        public static IServiceCollection AddDbTools<TDbProviderSource>(
            this IServiceCollection srv,
            IConfiguration configuration)
        where TDbProviderSource : class, IDbProviderSource
        {
            if (srv == null) throw new ArgumentNullException(nameof(srv));

            srv.AddSingleton<IConnectionStringProvider>(new DefaultConnectionStringProvider(configuration));
            srv.AddSingleton<IDbProviderSource, TDbProviderSource>();
            srv.AddSingleton<IDbManager, DefaultDbManager>();

            return srv;
        }

        [Obsolete("Use 'AddDbTools(IServiceCollection, IDbProviderSource)' instead")]
        public static IServiceCollection AddDbTools(
            this IServiceCollection srv,
            IConnectionStringProvider csProvider,
            IDbProviderSource dbProviderSource)
        {
            if (srv == null) throw new ArgumentNullException(nameof(srv));
            if (dbProviderSource == null) throw new ArgumentNullException(nameof(dbProviderSource));

            srv.AddSingleton(csProvider);
            srv.AddSingleton(dbProviderSource);
            srv.AddSingleton<IDbManager, DefaultDbManager>();

            return srv;
        }

        [Obsolete("Use 'AddDbTools(IServiceCollection, IDataProvider)' instead")]
        public static IServiceCollection AddDbTools(
            this IServiceCollection srv,
            IConfiguration configuration,
            IDataProvider defaultDataProvider)
        {
            return AddDbTools(srv,
                new DefaultConnectionStringProvider(configuration),
                new SingleDbProviderSource(defaultDataProvider));
        }
        
        /// <summary>
        /// Adds DB tools with specific data provider source
        /// </summary>
        public static IServiceCollection AddDbTools<TDbProviderSource>(this IServiceCollection srv)
            where TDbProviderSource : class, IDbProviderSource
        {
            return srv.AddSingleton<IDbProviderSource, TDbProviderSource>()
                .AddSingleton<IDbManager, DefaultDbManager>();
        }

        /// <summary>
        /// Adds DB tools with specific data provider source
        /// </summary>
        public static IServiceCollection AddDbTools(this IServiceCollection srv, IDbProviderSource dbProviderSource)
        {
            return srv
                .AddSingleton(dbProviderSource)
                .AddSingleton<IDbManager, DefaultDbManager>();
        }

        /// <summary>
        /// Adds DB tools with specific data provider
        /// </summary>
        public static IServiceCollection AddDbTools(this IServiceCollection srv, IDataProvider defaultDataProvider)
        {
            return AddDbTools(srv, new SingleDbProviderSource(defaultDataProvider));
        }

        
        /// <summary>
        /// Configure DB tools with default config section "DB"
        /// </summary>
        public static IServiceCollection ConfigureDbTools(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddSingleton<IConnectionStringProvider>(new DefaultConnectionStringProvider(configuration));
        }

        /// <summary>
        /// Configure DB tools with specified config section name
        /// </summary>
        public static IServiceCollection ConfigureDbTools(this IServiceCollection services, IConfiguration configuration, string sectionName)
        {
            return services.AddSingleton<IConnectionStringProvider>(new DefaultConnectionStringProvider(configuration, sectionName));
        }
}
}
