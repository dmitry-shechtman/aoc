﻿using System.Collections.Generic;
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
        public static string[] GetGroupValues(this Regex regex, string input) =>
            regex.Match(input).Groups.GetValues();

        public static T[] GetGroupValues<T>(this Regex regex, string input,
            IFormatProvider? provider = null)
                where T : IConvertible =>
                    regex.Match(input).Groups
                        .GetValues<T>(provider: provider);

        public static T[] GetGroupValues<T>(this Regex regex, string input,
            ParseSpan<T> selector) =>
                regex.Match(input).Groups
                    .GetValues(selector);

        public static T[] GetGroupValues<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> selector,
            IFormatProvider? provider = null) =>
                regex.Match(input).Groups
                    .GetValues(selector, provider: provider);

        public static T[] GetGroupValues<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> selector,
            IFormatProvider? provider = null, NumberStyles styles = 0) =>
                regex.Match(input).Groups
                    .GetValues(selector, provider: provider, styles: styles);

        public static T[] GetGroupValues<T>(this Regex regex, string input,
            CultureInfo? culture) =>
                regex.Match(input).Groups
                    .GetValues<T>(culture: culture);

        public static T[] GetGroupValues<T>(this Regex regex, string input,
            TypeConverter converter, CultureInfo? culture = null) =>
                regex.Match(input).Groups
                    .GetValues<T>(converter, culture: culture);

        public static T[] GetGroupValuesInvariant<T>(this Regex regex, string input) =>
            regex.Match(input).Groups
                .GetValuesInvariant<T>();

        public static T[] GetGroupValuesInvariant<T>(this Regex regex, string input,
            TypeConverter converter) =>
                regex.Match(input).Groups
                    .GetValuesInvariant<T>(converter);

        public static IEnumerable<string[]> EnumerateValuesMany(this Regex regex, string input,
            Range? range = null) =>
                regex.Matches(input)
                    .Select(m => m.Groups
                        .GetValues(range));

        public static IEnumerable<T[]> EnumerateValuesMany<T>(this Regex regex, string input,
            Range? range = null, IFormatProvider? provider = null)
                where T : IConvertible =>
                    regex.Matches(input)
                        .Select(m => m.Groups
                            .GetValues<T>(range, provider));

        public static IEnumerable<T[]> EnumerateValuesMany<T>(this Regex regex, string input,
            ParseSpan<T> selector, Range? range = null) =>
                regex.Matches(input)
                    .Select(m => m.Groups
                        .GetValues(selector, range));

        public static IEnumerable<T[]> EnumerateValuesMany<T>(this Regex regex, string input,
            ParseSpan<T, IFormatProvider> selector,
            Range? range = null, IFormatProvider? provider = null) =>
                regex.Matches(input)
                    .Select(m => m.Groups
                        .GetValues(selector, range, provider));

        public static IEnumerable<T[]> EnumerateValuesMany<T>(this Regex regex, string input,
            ParseSpan<T, NumberStyles, IFormatProvider> selector,
            Range? range = null, IFormatProvider? provider = null, NumberStyles styles = 0) =>
                regex.Matches(input)
                    .Select(m => m.Groups
                        .GetValues(selector, range, provider, styles));

        public static IEnumerable<T[]> EnumerateValuesMany<T>(this Regex regex, string input,
            Range? range, CultureInfo? culture)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return converter.CanConvertTo(typeof(T))
                ? EnumerateValuesMany<T>(regex, input, converter, range, culture)
                : throw new InvalidOperationException();
        }

        public static IEnumerable<T[]> EnumerateValuesMany<T>(this Regex regex, string input,
            TypeConverter converter, Range? range = null, CultureInfo? culture = null) =>
                regex.Matches(input)
                    .Select(m => m.Groups.GetValues<T>(converter, range, culture));

        public static IEnumerable<T[]> EnumerateValuesManyInvariant<T>(this Regex regex, string input,
            Range? range = null)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return converter.CanConvertTo(typeof(T))
                ? EnumerateValuesManyInvariant<T>(regex, input, converter, range)
                : throw new InvalidOperationException();
        }

        public static IEnumerable<T[]> EnumerateValuesManyInvariant<T>(this Regex regex, string input,
            TypeConverter converter, Range? range = null) =>
                regex.Matches(input)
                    .Select(m => m.Groups.GetValuesInvariant<T>(converter, range));

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
    }
}
