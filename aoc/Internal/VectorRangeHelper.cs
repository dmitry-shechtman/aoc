using System;

namespace aoc.Internal
{
    sealed class RangeHelper<TRange, TVector, T> : Helper2<TRange, TVector, T, RangeHelperStrategy<TRange, TVector>, IVectorHelper<TVector, T>>
        where TRange : struct, IRange<TRange, TVector>
        where TVector : struct, IVector<TVector, T>
        where T : struct, IFormattable
    {
        public RangeHelper(FromSpan<TRange, TVector> fromSpan, IVectorHelper<TVector, T> vector)
            : base(fromSpan, vector)
        {
        }

        protected override RangeHelperStrategy<TRange, TVector> Strategy =>
            RangeHelperStrategy<TRange, TVector>.Instance;
    }
}
