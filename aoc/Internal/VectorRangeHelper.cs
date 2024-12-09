using System;

namespace aoc.Internal
{
    sealed class VectorRangeHelper<TRange, TVector> : Helper2<TRange, TVector, RangeHelperStrategy<TRange, TVector>, IVectorHelper<TVector>>
        where TRange : struct, IRange<TRange, TVector>
        where TVector : struct, IFormattable
    {
        public VectorRangeHelper(FromArray<TRange, TVector> fromArray, IVectorHelper<TVector> vector)
            : base(fromArray, vector)
        {
        }

        protected override RangeHelperStrategy<TRange, TVector> Strategy =>
            RangeHelperStrategy<TRange, TVector>.Instance;
    }
}
