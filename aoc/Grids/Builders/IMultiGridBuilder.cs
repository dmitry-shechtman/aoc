using System;
using System.Diagnostics.CodeAnalysis;

namespace aoc.Grids.Builders
{
    public interface IMultiGridBuilder<TMulti, TGrid> : IBaseGridBuilder<TMulti>
        where TMulti : MultiGrid<TMulti, TGrid>
        where TGrid : Grid2D<TGrid>
    {
        TMulti Parse(ReadOnlySpan<char> input, Func<char, bool> predicate);
        bool TryParse(ReadOnlySpan<char> input, Func<char, bool> predicate,
            [MaybeNullWhen(false)] out TMulti multi);

        TMulti Parse(ReadOnlySpan<char> input, Func<char, bool> predicate, out VectorRange range);
        bool TryParse(ReadOnlySpan<char> input, Func<char, bool> predicate, out VectorRange range,
            [MaybeNullWhen(false)] out TMulti multi);

        TMulti Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, ReadOnlySpan<char> separator);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, ReadOnlySpan<char> separator,
            [MaybeNullWhen(false)] out TMulti multi);

        TMulti Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, ReadOnlySpan<char> separator, out VectorRange range);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, ReadOnlySpan<char> separator, out VectorRange range,
            [MaybeNullWhen(false)] out TMulti multi);

        TMulti FromArray<TSize>(int[] array, TSize size, Range range)
            where TSize : struct, ISize2D<TSize, int>;

        TMulti FromArray(int[,] array, Range range);
    }
}
