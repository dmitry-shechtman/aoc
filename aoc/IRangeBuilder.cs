using System;

namespace aoc
{
    public interface IRangeBuilder<TRange, TVector> : IBuilder<TRange>
        where TRange : struct, IRange<TRange, TVector>
        where TVector : struct, IVector<TVector>
    {
        TRange Parse(ReadOnlySpan<char> input, char separator, char separator2, IFormatProvider provider = null);
        bool TryParse(ReadOnlySpan<char> input, char separator, char separator2, out TRange range);
        bool TryParse(ReadOnlySpan<char> input, char separator, char separator2, IFormatProvider provider, out TRange range);
    }
}
