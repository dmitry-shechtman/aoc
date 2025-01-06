using System;
using System.Collections;
using System.Collections.Generic;

namespace aoc
{
    public interface IMatrix<TSelf, TVector, T> : IEquatable<TSelf>, IReadOnlyList<TVector>, IFormattableEx
        where TSelf : struct, IMatrix<TSelf, TVector, T>
        where TVector : struct, IVector<TVector, TSelf, T>
        where T : struct
    {
        void Deconstruct(out TVector r1, out TVector r2);
        void Deconstruct(out TVector r1, out TVector r2, out TVector r3);

        int Cardinality { get; }

        TVector GetRow(int index);
        TVector GetColumn(int index);
        T GetElement(int row, int column);

        IEnumerable<TVector> EnumerateRows();
        IEnumerable<TVector> EnumerateColumns();

        TVector Mul(TVector vector);

        TVector IReadOnlyList<TVector>.this[int index] =>
            GetRow(index);

        int IReadOnlyCollection<TVector>.Count =>
            Cardinality + 1;

        IEnumerator<TVector> GetRowEnumerator() =>
            EnumerateRows().GetEnumerator();

        IEnumerator<TVector> GetColumnEnumerator() =>
            EnumerateColumns().GetEnumerator();

        IEnumerator<TVector> IEnumerable<TVector>.GetEnumerator() =>
            GetRowEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetRowEnumerator();
    }

    public interface IIntegerMatrix<TSelf, TVector, T>: IMatrix<TSelf, TVector, T>
        where TSelf : struct, IIntegerMatrix<TSelf, TVector, T>
        where TVector : struct, IIntegerVector<TVector, TSelf, T>
        where T : struct
    {
    }

    public interface IMatrix2D<TSelf, TVector, T> : IMatrix<TSelf, TVector, T>
        where TSelf : struct, IMatrix2D<TSelf, TVector, T>
        where TVector : struct, IVector<TVector, TSelf, T>, IVector2D<TVector, T>
        where T : struct
    {
        int IMatrix<TSelf, TVector, T>.Cardinality => 2;

        TVector R1 { get; }
        TVector R2 { get; }
        TVector R3 { get; }

        TVector C1 { get; }
        TVector C2 { get; }
        TVector C3 { get; }

        TVector IMatrix<TSelf, TVector, T>.GetRow(int index) => index switch
        {
            0 => R1,
            1 => R2,
            2 => R3,
            _ => throw new IndexOutOfRangeException()
        };

        TVector IMatrix<TSelf, TVector, T>.GetColumn(int index) => index switch
        {
            0 => C1,
            1 => C2,
            2 => C3,
            _ => throw new IndexOutOfRangeException()
        };

        T IMatrix<TSelf, TVector, T>.GetElement(int row, int column) =>
            this[row][column];

        IEnumerable<TVector> IMatrix<TSelf, TVector, T>.EnumerateRows()
        {
            yield return R1;
            yield return R2;
            yield return R3;
        }

        IEnumerable<TVector> IMatrix<TSelf, TVector, T>.EnumerateColumns()
        {
            yield return C1;
            yield return C2;
            yield return C3;
        }
    }

    public interface IMatrix3D<TSelf, TVector, T> : IMatrix<TSelf, TVector, T>
        where TSelf : struct, IMatrix3D<TSelf, TVector, T>
        where TVector : struct, IVector<TVector, TSelf, T>, IVector3D<TVector, T>
        where T : struct
    {
        int IMatrix<TSelf, TVector, T>.Cardinality => 3;

        TVector R1 { get; }
        TVector R2 { get; }
        TVector R3 { get; }
        TVector R4 { get; }

        TVector C1 { get; }
        TVector C2 { get; }
        TVector C3 { get; }
        TVector C4 { get; }

        void Deconstruct(out TVector r1, out TVector r2, out TVector r3, out TVector r4);

        TVector IMatrix<TSelf, TVector, T>.GetRow(int index) => index switch
        {
            0 => R1,
            1 => R2,
            2 => R3,
            3 => R4,
            _ => throw new IndexOutOfRangeException()
        };

        TVector IMatrix<TSelf, TVector, T>.GetColumn(int index) => index switch
        {
            0 => C1,
            1 => C2,
            2 => C3,
            3 => C4,
            _ => throw new IndexOutOfRangeException()
        };

        T IMatrix<TSelf, TVector, T>.GetElement(int row, int column) =>
            this[row][column];

        IEnumerable<TVector> IMatrix<TSelf, TVector, T>.EnumerateRows()
        {
            yield return R1;
            yield return R2;
            yield return R3;
            yield return R4;
        }

        IEnumerable<TVector> IMatrix<TSelf, TVector, T>.EnumerateColumns()
        {
            yield return C1;
            yield return C2;
            yield return C3;
            yield return C4;
        }
    }
}
