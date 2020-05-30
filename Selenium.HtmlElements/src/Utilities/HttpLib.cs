using System;
using System.Collections.Specialized;
using System.Linq;

namespace HtmlElements.Utilities
{
    internal static class HttpLib
    {
        /// <summary>
        /// Parses URL query string into name-value pairs
        /// </summary>
        /// <param name="query">Query string</param>
        /// <returns>Collection of URL-decoded query arguments</returns>
        public static NameValueCollection ParseQueryString(string query)
        {
            var collection = new NameValueCollection();

            if (string.IsNullOrWhiteSpace(query))
            {
                return collection;
            }

            var parts = query
                .Split(new[] {'&', '?'}, StringSplitOptions.RemoveEmptyEntries)
                .Select(ParseQueryArg)
                .ToList();

            foreach (var nameValuePair in parts)
            {
                collection.Add(nameValuePair[0], nameValuePair[1]);
            }

            return collection;
        }

        private static string[] ParseQueryArg(string arg)
        {
            if (string.IsNullOrWhiteSpace(arg)) return new[] {string.Empty, string.Empty};

            var pair = arg.Split(new[] {'='}, 2, StringSplitOptions.RemoveEmptyEntries);

            var name = Uri.UnescapeDataString(pair[0]);

            var value = pair.Length == 2
                ? Uri.UnescapeDataString(pair[1])
                : string.Empty;

            return new []{name, value};
        }
    }
}