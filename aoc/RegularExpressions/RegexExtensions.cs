using System.Collections.Generic;
using System.Linq;

namespace System.Text.RegularExpressions
{
    public static class RegexExtensions
    {
        public static string[] GetStrings(this Regex regex, string input) =>
            GetValues(regex, input, s => s);

        public static int[] GetInts(this Regex regex, string input) =>
            GetValues(regex, input, int.Parse);

        public static long[] GetLongs(this Regex regex, string input) =>
            GetValues(regex, input, long.Parse);

        public static T[] GetValues<T>(this Regex regex, string input, Func<string, T> selector) =>
            regex.Match(input).Groups.GetValues(selector);

        public static IEnumerable<string[]> SelectStringsMany(this Regex regex, string input) =>
            regex.Matches(input).Select(MatchExtensions.GetStrings);

        public static IEnumerable<int[]> SelectIntsMany(this Regex regex, string input) =>
            SelectValuesMany(regex, input, int.Parse);

        public static IEnumerable<long[]> SelectLongsMany(this Regex regex, string input) =>
            SelectValuesMany(regex, input, long.Parse);

        public static IEnumerable<T[]> SelectValuesMany<T>(this Regex regex, string input,
            Func<string, T> selector) =>
                regex.Matches(input)
                    .Select(m => m.Groups.GetValues(selector));

        public static Dictionary<string, string[]> GetAllStrings(this Regex regex, string input) =>
            regex.Match(input).Groups.GetAllStrings();

        public static Dictionary<string, int[]> GetAllInts(this Regex regex, string input) =>
            regex.Match(input).Groups.GetAllInts();

        public static Dictionary<string, long[]> GetAllLongs(this Regex regex, string input) =>
            regex.Match(input).Groups.GetAllLongs();

        public static Dictionary<string, T[]> GetAllValues<T>(this Regex regex, string input,
            Func<Group, T[]> selector) =>
                regex.Match(input).Groups.GetAllValues(selector);
    }
}
