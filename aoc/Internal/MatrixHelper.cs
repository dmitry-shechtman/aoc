﻿using System;
using System.Linq;

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

    abstract class MatrixHelper<TMatrix, TVector, T, TStrategy, TVectorHelper> : Helper2<TMatrix, TVector, TStrategy, TVectorHelper>
        where TMatrix : struct, IMatrix<TMatrix, TVector, T>
        where TVector : struct, IVector<TVector, TMatrix, T>
        where T : struct, IFormattable
        where TStrategy : MatrixHelperStrategy<TStrategy, TMatrix, TVector, T>
        where TVectorHelper : IVectorHelper<TVector>
    {
        protected MatrixHelper(FromArray<TMatrix, TVector> fromRowArray, TVectorHelper vector)
            : base(fromRowArray, vector)
        {
            Vector = vector;
        }

        protected TMatrix FromRowArray(params TVector[] vectors) =>
            FromArray(vectors);

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
        where TMatrix : struct, IMatrix2D<TMatrix, TVector, T>
        where TVector : struct, IVector<TVector, TMatrix, T>, IVector2D<TVector, T>
        where T : struct, IFormattable
    {
        public Matrix2DHelper(FromArray<TMatrix, TVector> fromRowArray, Vector2DHelper<TVector, T> vector)
            : base(fromRowArray, vector)
        {
            Identity         = FromRowArray(Vector.East,  Vector.South);
            RotateRight      = FromRowArray(Vector.South, Vector.West);
            RotateLeft       = FromRowArray(Vector.North, Vector.East);
            MirrorHorizontal = FromRowArray(Vector.East,  Vector.North);
            MirrorVertical   = FromRowArray(Vector.West,  Vector.South);
            Flip             = FromRowArray(Vector.West,  Vector.North);
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
            FromRowArray(Vector.East, Vector.South, v);

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
        where TMatrix : struct, IMatrix3D<TMatrix, TVector, T>
        where TVector : struct, IVector<TVector, TMatrix, T>, IVector3D<TVector, T>
        where T : struct, IFormattable
    {
        public Matrix3DHelper(FromArray<TMatrix, TVector> fromRowArray, Vector3DHelper<TVector, T> vector)
            : base(fromRowArray, vector)
        {
            Identity = FromRowArray(Vector.East, Vector.South, Vector.Down);
        }

        public TMatrix Identity { get; }

        public TMatrix Translate(T x, T y, T z) =>
            Translate(Vector.FromArray(x, y, z));

        public TMatrix Translate(TVector v) =>
            FromRowArray(Vector.East, Vector.South, Vector.Down, v);

        protected override Matrix3DHelperStrategy<TMatrix, TVector, T> Strategy =>
            Matrix3DHelperStrategy<TMatrix, TVector, T>.Instance;
    }
}
