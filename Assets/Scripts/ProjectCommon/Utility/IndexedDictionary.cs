// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 06/18/2016@1:06 AM

using System;
using System.Collections.Generic;

namespace Assets.Scripts.ProjectCommon.Utility
{
    /// <summary>
    ///     An indexable Dictionary
    /// </summary>
    /// <remarks>http://www.codeproject.com/Articles/30729/Indexed-Dictionary</remarks>
    /// <typeparam name="TKey">type for the key</typeparam>
    /// <typeparam name="TValue">type for the value</typeparam>
    public class IndexedDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        #region Private variables

        protected List<TKey> m_col = new List<TKey>();
        protected bool m_bReplaceDuplicateKeys = true; //can key be inserted more than once?
        //if so then it will if it contains the key on add

        protected bool m_bThrowErrorOnInvalidRemove; //if true then throws an exception if the 

        #endregion

        #region Contructors

        public IndexedDictionary()
        {
            ValidateKeyType();
        }

        public IndexedDictionary(bool bReplaceDuplicateKeys)
        {
            ValidateKeyType();
            m_bReplaceDuplicateKeys = bReplaceDuplicateKeys;
        }

        public IndexedDictionary(bool bReplaceDuplicateKeys, bool bThrowErrorOnInvalidRemove)
            : this(bReplaceDuplicateKeys)
        {
            ValidateKeyType();
            m_bThrowErrorOnInvalidRemove = bThrowErrorOnInvalidRemove;
        }

        /// <summary>
        ///     Makes sure int is not used as dictionary key:
        /// </summary>
        private static void ValidateKeyType()
        {
            if (typeof (TKey) == typeof (int))
            {
                throw new IndexedDictionaryException("Key of type int is not supported.");
            }
        }

        #endregion

        #region Properties

        public bool ReplaceDuplicateKeys
        {
            get { return m_bReplaceDuplicateKeys; }
        }

        public bool ThrowErrorOnInvalidRemove
        {
            get { return m_bThrowErrorOnInvalidRemove; }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Override in derived class to manipulate key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual TKey TransformKey(TKey key)
        {
            return key;
        }

        public bool Contains(TKey key)
        {
            return base.ContainsKey(TransformKey(key));
        }

        public new virtual void Add(TKey key, TValue item)
        {
            AddAt(-1, key, item);
        }

        public virtual void AddAt(int index, TKey key, TValue item)
        {
            var e = new DictionaryBeforeEventArgs<TKey, TValue>
                (key, item);

            // Raise before events:
            var bubble = true;
            if (BeforeAdd != null)
            {
                foreach (DictionaryBeforeDelegate<TKey, TValue> function in BeforeAdd.GetInvocationList())
                {
                    e.Bubble = true;
                    function.Invoke(this, e);
                    bubble = bubble && e.Bubble;
                }
            }
            if (!bubble) return;

            // Add item:
            // Use value returend by event:
            if (m_bReplaceDuplicateKeys && ContainsKey(TransformKey(e.Key))) //check if it contains and remove
                Remove(TransformKey(e.Key));

            base.Add(TransformKey(e.Key), e.Value);

            if (index != -1)
                m_col.Insert(index, e.Key);
            else
                m_col.Add(e.Key);

            // Raise after events:
            if (AfterAdd != null)
            {
                AfterAdd.Invoke(this, e);
            }
        }

        public virtual void RemoveAt(int index)
        {
            if (m_bThrowErrorOnInvalidRemove)
                if (index < 0 || index >= m_col.Count)
                    throw new IndexedDictionaryException("Cannot remove invalid Index");
            var key = m_col[index];
            Remove(TransformKey(key));
        }

        public new void Remove(TKey key)
        {
            var bContains = ContainsKey(TransformKey(key));
            if (m_bThrowErrorOnInvalidRemove && !bContains)
                throw new IndexedDictionaryException("Key does not exist within the Dictionary");
            if (!bContains)
                return;

            // Raise before events:
            var e = new DictionaryBeforeEventArgs<TKey, TValue>
                (key, base[TransformKey(key)]);

            // Raise before events:
            var bubble = true;
            if (BeforeRemove != null)
            {
                foreach (var @delegate in BeforeRemove.GetInvocationList())
                {
                    var function = (DictionaryBeforeDelegate<TKey, TValue>) @delegate;
                    e.Bubble = true;
                    function.Invoke(this, e);
                    bubble = bubble && e.Bubble;
                }
            }

            if (!bubble)
                return;

            // Remove item:
            // Use value returend by event:
            m_col.Remove(e.Key);
            base.Remove(TransformKey(e.Key));

            // Raise after events:
            if (AfterRemove != null)
                AfterRemove.Invoke(this, e);
        }

        public TKey GetKeyByIndex(int index)
        {
            if (index < 0)
                return default(TKey);

            // Skip if the index is larger than we have values.
            return index > Count ? default(TKey) : m_col[index];
        }

        public new bool ContainsKey(TKey key)
        {
            return base.ContainsKey(TransformKey(key));
        }

        /// <summary>
        ///     Clears all values and raises events.
        /// </summary>
        public new void Clear()
        {
            var e = new DictionaryBeforeClearEventArgs();

            // Raise before events:
            var bubble = true;
            if (BeforeClear != null)
            {
                foreach (DictionaryBeforeClearDelegate function in BeforeClear.GetInvocationList())
                {
                    e.Bubble = true;
                    function.Invoke(this, e);
                    bubble = bubble && e.Bubble;
                }
            }
            if (!bubble) return;

            // Clear items item:
            base.Clear();
            m_col.Clear();

            // Raise after events:
            if (AfterClear != null)
            {
                AfterClear.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        ///     Searches for a key that matches the conditions defined by the specified predicate, and returnes a zero-based index
        ///     of the first occurrence in the list of keys.
        /// </summary>
        /// <returns></returns>
        public int GetIndex(Predicate<TKey> predicate)
        {
            return m_col.FindIndex(predicate);
        }

        #endregion

        #region Indexers

        public new TValue this[TKey key]
        {
            get { return base[TransformKey(key)]; }
            set
            {
                if (!base.ContainsKey(TransformKey(key)))
                {
                    throw new IndexedDictionaryException("Values cannot be added directly. Use Add() or AddAt() method.");
                }
                base[TransformKey(key)] = value;
            }
        }


        public TValue this[int index]
        {
            get { return this[m_col[index]]; }
            set { this[m_col[index]] = value; }
        }

        #endregion

        #region Events

        public event DictionaryBeforeDelegate<TKey, TValue> BeforeAdd;

        public event DictionaryBeforeDelegate<TKey, TValue> BeforeRemove;

        public event DictionaryAfterDelegate<TKey, TValue> AfterAdd;

        public event DictionaryAfterDelegate<TKey, TValue> AfterRemove;

        public event DictionaryBeforeClearDelegate BeforeClear;

        public event DictionaryAfterClearDelegate AfterClear;

        #endregion
    }

    public delegate void DictionaryBeforeDelegate<TKey, TValue>(object sender, DictionaryBeforeEventArgs<TKey, TValue> e
        );

    public delegate void DictionaryAfterDelegate<TKey, TValue>(object sender, DictionaryEventArgs<TKey, TValue> e);

    public delegate void DictionaryBeforeClearDelegate(object sender, DictionaryBeforeClearEventArgs e);

    public delegate void DictionaryAfterClearDelegate(object sender, EventArgs e);

    public class DictionaryEventArgs<TKey, TValue> : EventArgs
    {
        private TKey _key;

        private TValue _value;

        public DictionaryEventArgs(TKey key, TValue value)
        {
            _value = value;
            _key = key;
        }

        public TValue Value
        {
            get { return _value; }
        }

        public TKey Key
        {
            get { return _key; }
        }
    }

    public class DictionaryBeforeEventArgs<TKey, TValue> : DictionaryEventArgs<TKey, TValue>
    {
        private bool _bubble = true;

        public DictionaryBeforeEventArgs(TKey key, TValue value)
            : base(key, value)
        {
        }

        public bool Bubble
        {
            get { return _bubble; }
            set { _bubble = value; }
        }
    }

    public class DictionaryBeforeClearEventArgs
    {
        private bool _bubble = true;

        public bool Bubble
        {
            get { return _bubble; }
            set { _bubble = value; }
        }
    }

    public class IndexedDictionaryException : ApplicationException
    {
        public IndexedDictionaryException()
        {
        }

        public IndexedDictionaryException(string message)
            : base(message)
        {
        }

        public IndexedDictionaryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}