using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

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
            ParseSpan<T> selector) =>
                regex.Match(input).Groups
                    .GetGroupValues(selector);

        public static T[] GetGroupValues<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> selector,
            IFormatProvider? provider = null) =>
                regex.Match(input).Groups
                    .GetGroupValues(selector, provider: provider);

        public static T[] GetGroupValues<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> selector,
            IFormatProvider? provider = null, NumberStyles styles = 0) =>
                regex.Match(input).Groups
                    .GetGroupValues(selector, provider: provider, styles: styles);

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
            ParseSpan<T> selector) =>
                regex.Match(input).Groups
                    .EnumerateGroupValues(selector);

        public static IEnumerable<T> EnumerateGroupValues<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> selector,
            IFormatProvider? provider = null) =>
                regex.Match(input).Groups
                    .EnumerateGroupValues(selector, provider: provider);

        public static IEnumerable<T> EnumerateGroupValues<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> selector,
            IFormatProvider? provider = null, NumberStyles styles = 0) =>
                regex.Match(input).Groups
                    .EnumerateGroupValues(selector, provider: provider, styles: styles);

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
            ParseSpan<T> selector, Range? range = null) =>
                regex.Matches(input)
                    .Select(m => m.Groups
                        .GetGroupValues(selector, range));

        public static IEnumerable<T[]> EnumerateValuesMany<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> selector,
            Range? range = null, IFormatProvider? provider = null) =>
                regex.Matches(input)
                    .Select(m => m.Groups
                        .GetGroupValues(selector, range, provider));

        public static IEnumerable<T[]> EnumerateValuesMany<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> selector,
            Range? range = null, IFormatProvider? provider = null, NumberStyles styles = 0) =>
                regex.Matches(input)
                    .Select(m => m.Groups
                        .GetGroupValues(selector, range, provider, styles));

        public static IEnumerable<T[]> EnumerateValuesMany<T>(this Regex regex, string input,
            Range? range, CultureInfo? culture) =>
                EnumerateValuesMany<T>(regex, input, Util.GetConverter<T>(), range, culture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T[]> EnumerateValuesMany<T>(this Regex regex, string input,
            TypeConverter converter, Range? range = null, CultureInfo? culture = null) =>
                regex.Matches(input)
                    .Select(m => m.Groups.GetGroupValues<T>(converter, range, culture));

        public static IEnumerable<T[]> EnumerateValuesManyInvariant<T>(this Regex regex, string input,
            Range? range = null) =>
                EnumerateValuesManyInvariant<T>(regex, input, Util.GetConverter<T>(), range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
                    regex.Match(input).Groups[index].GetValues<T>(provider);

        public static T[] GetValues<T>(this Regex regex, string input, string key,
            IFormatProvider? provider = null)
                where T : IConvertible =>
                    regex.Match(input).Groups[key].GetValues<T>(provider);

        public static T[][] GetValues<T>(this Regex regex, string input,
            Range? range = null, IFormatProvider? provider = null)
                where T : IConvertible =>
                    regex.Match(input).Groups.GetValues<T>(range, provider);

        public static T[][] GetValues<T>(this Regex regex, string input,
            params Index[] indices)
                where T : IConvertible =>
                    regex.Match(input).Groups.GetValues<T>(null, indices);

        public static T[][] GetValues<T>(this Regex regex, string input,
            params string[] keys)
                where T : IConvertible =>
                    regex.Match(input).Groups.GetValues<T>(null, keys);

        public static T[][] GetValues<T>(this Regex regex, string input,
            IFormatProvider? provider, params Index[] indices)
                where T : IConvertible =>
                    regex.Match(input).Groups.GetValues<T>(provider, indices);

        public static T[][] GetValues<T>(this Regex regex, string input,
            IFormatProvider? provider, params string[] keys)
                where T : IConvertible =>
                    regex.Match(input).Groups.GetValues<T>(provider, keys);

        public static T[] GetValues<T>(this Regex regex, string input,
            Index index, CultureInfo? culture = null) =>
                GetValues<T>(regex, input, Util.GetConverter<T>(), index, culture);

        public static T[] GetValues<T>(this Regex regex, string input,
            string key, CultureInfo? culture = null) =>
                GetValues<T>(regex, input, Util.GetConverter<T>(), key, culture);

        public static T[][] GetValues<T>(this Regex regex, string input,
            CultureInfo? culture, Range? range = null) =>
                GetValues<T>(regex, input, Util.GetConverter<T>(), culture, range);

        public static T[][] GetValues<T>(this Regex regex, string input,
            CultureInfo? culture, params Index[] indices) =>
                GetValues<T>(regex, input, Util.GetConverter<T>(), culture, indices);

        public static T[][] GetValues<T>(this Regex regex, string input,
            CultureInfo? culture, params string[] keys) =>
                GetValues<T>(regex, input, Util.GetConverter<T>(), culture, keys);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>(this Regex regex, string input,
            TypeConverter converter, Index index, CultureInfo? culture = null) =>
                regex.Match(input).Groups[index].GetValues<T>(converter, culture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>(this Regex regex, string input,
            TypeConverter converter, string key, CultureInfo? culture = null) =>
                regex.Match(input).Groups[key].GetValues<T>(converter, culture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this Regex regex, string input,
            TypeConverter converter, CultureInfo? culture, Range? range) =>
                regex.Match(input).Groups.GetValues<T>(converter, culture, range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this Regex regex, string input,
            TypeConverter converter, CultureInfo? culture, params Index[] indices) =>
                regex.Match(input).Groups.GetValues<T>(converter, culture, indices);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this Regex regex, string input,
            TypeConverter converter, CultureInfo? culture, params string[] keys) =>
                regex.Match(input).Groups.GetValues<T>(converter, culture, keys);

        #endregion

        #region GetValuesInvariant

        public static T[] GetValuesInvariant<T>(this Regex regex, string input,
            Index index) =>
                GetValuesInvariant<T>(regex, input, Util.GetConverter<T>(), index);

        public static T[] GetValuesInvariant<T>(this Regex regex, string input,
            string key) =>
                GetValuesInvariant<T>(regex, input, Util.GetConverter<T>(), key);

        public static T[][] GetValuesInvariant<T>(this Regex regex, string input,
            Range? range = null) =>
                GetValuesInvariant<T>(regex, input, Util.GetConverter<T>(), range);

        public static T[][] GetValuesInvariant<T>(this Regex regex, string input,
            params Index[] indices) =>
                GetValuesInvariant<T>(regex, input, Util.GetConverter<T>(), indices);

        public static T[][] GetValuesInvariant<T>(this Regex regex, string input,
            params string[] keys) =>
                GetValuesInvariant<T>(regex, input, Util.GetConverter<T>(), keys);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValuesInvariant<T>(this Regex regex, string input,
            TypeConverter converter, Index index) =>
                regex.Match(input).Groups[index].GetValuesInvariant<T>(converter);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValuesInvariant<T>(this Regex regex, string input,
            TypeConverter converter, string key) =>
                regex.Match(input).Groups[key].GetValuesInvariant<T>(converter);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValuesInvariant<T>(this Regex regex, string input,
            TypeConverter converter, Range? range = null) =>
                regex.Match(input).Groups.GetValuesInvariant<T>(converter, range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValuesInvariant<T>(this Regex regex, string input,
            TypeConverter converter, params Index[] indices) =>
                regex.Match(input).Groups.GetValuesInvariant<T>(converter, indices);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValuesInvariant<T>(this Regex regex, string input,
            TypeConverter converter, params string[] keys) =>
                regex.Match(input).Groups.GetValuesInvariant<T>(converter, keys);

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
            ParseSpan<T> selector, Range? range = null) =>
                regex.Match(input).Groups
                    .EnumerateAllValues(selector, range);

        public static Dictionary<string, IEnumerable<T>> EnumerateAllValues<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> selector,
            Range? range = null, IFormatProvider? provider = null) =>
                regex.Match(input).Groups
                    .EnumerateAllValues(selector, range, provider);

        public static Dictionary<string, IEnumerable<T>> EnumerateAllValues<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> selector,
            Range? range = null, IFormatProvider? provider = null, NumberStyles styles = 0) =>
                regex.Match(input).Groups
                    .EnumerateAllValues(selector, range, provider, styles);

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
