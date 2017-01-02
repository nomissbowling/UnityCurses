// Created by Ron 'Maxwolf' McDowell (ron.mcdowell@gmail.com) 
// Timestamp 06/18/2016@1:06 AM

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Assets.Engine.Utility
{
    /// <summary>
    ///     Represents a circular doubly linked list.
    /// </summary>
    /// <remarks>https://navaneethkn.wordpress.com/2009/08/18/circular-linked-list/</remarks>
    /// <remarks>http://navaneeth.github.io/blog/2009/08/18/circular-linked-list-an-implementation-using-c-number/</remarks>
    /// <typeparam name="T">Specifies the element type of the linked list.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    public sealed class CircularLinkedList<T> : ICollection<T>, IEnumerable<T>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private readonly IEqualityComparer<T> comparer;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private int count;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private CircularLinkedListNode<T> head;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)] private CircularLinkedListNode<T> tail;

        /// <summary>
        ///     Initializes a new instance of CircularLinkedList.
        /// </summary>
        public CircularLinkedList()
            : this(null, EqualityComparer<T>.Default)
        {
        }

        /// <summary>
        ///     Initializes a new instance of CircularLinkedList.
        /// </summary>
        /// <param name="collection">Collection of objects that will be added to linked list</param>
        public CircularLinkedList(IEnumerable<T> collection)
            : this(collection, EqualityComparer<T>.Default)
        {
        }

        /// <summary>
        ///     Initializes a new instance of CircularLinkedList.
        /// </summary>
        /// <param name="comparer">Comparer used for item comparison</param>
        /// <exception cref="ArgumentNullException">comparer is null</exception>
        public CircularLinkedList(IEqualityComparer<T> comparer)
            : this(null, comparer)
        {
        }

        /// <summary>
        ///     Initializes a new instance of CircularLinkedList.
        /// </summary>
        /// <param name="collection">Collection of objects that will be added to linked list</param>
        /// <param name="comparer">Comparer used for item comparison</param>
        public CircularLinkedList(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");
            this.comparer = comparer;
            if (collection != null)
                foreach (var item in collection)
                    AddLast(item);
        }

        /// <summary>
        ///     Gets Tail node. Returns NULL if no tail node found
        /// </summary>
        public CircularLinkedListNode<T> Tail
        {
            get { return tail; }
        }

        /// <summary>
        ///     Gets the head node. Returns NULL if no node found
        /// </summary>
        public CircularLinkedListNode<T> Head
        {
            get { return head; }
        }

        /// <summary>
        ///     Gets the item at the current index
        /// </summary>
        /// <param name="index">Zero-based index</param>
        /// <exception cref="ArgumentOutOfRangeException">index is out of range</exception>
        public CircularLinkedListNode<T> this[int index]
        {
            get
            {
                if ((index >= count) || (index < 0))
                    throw new ArgumentOutOfRangeException("index");
                var node = head;
                for (var i = 0; i < index; i++)
                    node = node.Next;
                return node;
            }
        }

        /// <summary>
        ///     Gets total number of items in the list
        /// </summary>
        public int Count
        {
            get { return count; }
        }

        /// <summary>
        ///     Removes the first occurrence of the supplied item
        /// </summary>
        /// <param name="item">Item to be removed</param>
        /// <returns>TRUE if removed, else FALSE</returns>
        public bool Remove(T item)
        {
            // finding the first occurrence of this item
            var nodeToRemove = Find(item);
            if (nodeToRemove != null)
                return RemoveNode(nodeToRemove);
            return false;
        }

        /// <summary>
        ///     Clears the list
        /// </summary>
        public void Clear()
        {
            head = null;
            tail = null;
            count = 0;
        }

        /// <summary>
        ///     Gets a forward enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            var current = head;
            if (current != null)
                do
                {
                    yield return current.Value;
                    current = current.Next;
                } while (current != head);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Determines whether a value is in the list.
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <returns>TRUE if item exist, else FALSE</returns>
        public bool Contains(T item)
        {
            return Find(item) != null;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if ((arrayIndex < 0) || (arrayIndex > array.Length))
                throw new ArgumentOutOfRangeException("arrayIndex");

            var node = head;
            do
            {
                array[arrayIndex++] = node.Value;
                node = node.Next;
            } while (node != head);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        void ICollection<T>.Add(T item)
        {
            AddLast(item);
        }

        /// <summary>
        ///     Add a new item to the end of the list
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void AddLast(T item)
        {
            // if head is null, then this will be the first item
            if (head == null)
                AddFirstItem(item);
            else
            {
                var newNode = new CircularLinkedListNode<T>(item);
                tail.Next = newNode;
                newNode.Next = head;
                newNode.Previous = tail;
                tail = newNode;
                head.Previous = tail;
            }
            ++count;
        }

        private void AddFirstItem(T item)
        {
            head = new CircularLinkedListNode<T>(item);
            tail = head;
            head.Next = tail;
            head.Previous = tail;
        }

        /// <summary>
        ///     Adds item to the last
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void AddFirst(T item)
        {
            if (head == null)
                AddFirstItem(item);
            else
            {
                var newNode = new CircularLinkedListNode<T>(item);
                head.Previous = newNode;
                newNode.Previous = tail;
                newNode.Next = head;
                tail.Next = newNode;
                head = newNode;
            }
            ++count;
        }

        /// <summary>
        ///     Adds the specified item after the specified existing node in the list.
        /// </summary>
        /// <param name="linkedListNode">Existing node after which new item will be inserted</param>
        /// <param name="item">New item to be inserted</param>
        /// <exception cref="ArgumentNullException"><paramref name="linkedListNode" /> is NULL</exception>
        /// <exception cref="InvalidOperationException"><paramref name="linkedListNode" /> doesn't belongs to list</exception>
        public void AddAfter(CircularLinkedListNode<T> linkedListNode, T item)
        {
            if (linkedListNode == null)
                throw new ArgumentNullException("linkedListNode");
            // ensuring the supplied node belongs to this list
            var temp = FindNode(head, linkedListNode.Value);
            if (temp != linkedListNode)
                throw new InvalidOperationException("Node doesn't belongs to this list");

            var newNode = new CircularLinkedListNode<T>(item);
            newNode.Next = linkedListNode.Next;
            linkedListNode.Next.Previous = newNode;
            newNode.Previous = linkedListNode;
            linkedListNode.Next = newNode;

            // if the node adding is tail node, then re-pointing tail node
            if (linkedListNode == tail)
                tail = newNode;
            ++count;
        }

        /// <summary>
        ///     Adds the new item after the specified existing item in the list.
        /// </summary>
        /// <param name="existingItem">Existing item after which new item will be added</param>
        /// <param name="newItem">New item to be added to the list</param>
        /// <exception cref="ArgumentException"><paramref name="existingItem" /> doesn't exist in the list</exception>
        public void AddAfter(T existingItem, T newItem)
        {
            // finding a node for the existing item
            var node = Find(existingItem);
            if (node == null)
                throw new ArgumentException("existingItem doesn't exist in the list");
            AddAfter(node, newItem);
        }

        /// <summary>
        ///     Adds the specified item before the specified existing node in the list.
        /// </summary>
        /// <param name="linkedListNode">Existing node before which new item will be inserted</param>
        /// <param name="item">New item to be inserted</param>
        /// <exception cref="ArgumentNullException"><paramref name="linkedListNode" /> is NULL</exception>
        /// <exception cref="InvalidOperationException"><paramref name="linkedListNode" /> doesn't belongs to list</exception>
        public void AddBefore(CircularLinkedListNode<T> linkedListNode, T item)
        {
            if (linkedListNode == null)
                throw new ArgumentNullException("linkedListNode");
            // ensuring the supplied node belongs to this list
            var temp = FindNode(head, linkedListNode.Value);
            if (temp != linkedListNode)
                throw new InvalidOperationException("Node doesn't belongs to this list");

            var newNode = new CircularLinkedListNode<T>(item);
            linkedListNode.Previous.Next = newNode;
            newNode.Previous = linkedListNode.Previous;
            newNode.Next = linkedListNode;
            linkedListNode.Previous = newNode;

            // if the node adding is head node, then re-pointing head node
            if (linkedListNode == head)
                head = newNode;
            ++count;
        }

        /// <summary>
        ///     Adds the new item before the specified existing item in the list.
        /// </summary>
        /// <param name="existingItem">Existing item before which new item will be added</param>
        /// <param name="newItem">New item to be added to the list</param>
        /// <exception cref="ArgumentException"><paramref name="existingItem" /> doesn't exist in the list</exception>
        public void AddBefore(T existingItem, T newItem)
        {
            // finding a node for the existing item
            var node = Find(existingItem);
            if (node == null)
                throw new ArgumentException("existingItem doesn't exist in the list");
            AddBefore(node, newItem);
        }

        /// <summary>
        ///     Finds the supplied item and returns a node which contains item. Returns NULL if item not found
        /// </summary>
        /// <param name="item">Item to search</param>
        /// <returns><see cref="CircularLinkedListNode{T}" /> instance or NULL</returns>
        public CircularLinkedListNode<T> Find(T item)
        {
            var node = FindNode(head, item);
            return node;
        }

        private bool RemoveNode(CircularLinkedListNode<T> linkedListNodeToRemove)
        {
            var previous = linkedListNodeToRemove.Previous;
            previous.Next = linkedListNodeToRemove.Next;
            linkedListNodeToRemove.Next.Previous = linkedListNodeToRemove.Previous;

            // if this is head, we need to update the head reference
            if (head == linkedListNodeToRemove)
                head = linkedListNodeToRemove.Next;
            else if (tail == linkedListNodeToRemove)
                tail = tail.Previous;

            --count;
            return true;
        }

        /// <summary>
        ///     Removes all occurrences of the supplied item
        /// </summary>
        /// <param name="item">Item to be removed</param>
        public void RemoveAll(T item)
        {
            var removed = false;
            do
            {
                removed = Remove(item);
            } while (removed);
        }

        /// <summary>
        ///     Removes head
        /// </summary>
        /// <returns>TRUE if successfully removed, else FALSE</returns>
        public bool RemoveHead()
        {
            return RemoveNode(head);
        }

        /// <summary>
        ///     Removes tail
        /// </summary>
        /// <returns>TRUE if successfully removed, else FALSE</returns>
        public bool RemoveTail()
        {
            return RemoveNode(tail);
        }

        private CircularLinkedListNode<T> FindNode(CircularLinkedListNode<T> linkedListNode, T valueToCompare)
        {
            CircularLinkedListNode<T> result = null;
            if ((linkedListNode != null) && comparer.Equals(linkedListNode.Value, valueToCompare))
                result = linkedListNode;
            else if ((linkedListNode != null) && (result == null) && (linkedListNode.Next != head))
                result = FindNode(linkedListNode.Next, valueToCompare);
            return result;
        }

        /// <summary>
        ///     Gets a reverse enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetReverseEnumerator()
        {
            var current = tail;
            if (current != null)
                do
                {
                    yield return current.Value;
                    current = current.Previous;
                } while (current != tail);
        }
    }
}