using System;
using System.Collections.Generic;

namespace aoc.Grids
{
    public interface IGridBuilder<TGrid>
        where TGrid : Grid<TGrid>
    {
        string ToString(TGrid grid, Size size, ReadOnlySpan<char> format);
        string ToString(TGrid grid, VectorRange range, ReadOnlySpan<char> format);

        TGrid Parse(ReadOnlySpan<char> input);
        bool TryParse(ReadOnlySpan<char> input, out TGrid grid);
        TGrid Parse(ReadOnlySpan<char> input, out VectorRange range);
        bool TryParse(ReadOnlySpan<char> input, out VectorRange range, out TGrid grid);
        TGrid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out TGrid grid);
        TGrid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out VectorRange range);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out VectorRange range, out TGrid grid);
        TGrid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, Span<Vector> output);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, Span<Vector> output, out TGrid grid);
        TGrid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, Span<Vector> output, out VectorRange range);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, Span<Vector> output, out VectorRange range, out TGrid grid);
    }

    public interface IHeadingBuilder
    {
        int Parse(ReadOnlySpan<char> input);
        bool TryParse(ReadOnlySpan<char> input, out int heading);
    }

    public interface IVectorBuilder<TVector>
        where TVector : struct, IVector<TVector>
    {
        string ToString(TVector vector, char format);

        TVector Parse(ReadOnlySpan<char> input);
        bool TryParse(ReadOnlySpan<char> input, out TVector vector);
        IEnumerable<TVector> ParseAll(ReadOnlySpan<char> input, params char[] skip) => ParseAll(input, skip.AsSpan());
        IEnumerable<TVector> ParseAll(ReadOnlySpan<char> input, ReadOnlySpan<char> skip);
        bool TryParseAll(ReadOnlySpan<char> input, ReadOnlySpan<char> skip, out IEnumerable<TVector> vectors);
    }

    public interface IPathBuilder<TVector>
        where TVector : struct, IVector<TVector>
    {
        IEnumerable<PathSegment<TVector>> Parse(ReadOnlySpan<char> input);
        IEnumerable<PathSegment<TVector>> Parse(string input, char separator);
        IEnumerable<PathSegment<TVector>> Parse(string input, string separator);
        IEnumerable<PathSegment<TVector>> Parse(string[] ss);
        IEnumerable<PathSegment<TVector>> Parse(string[] ss, char separator);
        IEnumerable<PathSegment<TVector>> Parse(string[] ss, string separator);
    }
}
