using System;
using System.Diagnostics.CodeAnalysis;

namespace aoc.Grids.Builders
{
    public interface IGridBuilder<TGrid>
        where TGrid : Grid<TGrid>
    {
        string ToString(TGrid grid, Size size, ReadOnlySpan<char> format);
        string ToString(TGrid grid, VectorRange range, ReadOnlySpan<char> format);

        TGrid Parse(ReadOnlySpan<char> input);
        bool TryParse(ReadOnlySpan<char> input,
            [MaybeNullWhen(false)] out TGrid grid);

        TGrid Parse(ReadOnlySpan<char> input, out VectorRange range);
        bool TryParse(ReadOnlySpan<char> input, out VectorRange range,
            [MaybeNullWhen(false)] out TGrid grid);

        TGrid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format,
            [MaybeNullWhen(false)] out TGrid grid);

        TGrid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out VectorRange range);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out VectorRange range,
            [MaybeNullWhen(false)] out TGrid grid);

        TGrid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, Span<Vector> output);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, Span<Vector> output,
            [MaybeNullWhen(false)] out TGrid grid);

        TGrid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, Span<Vector> output, out VectorRange range);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, Span<Vector> output, out VectorRange range,
            [MaybeNullWhen(false)] out TGrid grid);
    }
}
