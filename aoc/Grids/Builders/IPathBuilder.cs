using System;
using System.Collections.Generic;

namespace aoc.Grids.Builders
{
    public interface IPathBuilder<TVector>
        where TVector : struct, IVector<TVector>
    {
        IEnumerable<PathSegment<TVector>> Parse(ReadOnlySpan<char> input);
        IEnumerable<PathSegment<TVector>> Parse(string? input, char separator);
        IEnumerable<PathSegment<TVector>> Parse(string? input, string separator);
        IEnumerable<PathSegment<TVector>> Parse(string[]? ss);
        IEnumerable<PathSegment<TVector>> Parse(string[]? ss, char separator);
        IEnumerable<PathSegment<TVector>> Parse(string[]? ss, string separator);
    }
}
