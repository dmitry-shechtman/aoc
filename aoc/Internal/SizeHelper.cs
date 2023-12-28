using System;

namespace aoc.Internal
{
    abstract class SizeHelperStrategy<TSelf> : Helper1Strategy<TSelf>
        where TSelf : SizeHelperStrategy<TSelf>
    {
        protected SizeHelperStrategy(params string[] formatKeys)
            : base(formatKeys)
        {
        }

        public override char DefaultSeparator => ':';
    }

    abstract class SizeHelper<TSize, TVector, T, TStrategy> : Helper1<TSize, T, TStrategy>
        where TSize : struct, ISize<TSize, TVector, T>
        where TVector : struct, IVector<TVector, T>
        where T : struct, IFormattable
        where TStrategy : IHelper1Strategy
    {
        protected SizeHelper(Func<T[], TSize> fromArray, TryParse1<T> tryParse)
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
        public Size2DHelper(Func<T[], TSize> fromArray, TryParse1<T> tryParse)
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
        public Size3DHelper(Func<T[], TSize> fromArray, TryParse1<T> tryParse)
            : base(fromArray, tryParse)
        {
        }

        protected override Size3DHelperStrategy Strategy => Size3DHelperStrategy.Instance;
    }
}
