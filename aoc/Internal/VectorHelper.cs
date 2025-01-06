using System;
using System.Collections.Generic;
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

        MatchCollection GetMatches(string input, out int count);
        bool TryParse(IEnumerator<Match> matches, Span<T> values);

        FromSpan<TVector, T> FromSpan { get; }
        int MinCount { get; }
    }

    abstract class VectorHelper<TVector, T, TStrategy> : Helper<TVector, T, TStrategy>, IVectorHelper<TVector, T>
        where TVector : unmanaged, IVector<TVector, T>
        where T : unmanaged, IFormattable
        where TStrategy : VectorHelperStrategy<TStrategy, TVector, T>
    {
        protected VectorHelper(FromSpan<TVector, T> fromSpan, INumberHelper<T> number)
            : base(fromSpan, number.TryParse)
        {
            Number = number;
        }

        protected sealed override MatchCollection GetMatches(string input, out int count) =>
            Number.GetMatches(input, out count);

        MatchCollection IVectorHelper<TVector, T>.GetMatches(string input, out int count) =>
            GetMatches(input, out count);

        bool IVectorHelper<TVector, T>.TryParse(IEnumerator<Match> matches, Span<T> values) =>
            TryParse(matches, values);

        int IVectorHelper<TVector, T>.MinCount =>
            MinCount;

        private INumberHelper<T> Number { get; }

        protected T NegativeOne => Number.NegativeOne;
        protected T Zero        => Number.Zero;
        protected T One         => Number.One;
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
        public Vector2DHelper(FromSpan<TVector, T> fromSpan, INumberHelper<T> number)
            : base(fromSpan, number)
        {
            POne  = FromArray(One,         One,         One);
            NOne  = FromArray(NegativeOne, NegativeOne, NegativeOne);
            North = FromArray(Zero,        NegativeOne, Zero);
            East  = FromArray(One,         Zero,        Zero);
            South = FromArray(Zero,        One,         Zero);
            West  = FromArray(NegativeOne, Zero,        Zero);
        }

        protected override Vector2DHelperStrategy<TVector, T> Strategy =>
            Vector2DHelperStrategy<TVector, T>.Instance;

        public TVector POne  { get; }
        public TVector NOne  { get; }
        public TVector North { get; }
        public TVector East  { get; }
        public TVector South { get; }
        public TVector West  { get; }
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
        public Vector3DHelper(FromSpan<TVector, T> fromSpan, INumberHelper<T> number)
            : base(fromSpan, number)
        {
            POne  = FromArray(One,         One,         One);
            NOne  = FromArray(NegativeOne, NegativeOne, NegativeOne);
            North = FromArray(Zero,        NegativeOne, Zero);
            East  = FromArray(One,         Zero,        Zero);
            South = FromArray(Zero,        One,         Zero);
            West  = FromArray(NegativeOne, Zero,        Zero);
            Up    = FromArray(Zero,        Zero,        NegativeOne);
            Down  = FromArray(Zero,        Zero,        One);
        }

        protected override Vector3DHelperStrategy<TVector, T> Strategy =>
            Vector3DHelperStrategy<TVector, T>.Instance;

        public TVector POne  { get; }
        public TVector NOne  { get; }
        public TVector North { get; }
        public TVector East  { get; }
        public TVector South { get; }
        public TVector West  { get; }
        public TVector Up    { get; }
        public TVector Down  { get; }
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
        public Vector4DHelper(FromSpan<TVector, T> fromSpan, INumberHelper<T> number)
            : base(fromSpan, number)
        {
            POne  = FromArray(One,         One,         One,         NegativeOne);
            NOne  = FromArray(NegativeOne, NegativeOne, NegativeOne, NegativeOne);
            North = FromArray(Zero,        NegativeOne, Zero,        Zero);
            East  = FromArray(One,         Zero,        Zero,        Zero);
            South = FromArray(Zero,        One,         Zero,        Zero);
            West  = FromArray(NegativeOne, Zero,        Zero,        Zero);
            Up    = FromArray(Zero,        Zero,        NegativeOne, Zero);
            Down  = FromArray(Zero,        Zero,        One,         Zero);
            Ana   = FromArray(Zero,        Zero,        Zero,        NegativeOne);
            Kata  = FromArray(Zero,        Zero,        Zero,        One);
        }

        protected override Vector4DHelperStrategy<TVector, T> Strategy =>
            Vector4DHelperStrategy<TVector, T>.Instance;

        public TVector POne  { get; }
        public TVector NOne  { get; }
        public TVector North { get; }
        public TVector East  { get; }
        public TVector South { get; }
        public TVector West  { get; }
        public TVector Up    { get; }
        public TVector Down  { get; }
        public TVector Ana   { get; }
        public TVector Kata  { get; }
    }
}
