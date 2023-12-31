﻿namespace aoc
{
    public sealed class LinkedListNode<T>
    {
        public LinkedListNode(T value, LinkedListNode<T> previous, LinkedListNode<T> next)
        {
            Value = value;
            if ((Previous = previous) is not null)
                Previous.Next = this;
            if ((Next = next) is not null)
                Next.Previous = this;
        }

        public LinkedListNode(T value)
        {
            Value = value;
            Previous = this;
            Next = this;
        }

        public T Value { get; set; }
        public LinkedListNode<T> Previous { get; private set; }
        public LinkedListNode<T> Next     { get; private set; }

        public void Remove()
        {
            if (Previous is not null)
                Previous.Next = Next;
            if (Next is not null)
                Next.Previous = Previous;
        }

        public LinkedListNode<T> Skip(int count)
        {
            var node = this;
            for (int i = 0; i < count; i++)
                node = node.Next;
            return node;
        }

        public LinkedListNode<T> SkipBack(int count)
        {
            var node = this;
            for (int i = 0; i < count; i++)
                node = node.Previous;
            return node;
        }
    }
}
