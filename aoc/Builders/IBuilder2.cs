using System;
using System.Globalization;

namespace aoc.Builders
{
    public interface IBuilder2<T> : IBuilder<T>
        where T : struct
    {
        T Parse(ReadOnlySpan<char> input, char separator, char separator2) =>
            Parse(input, separator, separator2, null);
        bool TryParse(ReadOnlySpan<char> input, char separator, char separator2, out T result);
        T Parse(ReadOnlySpan<char> input, char separator, char separator2, IFormatProvider? provider);
        bool TryParse(ReadOnlySpan<char> input, char separator, char separator2, IFormatProvider? provider, out T result);

        T Parse(ReadOnlySpan<char> input, char separator, char separator2, NumberStyles styles) =>
            Parse(input, separator, separator2, styles, null);
        bool TryParse(ReadOnlySpan<char> input, char separator, char separator2, NumberStyles styles, out T result);
        T Parse(ReadOnlySpan<char> input, char separator, char separator2, NumberStyles styles, IFormatProvider? provider);
        bool TryParse(ReadOnlySpan<char> input, char separator, char separator2, NumberStyles styles, IFormatProvider? provider, out T result);

        T Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, ReadOnlySpan<char> separator2) =>
            Parse(input, separator, separator2, null);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, ReadOnlySpan<char> separator2, out T result);
        T Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, ReadOnlySpan<char> separator2, IFormatProvider? provider);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, ReadOnlySpan<char> separator2, IFormatProvider? provider, out T result);

        T Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, ReadOnlySpan<char> separator2, NumberStyles styles) =>
            Parse(input, separator, separator2, styles, null);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, ReadOnlySpan<char> separator2, NumberStyles styles, out T result);
        T Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, ReadOnlySpan<char> separator2, NumberStyles styles, IFormatProvider? provider);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, ReadOnlySpan<char> separator2, NumberStyles styles, IFormatProvider? provider, out T result);
    }
}
