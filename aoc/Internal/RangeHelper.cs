using System;

namespace aoc.Internal
{
    sealed class RangeHelperStrategy : Helper2Strategy<RangeHelperStrategy>, IHelper1Strategy
    {
        private RangeHelperStrategy()
            : base("min", "max")
        {
        }

        public int Count => 2;
        public override int MinCount => 2;
        public override int MaxCount => 2;

        public override char DefaultSeparator => '~';
    }

    sealed class RangeHelper<TRange, T> : Helper1<TRange, T, RangeHelperStrategy>
        where TRange : struct, IRange<TRange, T>
        where T : struct, IFormattable
    {
        public RangeHelper(Func<T[], TRange> fromArray, TryParse1<T> tryParse)
            : base(fromArray, tryParse)
        {
        }

        protected override RangeHelperStrategy Strategy =>
            RangeHelperStrategy.Instance;
    }

    sealed class VectorRangeHelper<TRange, TVector> : Helper2<TRange, TVector, RangeHelperStrategy>
        where TRange : struct, IRange<TRange, TVector>
        where TVector : struct, IFormattable
    {
        public VectorRangeHelper(Func<TVector[], TRange> fromArray, TryParse2<TVector> tryParse)
            : base(fromArray, tryParse)
        {
        }

        protected override RangeHelperStrategy Strategy =>
            RangeHelperStrategy.Instance;
    }
}
