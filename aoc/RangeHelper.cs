using System;

namespace aoc
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

    sealed class RangeHelper<TRange, TValue> : Helper1<TRange, TValue, RangeHelperStrategy>
        where TRange : struct, IRange<TRange, TValue>
        where TValue : struct, IFormattable
    {
        public RangeHelper(Func<TValue[], TRange> fromArray, TryParseValue1<TValue> tryParse)
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
        public VectorRangeHelper(Func<TVector[], TRange> fromArray, TryParseValue2<TVector> tryParse)
            : base(fromArray, tryParse)
        {
        }

        protected override RangeHelperStrategy Strategy =>
            RangeHelperStrategy.Instance;
    }
}
