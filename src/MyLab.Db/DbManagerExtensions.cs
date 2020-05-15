using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB.Data;

namespace MyLab.Db
{
    /// <summary>
    /// Contains extensions for <see cref="IDbManager"/>
    /// </summary>
    public static class DbManagerExtensions
    {
        public static Task UseAsync(this IDbManager manager, Func<DataConnection, Task> action)
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            return manager.UseAsync(null, action);
        }

        public static Task<IEnumerable<TRes>> GetAsync<TRes>(this IDbManager manager, Func<DataConnection, Task<TRes[]>> func)
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            return manager.UseAsync(null, func);
        }

        public static async Task<TRes> FirstOrDefaultAsync<TRes>(this IDbManager manager, Func<DataConnection, Task<TRes[]>> func)
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            return (await manager.UseAsync(null, func)).FirstOrDefault();
        }

        public static async Task UseAsync(this IDbManager manager, string connectionStringName, Func<DataConnection, Task> action)
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            await using var c = manager.Connect(connectionStringName);

            await action(c);
        }

        public static async Task<IEnumerable<TRes>> UseAsync<TRes>(this IDbManager manager, string connectionStringName, Func<DataConnection, Task<TRes[]>> func)
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            await using var c = manager.Connect(connectionStringName);

            return await func(c);
        }
    }
}