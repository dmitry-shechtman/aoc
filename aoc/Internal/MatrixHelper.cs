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

    abstract class MatrixHelper<TMatrix, TVector, T, TStrategy, TVectorHelper> : Helper2<TMatrix, TVector, T, TStrategy, TVectorHelper>
        where TMatrix : unmanaged, IMatrix<TMatrix, TVector, T>
        where TVector : unmanaged, IVector<TVector, TMatrix, T>
        where T : unmanaged, IFormattable
        where TStrategy : MatrixHelperStrategy<TStrategy, TMatrix, TVector, T>
        where TVectorHelper : IVectorHelper<TVector, T>
    {
        protected MatrixHelper(FromSpan<TMatrix, TVector> fromRows, FromSpan<TMatrix, TVector> fromColumns, TVectorHelper vector)
            : base(fromRows, vector)
        {
            FromColumnSpan = fromColumns;
        }

        public TMatrix FromRows(T[][] rows) =>
            FromSpan(FromArrays(rows));

        public TMatrix FromRows(ReadOnlySpan<T> values, int size = 0) =>
            FromItems(values, values.Length, size > 0 ? size : MinCount, FromSpan);

        public TMatrix FromColumns(T[][] columns) =>
            FromColumnSpan(FromArrays(columns));

        public TMatrix FromColumns(ReadOnlySpan<T> values, int size = 0) =>
            FromItems(values, values.Length, size > 0 ? size : MinCount, FromColumnSpan);

        public TMatrix ParseRowsAny(string input, IFormatProvider provider) =>
            TryParseRowsAny(input, provider, out TMatrix matrix)
                ? matrix
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParseRowsAny(string input, out TMatrix matrix) =>
            TryParseRowsAny(input, null, out matrix);

        public bool TryParseRowsAny(string input, IFormatProvider provider, out TMatrix matrix) =>
            TryParseAny(input, provider, FromSpan, out matrix);

        public TMatrix[] ParseRowsAll(string input, IFormatProvider provider, int rowCount, int columnCount) =>
            TryParseRowsAll(input, provider, rowCount, columnCount, out TMatrix[] matrices)
                ? matrices
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParseRowsAll(string input, out TMatrix[] matrices) =>
            TryParseRowsAll(input, null, out matrices);

        public bool TryParseRowsAll(string input, IFormatProvider provider, out TMatrix[] matrices) =>
            TryParseRowsAll(input, provider, MinCount, out matrices);

        public bool TryParseRowsAll(string input, IFormatProvider provider, int rowCount, out TMatrix[] matrices) =>
            TryParseRowsAll(input, provider, rowCount, MinCount, out matrices);

        public bool TryParseRowsAll(string input, IFormatProvider provider, int rowCount, int columnCount, out TMatrix[] matrices) =>
            TryParseAll(input, provider, rowCount, columnCount, FromSpan, out matrices);

        public TMatrix ParseColumnsAny(string input, IFormatProvider provider) =>
            TryParseColumnsAny(input, provider, out TMatrix matrix)
                ? matrix
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParseColumnsAny(string input, out TMatrix matrix) =>
            TryParseColumnsAny(input, null, out matrix);

        public bool TryParseColumnsAny(string input, IFormatProvider provider, out TMatrix matrix) =>
            TryParseAny(input, provider, FromColumnSpan, out matrix);

        public TMatrix[] ParseColumnsAll(string input, IFormatProvider provider, int columnCount, int rowCount) =>
            TryParseColumnsAll(input, provider, columnCount, rowCount, out TMatrix[] matrices)
                ? matrices
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParseColumnsAll(string input, out TMatrix[] matrices) =>
            TryParseColumnsAll(input, null, out matrices);

        public bool TryParseColumnsAll(string input, IFormatProvider provider, out TMatrix[] matrices) =>
            TryParseColumnsAll(input, provider, MinCount, out matrices);

        public bool TryParseColumnsAll(string input, IFormatProvider provider, int columnCount, out TMatrix[] matrices) =>
            TryParseColumnsAll(input, provider, columnCount, MinCount, out matrices);

        public bool TryParseColumnsAll(string input, IFormatProvider provider, int columnCount, int rowCount, out TMatrix[] matrices) =>
            TryParseAll(input, provider, columnCount, rowCount, FromColumnSpan, out matrices);

        protected TMatrix FromRows(params TVector[] vectors) =>
            FromSpan(vectors);

        private TVector[] FromArrays(T[][] values)
        {
            var vectors = new TVector[values.Length];
            for (int i = 0; i < vectors.Length; i++)
                vectors[i] = Vector.FromSpan(values[i]);
            return vectors;
        }

        private FromSpan<TMatrix, TVector> FromColumnSpan { get; }
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
        public Matrix2DHelper(FromSpan<TMatrix, TVector> fromRows, FromSpan<TMatrix, TVector> fromColumns, Vector2DHelper<TVector, T> vector)
            : base(fromRows, fromColumns, vector)
        {
            Identity         = FromRows(Vector.East,  Vector.South);
            RotateRight      = FromRows(Vector.South, Vector.West);
            RotateLeft       = FromRows(Vector.North, Vector.East);
            MirrorHorizontal = FromRows(Vector.East,  Vector.North);
            MirrorVertical   = FromRows(Vector.West,  Vector.South);
            Flip             = FromRows(Vector.West,  Vector.North);
        }

        public TMatrix Identity         { get; }
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
            0           => Identity,
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

        protected override Matrix2DHelperStrategy<TMatrix, TVector, T> Strategy =>
            Matrix2DHelperStrategy<TMatrix, TVector, T>.Instance;
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
        public Matrix3DHelper(FromSpan<TMatrix, TVector> fromRows, FromSpan<TMatrix, TVector> fromColumns, Vector3DHelper<TVector, T> vector)
            : base(fromRows, fromColumns, vector)
        {
            Identity = FromRows(Vector.East, Vector.South, Vector.Down);
        }

        public TMatrix Identity { get; }

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

        protected override Matrix3DHelperStrategy<TMatrix, TVector, T> Strategy =>
            Matrix3DHelperStrategy<TMatrix, TVector, T>.Instance;
    }
}
