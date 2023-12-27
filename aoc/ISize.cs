using System;
using System.Collections.Generic;

namespace aoc
{
    public interface ISize<T>
        where T : struct
    {
        bool Contains(T value);
    }

    public interface ISize<TSelf, T> : ISize<T>, IEquatable<TSelf>
        where TSelf : struct, ISize<TSelf, T>
        where T : struct
    {
    }

    public interface ISize<TSelf, TVector, T> : ISize<TSelf, TVector>, IReadOnlyList<T>
        where TSelf : struct, ISize<TSelf, TVector, T>
        where TVector : struct, IVector<TVector, T>
        where T : struct
    {
        T Length { get; }
    }

    public interface ISize2D<TSelf, T>
        where TSelf : struct, ISize2D<TSelf, T>
        where T : struct
    {
        protected const int Cardinality = 2;

        T Width   { get; }
        T Height  { get; }
    }

    public interface ISize2D<TSelf, TVector, T> : ISize<TSelf, TVector, T>, ISize2D<TSelf, T>
        where TSelf : struct, ISize2D<TSelf, TVector, T>
        where TVector : struct, IVector2D<TVector, T>
        where T : struct
    {
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            yield return Width;
            yield return Height;
        }

        T IReadOnlyList<T>.this[int i] => i switch
        {
            0 => Width,
            1 => Height,
            _ => throw new IndexOutOfRangeException()
        };

        int IReadOnlyCollection<T>.Count => Cardinality;
    }

    public interface ISize3D<TSelf, T>
        where TSelf : struct, ISize3D<TSelf, T>
        where T : struct
    {
        protected const int Cardinality = 3;

        T Width   { get; }
        T Height  { get; }
        T Depth   { get; }
    }

    public interface ISize3D<TSelf, TVector, T> : ISize<TSelf, TVector, T>, ISize3D<TSelf, T>
        where TSelf : struct, ISize3D<TSelf, TVector, T>
        where TVector : struct, IVector3D<TVector, T>
        where T : struct
    {
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            yield return Width;
            yield return Height;
            yield return Depth;
        }

        T IReadOnlyList<T>.this[int i] => i switch
        {
            0 => Width,
            1 => Height,
            2 => Depth,
            _ => throw new IndexOutOfRangeException()
        };

        int IReadOnlyCollection<T>.Count => Cardinality;
    }
}
