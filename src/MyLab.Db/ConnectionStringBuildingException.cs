using System;
using System.Data.Common;

namespace MyLab.Db
{
    /// <summary>
    /// Throw when connection string building error
    /// </summary>
    public class ConnectionStringBuildingException : DbException
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ConnectionStringBuildingException"/>
        /// </summary>
        public ConnectionStringBuildingException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="ConnectionStringBuildingException"/>
        /// </summary>
        public ConnectionStringBuildingException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}