using System;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.VisualBasic;

namespace MyLab.Db
{
    /// <summary>
    /// Contains extensions for <see cref="LinqToDB.Data.DataConnection"/>
    /// </summary>
    public static class DataConnectionExtensions
    {
        /// <summary>
        /// Performs autocommit transaction
        /// </summary>
        public static async Task PerformAutoTransactionAsync(this DataConnection dataConnection, Func<DataConnection, Task> func)
        {
            if (dataConnection == null) throw new ArgumentNullException(nameof(dataConnection));

            using var transaction = await dataConnection.BeginTransactionAsync();

            try
            {
                await func(dataConnection);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Returns queryable source for specified mapping class for current connection, mapped to database table or view.
        /// </summary>
        public static ITable<T> Tab<T>(this DataConnection dataConnection)
            where T: class
        {
            if (dataConnection == null) throw new ArgumentNullException(nameof(dataConnection));

            return dataConnection.GetTable<T>();
        }
    }
}
