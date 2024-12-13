using System;
using System.Collections;
using System.Collections.Generic;

namespace aoc
{
    public sealed class LinkedList<T> : ICollection<T>, IReadOnlyCollection<T>
    {
        public LinkedListNode<T> First { get; private set; }
        public LinkedListNode<T> Last  { get; private set; }
        public int Count { get; private set; }

        bool ICollection<T>.IsReadOnly => false;

        public LinkedList(params T[] values)
            : this((IEnumerable<T>)values)
        {
        }

        public LinkedList(IEnumerable<T> values)
        {
            if (values is null)
                throw new ArgumentNullException(nameof(values));
            LinkedListNode<T> last = null, node;
            foreach (var value in values)
            {
                node = new(value, last, null);
                if (last is null)
                    First = node;
                last = node;
                ++Count;
            }
            Last = last;
        }

        public void Clear()
        {
            First = null;
            Last = null;
            Count = 0;
        }

        void ICollection<T>.Add(T value) =>
            AddLast(value);

        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
        {
            if (node is null)
                return AddLast(value);
            node = new(value, node.Previous, node);
            if (node.Previous is null)
                First = node;
            ++Count;
            return node;
        }

        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            if (node is null)
                throw new ArgumentNullException(nameof(node));
            if (newNode is null)
                throw new ArgumentNullException(nameof(newNode));
            newNode.Next = node;
            if ((newNode.Previous = node.Previous) is null)
                First = newNode;
            else
                node.Previous.Next = newNode;
            node.Previous = newNode;
            ++Count;
        }

        public LinkedListNode<T> AddBefore(Predicate<T> match, T value) =>
            AddBefore(Find(match), value);

        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
        {
            if (node is null)
                return AddFirst(value);
            node = new(value, node, node.Next);
            if (node.Next is null)
                Last = node;
            ++Count;
            return node;
        }

        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            if (node is null)
                throw new ArgumentNullException(nameof(node));
            if (newNode is null)
                throw new ArgumentNullException(nameof(newNode));
            newNode.Previous = node;
            if ((newNode.Next = node.Next) is null)
                Last = newNode;
            else
                node.Next.Previous = newNode;
            node.Next = newNode;
            ++Count;
        }

        public LinkedListNode<T> AddAfter(Predicate<T> match, T value) =>
            AddAfter(Find(match), value);

        public LinkedListNode<T> AddFirst(T value)
        {
            First = new(value, null, First);
            if (First.Next is null)
                Last = First;
            ++Count;
            return First;
        }

        public void AddFirst(LinkedListNode<T> node)
        {
            if (node is null)
                throw new ArgumentNullException(nameof(node));
            node.Previous = null;
            if ((node.Next = First) is null)
                Last = node;
            else
                First.Previous = node;
            First = node;
        }

        public T RemoveFirst()
        {
            var node = First;
            if (node is null)
                throw new InvalidOperationException();
            node.Remove();
            if ((First = node.Next) is null)
                Last = null;
            --Count;
            return node.Value;
        }

        public LinkedListNode<T> AddLast(T value)
        {
            Last = new(value, Last, null);
            if (Last.Previous is null)
                First = Last;
            ++Count;
            return Last;
        }

        public void AddLast(LinkedListNode<T> node)
        {
            if (node is null)
                throw new ArgumentNullException(nameof(node));
            node.Next = null;
            if ((node.Previous = Last) is null)
                First = node;
            else
                Last.Next = node;
            Last = node;
        }

        public T RemoveLast()
        {
            var node = Last;
            if (node is null)
                throw new InvalidOperationException();
            node.Remove();
            if ((Last = node.Previous) is null)
                First = null;
            --Count;
            return node.Value;
        }

        public void Remove(LinkedListNode<T> node)
        {
            if (node is null)
                throw new ArgumentNullException(nameof(node));
            node.Remove();
            if (node.Previous is null)
                First = node.Next;
            if (node.Next is null)
                Last = node.Previous;
            --Count;
        }

        public bool Remove(T value)
        {
            for (var node = First; node is not null; node = node.Next)
            {
                if (Equals(node.Value, value))
                {
                    Remove(node);
                    return true;
                }
            }
            return false;
        }

        public bool Contains(T value)
        {
            for (var node = First; node is not null; node = node.Next)
                if (Equals(node.Value, value))
                    return true;
            return false;
        }

        public static LinkedListNode<T> Find(LinkedListNode<T> node, Predicate<T> match)
        {
            while (node is not null && !match(node.Value))
                node = node.Next;
            return node;
        }

        public LinkedListNode<T> Find(Predicate<T> match) =>
            Find(First, match);

        public static LinkedListNode<T> FindPrevious(LinkedListNode<T> node, Predicate<T> match)
        {
            while (node is not null && !match(node.Value))
                node = node.Previous;
            return node;
        }

        public LinkedListNode<T> FindLast(Predicate<T> match) =>
            FindPrevious(Last, match);

        public void CopyTo(T[] array, int index)
        {
            for (var node = First; node is not null; node = node.Next)
                array[index++] = node.Value;
        }

        public static IEnumerator<T> GetEnumerator(LinkedListNode<T> node)
        {
            for (; node is not null; node = node.Next)
                yield return node.Value;
        }

        public IEnumerator<T> GetEnumerator() =>
            GetEnumerator(First);

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
