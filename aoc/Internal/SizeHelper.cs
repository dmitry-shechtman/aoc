using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace aoc.Internal
{
    abstract class SizeHelperStrategy<TSelf, TSize, TVector, T> : ListHelperStrategy<TSelf, TSize, T>
        where TSelf : SizeHelperStrategy<TSelf, TSize, TVector, T>
        where TSize : struct, ISize<TSize, TVector, T>
        where TVector : struct, IVector<TVector, T>
        where T : struct, IFormattable
    {
        protected SizeHelperStrategy(params string[] formatKeys)
            : base(formatKeys)
        {
        }

        public override char DefaultSeparator => ':';
    }

    abstract class SizeHelper<TSize, TVector, T, TStrategy> : Helper<TSize, T, TStrategy>
        where TSize : unmanaged, ISize<TSize, TVector, T>
        where TVector : unmanaged, IVector<TVector, T>
        where T : unmanaged, IFormattable
        where TStrategy: SizeHelperStrategy<TStrategy, TSize, TVector, T>
    {
        protected SizeHelper(FromSpan<TSize, T> fromSpan, TryParse<T> tryParse, IVectorHelper<TVector, T> vector)
            : base(fromSpan, tryParse)
        {
            Vector = vector;
        }

        protected sealed override IEnumerable<Match> GetMatches(string input, out int count) =>
            Vector.GetMatches(input, out count);

        private IVectorHelper<TVector, T> Vector { get; }
    }

    sealed class Size2DHelperStrategy<TSize, TVector, T> : SizeHelperStrategy<Size2DHelperStrategy<TSize, TVector, T>, TSize, TVector, T>
        where TSize : struct, ISize2D<TSize, TVector, T>
        where TVector : struct, IVector2D<TVector, T>
        where T : struct, IFormattable
    {
        private Size2DHelperStrategy()
            : base("w", "h")
        {
        }
    }

    sealed class Size2DHelper<TSize, TVector, T> : SizeHelper<TSize, TVector, T, Size2DHelperStrategy<TSize, TVector, T>>
        where TSize : unmanaged, ISize2D<TSize, TVector, T>
        where TVector : unmanaged, IVector2D<TVector, T>
        where T : unmanaged, IFormattable
    {
        public Size2DHelper(FromSpan<TSize, T> fromSpan, TryParse<T> tryParse, IVectorHelper<TVector, T> vector)
            : base(fromSpan, tryParse, vector)
        {
        }

        protected override Size2DHelperStrategy<TSize, TVector, T> Strategy =>
            Size2DHelperStrategy<TSize, TVector, T>.Instance;
    }

    sealed class Size3DHelperStrategy<TSize, TVector, T> : SizeHelperStrategy<Size3DHelperStrategy<TSize, TVector, T>, TSize, TVector, T>
        where TSize : struct, ISize3D<TSize, TVector, T>
        where TVector : struct, IVector3D<TVector, T>
        where T : struct, IFormattable
    {
        private Size3DHelperStrategy()
            : base("w", "h", "d")
        {
        }
    }

    sealed class Size3DHelper<TSize, TVector, T> : SizeHelper<TSize, TVector, T, Size3DHelperStrategy<TSize, TVector, T>>
        where TSize : unmanaged, ISize3D<TSize, TVector, T>
        where TVector : unmanaged, IVector3D<TVector, T>
        where T : unmanaged, IFormattable
    {
        public Size3DHelper(FromSpan<TSize, T> fromSpan, TryParse<T> tryParse, IVectorHelper<TVector, T> vector)
            : base(fromSpan, tryParse, vector)
        {
        }

        protected override Size3DHelperStrategy<TSize, TVector, T> Strategy =>
            Size3DHelperStrategy<TSize, TVector, T>.Instance;
    }

    sealed class Size4DHelperStrategy<TSize, TVector, T> : SizeHelperStrategy<Size4DHelperStrategy<TSize, TVector, T>, TSize, TVector, T>
        where TSize : struct, ISize4D<TSize, TVector, T>
        where TVector : struct, IVector4D<TVector, T>
        where T : struct, IFormattable
    {
        private Size4DHelperStrategy()
            : base("w", "h", "d", "a")
        {
        }
    }

    sealed class Size4DHelper<TSize, TVector, T> : SizeHelper<TSize, TVector, T, Size4DHelperStrategy<TSize, TVector, T>>
        where TSize : unmanaged, ISize4D<TSize, TVector, T>
        where TVector : unmanaged, IVector4D<TVector, T>
        where T : unmanaged, IFormattable
    {
        public Size4DHelper(FromSpan<TSize, T> fromSpan, TryParse<T> tryParse, IVectorHelper<TVector, T> vector)
            : base(fromSpan, tryParse, vector)
        {
        }

        protected override Size4DHelperStrategy<TSize, TVector, T> Strategy =>
            Size4DHelperStrategy<TSize, TVector, T>.Instance;
    }
}
