using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace MyLab.Db
{
    class ConnectionStringBuilder
    {
        private readonly string _csPattern;

        /// <summary>
        /// Initializes a new instance of <see cref="ConnectionStringBuilder"/>
        /// </summary>
        public ConnectionStringBuilder(string csPattern)
        {
            _csPattern = csPattern;
        }

        public string Build(ICsTagProvider csTagProvider)
        {
            if (csTagProvider == null) throw new ArgumentNullException(nameof(csTagProvider));

            var tags = new Dictionary<string, string>();

            foreach (Match match in Regex.Matches(_csPattern, "\\{(?<tag>\\w+)\\}"))
            {
                var key = match.Groups["tag"].Value;
                if(tags.ContainsKey(key))
                    continue;
                
                tags.Add(key, csTagProvider.ProvideTag(key));
            }

            var notFound = tags
                .Where(kv => kv.Value == null)
                .Select(kv => kv.Key)
                .ToArray();

            if(notFound.Length != 0)
                throw new ConnectionStringTagsNotFoundException(notFound);

            string resultCs = _csPattern;

            foreach (var tag in tags)
            {
                resultCs = resultCs.Replace("{" + tag.Key + "}", tag.Value);
            }

            return resultCs;
        }
    }
}
