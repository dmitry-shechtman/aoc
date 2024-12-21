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
            FromColumnArray = fromColumns;
            Vector = vector;
        }

        public TMatrix FromRows(T[][] rows) =>
            FromRows(FromArrays(rows));

        public TMatrix FromRows(ReadOnlySpan<T> values, int? chunkSize = null) =>
            FromRows(Chunk(values, chunkSize));

        public TMatrix FromColumns(T[][] columns) =>
            FromColumnArray(FromArrays(columns));

        public TMatrix FromColumns(ReadOnlySpan<T> values, int? chunkSize = null) =>
            FromColumnArray(Chunk(values, chunkSize));

        public TMatrix ParseRowsAny(string input) =>
            TryParseRowsAny(input, out TMatrix matrix)
                ? matrix
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParseRowsAny(string input, out TMatrix matrix)
        {
            matrix = default;
            if (!TryGetMatches(input, out MatchCollection matches))
                return false;
            Span<T> values = stackalloc T[matches.Count];
            if (!TryParse(matches, values))
                return false;
            matrix = FromRows(values);
            return true;
        }

        public TMatrix ParseColumnsAny(string input) =>
            TryParseColumnsAny(input, out TMatrix matrix)
                ? matrix
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParseColumnsAny(string input, out TMatrix matrix)
        {
            matrix = default;
            if (!TryGetMatches(input, out var matches))
                return false;
            Span<T> values = stackalloc T[matches.Count];
            if (!TryParse(matches, values))
                return false;
            matrix = FromColumns(values);
            return true;
        }

        private bool TryGetMatches(string input, out MatchCollection matches)
        {
            matches = Vector.GetMatches(input);
            return matches.Count == MinCount * MinCount
                || matches.Count == MinCount * MaxCount
                || matches.Count == MaxCount * MaxCount;
        }

        private bool TryParse(MatchCollection matches, Span<T> values)
        {
            for (int i = 0; i < matches.Count; i++)
                if (!Vector.TryParseItem(matches[i].Value, out values[i]))
                    return false;
            return true;
        }

        protected TMatrix FromRows(params TVector[] vectors) =>
            FromSpan(vectors);

        private TVector[] FromArrays(T[][] values)
        {
            var vectors = new TVector[values.Length];
            for (int i = 0; i < vectors.Length; i++)
                vectors[i] = Vector.FromSpan(values[i]);
            return vectors;
        }

        private TVector[] Chunk(ReadOnlySpan<T> values, int? chunkSize)
        {
            var size = chunkSize ?? MinCount;
            var vectors = new TVector[values.Length / size];
            for (int i = 0; i < vectors.Length; ++i)
                vectors[i] = Vector.FromSpan(values[(i * size)..((i + 1) * size)]);
            return vectors;
        }

        private FromSpan<TMatrix, TVector> FromColumnArray { get; }

        protected TVectorHelper Vector { get; }
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

        protected override Matrix3DHelperStrategy<TMatrix, TVector, T> Strategy =>
            Matrix3DHelperStrategy<TMatrix, TVector, T>.Instance;
    }
}
