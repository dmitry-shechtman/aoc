using System;

namespace aoc.Grids.Builders
{
    public interface IHeadingBuilder
    {
        int Parse(ReadOnlySpan<char> input);
        bool TryParse(ReadOnlySpan<char> input, out int heading);
    }
}
