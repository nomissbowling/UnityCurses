// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 06/18/2016@1:06 AM

using System.Collections.Generic;

namespace Assets.Scripts.Engine.Utility
{
    public class InverseComparer<T> : IComparer<T>
    {
        private readonly IComparer<T> _comparer;

        public InverseComparer(IComparer<T> comparer)
        {
            _comparer = comparer;
        }

        public IComparer<T> Comparer
        {
            get { return _comparer; }
        }

        public int Compare(T x, T y)
        {
            return Comparer.Compare(y, x);
        }
    }
}