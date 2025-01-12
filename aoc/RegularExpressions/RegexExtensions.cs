using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace System.Text.RegularExpressions
{
    public delegate T ParseSpan<T>(ReadOnlySpan<char> s);
    public delegate T ParseSpan<T, TArg>(ReadOnlySpan<char> s, TArg? arg);
    public delegate T ParseSpan<T, TArg1, TArg2>(ReadOnlySpan<char> s, TArg1? arg1, TArg2? arg2);

    public static class RegexExtensions
    {
        #region GetGroupValues

        public static string[] GetGroupValues(this Regex regex, string input) =>
            regex.Match(input).Groups.GetGroupValues();

        public static T[] GetGroupValues<T>(this Regex regex, string input,
            IFormatProvider? provider = null)
                where T : IConvertible =>
                    regex.Match(input).Groups
                        .GetGroupValues<T>(provider: provider);

        public static T[] GetGroupValues<T>(this Regex regex, string input,
            ParseSpan<T> parse) =>
                regex.Match(input).Groups
                    .GetGroupValues(parse);

        public static T[] GetGroupValues<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> parse,
            IFormatProvider? provider = null) =>
                regex.Match(input).Groups
                    .GetGroupValues(parse, provider: provider);

        public static T[] GetGroupValues<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            IFormatProvider? provider = null, NumberStyles styles = 0) =>
                regex.Match(input).Groups
                    .GetGroupValues(parse, provider: provider, styles: styles);

        public static T[] GetGroupValues<T>(this Regex regex, string input,
            CultureInfo? culture) =>
                regex.Match(input).Groups
                    .GetGroupValues<T>(culture: culture);

        public static T[] GetGroupValues<T>(this Regex regex, string input,
            TypeConverter converter, CultureInfo? culture = null) =>
                regex.Match(input).Groups
                    .GetGroupValues<T>(converter, culture: culture);

        public static T[] GetGroupValuesInvariant<T>(this Regex regex, string input) =>
            regex.Match(input).Groups
                .GetGroupValuesInvariant<T>();

        public static T[] GetGroupValuesInvariant<T>(this Regex regex, string input,
            TypeConverter converter) =>
                regex.Match(input).Groups
                    .GetGroupValuesInvariant<T>(converter);

        #endregion

        #region EnumerateGroupValues

        public static IEnumerable<string> EnumerateGroupValues(this Regex regex, string input) =>
            regex.Match(input).Groups.EnumerateGroupValues();

        public static IEnumerable<T> EnumerateGroupValues<T>(this Regex regex, string input,
            IFormatProvider? provider = null)
                where T : IConvertible =>
                    regex.Match(input).Groups
                        .EnumerateGroupValues<T>(provider: provider);

        public static IEnumerable<T> EnumerateGroupValues<T>(this Regex regex, string input,
            ParseSpan<T> parse) =>
                regex.Match(input).Groups
                    .EnumerateGroupValues(parse);

        public static IEnumerable<T> EnumerateGroupValues<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> parse,
            IFormatProvider? provider = null) =>
                regex.Match(input).Groups
                    .EnumerateGroupValues(parse, provider: provider);

        public static IEnumerable<T> EnumerateGroupValues<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            IFormatProvider? provider = null, NumberStyles styles = 0) =>
                regex.Match(input).Groups
                    .EnumerateGroupValues(parse, provider: provider, styles: styles);

        public static IEnumerable<T> EnumerateGroupValues<T>(this Regex regex, string input,
            CultureInfo? culture) =>
                regex.Match(input).Groups
                    .EnumerateGroupValues<T>(null, culture);

        public static IEnumerable<T> EnumerateGroupValues<T>(this Regex regex, string input,
            TypeConverter converter, CultureInfo? culture = null) =>
                regex.Match(input).Groups
                    .EnumerateGroupValues<T>(converter, null, culture);

        public static IEnumerable<T> EnumerateGroupValuesInvariant<T>(this Regex regex, string input,
            Range? range = null) =>
                regex.Match(input).Groups
                    .EnumerateGroupValuesInvariant<T>(range);

        public static IEnumerable<T> EnumerateGroupValuesInvariant<T>(this Regex regex, string input,
            TypeConverter converter, Range? range = null) =>
                regex.Match(input).Groups
                    .EnumerateGroupValuesInvariant<T>(converter, range);

        #endregion

        #region EnumerateValuesMany

        public static IEnumerable<string[]> EnumerateValuesMany(this Regex regex, string input,
            Range? range = null) =>
                regex.Matches(input)
                    .Select(m => m.Groups
                        .GetGroupValues(range));

        public static IEnumerable<T[]> EnumerateValuesMany<T>(this Regex regex, string input,
            Range? range = null, IFormatProvider? provider = null)
                where T : IConvertible =>
                    regex.Matches(input)
                        .Select(m => m.Groups
                            .GetGroupValues<T>(range, provider));

        public static IEnumerable<T[]> EnumerateValuesMany<T>(this Regex regex, string input,
            ParseSpan<T> parse, Range? range = null) =>
                regex.Matches(input)
                    .Select(m => m.Groups
                        .GetGroupValues(parse, range));

        public static IEnumerable<T[]> EnumerateValuesMany<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> parse,
            Range? range = null, IFormatProvider? provider = null) =>
                regex.Matches(input)
                    .Select(m => m.Groups
                        .GetGroupValues(parse, range, provider));

        public static IEnumerable<T[]> EnumerateValuesMany<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            Range? range = null, IFormatProvider? provider = null, NumberStyles styles = 0) =>
                regex.Matches(input)
                    .Select(m => m.Groups
                        .GetGroupValues(parse, range, provider, styles));

        public static IEnumerable<T[]> EnumerateValuesMany<T>(this Regex regex, string input,
            Range? range, CultureInfo? culture) =>
                regex.Matches(input)
                    .Select(m => m.Groups.GetGroupValues<T>(range, culture));

        public static IEnumerable<T[]> EnumerateValuesMany<T>(this Regex regex, string input,
            TypeConverter converter, Range? range = null, CultureInfo? culture = null) =>
                regex.Matches(input)
                    .Select(m => m.Groups.GetGroupValues<T>(converter, range, culture));

        public static IEnumerable<T[]> EnumerateValuesManyInvariant<T>(this Regex regex, string input,
            Range? range = null) =>
                regex.Matches(input)
                    .Select(m => m.Groups.GetGroupValuesInvariant<T>(range));

        public static IEnumerable<T[]> EnumerateValuesManyInvariant<T>(this Regex regex, string input,
            TypeConverter converter, Range? range = null) =>
                regex.Matches(input)
                    .Select(m => m.Groups.GetGroupValuesInvariant<T>(converter, range));

        #endregion

        #region GetValues

        public static string[] GetValues(this Regex regex, string input, Index index) =>
            regex.Match(input).Groups[index].GetValues();

        public static string[] GetValues(this Regex regex, string input, string key) =>
            regex.Match(input).Groups[key].GetValues();

        public static string[][] GetValues(this Regex regex, string input,
            Range? range = null) =>
                regex.Match(input).Groups.GetValues(range);

        public static string[][] GetValues(this Regex regex, string input,
            params Index[] indices) =>
                regex.Match(input).Groups.GetValues(indices);

        public static string[][] GetValues(this Regex regex, string input,
            params string[] keys) =>
                regex.Match(input).Groups.GetValues(keys);

        public static T[] GetValues<T>(this Regex regex, string input, Index index,
            IFormatProvider? provider = null)
                where T : IConvertible =>
                    regex.Match(input).Groups[index]
                        .GetValues<T>(provider);

        public static T[] GetValues<T>(this Regex regex, string input, string key,
            IFormatProvider? provider = null)
                where T : IConvertible =>
                    regex.Match(input).Groups[key]
                        .GetValues<T>(provider);

        public static T[] GetValues<T>(this Regex regex, string input,
            ParseSpan<T> parse, Index index) =>
                regex.Match(input).Groups[index]
                    .GetValues(parse);

        public static T[] GetValues<T>(this Regex regex, string input,
            ParseSpan<T> parse, string key) =>
                regex.Match(input).Groups[key]
                    .GetValues(parse);

        public static T[] GetValues<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> parse, Index index,
            IFormatProvider? provider = null) =>
                regex.Match(input).Groups[index]
                    .GetValues(parse, provider);

        public static T[] GetValues<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> parse, string key,
            IFormatProvider? provider = null) =>
                regex.Match(input).Groups[key]
                    .GetValues(parse, provider);

        public static T[] GetValues<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> parse, Index index,
            IFormatProvider? provider = null, NumberStyles styles = 0) =>
                regex.Match(input).Groups[index]
                    .GetValues(parse, provider, styles);

        public static T[] GetValues<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> parse, string key,
            IFormatProvider? provider = null, NumberStyles styles = 0) =>
                regex.Match(input).Groups[key]
                    .GetValues(parse, provider, styles);

        public static T[][] GetValues<T>(this Regex regex, string input,
            Range? range = null, IFormatProvider? provider = null)
                where T : IConvertible =>
                    regex.Match(input).Groups
                        .GetValues<T>(range, provider);

        public static T[][] GetValues<T>(this Regex regex, string input,
            params Index[] indices)
                where T : IConvertible =>
                    regex.Match(input).Groups
                        .GetValues<T>(provider: null, indices);

        public static T[][] GetValues<T>(this Regex regex, string input,
            params string[] keys)
                where T : IConvertible =>
                    regex.Match(input).Groups
                        .GetValues<T>(provider: null, keys);

        public static T[][] GetValues<T>(this Regex regex, string input,
            IFormatProvider? provider, params Index[] indices)
                where T : IConvertible =>
                    regex.Match(input).Groups
                        .GetValues<T>(provider, indices);

        public static T[][] GetValues<T>(this Regex regex, string input,
            IFormatProvider? provider, params string[] keys)
                where T : IConvertible =>
                    regex.Match(input).Groups
                        .GetValues<T>(provider, keys);

        public static T[] GetValues<T>(this Regex regex, string input,
            Index index, CultureInfo? culture = null) =>
                regex.Match(input).Groups[index]
                    .GetValues<T>(culture);

        public static T[] GetValues<T>(this Regex regex, string input,
            string key, CultureInfo? culture = null) =>
                regex.Match(input).Groups[key]
                    .GetValues<T>(culture);

        public static T[][] GetValues<T>(this Regex regex, string input,
            CultureInfo? culture, Range? range = null) =>
                regex.Match(input).Groups
                    .GetValues<T>(culture, range);

        public static T[][] GetValues<T>(this Regex regex, string input,
            CultureInfo? culture, params Index[] indices) =>
                regex.Match(input).Groups
                    .GetValues<T>(culture, indices);

        public static T[][] GetValues<T>(this Regex regex, string input,
            CultureInfo? culture, params string[] keys) =>
                regex.Match(input).Groups
                    .GetValues<T>(culture, keys);

        public static T[] GetValues<T>(this Regex regex, string input,
            TypeConverter converter, Index index, CultureInfo? culture = null) =>
                regex.Match(input).Groups[index]
                    .GetValues<T>(converter, culture);

        public static T[] GetValues<T>(this Regex regex, string input,
            TypeConverter converter, string key, CultureInfo? culture = null) =>
                regex.Match(input).Groups[key]
                    .GetValues<T>(converter, culture);

        public static T[][] GetValues<T>(this Regex regex, string input,
            TypeConverter converter, CultureInfo? culture, Range? range = null) =>
                regex.Match(input).Groups
                    .GetValues<T>(converter, culture, range);

        public static T[][] GetValues<T>(this Regex regex, string input,
            TypeConverter converter, CultureInfo? culture, params Index[] indices) =>
                regex.Match(input).Groups
                    .GetValues<T>(converter, culture, indices);

        public static T[][] GetValues<T>(this Regex regex, string input,
            TypeConverter converter, CultureInfo? culture, params string[] keys) =>
                regex.Match(input).Groups
                    .GetValues<T>(converter, culture, keys);

        public static T[][] GetValues<T>(this Regex regex, string input,
            ParseSpan<T> parse, params Index[] indices) =>
                regex.Match(input).Groups
                    .GetValues(parse, indices);

        public static T[][] GetValues<T>(this Regex regex, string input,
            ParseSpan<T> parse, params string[] keys) =>
                regex.Match(input).Groups
                    .GetValues(parse, keys);

        public static T[][] GetValues<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> parse,
            IFormatProvider? provider, params Index[] indices) =>
                regex.Match(input).Groups
                    .GetValues(parse, provider, indices);

        public static T[][] GetValues<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> parse,
            IFormatProvider? provider, params string[] keys) =>
                regex.Match(input).Groups
                    .GetValues(parse, provider, keys);

        public static T[][] GetValues<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            IFormatProvider? provider, NumberStyles styles, params Index[] indices) =>
                regex.Match(input).Groups
                    .GetValues(parse, provider, styles, indices);

        public static T[][] GetValues<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            IFormatProvider? provider, NumberStyles styles, params string[] keys) =>
                regex.Match(input).Groups
                    .GetValues(parse, provider, styles, keys);

        public static T[][] GetValues<T>(this Regex regex, string input,
            ParseSpan<T> parse, Range? range = null) =>
                regex.Match(input).Groups
                    .GetValues(parse, range);

        public static T[][] GetValues<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> parse,
            Range? range = null, IFormatProvider? provider = null) =>
                regex.Match(input).Groups
                    .GetValues(parse, range, provider);

        public static T[][] GetValues<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            Range? range = null, IFormatProvider? provider = null, NumberStyles styles = 0) =>
                regex.Match(input).Groups
                    .GetValues(parse, range, provider, styles);

        #endregion

        #region GetValuesInvariant

        public static T[] GetValuesInvariant<T>(this Regex regex, string input,
            Index index) =>
                regex.Match(input).Groups[index]
                    .GetValuesInvariant<T>();

        public static T[] GetValuesInvariant<T>(this Regex regex, string input,
            string key) =>
                regex.Match(input).Groups[key]
                    .GetValuesInvariant<T>();

        public static T[][] GetValuesInvariant<T>(this Regex regex, string input,
            Range? range = null) =>
                regex.Match(input).Groups
                    .GetValuesInvariant<T>(range);

        public static T[][] GetValuesInvariant<T>(this Regex regex, string input,
            params Index[] indices) =>
                regex.Match(input).Groups
                    .GetValuesInvariant<T>(indices);

        public static T[][] GetValuesInvariant<T>(this Regex regex, string input,
            params string[] keys) =>
                regex.Match(input).Groups
                    .GetValuesInvariant<T>(keys);

        public static T[] GetValuesInvariant<T>(this Regex regex, string input,
            TypeConverter converter, Index index) =>
                regex.Match(input).Groups[index]
                    .GetValuesInvariant<T>(converter);

        public static T[] GetValuesInvariant<T>(this Regex regex, string input,
            TypeConverter converter, string key) =>
                regex.Match(input).Groups[key]
                    .GetValuesInvariant<T>(converter);

        public static T[][] GetValuesInvariant<T>(this Regex regex, string input,
            TypeConverter converter, Range? range = null) =>
                regex.Match(input).Groups
                    .GetValuesInvariant<T>(converter, range);

        public static T[][] GetValuesInvariant<T>(this Regex regex, string input,
            TypeConverter converter, params Index[] indices) =>
                regex.Match(input).Groups
                    .GetValuesInvariant<T>(converter, indices);

        public static T[][] GetValuesInvariant<T>(this Regex regex, string input,
            TypeConverter converter, params string[] keys) =>
                regex.Match(input).Groups
                    .GetValuesInvariant<T>(converter, keys);

        public static T[][] GetValuesInvariant<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> parse,
            Range? range = null) =>
                regex.Match(input).Groups
                    .GetValuesInvariant(parse, range);

        public static T[][] GetValuesInvariant<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            Range? range = null, NumberStyles styles = 0) =>
                regex.Match(input).Groups
                    .GetValuesInvariant(parse, range, styles);

        public static T[] GetValuesInvariant<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> parse, Index index) =>
                regex.Match(input).Groups
                    .GetValuesInvariant(parse, index);

        public static T[] GetValuesInvariant<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> parse, string key) =>
                regex.Match(input).Groups
                    .GetValuesInvariant(parse, key);

        public static T[] GetValuesInvariant<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> parse, Index index,
            NumberStyles styles = 0) =>
                regex.Match(input).Groups
                    .GetValuesInvariant(parse, index, styles);

        public static T[] GetValuesInvariant<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> parse, string key,
            NumberStyles styles = 0) =>
                regex.Match(input).Groups
                    .GetValuesInvariant(parse, key, styles);

        #endregion

        #region EnumerateValues

        public static IEnumerable<string> EnumerateValues(this Regex regex, string input, Index index) =>
            regex.Match(input).Groups[index].EnumerateValues();

        public static IEnumerable<string> EnumerateValues(this Regex regex, string input, string key) =>
            regex.Match(input).Groups[key].EnumerateValues();

        public static IEnumerable<string>[] EnumerateValues(this Regex regex, string input,
            Range? range = null) =>
                regex.Match(input).Groups.EnumerateValues(range);

        public static IEnumerable<string>[] EnumerateValues(this Regex regex, string input,
            params Index[] indices) =>
                regex.Match(input).Groups.EnumerateValues(indices);

        public static IEnumerable<string>[] EnumerateValues(this Regex regex, string input,
            params string[] keys) =>
                regex.Match(input).Groups.EnumerateValues(keys);

        public static IEnumerable<T> EnumerateValues<T>(this Regex regex, string input, Index index,
            IFormatProvider? provider = null)
                where T : IConvertible =>
                    regex.Match(input).Groups[index]
                        .EnumerateValues<T>(provider);

        public static IEnumerable<T> EnumerateValues<T>(this Regex regex, string input, string key,
            IFormatProvider? provider = null)
                where T : IConvertible =>
                    regex.Match(input).Groups[key]
                        .EnumerateValues<T>(provider);

        public static IEnumerable<T> EnumerateValues<T>(this Regex regex, string input,
            ParseSpan<T> parse, Index index) =>
                regex.Match(input).Groups[index]
                    .EnumerateValues(parse);

        public static IEnumerable<T> EnumerateValues<T>(this Regex regex, string input,
            ParseSpan<T> parse, string key) =>
                regex.Match(input).Groups[key]
                    .EnumerateValues(parse);

        public static IEnumerable<T> EnumerateValues<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> parse, Index index,
            IFormatProvider? provider = null) =>
                regex.Match(input).Groups[index]
                    .EnumerateValues(parse, provider);

        public static IEnumerable<T> EnumerateValues<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> parse, string key,
            IFormatProvider? provider = null) =>
                regex.Match(input).Groups[key]
                    .EnumerateValues(parse, provider);

        public static IEnumerable<T> EnumerateValues<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> parse, Index index,
            IFormatProvider? provider = null, NumberStyles styles = 0) =>
                regex.Match(input).Groups[index]
                    .EnumerateValues(parse, provider, styles);

        public static IEnumerable<T> EnumerateValues<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> parse, string key,
            IFormatProvider? provider = null, NumberStyles styles = 0) =>
                regex.Match(input).Groups[key]
                    .EnumerateValues(parse, provider, styles);

        public static IEnumerable<T>[] EnumerateValues<T>(this Regex regex, string input,
            Range? range = null, IFormatProvider? provider = null)
                where T : IConvertible =>
                    regex.Match(input).Groups
                        .EnumerateValues<T>(range, provider);

        public static IEnumerable<T>[] EnumerateValues<T>(this Regex regex, string input,
            params Index[] indices)
                where T : IConvertible =>
                    regex.Match(input).Groups
                        .EnumerateValues<T>(provider: null, indices);

        public static IEnumerable<T>[] EnumerateValues<T>(this Regex regex, string input,
            params string[] keys)
                where T : IConvertible =>
                    regex.Match(input).Groups
                        .EnumerateValues<T>(provider: null, keys);

        public static IEnumerable<T>[] EnumerateValues<T>(this Regex regex, string input,
            IFormatProvider? provider, params Index[] indices)
                where T : IConvertible =>
                    regex.Match(input).Groups
                        .EnumerateValues<T>(provider, indices);

        public static IEnumerable<T>[] EnumerateValues<T>(this Regex regex, string input,
            IFormatProvider? provider, params string[] keys)
                where T : IConvertible =>
                    regex.Match(input).Groups
                        .EnumerateValues<T>(provider, keys);

        public static IEnumerable<T> EnumerateValues<T>(this Regex regex, string input,
            Index index, CultureInfo? culture = null) =>
                regex.Match(input).Groups[index]
                    .EnumerateValues<T>(culture);

        public static IEnumerable<T> EnumerateValues<T>(this Regex regex, string input,
            string key, CultureInfo? culture = null) =>
                regex.Match(input).Groups[key]
                    .EnumerateValues<T>(culture);

        public static IEnumerable<T>[] EnumerateValues<T>(this Regex regex, string input,
            CultureInfo? culture, Range? range = null) =>
                regex.Match(input).Groups
                    .EnumerateValues<T>(culture, range);

        public static IEnumerable<T>[] EnumerateValues<T>(this Regex regex, string input,
            CultureInfo? culture, params Index[] indices) =>
                regex.Match(input).Groups
                    .EnumerateValues<T>(culture, indices);

        public static IEnumerable<T>[] EnumerateValues<T>(this Regex regex, string input,
            CultureInfo? culture, params string[] keys) =>
                regex.Match(input).Groups
                    .EnumerateValues<T>(culture, keys);

        public static IEnumerable<T> EnumerateValues<T>(this Regex regex, string input,
            TypeConverter converter, Index index, CultureInfo? culture = null) =>
                regex.Match(input).Groups[index]
                    .EnumerateValues<T>(converter, culture);

        public static IEnumerable<T> EnumerateValues<T>(this Regex regex, string input,
            TypeConverter converter, string key, CultureInfo? culture = null) =>
                regex.Match(input).Groups[key]
                    .EnumerateValues<T>(converter, culture);

        public static IEnumerable<T>[] EnumerateValues<T>(this Regex regex, string input,
            TypeConverter converter, CultureInfo? culture, Range? range = null) =>
                regex.Match(input).Groups
                    .EnumerateValues<T>(converter, culture, range);

        public static IEnumerable<T>[] EnumerateValues<T>(this Regex regex, string input,
            TypeConverter converter, CultureInfo? culture, params Index[] indices) =>
                regex.Match(input).Groups
                    .EnumerateValues<T>(converter, culture, indices);

        public static IEnumerable<T>[] EnumerateValues<T>(this Regex regex, string input,
            TypeConverter converter, CultureInfo? culture, params string[] keys) =>
                regex.Match(input).Groups
                    .EnumerateValues<T>(converter, culture, keys);

        public static IEnumerable<T>[] EnumerateValues<T>(this Regex regex, string input,
            ParseSpan<T> parse, params Index[] indices) =>
                regex.Match(input).Groups
                    .EnumerateValues(parse, indices);

        public static IEnumerable<T>[] EnumerateValues<T>(this Regex regex, string input,
            ParseSpan<T> parse, params string[] keys) =>
                regex.Match(input).Groups
                    .EnumerateValues(parse, keys);

        public static IEnumerable<T>[] EnumerateValues<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> parse,
            IFormatProvider? provider, params Index[] indices) =>
                regex.Match(input).Groups
                    .EnumerateValues(parse, provider, indices);

        public static IEnumerable<T>[] EnumerateValues<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> parse,
            IFormatProvider? provider, params string[] keys) =>
                regex.Match(input).Groups
                    .EnumerateValues(parse, provider, keys);

        public static IEnumerable<T>[] EnumerateValues<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            IFormatProvider? provider, NumberStyles styles, params Index[] indices) =>
                regex.Match(input).Groups
                    .EnumerateValues(parse, provider, styles, indices);

        public static IEnumerable<T>[] EnumerateValues<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            IFormatProvider? provider, NumberStyles styles, params string[] keys) =>
                regex.Match(input).Groups
                    .EnumerateValues(parse, provider, styles, keys);

        public static IEnumerable<T>[] EnumerateValues<T>(this Regex regex, string input,
            ParseSpan<T> parse, Range? range = null) =>
                regex.Match(input).Groups
                    .EnumerateValues(parse, range);

        public static IEnumerable<T>[] EnumerateValues<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> parse,
            Range? range = null, IFormatProvider? provider = null) =>
                regex.Match(input).Groups
                    .EnumerateValues(parse, range, provider);

        public static IEnumerable<T>[] EnumerateValues<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            Range? range = null, IFormatProvider? provider = null, NumberStyles styles = 0) =>
                regex.Match(input).Groups
                    .EnumerateValues(parse, range, provider, styles);

        #endregion

        #region EnumerateValuesInvariant

        public static IEnumerable<T> EnumerateValuesInvariant<T>(this Regex regex, string input,
            Index index) =>
                regex.Match(input).Groups[index]
                    .EnumerateValuesInvariant<T>();

        public static IEnumerable<T> EnumerateValuesInvariant<T>(this Regex regex, string input,
            string key) =>
                regex.Match(input).Groups[key]
                    .EnumerateValuesInvariant<T>();

        public static IEnumerable<T>[] EnumerateValuesInvariant<T>(this Regex regex, string input,
            Range? range = null) =>
                regex.Match(input).Groups
                    .EnumerateValuesInvariant<T>(range);

        public static IEnumerable<T>[] EnumerateValuesInvariant<T>(this Regex regex, string input,
            params Index[] indices) =>
                regex.Match(input).Groups
                    .EnumerateValuesInvariant<T>(indices);

        public static IEnumerable<T>[] EnumerateValuesInvariant<T>(this Regex regex, string input,
            params string[] keys) =>
                regex.Match(input).Groups
                    .EnumerateValuesInvariant<T>(keys);

        public static IEnumerable<T> EnumerateValuesInvariant<T>(this Regex regex, string input,
            TypeConverter converter, Index index) =>
                regex.Match(input).Groups[index]
                    .EnumerateValuesInvariant<T>(converter);

        public static IEnumerable<T> EnumerateValuesInvariant<T>(this Regex regex, string input,
            TypeConverter converter, string key) =>
                regex.Match(input).Groups[key]
                    .EnumerateValuesInvariant<T>(converter);

        public static IEnumerable<T>[] EnumerateValuesInvariant<T>(this Regex regex, string input,
            TypeConverter converter, Range? range = null) =>
                regex.Match(input).Groups
                    .EnumerateValuesInvariant<T>(converter, range);

        public static IEnumerable<T>[] EnumerateValuesInvariant<T>(this Regex regex, string input,
            TypeConverter converter, params Index[] indices) =>
                regex.Match(input).Groups
                    .EnumerateValuesInvariant<T>(converter, indices);

        public static IEnumerable<T>[] EnumerateValuesInvariant<T>(this Regex regex, string input,
            TypeConverter converter, params string[] keys) =>
                regex.Match(input).Groups
                    .EnumerateValuesInvariant<T>(converter, keys);

        public static IEnumerable<T>[] EnumerateValuesInvariant<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> parse,
            Range? range = null) =>
                regex.Match(input).Groups
                    .EnumerateValuesInvariant(parse, range);

        public static IEnumerable<T>[] EnumerateValuesInvariant<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            Range? range = null, NumberStyles styles = 0) =>
                regex.Match(input).Groups
                    .EnumerateValuesInvariant(parse, range, styles);

        public static IEnumerable<T> EnumerateValuesInvariant<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> parse, Index index) =>
                regex.Match(input).Groups
                    .EnumerateValuesInvariant(parse, index);

        public static IEnumerable<T> EnumerateValuesInvariant<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> parse, string key) =>
                regex.Match(input).Groups
                    .EnumerateValuesInvariant(parse, key);

        public static IEnumerable<T> EnumerateValuesInvariant<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> parse, Index index,
            NumberStyles styles = 0) =>
                regex.Match(input).Groups
                    .EnumerateValuesInvariant(parse, index, styles);

        public static IEnumerable<T> EnumerateValuesInvariant<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> parse, string key,
            NumberStyles styles = 0) =>
                regex.Match(input).Groups
                    .EnumerateValuesInvariant(parse, key, styles);

        #endregion

        #region GetAllValues

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
            ParseSpan<T> parse, Range? range = null) =>
                regex.Match(input).Groups
                    .GetAllValues(parse, range);

        public static Dictionary<string, T[]> GetAllValues<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> parse,
            Range? range = null, IFormatProvider? provider = null) =>
                regex.Match(input).Groups
                    .GetAllValues(parse, range, provider);

        public static Dictionary<string, T[]> GetAllValues<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            Range? range = null, IFormatProvider? provider = null, NumberStyles styles = 0) =>
                regex.Match(input).Groups
                    .GetAllValues(parse, range, provider, styles);

        public static Dictionary<string, T[]> GetAllValues<T>(this Regex regex, string input,
            Range? range, CultureInfo? culture) =>
                regex.Match(input).Groups
                    .GetAllValues<T>(range, culture);

        public static Dictionary<string, T[]> GetAllValues<T>(this Regex regex, string input,
            TypeConverter converter, Range? range = null, CultureInfo? culture = null) =>
                regex.Match(input).Groups
                    .GetAllValues<T>(converter, range, culture);

        public static Dictionary<string, T[]> GetAllValuesInvariant<T>(this Regex regex, string input,
            Range? range = null) =>
                regex.Match(input).Groups
                    .GetAllValuesInvariant<T>(range);

        public static Dictionary<string, T[]> GetAllValuesInvariant<T>(this Regex regex, string input,
            TypeConverter converter, Range? range = null) =>
                regex.Match(input).Groups
                    .GetAllValuesInvariant<T>(converter, range);

        #endregion

        #region EnumerateAllValues

        public static Dictionary<string, IEnumerable<string>> EnumerateAllValues(this Regex regex, string input,
            Range? range = null) =>
                regex.Match(input).Groups
                    .EnumerateAllValues(range);

        public static Dictionary<string, IEnumerable<T>> EnumerateAllValues<T>(this Regex regex, string input,
            Range? range = null, IFormatProvider? provider = null)
                where T : IConvertible =>
                    regex.Match(input).Groups
                        .EnumerateAllValues<T>(range, provider);

        public static Dictionary<string, IEnumerable<T>> EnumerateAllValues<T>(this Regex regex, string input,
            ParseSpan<T> parse, Range? range = null) =>
                regex.Match(input).Groups
                    .EnumerateAllValues(parse, range);

        public static Dictionary<string, IEnumerable<T>> EnumerateAllValues<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> parse,
            Range? range = null, IFormatProvider? provider = null) =>
                regex.Match(input).Groups
                    .EnumerateAllValues(parse, range, provider);

        public static Dictionary<string, IEnumerable<T>> EnumerateAllValues<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            Range? range = null, IFormatProvider? provider = null, NumberStyles styles = 0) =>
                regex.Match(input).Groups
                    .EnumerateAllValues(parse, range, provider, styles);

        public static Dictionary<string, IEnumerable<T>> EnumerateAllValues<T>(this Regex regex, string input,
            Range? range, CultureInfo? culture) =>
                regex.Match(input).Groups
                    .EnumerateAllValues<T>(range, culture);

        public static Dictionary<string, IEnumerable<T>> EnumerateAllValues<T>(this Regex regex, string input,
            TypeConverter converter, Range? range, CultureInfo? culture) =>
                regex.Match(input).Groups
                    .EnumerateAllValues<T>(converter, range, culture);

        public static Dictionary<string, IEnumerable<T>> EnumerateAllValuesInvariant<T>(this Regex regex, string input,
            Range? range = null) =>
                regex.Match(input).Groups
                    .EnumerateAllValuesInvariant<T>(range);

        public static Dictionary<string, IEnumerable<T>> EnumerateAllValuesInvariant<T>(this Regex regex, string input,
            TypeConverter converter, Range? range = null) =>
                regex.Match(input).Groups
                    .EnumerateAllValuesInvariant<T>(converter, range);

        #endregion
    }
}
