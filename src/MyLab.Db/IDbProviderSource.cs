using System.Collections.Generic;
using LinqToDB.DataProvider;

namespace MyLab.Db
{
    /// <summary>
    /// Provides <see cref="IDataProvider"/> for connection strings by name
    /// </summary>
    public interface IDbProviderSource
    {
        IDataProvider Provide(string connectionStringName);
    }

    public class MapDbProviderSource : IDbProviderSource
    {
        private readonly IDictionary<string, IDataProvider> _providersMap;

        /// <summary>
        /// Initializes a new instance of <see cref="MapDbProviderSource"/>
        /// </summary>
        public MapDbProviderSource(IDictionary<string, IDataProvider> providersMap)
        {
            _providersMap = providersMap;
        }
        public IDataProvider Provide(string connectionStringName)
        {
            _providersMap.TryGetValue(connectionStringName, out var dataProvider);
            return dataProvider;
        }
    }

    public class SingleDbProviderSource : IDbProviderSource
    {
        private readonly IDataProvider _dataProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="SingleDbProviderSource"/>
        /// </summary>
        public SingleDbProviderSource(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }
        public IDataProvider Provide(string connectionStringName)
        {
            return _dataProvider;
        }
    }
}
