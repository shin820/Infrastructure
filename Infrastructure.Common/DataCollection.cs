using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    /// <summary>
    /// DataCollection
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <typeparam name="TComparer">The type of the comparer.</typeparam>
    [Serializable]
    public abstract class DataCollection<TKey, TValue, TComparer> : IEnumerable<KeyValuePair<TKey, TValue>>
        where TComparer : IEqualityComparer<TKey>
    {
        private IDictionary<TKey, TValue> _innerDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataCollection{TKey, TValue, TComparer}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        protected internal DataCollection(TComparer comparer)
        {
            this._innerDictionary = new Dictionary<TKey, TValue>(comparer);
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this._innerDictionary.Add(item);
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(TKey key, TValue value)
        {
            this._innerDictionary.Add(key, value);
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="items">The items.</param>
        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            if (items != null)
            {
                ICollection<KeyValuePair<TKey, TValue>> is2 = this._innerDictionary;
                foreach (KeyValuePair<TKey, TValue> pair in items)
                {
                    is2.Add(pair);
                }
            }
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="items">The items.</param>
        public void AddRange(params KeyValuePair<TKey, TValue>[] items)
        {
            this.AddRange((IEnumerable<KeyValuePair<TKey, TValue>>)items);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this._innerDictionary.Clear();
        }

        /// <summary>
        /// Clears the internal.
        /// </summary>
        internal void ClearInternal()
        {
            this._innerDictionary.Clear();
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>bool</returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return this._innerDictionary.Contains(item);
        }

        /// <summary>
        /// Determines whether [contains] [the specified key].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>bool</returns>
        public bool Contains(TKey key)
        {
            return this._innerDictionary.ContainsKey(key);
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>bool</returns>
        public bool ContainsKey(TKey key)
        {
            return this._innerDictionary.ContainsKey(key);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this._innerDictionary.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>IEnumerator{KeyValuePair{TKey, TValue}}</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this._innerDictionary.GetEnumerator();
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>bool</returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return this._innerDictionary.Remove(item);
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>bool</returns>
        public bool Remove(TKey key)
        {
            return this._innerDictionary.Remove(key);
        }

        /// <summary>
        /// Removes the internal.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>bool</returns>
        internal bool RemoveInternal(TKey key)
        {
            return this._innerDictionary.Remove(key);
        }

        /// <summary>
        /// Sets the item internal.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        internal void SetItemInternal(TKey key, TValue value)
        {
            this._innerDictionary[key] = value;
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举器。
        /// </summary>
        /// <returns>
        /// 可用于循环访问集合的 <see cref="T:System.Collections.IEnumerator" /> 对象。
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._innerDictionary.GetEnumerator();
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>bool</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return this._innerDictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get
            {
                return this._innerDictionary.Count;
            }
        }

        /// <summary>
        /// Gets or sets the TValue with the specified key.
        /// </summary>
        /// <value>
        /// The TValue
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns>TValue</returns>
        public virtual TValue this[TKey key]
        {
            get
            {
                return this._innerDictionary[key];
            }

            set
            {
                this._innerDictionary[key] = value;
            }
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>
        /// The keys.
        /// </value>
        public ICollection<TKey> Keys
        {
            get
            {
                return this._innerDictionary.Keys;
            }
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public ICollection<TValue> Values
        {
            get
            {
                return this._innerDictionary.Values;
            }
        }
    }
}
