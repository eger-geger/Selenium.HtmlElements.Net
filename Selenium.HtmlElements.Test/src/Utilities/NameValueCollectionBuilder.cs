using System.Collections.Specialized;

namespace HtmlElements.Test.Utilities
{
    public class NameValueCollectionBuilder
    {
        private readonly NameValueCollection _collection = new();

        public NameValueCollectionBuilder Add(string name, string value)
        {
            _collection.Add(name, value);
            return this;
        }

        public NameValueCollection NameValueCollection => new(_collection);
    }
}