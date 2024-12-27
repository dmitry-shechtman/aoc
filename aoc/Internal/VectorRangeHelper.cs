using System;

namespace aoc.Internal
{
    sealed class RangeHelper<TRange, TVector, T> : Helper2<TRange, TVector, T, RangeHelperStrategy<TRange, TVector>, IVectorHelper<TVector, T>>
        where TRange : unmanaged, IRange<TRange, TVector>
        where TVector : unmanaged, IVector<TVector, T>
        where T : unmanaged, IFormattable
    {
        public RangeHelper(FromSpan<TRange, TVector> fromSpan, IVectorHelper<TVector, T> vector)
            : base(fromSpan, vector)
        {
        }

        protected override int GetChunkSize(int count) =>
            count == Vector.MinCount * 2
                ? Vector.MinCount
                : 0;

        protected override RangeHelperStrategy<TRange, TVector> Strategy =>
            RangeHelperStrategy<TRange, TVector>.Instance;
    }
}
