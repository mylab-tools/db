using System;
using System.Threading.Tasks;
using LinqToDB.Data;

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
    }
}
