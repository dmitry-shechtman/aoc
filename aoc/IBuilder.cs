using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace aoc
{
    public interface IBuilder<T>
    {
        T Parse(ReadOnlySpan<char> input, IFormatProvider provider = null);
        bool TryParse(ReadOnlySpan<char> input, out T result);
        bool TryParse(ReadOnlySpan<char> input, IFormatProvider provider, out T result);

        T Parse(ReadOnlySpan<char> input, NumberStyles styles, IFormatProvider provider = null);
        bool TryParse(ReadOnlySpan<char> input, NumberStyles styles, out T result);
        bool TryParse(ReadOnlySpan<char> input, NumberStyles styles, IFormatProvider provider, out T result);

        T Parse(ReadOnlySpan<char> input, char separator, IFormatProvider provider = null);
        bool TryParse(ReadOnlySpan<char> input, char separator, out T result);
        bool TryParse(ReadOnlySpan<char> input, char separator, IFormatProvider provider, out T result);

        T Parse(ReadOnlySpan<char> input, char separator, NumberStyles styles, IFormatProvider provider = null);
        bool TryParse(ReadOnlySpan<char> input, char separator, NumberStyles styles, out T result);
        bool TryParse(ReadOnlySpan<char> input, char separator, NumberStyles styles, IFormatProvider provider, out T result);

        T Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, IFormatProvider provider = null);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, out T result);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, IFormatProvider provider, out T result);

        T Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, NumberStyles styles, IFormatProvider provider = null);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, NumberStyles styles, out T result);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, NumberStyles styles, IFormatProvider provider, out T result);

        T Parse(string input, Regex separator, IFormatProvider provider = null);
        bool TryParse(string input, Regex separator, out T result);
        bool TryParse(string input, Regex separator, IFormatProvider provider, out T result);

        T ParseAny(string input, IFormatProvider provider = null);
        bool TryParseAny(string input, out T result);
        bool TryParseAny(string input, IFormatProvider provider, out T result);
        
        T ParseAny(string input, NumberStyles styles, IFormatProvider provider = null);
        bool TryParseAny(string input, NumberStyles styles, out T result);
        bool TryParseAny(string input, NumberStyles styles, IFormatProvider provider, out T result);

        T[] ParseAll(string input, IFormatProvider provider = null);
        bool TryParseAll(string input, out T[] results);
        bool TryParseAll(string input, IFormatProvider provider, out T[] results);

        T[] ParseAll(string input, NumberStyles styles, IFormatProvider provider = null);
        bool TryParseAll(string input, NumberStyles styles, out T[] results);
        bool TryParseAll(string input, NumberStyles styles, IFormatProvider provider, out T[] results);

        T[] ParseAll(string input, IFormatProvider provider, int size);
        bool TryParseAll(string input, int size, out T[] results);
        bool TryParseAll(string input, IFormatProvider provider, int size, out T[] results);

        T[] ParseAll(string input, NumberStyles styles, IFormatProvider provider, int size);
        bool TryParseAll(string input, NumberStyles styles, int size, out T[] results);
        bool TryParseAll(string input, NumberStyles styles, IFormatProvider provider, int size, out T[] results);
    }
}
