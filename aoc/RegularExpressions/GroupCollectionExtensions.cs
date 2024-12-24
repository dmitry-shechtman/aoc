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

        public static IEnumerable<Group> Skip(this GroupCollection groups, Range? range)
        {
            var (offset, length) = range?.GetOffsetAndLength(groups.Count)
                ?? (0, groups.Count);
            return groups.Skip(offset).Take(length);
        }

        public static Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(this GroupCollection groups,
            Func<Group, TKey> keySelector,
            Func<Group, TElement> elementSelector)
            where TKey : notnull =>
                Enumerable.ToDictionary(groups, keySelector, elementSelector);
#endif

        public static T[] GetValues<T>(this GroupCollection groups, Range? range = null)
            where T : IConvertible =>
                GetValues(groups, StringExtensions.ConvertTo<T>, range);

        public static T[] GetValues<T>(this GroupCollection groups,
            Func<string, T> selector, Range? range = null) =>
                groups.Skip(range)
                    .Select(g => selector(g.Value)).ToArray();

        public static IEnumerable<T> SelectValues<T>(this GroupCollection groups, Range? range = null)
            where T : IConvertible =>
                SelectValues(groups, StringExtensions.ConvertTo<T>, range);

        public static IEnumerable<T> SelectValues<T>(this GroupCollection groups,
            Func<string, T> selector, Range? range = null) =>
                groups.Skip(range)
                    .Select(g => selector(g.Value));

        public static Dictionary<string, T[]> GetAllValues<T>(this GroupCollection groups, Range? range = null)
            where T : IConvertible =>
                GetAllValues(groups, StringExtensions.ConvertTo<T>, range);

        public static Dictionary<string, T[]> GetAllValues<T>(this GroupCollection groups,
            Func<string, T> selector, Range? range = null) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.GetValues(selector));

        public static Dictionary<string, IEnumerable<T>> SelectAllValues<T>(this GroupCollection groups,
            Range? range = null)
                where T : IConvertible =>
                    SelectAllValues(groups, StringExtensions.ConvertTo<T>, range);

        public static Dictionary<string, IEnumerable<T>> SelectAllValues<T>(this GroupCollection groups,
            Func<string, T> selector, Range? range = null) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.SelectValues(selector));
    }
}
