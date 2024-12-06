using System;

namespace aoc.Internal
{
    sealed class RangeHelperStrategy : HelperStrategy<RangeHelperStrategy>
    {
        private RangeHelperStrategy()
            : base("min", "max")
        {
        }

        public override int MinCount => 2;
        public override int MaxCount => 2;

        public override char DefaultSeparator => '~';

        protected override string SeparatorString =>
            $" {DefaultSeparator} ";

        public static T GetItem<TRange, T>(IRange<TRange, T> range, int i)
            where TRange : struct, IRange<TRange, T>
            where T : struct, IFormattable => i switch
            {
                0 => range.Min,
                1 => range.Max,
                _ => throw new IndexOutOfRangeException(),
            };
    }

    sealed class RangeHelper<TRange, T> : Helper<TRange, T, RangeHelperStrategy>
        where TRange : struct, IRange<TRange, T>
        where T : struct, IFormattable
    {
        public RangeHelper(Func<T[], TRange> fromArray, TryParse<T> tryParse)
            : base(fromArray, tryParse)
        {
        }

        protected override T GetItem(TRange range, int i) =>
            RangeHelperStrategy.GetItem(range, i);

        protected override RangeHelperStrategy Strategy =>
            RangeHelperStrategy.Instance;
    }

    sealed class VectorRangeHelper<TRange, TVector> : Helper2<TRange, TVector, RangeHelperStrategy, IVectorHelper<TVector>>
        where TRange : struct, IRange<TRange, TVector>
        where TVector : struct, IFormattable
    {
        public VectorRangeHelper(Func<TVector[], TRange> fromArray, IVectorHelper<TVector> vector)
            : base(fromArray, vector)
        {
        }

        protected override TVector GetItem(TRange range, int i) =>
            RangeHelperStrategy.GetItem(range, i);

        protected override RangeHelperStrategy Strategy =>
            RangeHelperStrategy.Instance;
    }
}
