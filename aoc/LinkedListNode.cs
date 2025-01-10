using System;

namespace aoc
{
    public sealed class LinkedListNode<T>
    {
        public LinkedListNode(T value, LinkedListNode<T>? previous, LinkedListNode<T>? next)
        {
            this.value = value;
            if ((Previous = previous) is not null)
                Previous.Next = this;
            if ((Next = next) is not null)
                Next.Previous = this;
        }

        public LinkedListNode(T value)
        {
            this.value = value;
            Previous = this;
            Next = this;
        }

        private T value;
        public T Value
        {
            get => value;
            set => this.value = value;
        }

        public ref T ValueRef => ref value;
        
        public LinkedListNode<T>? Previous { get; internal set; }
        public LinkedListNode<T>? Next     { get; internal set; }

        public void Remove()
        {
            if (Previous is not null)
                Previous.Next = Next;
            if (Next is not null)
                Next.Previous = Previous;
        }

        public LinkedListNode<T>? Skip(int count)
        {
            var node = this;
            for (int i = 0; node is not null && i < count; i++)
                node = node.Next
                    ?? throw new ArgumentOutOfRangeException(nameof(count));
            return node;
        }

        public LinkedListNode<T>? SkipBack(int count)
        {
            var node = this;
            for (int i = 0; node is not null && i < count; i++)
                node = node.Previous
                    ?? throw new ArgumentOutOfRangeException(nameof(count));
            return node;
        }
    }
}
