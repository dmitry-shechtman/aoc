﻿using System;
using System.Text.RegularExpressions;

namespace aoc.Internal
{
    abstract class VectorHelperStrategy<TSelf, TVector, T> : ListHelperStrategy<TSelf, TVector, T>
        where TVector : struct, IVector<TVector, T>
        where T : struct, IFormattable
        where TSelf : VectorHelperStrategy<TSelf, TVector, T>
    {
        protected VectorHelperStrategy(params string[] formatKeys)
            : base(formatKeys)
        {
        }

        public override char DefaultSeparator => ',';
    }

    interface IVectorHelper<TVector, T>
        where TVector : unmanaged, IVector<TVector, T>
        where T : struct, IFormattable
    {
        bool TryParse(ReadOnlySpan<char> input, out TVector vector);
        bool TryParse(ReadOnlySpan<char> input, char separator, out TVector vector);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, out TVector vector);
        bool TryParse(string s, Regex separator, out TVector vector);

        MatchCollection GetMatches(string input);
        bool TryParse(MatchCollection matches, Span<T> values);

        FromSpan<TVector, T> FromSpan { get; }
        int MinCount { get; }
    }

    abstract class VectorHelper<TVector, T, TStrategy> : Helper<TVector, T, TStrategy>, IVectorHelper<TVector, T>
        where TVector : unmanaged, IVector<TVector, T>
        where T : unmanaged, IFormattable
        where TStrategy : VectorHelperStrategy<TStrategy, TVector, T>
    {
        private readonly Lazy<Regex> _regex;
        private Regex Regex => _regex.Value;

        protected VectorHelper(FromSpan<TVector, T> fromSpan, TryParse<T> tryParse, T minusOne, T zero, T one, string pattern)
            : base(fromSpan, tryParse)
        {
            InitHeadings(minusOne, zero, one);
            _regex = new(() => new(pattern));
        }

        protected sealed override MatchCollection GetMatches(string input) =>
            Regex.Matches(input);

        MatchCollection IVectorHelper<TVector, T>.GetMatches(string input) =>
            GetMatches(input);

        bool IVectorHelper<TVector, T>.TryParse(MatchCollection matches, Span<T> values) =>
            TryParse(matches, values);

        int IVectorHelper<TVector, T>.MinCount =>
            MinCount;

        protected abstract void InitHeadings(T minusOne, T zero, T one);
    }

    sealed class Vector2DHelperStrategy<TVector, T> : VectorHelperStrategy<Vector2DHelperStrategy<TVector, T>, TVector, T>
        where TVector : struct, IVector2D<TVector, T>
        where T : struct, IFormattable
    {
        private Vector2DHelperStrategy()
            : base("x", "y")
        {
        }
    }

    sealed class Vector2DHelper<TVector, T> : VectorHelper<TVector, T, Vector2DHelperStrategy<TVector, T>>
        where TVector : unmanaged, IVector2D<TVector, T>
        where T : unmanaged, IFormattable
    {
        public Vector2DHelper(FromSpan<TVector, T> fromSpan, TryParse<T> tryParse, T minusOne, T zero, T one, string pattern)
            : base(fromSpan, tryParse, minusOne, zero, one, pattern)
        {
        }

        protected override void InitHeadings(T minusOne, T zero, T one)
        {
            North = FromArray(zero,     minusOne, zero);
            East  = FromArray(one,      zero,     zero);
            South = FromArray(zero,     one,      zero);
            West  = FromArray(minusOne, zero,     zero);
        }

        protected override Vector2DHelperStrategy<TVector, T> Strategy =>
            Vector2DHelperStrategy<TVector, T>.Instance;

        public TVector North { get; private set; }
        public TVector East  { get; private set; }
        public TVector South { get; private set; }
        public TVector West  { get; private set; }
    }

    sealed class Vector3DHelperStrategy<TVector, T> : VectorHelperStrategy<Vector3DHelperStrategy<TVector, T>, TVector, T>
        where TVector : struct, IVector3D<TVector, T>
        where T : struct, IFormattable
    {
        private Vector3DHelperStrategy()
            : base("x", "y", "z")
        {
        }
    }

    sealed class Vector3DHelper<TVector, T> : VectorHelper<TVector, T, Vector3DHelperStrategy<TVector, T>>
        where TVector : unmanaged, IVector3D<TVector, T>
        where T : unmanaged, IFormattable
    {
        public Vector3DHelper(FromSpan<TVector, T> fromSpan, TryParse<T> tryParse, T minusOne, T zero, T one, string pattern)
            : base(fromSpan, tryParse, minusOne, zero, one, pattern)
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

        protected override Vector3DHelperStrategy<TVector, T> Strategy =>
            Vector3DHelperStrategy<TVector, T>.Instance;

        public TVector North { get; private set; }
        public TVector East  { get; private set; }
        public TVector South { get; private set; }
        public TVector West  { get; private set; }
        public TVector Up    { get; private set; }
        public TVector Down  { get; private set; }
    }

    sealed class Vector4DHelperStrategy<TVector, T> : VectorHelperStrategy<Vector4DHelperStrategy<TVector, T>, TVector, T>
        where TVector : struct, IVector4D<TVector, T>
        where T : struct, IFormattable
    {
        private Vector4DHelperStrategy()
            : base("x", "y", "z", "w")
        {
        }
    }

    sealed class Vector4DHelper<TVector, T> : VectorHelper<TVector, T, Vector4DHelperStrategy<TVector, T>>
        where TVector : unmanaged, IVector4D<TVector, T>
        where T : unmanaged, IFormattable
    {
        public Vector4DHelper(FromSpan<TVector, T> fromSpan, TryParse<T> tryParse, T minusOne, T zero, T one, string pattern)
            : base(fromSpan, tryParse, minusOne, zero, one, pattern)
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

        protected override Vector4DHelperStrategy<TVector, T> Strategy =>
            Vector4DHelperStrategy<TVector, T>.Instance;

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
