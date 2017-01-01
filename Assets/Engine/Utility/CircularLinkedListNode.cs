// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 06/18/2016@1:06 AM

using System.Diagnostics;

namespace Assets.Engine.Utility
{
    /// <summary>
    ///     Represents a node
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("Value = {Value}")]
    public sealed class CircularLinkedListNode<T>
    {
        private readonly T _value;

        /// <summary>
        ///     Initializes a new <see cref="CircularLinkedListNode{T}" /> instance
        /// </summary>
        /// <param name="item">Value to be assigned</param>
        internal CircularLinkedListNode(T item)
        {
            _value = item;
        }

        /// <summary>
        ///     Gets the Value
        /// </summary>
        public T Value
        {
            get { return _value; }
        }

        /// <summary>
        ///     Gets next node
        /// </summary>
        public CircularLinkedListNode<T> Next { get; internal set; }

        /// <summary>
        ///     Gets previous node
        /// </summary>
        public CircularLinkedListNode<T> Previous { get; internal set; }
    }
}