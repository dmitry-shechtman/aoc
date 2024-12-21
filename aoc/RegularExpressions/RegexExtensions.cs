using System.Collections.Generic;
using System.Linq;

namespace System.Text.RegularExpressions
{
    public static class RegexExtensions
    {
        public static T[] GetValues<T>(this Regex regex, string input)
            where T : IConvertible =>
                GetValues(regex, input, StringExtensions.ConvertTo<T>);

        public static T[] GetValues<T>(this Regex regex, string input,
            Func<string, T> selector) =>
                regex.Match(input).Groups.GetValues(selector);

        public static IEnumerable<T[]> SelectValuesMany<T>(this Regex regex, string input,
            Range range = default)
                where T : IConvertible =>
                    SelectValuesMany(regex, input, StringExtensions.ConvertTo<T>, range);

        public static IEnumerable<T[]> SelectValuesMany<T>(this Regex regex, string input,
            Func<string, T> selector, Range range = default) =>
                regex.Matches(input)
                    .Select(m => m.Groups.GetValues(selector, range));

        public static Dictionary<string, T[]> GetAllValues<T>(this Regex regex, string input,
            Range range = default)
                where T : IConvertible =>
                    GetAllValues(regex, input, StringExtensions.ConvertTo<T>, range);

        public static Dictionary<string, T[]> GetAllValues<T>(this Regex regex, string input,
            Func<string, T> selector, Range range = default) =>
                regex.Match(input).Groups.GetAllValues(selector, range);

        public static Dictionary<string, IEnumerable<T>> SelectAllValues<T>(this Regex regex, string input,
            Range range = default)
                where T : IConvertible =>
                    SelectAllValues(regex, input, StringExtensions.ConvertTo<T>, range);

        public static Dictionary<string, IEnumerable<T>> SelectAllValues<T>(this Regex regex, string input,
            Func<string, T> selector, Range range = default) =>
                regex.Match(input).Groups.SelectAllValues(selector, range);
    }
}
