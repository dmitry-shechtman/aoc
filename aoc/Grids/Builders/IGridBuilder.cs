using System;
using System.Diagnostics.CodeAnalysis;

namespace aoc.Grids.Builders
{
    public interface IGridBuilder<TGrid> : IBaseGridBuilder<TGrid>
        where TGrid : Grid<TGrid>
    {
        TGrid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, Span<Vector> output);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, Span<Vector> output,
            [MaybeNullWhen(false)] out TGrid grid);

        TGrid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, Span<Vector> output, out VectorRange range);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, Span<Vector> output, out VectorRange range,
            [MaybeNullWhen(false)] out TGrid grid);
    }
}
