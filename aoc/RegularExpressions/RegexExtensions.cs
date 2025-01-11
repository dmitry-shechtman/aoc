using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System.Text.RegularExpressions
{
    public delegate T ParseSpan<T>(ReadOnlySpan<char> s);
    public delegate T ParseSpan<T, TArg>(ReadOnlySpan<char> s, TArg? arg);
    public delegate T ParseSpan<T, TArg1, TArg2>(ReadOnlySpan<char> s, TArg1? arg1, TArg2? arg2);

    public static class RegexExtensions
    {
        public static string[] GetValues(this Regex regex, string input) =>
            regex.Match(input).Groups.GetValues();

        public static T[] GetValues<T>(this Regex regex, string input,
            IFormatProvider? provider = null)
                where T : IConvertible =>
                    regex.Match(input).Groups
                        .GetValues<T>(provider: provider);

        public static T[] GetValues<T>(this Regex regex, string input,
            ParseSpan<T> selector) =>
                regex.Match(input).Groups
                    .GetValues(selector);

        public static T[] GetValues<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> selector,
            IFormatProvider? provider = null) =>
                regex.Match(input).Groups
                    .GetValues(selector, provider: provider);

        public static T[] GetValues<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> selector,
            IFormatProvider? provider = null, NumberStyles styles = 0) =>
                regex.Match(input).Groups
                    .GetValues(selector, provider: provider, styles: styles);

        public static IEnumerable<string[]> SelectValuesMany(this Regex regex, string input,
            Range? range = null) =>
                regex.Matches(input)
                    .Select(m => m.Groups
                        .GetValues(range));

        public static IEnumerable<T[]> SelectValuesMany<T>(this Regex regex, string input,
            Range? range = null, IFormatProvider? provider = null)
                where T : IConvertible =>
                    regex.Matches(input)
                        .Select(m => m.Groups
                            .GetValues<T>(range, provider));

        public static IEnumerable<T[]> SelectValuesMany<T>(this Regex regex, string input,
            ParseSpan<T> selector, Range? range = null) =>
                regex.Matches(input)
                    .Select(m => m.Groups
                        .GetValues(selector, range));

        public static IEnumerable<T[]> SelectValuesMany<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> selector,
            Range? range = null, IFormatProvider? provider = null) =>
                regex.Matches(input)
                    .Select(m => m.Groups
                        .GetValues(selector, range, provider));

        public static IEnumerable<T[]> SelectValuesMany<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> selector,
            Range? range = null, IFormatProvider? provider = null, NumberStyles styles = 0) =>
                regex.Matches(input)
                    .Select(m => m.Groups
                        .GetValues(selector, range, provider, styles));

        public static Dictionary<string, string[]> GetAllValues(this Regex regex, string input,
            Range? range = null) =>
                regex.Match(input).Groups
                    .GetAllValues(range);

        public static Dictionary<string, T[]> GetAllValues<T>(this Regex regex, string input,
            Range? range = null, IFormatProvider? provider = null)
                where T : IConvertible =>
                    regex.Match(input).Groups
                        .GetAllValues<T>(range, provider);

        public static Dictionary<string, T[]> GetAllValues<T>(this Regex regex, string input,
            ParseSpan<T> selector, Range? range = null) =>
                regex.Match(input).Groups
                    .GetAllValues(selector, range);

        public static Dictionary<string, T[]> GetAllValues<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> selector,
            Range? range = null, IFormatProvider? provider = null) =>
                regex.Match(input).Groups
                    .GetAllValues(selector, range, provider);

        public static Dictionary<string, T[]> GetAllValues<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> selector,
            Range? range = null, IFormatProvider? provider = null, NumberStyles styles = 0) =>
                regex.Match(input).Groups
                    .GetAllValues(selector, range, provider, styles);

        public static Dictionary<string, IEnumerable<string>> SelectAllValues(this Regex regex, string input,
            Range? range = null) =>
                regex.Match(input).Groups
                    .SelectAllValues(range);

        public static Dictionary<string, IEnumerable<T>> SelectAllValues<T>(this Regex regex, string input,
            Range? range = null, IFormatProvider? provider = null)
                where T : IConvertible =>
                    regex.Match(input).Groups
                        .SelectAllValues<T>(range, provider);

        public static Dictionary<string, IEnumerable<T>> SelectAllValues<T>(this Regex regex, string input,
            ParseSpan<T> selector, Range? range = null) =>
                regex.Match(input).Groups
                    .SelectAllValues(selector, range);

        public static Dictionary<string, IEnumerable<T>> SelectAllValues<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> selector,
            Range? range = null, IFormatProvider? provider = null) =>
                regex.Match(input).Groups
                    .SelectAllValues(selector, range, provider);

        public static Dictionary<string, IEnumerable<T>> SelectAllValues<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> selector,
            Range? range = null, IFormatProvider? provider = null, NumberStyles styles = 0) =>
                regex.Match(input).Groups
                    .SelectAllValues(selector, range, provider, styles);
    }
}
