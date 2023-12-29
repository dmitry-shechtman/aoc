using System;
using System.Collections;
using System.Collections.Generic;

namespace aoc
{
    public interface IRange<TSelf, T> : ISize<TSelf, T>, IReadOnlyList<T>
        where TSelf : struct, IRange<TSelf, T>
        where T : struct
    {
        private const int Cardinality = 2;

        T Min { get; }
        T Max { get; }

        void Deconstruct(out T min, out T max);

        bool IsMatch(T value);
        bool IsMatch(TSelf other);
        bool Contains(TSelf other);

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            yield return Min;
            yield return Max;
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        T IReadOnlyList<T>.this[int i] => i switch
        {
            0 => Min,
            1 => Max,
            _ => throw new IndexOutOfRangeException(),
        };

        int IReadOnlyCollection<T>.Count =>
            Cardinality;
    }

    public interface IRange<TSelf, TVector, T> : IRange<TSelf, TVector>
        where TSelf : struct, IRange<TSelf, TVector, T>
        where TVector : struct, IVector<TVector, T>
        where T : struct
    {
        T Length { get; }
    }

    public interface IIntegerRange<TSelf, T> : IRange<TSelf, T>, IReadOnlyList<T>
        where TSelf : struct, IIntegerRange<TSelf, T>
        where T : struct
    {
        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }

    public interface IRange2D<TSelf, TVector, T> : IRange<TSelf, TVector, T>, ISize2D<TSelf, T>
        where TSelf : struct, IRange2D<TSelf, TVector, T>
        where TVector : struct, IVector2D<TVector, T>
        where T : struct
    {
    }

    public interface IRange3D<TSelf, TVector, T> : IRange<TSelf, TVector, T>, ISize3D<TSelf, T>
        where TSelf : struct, IRange3D<TSelf, TVector, T>
        where TVector : struct, IVector3D<TVector, T>
        where T : struct
    {
    }

    public interface IRange4D<TSelf, TVector, T> : IRange<TSelf, TVector, T>, ISize4D<TSelf, T>
        where TSelf : struct, IRange4D<TSelf, TVector, T>
        where TVector : struct, IVector4D<TVector, T>
        where T : struct
    {
    }
}
