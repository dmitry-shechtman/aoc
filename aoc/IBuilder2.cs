using System;

namespace aoc
{
    public interface IBuilder2<T> : IBuilder<T>
    {
        T Parse(ReadOnlySpan<char> input, char separator, char separator2, IFormatProvider provider = null);
        bool TryParse(ReadOnlySpan<char> input, char separator, char separator2, IFormatProvider provider, out T result);
        bool TryParse(ReadOnlySpan<char> input, char separator, char separator2, out T result);
    }
}
