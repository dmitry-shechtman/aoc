using System;
using System.Collections;
using System.Collections.Generic;

namespace aoc
{
    public interface IVector<T> : IReadOnlyList<T>, IFormattable
        where T : struct
    {
        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }

    public interface IVector<TSelf, T> : IVector<T>, IEquatable<TSelf>
        where TSelf : struct, IVector<TSelf, T>
        where T : struct
    {
        T Abs();
        TSelf Abs2();
        TSelf Sign();
        TSelf Neg();
        TSelf Add(TSelf other);
        TSelf Sub(TSelf other);
        TSelf Mul(T scalar);
        TSelf Div(T scalar);
        T Dot(TSelf other);
    }

    public interface IVector2D<TSelf, T> : IVector<TSelf, T>
        where TSelf : struct, IVector2D<TSelf, T>
        where T : struct
    {
        internal const int Cardinality = 2;

        T X { get; }
        T Y { get; }

        void Deconstruct(out T x, out T y);

        int IReadOnlyCollection<T>.Count =>
            Cardinality;
    }

    public interface IVector3D<TSelf, T> : IVector<TSelf, T>
        where TSelf : struct, IVector3D<TSelf, T>
        where T : struct
    {
        internal const int Cardinality = 3;

        T X { get; }
        T Y { get; }
        T Z { get; }

        void Deconstruct(out T x, out T y, out T z);

        int IReadOnlyCollection<T>.Count =>
            Cardinality;
    }

    public interface IVector3D<TSelf, TVector2D, T> : IVector3D<TSelf, T>
        where TSelf : struct, IVector3D<TSelf, TVector2D, T>
        where TVector2D : struct, IVector2D<TVector2D, T>
        where T : struct
    {
        void Deconstruct(out TVector2D vector, out T z);
    }
}
