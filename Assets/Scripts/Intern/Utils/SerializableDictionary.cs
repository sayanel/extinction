using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


namespace Extinction
{
    namespace Utils
    {
        [Serializable]
        public class SerializableDictionary<Key, Value> : IEnumerable
        {
            [SerializeField]
            private List<Key> keys = new List<Key>();

            [SerializeField]
            private List<Value> values = new List<Value>();

            public void Add(Key key, Value value)
            {
                if (keys.Contains(key))
                {
                    int index = keys.IndexOf(key);
                    values[index] = value;
                }
                else
                {
                    keys.Add(key);
                    values.Add(value);
                }
            }

            public bool ContainsKey(Key key)
            {
                return keys.Contains(key);
            }

            public Value this[Key key]
            {
                get
                {
                    return Get(key);
                }
                set
                {
                    Set(key, value);
                }
            }

            private Value Get(Key key)
            {
                if (ContainsKey(key))
                {
                    int index = keys.IndexOf(key);
                    return values[index];
                }
                else
                {
                    return default(Value);
                }
            }

            private void Set(Key key, Value value)
            {
                if (ContainsKey(key))
                {
                    int index = keys.IndexOf(key);
                    values[index] = value;
                }
                else
                {
                    keys.Add(key);
                    values.Add(value);
                }
            }

            public void Clear()
            {
                keys = new List<Key>();
                values = new List<Value>();
            }

            public int Count()
            {
                return keys.Count;
            }

            #region IEnumerable implementation
            public IEnumerator GetEnumerator()
            {
                return values.GetEnumerator();
            }
            #endregion

        }
    }
}