using System;
using LinqToDB;

namespace MyLab.Db
{
    /// <summary>
    /// Contains extensions for <see cref="LinqToDB.IDataContext"/>
    /// </summary>
    public static class DataContextExtensions
    {
        /// <summary>
        /// Returns queryable source for specified mapping class for current connection, mapped to database table or view.
        /// </summary>
        public static ITable<T> Tab<T>(this IDataContext dataContext)
            where T : class
        {
            if (dataContext == null) throw new ArgumentNullException(nameof(dataContext));

            return dataContext.GetTable<T>();
        }
    }
}