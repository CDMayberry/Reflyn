using System;
using System.Collections;
using System.Collections.Specialized;

namespace Reflyn.Collections
{
    public class StringSet : ICollection
    {
        private readonly IDictionary _dictionary = new HybridDictionary();

        public bool IsSynchronized => false;

        public int Count => _dictionary.Count;

        public object SyncRoot => null;

        public bool IsReadOnly => false;

        public bool IsFixedSize => false;

        public void CopyTo(Array array, int index)
        {
            _dictionary.Keys.CopyTo(array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return _dictionary.Keys.GetEnumerator();
        }

        public void Remove(string value)
        {
            _dictionary.Remove(value);
        }

        public bool Contains(object value)
        {
            return _dictionary.Contains(value);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public void Add(string value)
        {
            if (!_dictionary.Contains(value))
            {
                _dictionary.Add(value, null);
            }
        }
    }
}
