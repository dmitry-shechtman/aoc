using System;
using System.Collections;
using System.Collections.Generic;

namespace aoc
{
    public sealed class LinkedList<T> : IEnumerable<T>
    {
        public LinkedListNode<T> First { get; private set; }
        public LinkedListNode<T> Last  { get; private set; }

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

        public LinkedListNode<T> Find(Predicate<T> match)
        {
            for (var node = First; node is not null; node = node.Next)
                if (match(node.Value))
                    return node;
            return null;
        }

        public LinkedListNode<T> FindLast(Predicate<T> match)
        {
            for (var node = Last; node is not null; node = node.Previous)
                if (match(node.Value))
                    return node;
            return null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var node = First; node is not null; node = node.Next)
                yield return node.Value;
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

    }
}
