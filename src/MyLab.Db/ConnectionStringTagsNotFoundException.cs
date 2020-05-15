using System.Collections.Immutable;

namespace MyLab.Db
{
    /// <summary>
    /// Throw when connection string tags not found in configuration when cs building 
    /// </summary>
    public class ConnectionStringTagsNotFoundException : ConnectionStringBuildingException
    {
        public ImmutableArray<string> AbsentTags { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="ConnectionStringTagsNotFoundException"/>
        /// </summary>
        public ConnectionStringTagsNotFoundException(string[] absentTags)
            : base("Connection string tags not found: " + string.Join(',', absentTags))
        {
            AbsentTags = absentTags.ToImmutableArray();
        }
    }
}