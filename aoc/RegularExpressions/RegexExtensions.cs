using System.Collections.Generic;
using System.Linq;

namespace System.Text.RegularExpressions
{
    public static class RegexExtensions
    {
        public static string[] GetValues(this Regex regex, string input) =>
            GetValues(regex, input, s => s);

        public static T[] GetValues<T>(this Regex regex, string input,
            IFormatProvider? provider = null)
                where T : IConvertible =>
                    GetValues(regex, input, StringExtensions.ConvertTo<T>, provider);

        public static T[] GetValues<T>(this Regex regex, string input,
            Func<string, T> selector) =>
                regex.Match(input).Groups.GetValues(selector);

        public static T[] GetValues<T>(this Regex regex, string input,
            Func<string, IFormatProvider?, T> selector,
            IFormatProvider? provider = null) =>
                regex.Match(input).Groups.GetValues(selector, provider: provider);

        public static IEnumerable<string[]> SelectValuesMany(this Regex regex, string input,
            Range? range = null) =>
                SelectValuesMany(regex, input, s => s, range);

        public static IEnumerable<T[]> SelectValuesMany<T>(this Regex regex, string input,
            Range? range = null, IFormatProvider? provider = null)
                where T : IConvertible =>
                    SelectValuesMany(regex, input, StringExtensions.ConvertTo<T>, range, provider);

        public static IEnumerable<T[]> SelectValuesMany<T>(this Regex regex, string input,
            Func<string, T> selector, Range? range = null) =>
                regex.Matches(input)
                    .Select(m => m.Groups.GetValues(selector, range));

        public static IEnumerable<T[]> SelectValuesMany<T>(this Regex regex, string input,
            Func<string, IFormatProvider?, T> selector,
            Range? range = null, IFormatProvider? provider = null) =>
                regex.Matches(input)
                    .Select(m => m.Groups.GetValues(selector, range, provider));

        public static Dictionary<string, string[]> GetAllValues(this Regex regex, string input,
            Range? range = null) =>
                GetAllValues(regex, input, s => s, range);

        public static Dictionary<string, T[]> GetAllValues<T>(this Regex regex, string input,
            Range? range = null, IFormatProvider? provider = null)
                where T : IConvertible =>
                    GetAllValues(regex, input, StringExtensions.ConvertTo<T>, range, provider);

        public static Dictionary<string, T[]> GetAllValues<T>(this Regex regex, string input,
            Func<string, T> selector, Range? range = null) =>
                regex.Match(input).Groups.GetAllValues(selector, range);

        public static Dictionary<string, T[]> GetAllValues<T>(this Regex regex, string input,
            Func<string, IFormatProvider?, T> selector,
            Range? range = null, IFormatProvider? provider = null) =>
                regex.Match(input).Groups.GetAllValues(selector, range, provider);

        public static Dictionary<string, IEnumerable<string>> SelectAllValues(this Regex regex, string input,
            Range? range = null) =>
                SelectAllValues(regex, input, s => s, range);

        public static Dictionary<string, IEnumerable<T>> SelectAllValues<T>(this Regex regex, string input,
            Range? range = null, IFormatProvider? provider = null)
                where T : IConvertible =>
                    SelectAllValues(regex, input, StringExtensions.ConvertTo<T>, range, provider);

        public static Dictionary<string, IEnumerable<T>> SelectAllValues<T>(this Regex regex, string input,
            Func<string, T> selector, Range? range = null) =>
                regex.Match(input).Groups.SelectAllValues(selector, range);

        public static Dictionary<string, IEnumerable<T>> SelectAllValues<T>(this Regex regex, string input,
            Func<string, IFormatProvider?, T> selector,
            Range? range = null, IFormatProvider? provider = null) =>
                regex.Match(input).Groups.SelectAllValues(selector, range, provider);
    }
}
