using System.Collections.Generic;
using HtmlElements.LazyLoad;

namespace HtmlElements.Proxy
{
    internal class ElementListProxy<TElement> : AbstractReadOnlyList<TElement>
    {
        private readonly ILoader<IList<TElement>> _listLoader;

        public ElementListProxy(ILoader<IList<TElement>> listLoader)
        {
            _listLoader = listLoader;
        }

        public override int Count => TypedElementList.Count;

        public override TElement this[int index]
        {
            get => TypedElementList[index];

            set => throw ModificationAttemptException;
        }

        private IList<TElement> TypedElementList => _listLoader.Load();

        public override IEnumerator<TElement> GetEnumerator()
        {
            return TypedElementList.GetEnumerator();
        }

        public override bool Contains(TElement item)
        {
            return TypedElementList.Contains(item);
        }

        public override void CopyTo(TElement[] array, int arrayIndex)
        {
            TypedElementList.CopyTo(array, arrayIndex);
        }

        public override int IndexOf(TElement item)
        {
            return TypedElementList.IndexOf(item);
        }

        public override string ToString()
        {
            return string.Format("{0} loading elements with [{1}]", GetType().Name, _listLoader);
        }
    }
}