using LinqToDB;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.Internal.DataProvider.SQLite;

namespace MyLab.Db
{
    /// <summary>
    /// Provides the default implementation of the SQLite data provider.
    /// </summary>
    public class DefaultSQLiteProvider : SQLiteDataProvider
    {
        public DefaultSQLiteProvider() : base(ProviderName.SQLiteClassic, SQLiteProvider.System)
        {
            
        }
    }
}
