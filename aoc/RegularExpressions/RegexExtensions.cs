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
            int skipCount = 1)
                where T : IConvertible =>
                    SelectValuesMany(regex, input, StringExtensions.ConvertTo<T>, skipCount);

        public static IEnumerable<T[]> SelectValuesMany<T>(this Regex regex, string input,
            Func<string, T> selector, int skipCount = 1) =>
                regex.Matches(input)
                    .Select(m => m.Groups.GetValues(selector, skipCount));

        public static Dictionary<string, T[]> GetAllValues<T>(this Regex regex, string input,
            int skipCount = 1)
                where T : IConvertible =>
                    GetAllValues(regex, input, StringExtensions.ConvertTo<T>, skipCount);

        public static Dictionary<string, T[]> GetAllValues<T>(this Regex regex, string input,
            Func<string, T> selector, int skipCount = 1) =>
                regex.Match(input).Groups.GetAllValues(selector, skipCount);

        public static Dictionary<string, IEnumerable<T>> SelectAllValues<T>(this Regex regex, string input,
            int skipCount = 1)
                where T : IConvertible =>
                    SelectAllValues(regex, input, StringExtensions.ConvertTo<T>, skipCount);

        public static Dictionary<string, IEnumerable<T>> SelectAllValues<T>(this Regex regex, string input,
            Func<string, T> selector, int skipCount = 1) =>
                regex.Match(input).Groups.SelectAllValues(selector, skipCount);
    }
}
