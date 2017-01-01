// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 06/18/2016@1:06 AM

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.ProjectCommon.Utility
{
    public class EnumerableCollection<T> : ICollection<T>
    {
        private readonly IEnumerable<T> _enumerable;

        public EnumerableCollection(IEnumerable<T> enumerable, int count)
        {
            _enumerable = enumerable;
            Count = count;
        }

        public int Count { get; }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(T item)
        {
            return this.Any(v => item.Equals(v));
        }

        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }
    }
}