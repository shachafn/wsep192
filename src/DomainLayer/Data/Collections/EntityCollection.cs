using System;
using System.Collections;
using System.Collections.Generic;

namespace DomainLayer.Data.Collections
{
    public class EntityCollection<T> : IEnumerable<T>
    {
        protected Dictionary<Guid, T> _entitiesCollection = new Dictionary<Guid, T>();

        public T this[Guid key]
        {
            get
            {
                try
                {
                    return _entitiesCollection[key];
                }
                catch
                {
                    return default(T);
                }
            }
            set => _entitiesCollection[key] = value;
        }

        public ICollection<Guid> Keys => _entitiesCollection.Keys;

        public ICollection<T> Values => _entitiesCollection.Values;

        public int Count => _entitiesCollection.Count;

        public void Add(Guid key, T value)
        {
            _entitiesCollection.Add(key, value);
        }

        public void Clear()
        {
            _entitiesCollection.Clear();
        }

        public bool ContainsKey(Guid key) => _entitiesCollection.ContainsKey(key);

        public bool ContainsValue(T entity) => _entitiesCollection.ContainsValue(entity);

        public void Remove(Guid key)
        {
            _entitiesCollection.Remove(key);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _entitiesCollection.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _entitiesCollection.Values.GetEnumerator();
        }

    }
}
