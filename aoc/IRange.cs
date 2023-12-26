using System.Collections;
using System.Collections.Generic;

namespace aoc
{
    public interface IRange<T> : ISize<T>
        where T : struct
    {
        internal const int Cardinality = 2;

        T Min { get; }
        T Max { get; }

        void Deconstruct(out T min, out T max);

        bool IsMatch(T value);
    }

    public interface IRange<TSelf, T> : IRange<T>, ISize<TSelf, T>
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

    public interface IRange2D<TSelf, TVector, T> : IRange<TSelf, TVector>, ISize2D<TSelf, TVector, T>
        where TSelf : struct, IRange2D<TSelf, TVector, T>
        where TVector : struct, IVector2D<TVector, T>
        where T : struct
    {
    }

    public interface IRange3D<TSelf, TVector, T> : IRange<TSelf, TVector>, ISize3D<TSelf, TVector, T>
        where TSelf : struct, IRange3D<TSelf, TVector, T>
        where TVector : struct, IVector3D<TVector, T>
        where T : struct
    {
    }
}
