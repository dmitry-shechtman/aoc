using System;

namespace aoc.Internal
{
    abstract class SizeHelperStrategy<TSelf> : HelperStrategy<TSelf>
        where TSelf : SizeHelperStrategy<TSelf>
    {
        protected SizeHelperStrategy(params string[] formatKeys)
            : base(formatKeys)
        {
        }

        public override char DefaultSeparator => ':';
    }

    abstract class SizeHelper<TSize, TVector, T, TStrategy> : Helper<TSize, T, TStrategy>
        where TSize : struct, ISize<TSize, TVector, T>
        where TVector : struct, IVector<TVector, T>
        where T : struct, IFormattable
        where TStrategy : IHelperStrategy
    {
        protected SizeHelper(Func<T[], TSize> fromArray, TryParse<T> tryParse)
            : base(fromArray, tryParse)
        {
        }
    }

    sealed class Size2DHelperStrategy : SizeHelperStrategy<Size2DHelperStrategy>
    {
        private Size2DHelperStrategy()
            : base("w", "h")
        {
        }
    }

    sealed class Size2DHelper<TSize, TVector, T> : SizeHelper<TSize, TVector, T, Size2DHelperStrategy>
        where TSize : struct, ISize2D<TSize, TVector, T>
        where TVector : struct, IVector2D<TVector, T>
        where T : struct, IFormattable
    {
        public Size2DHelper(Func<T[], TSize> fromArray, TryParse<T> tryParse)
            : base(fromArray, tryParse)
        {
        }

        protected override Size2DHelperStrategy Strategy => Size2DHelperStrategy.Instance;
    }

    sealed class Size3DHelperStrategy : SizeHelperStrategy<Size3DHelperStrategy>
    {
        private Size3DHelperStrategy()
            : base("w", "h", "d")
        {
        }
    }

    sealed class Size3DHelper<TSize, TVector, T> : SizeHelper<TSize, TVector, T, Size3DHelperStrategy>
        where TSize : struct, ISize3D<TSize, TVector, T>
        where TVector : struct, IVector3D<TVector, T>
        where T : struct, IFormattable
    {
        public Size3DHelper(Func<T[], TSize> fromArray, TryParse<T> tryParse)
            : base(fromArray, tryParse)
        {
        }

        protected override Size3DHelperStrategy Strategy => Size3DHelperStrategy.Instance;
    }

    sealed class Size4DHelperStrategy : SizeHelperStrategy<Size4DHelperStrategy>
    {
        private Size4DHelperStrategy()
            : base("w", "h", "d", "a")
        {
        }
    }

    sealed class Size4DHelper<TSize, TVector, T> : SizeHelper<TSize, TVector, T, Size4DHelperStrategy>
        where TSize : struct, ISize4D<TSize, TVector, T>
        where TVector : struct, IVector4D<TVector, T>
        where T : struct, IFormattable
    {
        public Size4DHelper(Func<T[], TSize> fromArray, TryParse<T> tryParse)
            : base(fromArray, tryParse)
        {
        }

        protected override Size4DHelperStrategy Strategy => Size4DHelperStrategy.Instance;
    }
}
