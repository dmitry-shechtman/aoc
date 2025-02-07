﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace aoc
{
    public interface IVector<TSelf> : IEquatable<TSelf>, IFormattableEx
        where TSelf : IVector<TSelf>
    {
        TSelf Abs2();
        TSelf Sign();
        TSelf Neg();
        TSelf Add(TSelf other);
        TSelf Sub(TSelf other);
        TSelf Min(TSelf other);
        TSelf Max(TSelf other);
    }

    public interface IVector<TSelf, T> : IVector<TSelf>, IReadOnlyList<T>
        where TSelf : struct, IVector<TSelf, T>
        where T : struct
    {
        T Abs();
        TSelf Mul(T scalar);
        TSelf Div(T scalar);
        T Dot(TSelf other);

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }

    public interface IIntegerVector<TSelf, T> : IVector<TSelf, T>
        where TSelf : struct, IIntegerVector<TSelf, T>
        where T : struct
    {
        TSelf Mod(TSelf other);
    }

    public interface IVector<TSelf, TMatrix, T> : IVector<TSelf, T>
        where TSelf : struct, IVector<TSelf, TMatrix, T>
        where TMatrix : struct, IMatrix<TMatrix, TSelf, T>
        where T : struct
    {
        TSelf Mul(TMatrix matrix);
    }

    public interface IIntegerVector<TSelf, TMatrix, T> : IVector<TSelf, TMatrix, T>, IIntegerVector<TSelf, T>
        where TSelf : struct, IIntegerVector<TSelf, TMatrix, T>
        where TMatrix : struct, IIntegerMatrix<TMatrix, TSelf, T>
        where T : struct
    {
    }

    public interface IVector2D<TSelf, T> : IVector<TSelf, T>
        where TSelf : struct, IVector2D<TSelf, T>
        where T : struct
    {
        T X { get; }
        T Y { get; }

        void Deconstruct(out T x, out T y);

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            yield return X;
            yield return Y;
        }

        T IReadOnlyList<T>.this[int i] => i switch
        {
            0 => X,
            1 => Y,
            _ => throw new IndexOutOfRangeException(),
        };

        int IReadOnlyCollection<T>.Count => 2;
    }

    public interface IVector3D<TSelf, T> : IVector<TSelf, T>
        where TSelf : struct, IVector3D<TSelf, T>
        where T : struct
    {
        T X { get; }
        T Y { get; }
        T Z { get; }

        void Deconstruct(out T x, out T y, out T z);

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            yield return X;
            yield return Y;
            yield return Z;
        }

        T IReadOnlyList<T>.this[int i] => i switch
        {
            0 => X,
            1 => Y,
            2 => Z,
            _ => throw new IndexOutOfRangeException(),
        };

        int IReadOnlyCollection<T>.Count => 3;
    }

    public interface IVector3D<TSelf, TVector2D, T> : IVector3D<TSelf, T>
        where TSelf : struct, IVector3D<TSelf, TVector2D, T>
        where TVector2D : struct, IVector2D<TVector2D, T>
        where T : struct
    {
        void Deconstruct(out TVector2D vector, out T z);
    }

    public interface IVector4D<TSelf, T> : IVector<TSelf, T>
        where TSelf : struct, IVector4D<TSelf, T>
        where T : struct
    {
        T X { get; }
        T Y { get; }
        T Z { get; }
        T W { get; }

        void Deconstruct(out T x, out T y, out T z, out T w);

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            yield return X;
            yield return Y;
            yield return Z;
            yield return W;
        }

        T IReadOnlyList<T>.this[int i] => i switch
        {
            0 => X,
            1 => Y,
            2 => Z,
            3 => W,
            _ => throw new IndexOutOfRangeException(),
        };

        int IReadOnlyCollection<T>.Count => 4;
    }

    public interface IVector4D<TSelf, TVector3D, TVector2D, T> : IVector4D<TSelf, T>
        where TSelf : struct, IVector4D<TSelf, TVector3D, TVector2D, T>
        where TVector3D : struct, IVector3D<TVector3D, T>
        where TVector2D : struct, IVector2D<TVector2D, T>
        where T : struct
    {
        void Deconstruct(out TVector3D vector, out T w);
        void Deconstruct(out TVector2D vector, out T z, out T w);
    }
}
