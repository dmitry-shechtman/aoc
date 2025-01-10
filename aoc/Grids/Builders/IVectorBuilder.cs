using System;
using System.Collections.Generic;

namespace aoc.Grids.Builders
{
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
}
