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

    interface IVectorHelper<TVector, T> : IItemHelper<TVector>, IParseHelper<TVector, char>
        where TVector : unmanaged, IVector<TVector, T>
        where T : struct, IFormattable
    {
        bool TryParse(IEnumerator<Match> matches, IFormatProvider provider, Span<T> values);

        FromSpan<TVector, T> FromSpan { get; }
        int MinCount { get; }
    }

    abstract class VectorHelper<TVector, T, TStrategy> : Helper<TVector, T, TStrategy>, IVectorHelper<TVector, T>
        where TVector : unmanaged, IVector<TVector, T>
        where T : unmanaged, IFormattable
        where TStrategy : VectorHelperStrategy<TStrategy, TVector, T>
    {
        protected VectorHelper(FromSpan<TVector, T> fromSpan, INumberHelper<T> number,
            TVector pOne, TVector nOne)
                : base(fromSpan, number)
        {
            One = pOne;
            NegativeOne = nOne;
        }

        public TVector One         { get; }
        public TVector NegativeOne { get; }
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
        public Vector2DHelper(FromSpan<TVector, T> fromSpan, INumberHelper<T> T)
            : base(fromSpan, T,
                  pOne: fromSpan(new[] { T.One,         T.One,         T.One}),
                  nOne: fromSpan(new[] { T.NegativeOne, T.NegativeOne, T.NegativeOne }))
        {
            North = FromArray(T.Zero,        T.NegativeOne, T.Zero);
            East  = FromArray(T.One,         T.Zero,        T.Zero);
            South = FromArray(T.Zero,        T.One,         T.Zero);
            West  = FromArray(T.NegativeOne, T.Zero,        T.Zero);
        }

        protected override Vector2DHelperStrategy<TVector, T> Strategy =>
            Vector2DHelperStrategy<TVector, T>.Instance;

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
        public Vector3DHelper(FromSpan<TVector, T> fromSpan, INumberHelper<T> T)
            : base(fromSpan, T,
                  pOne: fromSpan(new[] { T.One,         T.One,         T.One}),
                  nOne: fromSpan(new[] { T.NegativeOne, T.NegativeOne, T.NegativeOne }))
        {
            North = FromArray(T.Zero,        T.NegativeOne, T.Zero);
            East  = FromArray(T.One,         T.Zero,        T.Zero);
            South = FromArray(T.Zero,        T.One,         T.Zero);
            West  = FromArray(T.NegativeOne, T.Zero,        T.Zero);
            Up    = FromArray(T.Zero,        T.Zero,        T.NegativeOne);
            Down  = FromArray(T.Zero,        T.Zero,        T.One);
        }

        protected override Vector3DHelperStrategy<TVector, T> Strategy =>
            Vector3DHelperStrategy<TVector, T>.Instance;

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
        public Vector4DHelper(FromSpan<TVector, T> fromSpan, INumberHelper<T> T)
            : base(fromSpan, T,
                  pOne: fromSpan(new[] { T.One,         T.One,         T.One,         T.One }),
                  nOne: fromSpan(new[] { T.NegativeOne, T.NegativeOne, T.NegativeOne, T.NegativeOne }))
        {
            North = FromArray(T.Zero,        T.NegativeOne, T.Zero,        T.Zero);
            East  = FromArray(T.One,         T.Zero,        T.Zero,        T.Zero);
            South = FromArray(T.Zero,        T.One,         T.Zero,        T.Zero);
            West  = FromArray(T.NegativeOne, T.Zero,        T.Zero,        T.Zero);
            Up    = FromArray(T.Zero,        T.Zero,        T.NegativeOne, T.Zero);
            Down  = FromArray(T.Zero,        T.Zero,        T.One,         T.Zero);
            Ana   = FromArray(T.Zero,        T.Zero,        T.Zero,        T.NegativeOne);
            Kata  = FromArray(T.Zero,        T.Zero,        T.Zero,        T.One);
        }

        protected override Vector4DHelperStrategy<TVector, T> Strategy =>
            Vector4DHelperStrategy<TVector, T>.Instance;

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
