using System;
using System.Collections;
using System.Collections.Generic;

namespace aoc
{
    public interface IRange<T>
        where T : struct
    {
        internal const int Cardinality = 2;

        T Min { get; }
        T Max { get; }

        void Deconstruct(out T min, out T max);

        bool IsMatch(T value);
        bool Contains(T value);
    }

    public interface IRange<TSelf, T> : IRange<T>, IEquatable<TSelf>
        where TSelf : struct, IRange<TSelf, T>
        where T : struct
    {
        bool IsMatch(TSelf other);
        bool Contains(TSelf other);
    }

    public interface IIntegerRange<TSelf, T> : IRange<TSelf, T>, IReadOnlyList<T>
        where TSelf : struct, IIntegerRange<TSelf, T>
        where T : struct
    {
        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
