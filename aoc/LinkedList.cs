using System;
using System.Collections;
using System.Collections.Generic;

namespace aoc
{
    public sealed class LinkedList<T> : IEnumerable<T>
    {
        public LinkedListNode<T> First { get; private set; }
        public LinkedListNode<T> Last  { get; private set; }

        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
        {
            if (node is null)
                return AddLast(value);
            node = new(value, node.Previous, node);
            if (node.Previous is null)
                First = node;
            return node;
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
            return node;
        }

        public LinkedListNode<T> AddAfter(Predicate<T> match, T value) =>
            AddAfter(Find(match), value);

        public LinkedListNode<T> AddFirst(T value)
        {
            First = new(value, null, First);
            if (First.Next is null)
                Last = First;
            return First;
        }

        public T RemoveFirst()
        {
            var node = First;
            if (node is null)
                throw new InvalidOperationException();
            node.Remove();
            if ((First = node.Next) is null)
                Last = null;
            return node.Value;
        }

        public LinkedListNode<T> AddLast(T value)
        {
            Last = new(value, Last, null);
            if (Last.Previous is null)
                First = Last;
            return Last;
        }

        public T RemoveLast()
        {
            var node = Last;
            if (node is null)
                throw new InvalidOperationException();
            node.Remove();
            if ((Last = node.Previous) is null)
                First = null;
            return node.Value;
        }

        public void Remove(LinkedListNode<T> node)
        {
            node.Remove();
            if (node.Previous is null)
                First = node.Next;
            if (node.Next is null)
                Last = node.Previous;
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
