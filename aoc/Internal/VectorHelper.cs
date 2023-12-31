﻿using System;
using System.Text.RegularExpressions;

namespace aoc.Internal
{
    abstract class VectorHelperStrategy<TSelf> : HelperStrategy<TSelf>
        where TSelf : VectorHelperStrategy<TSelf>
    {
        protected VectorHelperStrategy(params string[] formatKeys)
            : base(formatKeys)
        {
        }

        public override char DefaultSeparator => ',';

    }

    interface IVectorHelper<TVector>
        where TVector : struct
    {
        bool TryParse(string s, out TVector vector);
        bool TryParse(string s, char separator, out TVector vector);
        bool TryParse(string s, string separator, out TVector vector);
        bool TryParse(string s, Regex separator, out TVector vector);
    }

    abstract class VectorHelper<TVector, T, TStrategy> : Helper<TVector, T, TStrategy>, IVectorHelper<TVector>
        where TVector : struct, IVector<TVector, T>
        where T : struct, IFormattable
        where TStrategy : VectorHelperStrategy<TStrategy>
    {
        protected VectorHelper(Func<T[], TVector> fromArray, TryParse<T> tryParse, T minusOne, T zero, T one)
            : base(fromArray, tryParse)
        {
            InitHeadings(minusOne, zero, one);
        }

        public new TVector FromArray(params T[] values) =>
            base.FromArray(values);

        protected abstract void InitHeadings(T minusOne, T zero, T one);
    }

    sealed class Vector2DHelperStrategy : VectorHelperStrategy<Vector2DHelperStrategy>
    {
        private Vector2DHelperStrategy()
            : base("x", "y")
        {
        }
    }

    sealed class Vector2DHelper<TVector, T> : VectorHelper<TVector, T, Vector2DHelperStrategy>
        where TVector : struct, IVector2D<TVector, T>
        where T : struct, IFormattable
    {
        public Vector2DHelper(Func<T[], TVector> fromArray, TryParse<T> tryParse, T minusOne, T zero, T one)
            : base(fromArray, tryParse, minusOne, zero, one)
        {
        }

        protected override void InitHeadings(T minusOne, T zero, T one)
        {
            North = FromArray(zero,     minusOne, zero);
            East  = FromArray(one,      zero,     zero);
            South = FromArray(zero,     one,      zero);
            West  = FromArray(minusOne, zero,     zero);
        }

        protected override Vector2DHelperStrategy Strategy => Vector2DHelperStrategy.Instance;

        public TVector North { get; private set; }
        public TVector East  { get; private set; }
        public TVector South { get; private set; }
        public TVector West  { get; private set; }
    }

    sealed class Vector3DHelperStrategy : VectorHelperStrategy<Vector3DHelperStrategy>
    {
        private Vector3DHelperStrategy()
            : base("x", "y", "z")
        {
        }
    }

    sealed class Vector3DHelper<TVector, T> : VectorHelper<TVector, T, Vector3DHelperStrategy>
        where TVector : struct, IVector3D<TVector, T>
        where T : struct, IFormattable
    {
        public Vector3DHelper(Func<T[], TVector> fromArray, TryParse<T> tryParse, T minusOne, T zero, T one)
            : base(fromArray, tryParse, minusOne, zero, one)
        {
        }

        protected override void InitHeadings(T minusOne, T zero, T one)
        {
            North = FromArray(zero,     minusOne, zero);
            East  = FromArray(one,      zero,     zero);
            South = FromArray(zero,     one,      zero);
            West  = FromArray(minusOne, zero,     zero);
            Up    = FromArray(zero,     zero,     minusOne);
            Down  = FromArray(zero,     zero,     one);
        }

        protected override Vector3DHelperStrategy Strategy => Vector3DHelperStrategy.Instance;

        public TVector North { get; private set; }
        public TVector East  { get; private set; }
        public TVector South { get; private set; }
        public TVector West  { get; private set; }
        public TVector Up    { get; private set; }
        public TVector Down  { get; private set; }
    }

    sealed class Vector4DHelperStrategy : VectorHelperStrategy<Vector4DHelperStrategy>
    {
        private Vector4DHelperStrategy()
            : base("x", "y", "z", "w")
        {
        }
    }

    sealed class Vector4DHelper<TVector, T> : VectorHelper<TVector, T, Vector4DHelperStrategy>
        where TVector : struct, IVector4D<TVector, T>
        where T : struct, IFormattable
    {
        public Vector4DHelper(Func<T[], TVector> fromArray, TryParse<T> tryParse, T minusOne, T zero, T one)
            : base(fromArray, tryParse, minusOne, zero, one)
        {
        }

        protected override void InitHeadings(T minusOne, T zero, T one)
        {
            North = FromArray(zero,     minusOne, zero,     zero);
            East  = FromArray(one,      zero,     zero,     zero);
            South = FromArray(zero,     one,      zero,     zero);
            West  = FromArray(minusOne, zero,     zero,     zero);
            Up    = FromArray(zero,     zero,     minusOne, zero);
            Down  = FromArray(zero,     zero,     one,      zero);
            Ana   = FromArray(zero,     zero,     zero,     minusOne);
            Kata  = FromArray(zero,     zero,     zero,     one);
        }

        protected override Vector4DHelperStrategy Strategy => Vector4DHelperStrategy.Instance;

        public TVector North { get; private set; }
        public TVector East  { get; private set; }
        public TVector South { get; private set; }
        public TVector West  { get; private set; }
        public TVector Up    { get; private set; }
        public TVector Down  { get; private set; }
        public TVector Ana   { get; private set; }
        public TVector Kata  { get; private set; }
    }
}
