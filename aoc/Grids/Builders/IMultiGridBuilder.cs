using System;
using System.Diagnostics.CodeAnalysis;

namespace aoc.Grids.Builders
{
    public interface IMultiGridBuilder<TMulti, TGrid>
        where TMulti : MultiGrid<TMulti, TGrid>
        where TGrid : Grid<TGrid>
    {
        string ToString(TMulti multi, Size size, ReadOnlySpan<char> format);
        string ToString(TMulti multi, VectorRange range, ReadOnlySpan<char> format);

        TMulti Parse(ReadOnlySpan<char> input);
        bool TryParse(ReadOnlySpan<char> input,
            [MaybeNullWhen(false)] out TMulti multi);

        TMulti Parse(ReadOnlySpan<char> input, out VectorRange range);
        bool TryParse(ReadOnlySpan<char> input, out VectorRange range,
            [MaybeNullWhen(false)] out TMulti multi);

        TMulti Parse(ReadOnlySpan<char> input, Func<char, bool> predicate);
        bool TryParse(ReadOnlySpan<char> input, Func<char, bool> predicate,
            [MaybeNullWhen(false)] out TMulti multi);

        TMulti Parse(ReadOnlySpan<char> input, Func<char, bool> predicate, out VectorRange range);
        bool TryParse(ReadOnlySpan<char> input, Func<char, bool> predicate, out VectorRange range,
            [MaybeNullWhen(false)] out TMulti multi);

        TMulti Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format,
            [MaybeNullWhen(false)] out TMulti multi);

        TMulti Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out VectorRange range);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out VectorRange range,
            [MaybeNullWhen(false)] out TMulti multi);

        TMulti Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, ReadOnlySpan<char> separator);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, ReadOnlySpan<char> separator,
            [MaybeNullWhen(false)] out TMulti multi);

        TMulti Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, ReadOnlySpan<char> separator, out VectorRange range);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, ReadOnlySpan<char> separator, out VectorRange range,
            [MaybeNullWhen(false)] out TMulti multi);
    }
}
