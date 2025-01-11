using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions
{
    public static class GroupCollectionExtensions
    {
#if !NET8_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TResult> Select<TResult>(this GroupCollection groups, Func<Group, TResult> selector) =>
            Enumerable.Select(groups, selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Group> Skip(this GroupCollection groups, int count) =>
            Enumerable.Skip<Group>(groups, count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Group> Take(this GroupCollection groups, int count) =>
            Enumerable.Take<Group>(groups, count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Group> Skip(GroupCollection groups, Range range)
        {
            var (offset, length) = range.GetOffsetAndLength(groups.Count);
            return groups.Skip(offset).Take(length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<Group> Skip(this GroupCollection groups, Range? range) =>
            range is null ? groups : Skip(groups, range.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(this GroupCollection groups,
            Func<Group, TKey> keySelector,
            Func<Group, TElement> elementSelector)
            where TKey : notnull =>
                Enumerable.ToDictionary(groups, keySelector, elementSelector);
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[] GetValues(this GroupCollection groups,
            Range? range = null) =>
                groups.Skip(range)
                    .Select(g => g.Value).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>(this GroupCollection groups,
            Range? range = null, IFormatProvider? provider = null)
                where T : IConvertible =>
                    groups.Skip(range)
                        .Select(g => g.Value.ConvertTo<T>(provider)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>(this GroupCollection groups,
            ParseSpan<T> selector, Range? range = null) =>
                groups.Skip(range)
                    .Select(g => selector(g.ValueSpan)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>(this GroupCollection groups,
            ParseSpan<T, IFormatProvider> selector,
            Range? range = null, IFormatProvider? provider = null) =>
                groups.Skip(range)
                    .Select(g => selector(g.ValueSpan, provider)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>(this GroupCollection groups,
            ParseSpan<T, NumberStyles, IFormatProvider> selector,
            Range? range = null, IFormatProvider? provider = null, NumberStyles styles = 0) =>
                groups.Skip(range)
                    .Select(g => selector(g.ValueSpan, styles, provider)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<string> SelectValues(this GroupCollection groups,
            Range? range = null) =>
                groups.Skip(range)
                    .Select(g => g.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> SelectValues<T>(this GroupCollection groups,
            Range? range = null, IFormatProvider? provider = null)
                where T : IConvertible =>
                    groups.Skip(range)
                        .Select(g => g.Value.ConvertTo<T>(provider));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> SelectValues<T>(this GroupCollection groups,
            ParseSpan<T> selector, Range? range = null) =>
                groups.Skip(range)
                    .Select(g => selector(g.ValueSpan));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> SelectValues<T>(this GroupCollection groups,
            ParseSpan<T, IFormatProvider> selector,
            Range? range = null, IFormatProvider? provider = null) =>
                groups.Skip(range)
                    .Select(g => selector(g.ValueSpan, provider));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> SelectValues<T>(this GroupCollection groups,
            ParseSpan<T, NumberStyles, IFormatProvider> selector,
            Range? range = null, IFormatProvider? provider = null, NumberStyles styles = 0) =>
                groups.Skip(range)
                    .Select(g => selector(g.ValueSpan, styles, provider));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, string[]> GetAllValues(this GroupCollection groups,
            Range? range = null) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.GetValues());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, T[]> GetAllValues<T>(this GroupCollection groups,
            Range? range = null, IFormatProvider? provider = null)
                where T : IConvertible =>
                    groups.Skip(range)
                        .ToDictionary(g => g.Name, g => g.GetValues<T>(provider));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, T[]> GetAllValues<T>(this GroupCollection groups,
            ParseSpan<T> selector, Range? range = null) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.GetValues(selector));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, T[]> GetAllValues<T>(this GroupCollection groups,
            ParseSpan<T, IFormatProvider> selector,
            Range? range = null, IFormatProvider? provider = null) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.GetValues(selector, provider));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, T[]> GetAllValues<T>(this GroupCollection groups,
            ParseSpan<T, NumberStyles, IFormatProvider> selector,
            Range? range = null, IFormatProvider? provider = null, NumberStyles styles = 0) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.GetValues(selector, provider, styles));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, IEnumerable<string>> SelectAllValues(this GroupCollection groups,
            Range? range = null) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.SelectValues());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, IEnumerable<T>> SelectAllValues<T>(this GroupCollection groups,
            Range? range = null, IFormatProvider? provider = null)
                where T : IConvertible =>
                    groups.Skip(range)
                        .ToDictionary(g => g.Name, g => g.SelectValues<T>(provider));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, IEnumerable<T>> SelectAllValues<T>(this GroupCollection groups,
            ParseSpan<T> selector, Range? range = null) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.SelectValues(selector));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, IEnumerable<T>> SelectAllValues<T>(this GroupCollection groups,
            ParseSpan<T, IFormatProvider> selector,
            Range? range = null, IFormatProvider? provider = null) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.SelectValues(selector, provider));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, IEnumerable<T>> SelectAllValues<T>(this GroupCollection groups,
            ParseSpan<T, NumberStyles, IFormatProvider> selector,
            Range? range = null, IFormatProvider? provider = null, NumberStyles styles = 0) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.SelectValues(selector, provider, styles));
    }
}
