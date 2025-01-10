namespace aoc.Builders
{
    public interface IRangeBuilder<TRange, TVector> : IBuilder2<TRange>
        where TRange : struct, IRange<TRange, TVector>
        where TVector : struct, IVector<TVector>
    {
    }
}
