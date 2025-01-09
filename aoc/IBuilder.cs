using System;
using System.Globalization;

namespace aoc
{
    public interface IBuilder<T>
    {
        T Parse(ReadOnlySpan<char> input) =>
            Parse(input, provider: null);
        bool TryParse(ReadOnlySpan<char> input, out T result);
        T Parse(ReadOnlySpan<char> input, IFormatProvider provider);
        bool TryParse(ReadOnlySpan<char> input, IFormatProvider provider, out T result);

        T Parse(ReadOnlySpan<char> input, NumberStyles styles) =>
            Parse(input, styles, null);
        bool TryParse(ReadOnlySpan<char> input, NumberStyles styles, out T result);
        T Parse(ReadOnlySpan<char> input, NumberStyles styles, IFormatProvider provider);
        bool TryParse(ReadOnlySpan<char> input, NumberStyles styles, IFormatProvider provider, out T result);

        T Parse(ReadOnlySpan<char> input, char separator) =>
            Parse(input, separator, null);
        bool TryParse(ReadOnlySpan<char> input, char separator, out T result);
        T Parse(ReadOnlySpan<char> input, char separator, IFormatProvider provider);
        bool TryParse(ReadOnlySpan<char> input, char separator, IFormatProvider provider, out T result);

        T Parse(ReadOnlySpan<char> input, char separator, NumberStyles styles) =>
            Parse(input, separator, styles, null);
        bool TryParse(ReadOnlySpan<char> input, char separator, NumberStyles styles, out T result);
        T Parse(ReadOnlySpan<char> input, char separator, NumberStyles styles, IFormatProvider provider);
        bool TryParse(ReadOnlySpan<char> input, char separator, NumberStyles styles, IFormatProvider provider, out T result);

        T Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator) =>
            Parse(input, separator, null);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, out T result);
        T Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, IFormatProvider provider);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, IFormatProvider provider, out T result);

        T Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, NumberStyles styles) =>
            Parse(input, separator, styles, null);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, NumberStyles styles, out T result);
        T Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, NumberStyles styles, IFormatProvider provider);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, NumberStyles styles, IFormatProvider provider, out T result);

        T ParseAny(string input) =>
            ParseAny(input, null);
        bool TryParseAny(string input, out T result);
        T ParseAny(string input, IFormatProvider provider);
        bool TryParseAny(string input, IFormatProvider provider, out T result);

        T ParseAny(string input, NumberStyles styles) =>
            ParseAny(input, styles, null);
        bool TryParseAny(string input, NumberStyles styles, out T result);
        T ParseAny(string input, NumberStyles styles, IFormatProvider provider);
        bool TryParseAny(string input, NumberStyles styles, IFormatProvider provider, out T result);

        T[] ParseAll(string input) =>
            ParseAll(input, null);
        bool TryParseAll(string input, out T[] results);
        T[] ParseAll(string input, IFormatProvider provider);
        bool TryParseAll(string input, IFormatProvider provider, out T[] results);

        T[] ParseAll(string input, NumberStyles styles) =>
            ParseAll(input, styles, null);
        bool TryParseAll(string input, NumberStyles styles, out T[] results);
        T[] ParseAll(string input, NumberStyles styles, IFormatProvider provider);
        bool TryParseAll(string input, NumberStyles styles, IFormatProvider provider, out T[] results);

        T[] ParseAll(string input, int size) =>
            ParseAll(input, null, size);
        bool TryParseAll(string input, int size, out T[] results);
        T[] ParseAll(string input, IFormatProvider provider, int size);
        bool TryParseAll(string input, IFormatProvider provider, int size, out T[] results);

        T[] ParseAll(string input, NumberStyles styles, int size) =>
            ParseAll(input, styles, null, size);
        bool TryParseAll(string input, NumberStyles styles, int size, out T[] results);
        T[] ParseAll(string input, NumberStyles styles, IFormatProvider provider, int size);
        bool TryParseAll(string input, NumberStyles styles, IFormatProvider provider, int size, out T[] results);
    }
}
