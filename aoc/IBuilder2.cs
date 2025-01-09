using System;
using System.Globalization;

namespace aoc
{
    public interface IBuilder2<T> : IBuilder<T>
    {
        T Parse(ReadOnlySpan<char> input, char separator, char separator2, IFormatProvider provider = null);
        bool TryParse(ReadOnlySpan<char> input, char separator, char separator2, out T result);
        bool TryParse(ReadOnlySpan<char> input, char separator, char separator2, IFormatProvider provider, out T result);

        T Parse(ReadOnlySpan<char> input, char separator, char separator2, NumberStyles styles, IFormatProvider provider = null);
        bool TryParse(ReadOnlySpan<char> input, char separator, char separator2, NumberStyles styles, out T result);
        bool TryParse(ReadOnlySpan<char> input, char separator, char separator2, NumberStyles styles, IFormatProvider provider, out T result);

        T Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, ReadOnlySpan<char> separator2, IFormatProvider provider = null);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, ReadOnlySpan<char> separator2, out T result);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, ReadOnlySpan<char> separator2, IFormatProvider provider, out T result);

        T Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, ReadOnlySpan<char> separator2, NumberStyles styles, IFormatProvider provider = null);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, ReadOnlySpan<char> separator2, NumberStyles styles, out T result);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, ReadOnlySpan<char> separator2, NumberStyles styles, IFormatProvider provider, out T result);
    }
}
