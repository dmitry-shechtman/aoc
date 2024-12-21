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

        public static IEnumerable<Group> Skip(this GroupCollection groups, Range range)
        {
            var start = GetIndex(range.Start, range, groups.Count);
            var end = GetIndex(range.End, range, groups.Count, groups.Count);
            return groups.Skip(start).Take(end - start);
        }

        public static Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(this GroupCollection groups,
            Func<Group, TKey> keySelector,
            Func<Group, TElement> elementSelector)
            where TKey : notnull =>
                Enumerable.ToDictionary(groups, keySelector, elementSelector);

        private static int GetIndex(Index index, Range range, int count, int @default = 0) =>
            index.IsFromEnd
                ? count - index.Value
                : range.Equals(default)
                    ? @default
                    : index.Value;
#endif

        public static T[] GetValues<T>(this GroupCollection groups, Range range = default)
            where T : IConvertible =>
                GetValues(groups, StringExtensions.ConvertTo<T>, range);

        public static T[] GetValues<T>(this GroupCollection groups,
            Func<string, T> selector, Range range = default) =>
                groups.Skip(range)
                    .Select(g => selector(g.Value)).ToArray();

        public static IEnumerable<T> SelectValues<T>(this GroupCollection groups, Range range = default)
            where T : IConvertible =>
                SelectValues(groups, StringExtensions.ConvertTo<T>, range);

        public static IEnumerable<T> SelectValues<T>(this GroupCollection groups,
            Func<string, T> selector, Range range = default) =>
                groups.Skip(range)
                    .Select(g => selector(g.Value));

        public static Dictionary<string, T[]> GetAllValues<T>(this GroupCollection groups, Range range = default)
            where T : IConvertible =>
                GetAllValues(groups, StringExtensions.ConvertTo<T>, range);

        public static Dictionary<string, T[]> GetAllValues<T>(this GroupCollection groups,
            Func<string, T> selector, Range range = default) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.GetValues(selector));

        public static Dictionary<string, IEnumerable<T>> SelectAllValues<T>(this GroupCollection groups,
            Range range = default)
                where T : IConvertible =>
                    SelectAllValues(groups, StringExtensions.ConvertTo<T>, range);

        public static Dictionary<string, IEnumerable<T>> SelectAllValues<T>(this GroupCollection groups,
            Func<string, T> selector, Range range = default) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.SelectValues(selector));
    }
}
