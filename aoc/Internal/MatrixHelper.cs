using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc.Internal
{
    abstract class MatrixHelperStrategy<TSelf, TMatrix, TVector, T> : HelperStrategy<TSelf, TMatrix, TVector>
        where TSelf : MatrixHelperStrategy<TSelf, TMatrix, TVector, T>
        where TMatrix : struct, IMatrix<TMatrix, TVector, T>
        where TVector : struct, IVector<TVector, TMatrix, T>
        where T : struct, IFormattable
    {
        protected MatrixHelperStrategy(int cardinality)
            : base(Enumerable.Range(1, cardinality + 1).Select(i => $"r{i}").ToArray())
        {
            MaxCount = cardinality + 1;
        }

        public override int MinCount => 2;
        public override int MaxCount { get; }

        public override char DefaultSeparator => ';';

        protected override string SeparatorString =>
            $"{DefaultSeparator} ";

        public sealed override bool TryGetItem(TMatrix matrix, string format, IFormatProvider provider, ref int i, out IFormattable item) => (item = format[i] switch
        {
            'r' => matrix.GetRow(format[++i] - '1'),
            'c' => matrix.GetColumn(format[++i] - '1'),
            'm' => matrix[format[++i] - '1'][format[++i] - '1'],
            _ => null,
        }) is not null;
    }

    sealed class MatrixBuilder<TMatrix, TVector, T, TStrategy, TVectorHelper> : Helper2<TMatrix, TVector, T, TStrategy, TVectorHelper>, IMatrixBuilder<TMatrix, TVector, T>
        where TMatrix : unmanaged, IMatrix<TMatrix, TVector, T>
        where TVector : unmanaged, IVector<TVector, TMatrix, T>
        where T : unmanaged, IFormattable
        where TStrategy : MatrixHelperStrategy<TStrategy, TMatrix, TVector, T>
        where TVectorHelper : IVectorHelper<TVector, T>
    {
        public MatrixBuilder(TStrategy strategy,
            FromSpan<TMatrix, TVector> fromSpan, TVectorHelper vector)
                : base(strategy, fromSpan, vector)
        {
        }

        public TMatrix FromVectors(TVector[] vectors) =>
            FromSpan(vectors);

        public TMatrix FromVectors(ReadOnlySpan<TVector> vectors) =>
            FromSpan(vectors);

        public TMatrix FromVectors(T[][] values)
        {
            Span<TVector> vectors = stackalloc TVector[values.Length];
            for (int i = 0; i < vectors.Length; i++)
                vectors[i] = Vector.FromSpan(values[i]);
            return FromSpan(vectors);
        }

        public TMatrix FromElements(T[] values) =>
            FromElements(values.AsSpan());

        public TMatrix FromElements(ReadOnlySpan<T> values) =>
            FromItems(values, values.Length, MinCount);

        public TMatrix FromElements(ReadOnlySpan<T> values, int size) =>
            FromItems(values, values.Length, size > 0 ? size : MinCount);
    }

    abstract class MatrixHelper<TMatrix, TVector, T, TStrategy, TVectorHelper> : Helper2<TMatrix, TVector, T, TStrategy, TVectorHelper>
        where TMatrix : unmanaged, IMatrix<TMatrix, TVector, T>
        where TVector : unmanaged, IVector<TVector, TMatrix, T>
        where T : unmanaged, IFormattable
        where TStrategy : MatrixHelperStrategy<TStrategy, TMatrix, TVector, T>
        where TVectorHelper : IVectorHelper<TVector, T>
    {
        protected MatrixHelper(TStrategy strategy,
            FromSpan<TMatrix, TVector> fromRows, FromSpan<TMatrix, TVector> fromColumns, TVectorHelper vector,
            TMatrix pOne, TMatrix nOne)
                : base(strategy, fromRows, vector)
        {
            One = pOne;
            NegativeOne = nOne;
            Rows = new(strategy, fromRows, vector);
            Columns = new(strategy, fromColumns, vector);
        }

        protected TMatrix FromRows(params TVector[] vectors) =>
            FromSpan(vectors);

        public TMatrix One         { get; }
        public TMatrix NegativeOne { get; }

        public MatrixBuilder<TMatrix, TVector, T, TStrategy, TVectorHelper> Rows    { get; }
        public MatrixBuilder<TMatrix, TVector, T, TStrategy, TVectorHelper> Columns { get; }
    }

    sealed class Matrix2DHelperStrategy<TMatrix, TVector, T> : MatrixHelperStrategy<Matrix2DHelperStrategy<TMatrix, TVector, T>, TMatrix, TVector, T>
        where TMatrix : struct, IMatrix2D<TMatrix, TVector, T>
        where TVector : struct, IVector<TVector, TMatrix, T>, IVector2D<TVector, T>
        where T : struct, IFormattable
    {
        private Matrix2DHelperStrategy()
            : base(2)
        {
        }
    }

    sealed class Matrix2DHelper<TMatrix, TVector, T> :
            MatrixHelper<TMatrix, TVector, T, Matrix2DHelperStrategy<TMatrix, TVector, T>, Vector2DHelper<TVector, T>>
        where TMatrix : unmanaged, IMatrix2D<TMatrix, TVector, T>
        where TVector : unmanaged, IVector<TVector, TMatrix, T>, IVector2D<TVector, T>
        where T : unmanaged, IFormattable
    {
        public Matrix2DHelper(FromSpan<TMatrix, TVector> fromRows, FromSpan<TMatrix, TVector> fromColumns, Vector2DHelper<TVector, T> TVector)
            : base(Matrix2DHelperStrategy<TMatrix, TVector, T>.Instance,
                  fromRows, fromColumns, TVector,
                  pOne: fromRows(new[] { TVector.East, TVector.South }),
                  nOne: fromRows(new[] { TVector.West, TVector.North }))
        {
            RotateRight      = FromRows(TVector.South, TVector.West);
            RotateLeft       = FromRows(TVector.North, TVector.East);
            MirrorHorizontal = FromRows(TVector.East,  TVector.North);
            MirrorVertical   = FromRows(TVector.West,  TVector.South);
            Flip             = FromRows(TVector.West,  TVector.North);
        }

        public TMatrix RotateRight      { get; }
        public TMatrix RotateLeft       { get; }
        public TMatrix MirrorHorizontal { get; }
        public TMatrix MirrorVertical   { get; }
        public TMatrix Flip             { get; }

        public TMatrix Translate(T x, T y) =>
            Translate(Vector.FromArray(x, y));

        public TMatrix Translate(TVector v) =>
            FromRows(Vector.East, Vector.South, v);

        public TMatrix Rotate(int degrees) => degrees switch
        {
            0           => One,
            90  or -270 => RotateRight,
            180 or -180 => Flip,
            270 or  -90 => RotateLeft,
            _ => throw new(),
        };

        protected override int GetChunkSize(int count) => count switch
        {
            2 * 2 => 2,
            2 * 3 => 2,
            3 * 3 => 3,
            _ => 0
        };
    }

    sealed class Matrix3DHelperStrategy<TMatrix, TVector, T> : MatrixHelperStrategy<Matrix3DHelperStrategy<TMatrix, TVector, T>, TMatrix, TVector, T>
        where TMatrix : struct, IMatrix3D<TMatrix, TVector, T>
        where TVector : struct, IVector<TVector, TMatrix, T>, IVector3D<TVector, T>
        where T : struct, IFormattable
    {
        private Matrix3DHelperStrategy()
            : base(3)
        {
        }
    }

    sealed class Matrix3DHelper<TMatrix, TVector, T> :
            MatrixHelper<TMatrix, TVector, T, Matrix3DHelperStrategy<TMatrix, TVector, T>, Vector3DHelper<TVector, T>>
        where TMatrix : unmanaged, IMatrix3D<TMatrix, TVector, T>
        where TVector : unmanaged, IVector<TVector, TMatrix, T>, IVector3D<TVector, T>
        where T : unmanaged, IFormattable
    {
        public Matrix3DHelper(FromSpan<TMatrix, TVector> fromRows, FromSpan<TMatrix, TVector> fromColumns, Vector3DHelper<TVector, T> TVector)
            : base(Matrix3DHelperStrategy<TMatrix, TVector, T>.Instance,
                  fromRows, fromColumns, TVector,
                  pOne: fromRows(new[] { TVector.East, TVector.South, TVector.Down }),
                  nOne: fromRows(new[] { TVector.West, TVector.North, TVector.Up }))
        {
        }

        public TMatrix Translate(T x, T y, T z) =>
            Translate(Vector.FromArray(x, y, z));

        public TMatrix Translate(TVector v) =>
            FromRows(Vector.East, Vector.South, Vector.Down, v);

        protected override int GetChunkSize(int count) => count switch
        {
            2 * 3 => 3,
            3 * 3 => 3,
            3 * 4 => 3,
            4 * 4 => 4,
            _ => 0
        };
    }
}
