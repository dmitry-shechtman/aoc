using System.Collections.Generic;
using System.Linq;

namespace System.Text.RegularExpressions
{
    public static class GroupCollectionExtensions
    {
#if !NET8_0_OR_GREATER
        public static IEnumerable<TResult> Select<TResult>(this GroupCollection groups, Func<Group, TResult> selector) =>
            Enumerable.Select(groups, selector);

        public static IEnumerable<Group> Skip(this GroupCollection groups, int count) =>
            Enumerable.Skip<Group>(groups, count);

        public static IEnumerable<Group> Take(this GroupCollection groups, int count) =>
            Enumerable.Take<Group>(groups, count);

        public static Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(this GroupCollection groups,
            Func<Group, TKey> keySelector,
            Func<Group, TElement> elementSelector)
            where TKey : notnull =>
                Enumerable.ToDictionary(groups, keySelector, elementSelector);
#endif

        public static string[] GetStrings(this GroupCollection groups) =>
            groups.Select(g => g.Value).ToArray();

        public static int[] GetInts(this GroupCollection groups) =>
            GetValues(groups, int.Parse);

        public static long[] GetLongs(this GroupCollection groups) =>
            GetValues(groups, long.Parse);

        public static T[] GetValues<T>(this GroupCollection groups,
            Func<string, T> selector) =>
                groups.Skip(1)
                    .Select(g => selector(g.Value)).ToArray();

        public static Dictionary<string, string[]> GetAllStrings(this GroupCollection groups) =>
            GetAllValues(groups, GroupExtensions.GetStrings);

        public static Dictionary<string, int[]> GetAllInts(this GroupCollection groups) =>
            GetAllValues(groups, GroupExtensions.GetInts);

        public static Dictionary<string, long[]> GetAllLongs(this GroupCollection groups) =>
            GetAllValues(groups, GroupExtensions.GetLongs);

        public static Dictionary<string, T[]> GetAllValues<T>(this GroupCollection groups,
            Func<Group, T[]> selector) =>
                groups.Skip(1)
                    .ToDictionary(g => g.Name, selector);
    }
}
