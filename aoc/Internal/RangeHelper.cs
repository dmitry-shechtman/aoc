using System;

namespace aoc.Internal
{
    sealed class RangeHelperStrategy<TRange, T> : HelperStrategy<RangeHelperStrategy<TRange, T>, TRange, T>
        where TRange : struct, IRange<TRange, T>
        where T : struct, IFormattable
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

        public static T GetItem(IRange<TRange, T> range, int i) => i switch
        {
            0 => range.Min,
            1 => range.Max,
            _ => throw new IndexOutOfRangeException(),
        };
    }

    sealed class RangeHelper<TRange, T> : Helper<TRange, T, RangeHelperStrategy<TRange, T>>
        where TRange : struct, IRange<TRange, T>
        where T : struct, IFormattable
    {
        public RangeHelper(Func<T[], TRange> fromArray, TryParse<T> tryParse)
            : base(fromArray, tryParse)
        {
        }

        protected override T GetItem(TRange value, int i) =>
            RangeHelperStrategy<TRange, T>.GetItem(value, i);

        protected override RangeHelperStrategy<TRange, T> Strategy =>
            RangeHelperStrategy<TRange, T>.Instance;
    }

    sealed class VectorRangeHelper<TRange, TVector> : Helper2<TRange, TVector, RangeHelperStrategy<TRange, TVector>, IVectorHelper<TVector>>
        where TRange : struct, IRange<TRange, TVector>
        where TVector : struct, IFormattable
    {
        public VectorRangeHelper(Func<TVector[], TRange> fromArray, IVectorHelper<TVector> vector)
            : base(fromArray, vector)
        {
        }

        protected override TVector GetItem(TRange value, int i) =>
            RangeHelperStrategy<TRange, TVector>.GetItem(value, i);

        protected override RangeHelperStrategy<TRange, TVector> Strategy =>
            RangeHelperStrategy<TRange, TVector>.Instance;
    }
}
