using System;
using System.Collections;
using System.Collections.Generic;

namespace HtmlElements.Proxy
{
    internal abstract class AbstractReadOnlyList<TObject> : IList<TObject>
    {
        protected static NotSupportedException ModificationAttemptException => new("Attempted to modify read-only collection");

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TObject item)
        {
            throw ModificationAttemptException;
        }

        public void Clear()
        {
            throw ModificationAttemptException;
        }

        public bool Remove(TObject item)
        {
            throw ModificationAttemptException;
        }

        public bool IsReadOnly => true;

        public void Insert(int index, TObject item)
        {
            throw ModificationAttemptException;
        }

        public void RemoveAt(int index)
        {
            throw ModificationAttemptException;
        }

        public abstract TObject this[int index] { get; set; }

        public abstract int Count { get; }

        public abstract int IndexOf(TObject item);

        public abstract void CopyTo(TObject[] array, int arrayIndex);

        public abstract bool Contains(TObject item);

        public abstract IEnumerator<TObject> GetEnumerator();
    }
}