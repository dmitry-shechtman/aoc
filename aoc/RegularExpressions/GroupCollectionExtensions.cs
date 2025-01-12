using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions
{
    public static class GroupCollectionExtensions
    {
        #region Backports

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
        public static IEnumerable<Group> Skip(this GroupCollection groups, Range range)
        {
            var (offset, length) = range.GetOffsetAndLength(groups.Count);
            return groups.Skip(offset).Take(length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(this GroupCollection groups,
            Func<Group, TKey> keySelector,
            Func<Group, TElement> elementSelector)
            where TKey : notnull =>
                Enumerable.ToDictionary(groups, keySelector, elementSelector);
#endif

        #endregion

        #region Skip

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IEnumerable<Group> Skip(this GroupCollection groups, Range? range) =>
            range is null ? groups : Skip(groups, range.Value);

        #endregion

        #region GetGroupValues

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[] GetGroupValues(this GroupCollection groups,
            Range? range = null) =>
                groups.Skip(range)
                    .Select(g => g.Value).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetGroupValues<T>(this GroupCollection groups,
            Range? range = null, IFormatProvider? provider = null)
                where T : IConvertible =>
                    groups.Skip(range)
                        .Select(g => g.Value.ConvertTo<T>(provider)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetGroupValues<T>(this GroupCollection groups,
            ParseSpan<T> parse, Range? range = null) =>
                groups.Skip(range)
                    .Select(g => parse(g.ValueSpan)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetGroupValues<T>(this GroupCollection groups,
            ParseSpan<T, IFormatProvider> parse,
            Range? range = null, IFormatProvider? provider = null) =>
                groups.Skip(range)
                    .Select(g => parse(g.ValueSpan, provider)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetGroupValues<T>(this GroupCollection groups,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            Range? range = null, IFormatProvider? provider = null, NumberStyles styles = 0) =>
                groups.Skip(range)
                    .Select(g => parse(g.ValueSpan, styles, provider)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetGroupValues<T>(this GroupCollection groups,
            Range? range = null, CultureInfo? culture = null) =>
                GetGroupValues<T>(groups, Util.GetConverter<T>(), range, culture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetGroupValues<T>(this GroupCollection groups,
            TypeConverter converter, Range? range = null, CultureInfo? culture = null) =>
                groups.Skip(range)
                    .Select(g => converter.ConvertFromString<T>(g.Value, culture)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetGroupValuesInvariant<T>(this GroupCollection groups,
            Range? range = null) =>
                GetGroupValuesInvariant<T>(groups, Util.GetConverter<T>(), range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetGroupValuesInvariant<T>(this GroupCollection groups,
            TypeConverter converter, Range? range = null) =>
                groups.Skip(range)
                    .Select(g => converter.ConvertFromInvariantString<T>(g.Value)).ToArray();

        #endregion

        #region EnumerateGroupValues

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<string> EnumerateGroupValues(this GroupCollection groups,
            Range? range = null) =>
                groups.Skip(range)
                    .Select(g => g.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateGroupValues<T>(this GroupCollection groups,
            Range? range = null, IFormatProvider? provider = null)
                where T : IConvertible =>
                    groups.Skip(range)
                        .Select(g => g.Value.ConvertTo<T>(provider));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateGroupValues<T>(this GroupCollection groups,
            ParseSpan<T> parse, Range? range = null) =>
                groups.Skip(range)
                    .Select(g => parse(g.ValueSpan));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateGroupValues<T>(this GroupCollection groups,
            ParseSpan<T, IFormatProvider> parse,
            Range? range = null, IFormatProvider? provider = null) =>
                groups.Skip(range)
                    .Select(g => parse(g.ValueSpan, provider));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateGroupValues<T>(this GroupCollection groups,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            Range? range = null, IFormatProvider? provider = null, NumberStyles styles = 0) =>
                groups.Skip(range)
                    .Select(g => parse(g.ValueSpan, styles, provider));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateGroupValues<T>(this GroupCollection groups,
            Range? range, CultureInfo? culture) =>
                EnumerateGroupValues<T>(groups, Util.GetConverter<T>(), range, culture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateGroupValues<T>(this GroupCollection groups,
            TypeConverter converter, Range? range = null, CultureInfo? culture = null) =>
                groups.Skip(range)
                    .Select(g => converter.ConvertFromString<T>(g.Value, culture));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateGroupValuesInvariant<T>(this GroupCollection groups,
            Range? range = null) =>
                EnumerateGroupValuesInvariant<T>(groups, Util.GetConverter<T>(), range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateGroupValuesInvariant<T>(this GroupCollection groups,
            TypeConverter converter, Range? range = null) =>
                groups.Skip(range)
                    .Select(g => converter.ConvertFromInvariantString<T>(g.Value));

        #endregion

        #region GetValues

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[][] GetValues(this GroupCollection groups,
            Range? range = null) =>
                groups.Skip(range).Select(g => g.GetValues()).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[][] GetValues(this GroupCollection groups,
            params Index[] indices) =>
                indices.Select(i => groups[i].GetValues()).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[][] GetValues(this GroupCollection groups,
            params string[] keys) =>
                keys.Select(k => groups[k].GetValues()).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            Range? range, IFormatProvider? provider = null)
                where T : IConvertible =>
                    groups.Skip(range).Select(g => g.GetValues<T>(provider)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            IFormatProvider? provider, params Index[] indices)
                where T : IConvertible =>
                    indices.Select(i => groups[i].GetValues<T>(provider)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            IFormatProvider? provider, params string[] keys)
                where T : IConvertible =>
                    keys.Select(k => groups[k].GetValues<T>(provider)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            CultureInfo? culture, Range? range = null) =>
                GetValues<T>(groups, Util.GetConverter<T>(), culture, range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            CultureInfo? culture, params Index[] indices) =>
                GetValues<T>(groups, Util.GetConverter<T>(), culture, indices);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            CultureInfo? culture, params string[] keys) =>
                GetValues<T>(groups, Util.GetConverter<T>(), culture, keys);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            ParseSpan<T> parse, params Index[] indices) =>
                indices.Select(i => groups[i].GetValues(parse)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            ParseSpan<T> parse, params string[] keys) =>
                keys.Select(k => groups[k].GetValues(parse)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            ParseSpan<T, IFormatProvider> parse,
            IFormatProvider? provider, params Index[] indices) =>
                indices.Select(i => groups[i].GetValues(parse, provider)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            ParseSpan<T, IFormatProvider> parse,
            IFormatProvider? provider, params string[] keys) =>
                keys.Select(k => groups[k].GetValues(parse, provider)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            IFormatProvider? provider, NumberStyles styles, params Index[] indices) =>
                indices.Select(i => groups[i].GetValues(parse, provider, styles)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            IFormatProvider? provider, NumberStyles styles, params string[] keys) =>
                keys.Select(k => groups[k].GetValues(parse, provider, styles)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            ParseSpan<T> parse, Range? range = null) =>
                groups.Skip(range)
                    .Select(g => g.GetValues(parse)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            ParseSpan<T, IFormatProvider> parse,
            Range? range = null, IFormatProvider? provider = null) =>
                groups.Skip(range)
                    .Select(g => g.GetValues(parse, provider)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            Range? range = null, IFormatProvider? provider = null, NumberStyles styles = 0) =>
                groups.Skip(range)
                    .Select(g => g.GetValues(parse, provider, styles)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            TypeConverter converter, CultureInfo? culture, Range? range = null) =>
                groups.Skip(range)
                    .Select(g => g.GetValues<T>(converter, culture)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            TypeConverter converter, CultureInfo? culture, params Index[] indices) =>
                indices.Select(i => groups[i].GetValues<T>(converter, culture)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            TypeConverter converter, CultureInfo? culture, params string[] keys) =>
                keys.Select(k => groups[k].GetValues<T>(converter, culture)).ToArray();

        #endregion

        #region GetValuesInvariant

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValuesInvariant<T>(this GroupCollection groups,
            Range? range = null) =>
                GetValuesInvariant<T>(groups, Util.GetConverter<T>(), range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValuesInvariant<T>(this GroupCollection groups,
            params Index[] indices) =>
                GetValuesInvariant<T>(groups, Util.GetConverter<T>(), indices);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValuesInvariant<T>(this GroupCollection groups,
            params string[] keys) =>
                GetValuesInvariant<T>(groups, Util.GetConverter<T>(), keys);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValuesInvariant<T>(this GroupCollection groups,
            TypeConverter converter, Range? range = null) =>
                groups.Skip(range)
                    .Select(g => g.GetValuesInvariant<T>(converter)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValuesInvariant<T>(this GroupCollection groups,
            TypeConverter converter, params Index[] indices) =>
                indices.Select(i => groups[i].GetValuesInvariant<T>(converter)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValuesInvariant<T>(this GroupCollection groups,
            TypeConverter converter, params string[] keys) =>
                keys.Select(k => groups[k].GetValuesInvariant<T>(converter)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValuesInvariant<T>(this GroupCollection groups,
            ParseSpan<T, IFormatProvider> parse,
            Range? range = null) =>
                groups.Skip(range)
                    .Select(g => g.GetValuesInvariant(parse)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValuesInvariant<T>(this GroupCollection groups,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            Range? range = null, NumberStyles styles = 0) =>
                groups.Skip(range)
                    .Select(g => g.GetValuesInvariant(parse, styles)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValuesInvariant<T>(this GroupCollection groups,
            ParseSpan<T, IFormatProvider> parse, Index index) =>
                groups[index].GetValuesInvariant(parse);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValuesInvariant<T>(this GroupCollection groups,
            ParseSpan<T, IFormatProvider> parse, string key) =>
                groups[key].GetValuesInvariant(parse);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValuesInvariant<T>(this GroupCollection groups,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            Index index, NumberStyles styles = 0) =>
                groups[index].GetValuesInvariant(parse, styles);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValuesInvariant<T>(this GroupCollection groups,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            string key, NumberStyles styles = 0) =>
                groups[key].GetValuesInvariant(parse, styles);

        #endregion

        #region EnumerateValues

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<string>[] EnumerateValues(this GroupCollection groups,
            Range? range = null) =>
                groups.Skip(range).Select(g => g.EnumerateValues()).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<string>[] EnumerateValues(this GroupCollection groups,
            params Index[] indices) =>
                indices.Select(i => groups[i].EnumerateValues()).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<string>[] EnumerateValues(this GroupCollection groups,
            params string[] keys) =>
                keys.Select(k => groups[k].EnumerateValues()).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            Range? range, IFormatProvider? provider = null)
                where T : IConvertible =>
                    groups.Skip(range).Select(g => g.EnumerateValues<T>(provider)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            IFormatProvider? provider, params Index[] indices)
                where T : IConvertible =>
                    indices.Select(i => groups[i].EnumerateValues<T>(provider)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            IFormatProvider? provider, params string[] keys)
                where T : IConvertible =>
                    keys.Select(k => groups[k].EnumerateValues<T>(provider)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            CultureInfo? culture, Range? range = null) =>
                EnumerateValues<T>(groups, Util.GetConverter<T>(), culture, range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            CultureInfo? culture, params Index[] indices) =>
                EnumerateValues<T>(groups, Util.GetConverter<T>(), culture, indices);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            CultureInfo? culture, params string[] keys) =>
                EnumerateValues<T>(groups, Util.GetConverter<T>(), culture, keys);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            ParseSpan<T> parse, params Index[] indices) =>
                indices.Select(i => groups[i].EnumerateValues(parse)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            ParseSpan<T> parse, params string[] keys) =>
                keys.Select(k => groups[k].EnumerateValues(parse)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            ParseSpan<T, IFormatProvider> parse,
            IFormatProvider? provider, params Index[] indices) =>
                indices.Select(i => groups[i].EnumerateValues(parse, provider)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            ParseSpan<T, IFormatProvider> parse,
            IFormatProvider? provider, params string[] keys) =>
                keys.Select(k => groups[k].EnumerateValues(parse, provider)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            IFormatProvider? provider, NumberStyles styles, params Index[] indices) =>
                indices.Select(i => groups[i].EnumerateValues(parse, provider, styles)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            IFormatProvider? provider, NumberStyles styles, params string[] keys) =>
                keys.Select(k => groups[k].EnumerateValues(parse, provider, styles)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            ParseSpan<T> parse, Range? range = null) =>
                groups.Skip(range)
                    .Select(g => g.EnumerateValues(parse)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            ParseSpan<T, IFormatProvider> parse,
            Range? range = null, IFormatProvider? provider = null) =>
                groups.Skip(range)
                    .Select(g => g.EnumerateValues(parse, provider)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            Range? range = null, IFormatProvider? provider = null, NumberStyles styles = 0) =>
                groups.Skip(range)
                    .Select(g => g.EnumerateValues(parse, provider, styles)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            TypeConverter converter, CultureInfo? culture, Range? range = null) =>
                groups.Skip(range)
                    .Select(g => g.EnumerateValues<T>(converter, culture)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            TypeConverter converter, CultureInfo? culture, params Index[] indices) =>
                indices.Select(i => groups[i].EnumerateValues<T>(converter, culture)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            TypeConverter converter, CultureInfo? culture, params string[] keys) =>
                keys.Select(k => groups[k].EnumerateValues<T>(converter, culture)).ToArray();

        #endregion

        #region EnumerateValuesInvariant

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValuesInvariant<T>(this GroupCollection groups,
            Range? range = null) =>
                EnumerateValuesInvariant<T>(groups, Util.GetConverter<T>(), range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValuesInvariant<T>(this GroupCollection groups,
            params Index[] indices) =>
                EnumerateValuesInvariant<T>(groups, Util.GetConverter<T>(), indices);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValuesInvariant<T>(this GroupCollection groups,
            params string[] keys) =>
                EnumerateValuesInvariant<T>(groups, Util.GetConverter<T>(), keys);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValuesInvariant<T>(this GroupCollection groups,
            TypeConverter converter, Range? range = null) =>
                groups.Skip(range)
                    .Select(g => g.EnumerateValuesInvariant<T>(converter)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValuesInvariant<T>(this GroupCollection groups,
            TypeConverter converter, params Index[] indices) =>
                indices.Select(i => groups[i].EnumerateValuesInvariant<T>(converter)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValuesInvariant<T>(this GroupCollection groups,
            TypeConverter converter, params string[] keys) =>
                keys.Select(k => groups[k].EnumerateValuesInvariant<T>(converter)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValuesInvariant<T>(this GroupCollection groups,
            ParseSpan<T, IFormatProvider> parse,
            Range? range = null) =>
                groups.Skip(range)
                    .Select(g => g.EnumerateValuesInvariant(parse)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValuesInvariant<T>(this GroupCollection groups,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            Range? range = null, NumberStyles styles = 0) =>
                groups.Skip(range)
                    .Select(g => g.EnumerateValuesInvariant(parse, styles)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateValuesInvariant<T>(this GroupCollection groups,
            ParseSpan<T, IFormatProvider> parse, Index index) =>
                groups[index].EnumerateValuesInvariant(parse);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateValuesInvariant<T>(this GroupCollection groups,
            ParseSpan<T, IFormatProvider> parse, string key) =>
                groups[key].EnumerateValuesInvariant(parse);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateValuesInvariant<T>(this GroupCollection groups,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            Index index, NumberStyles styles = 0) =>
                groups[index].EnumerateValuesInvariant(parse, styles);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateValuesInvariant<T>(this GroupCollection groups,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            string key, NumberStyles styles = 0) =>
                groups[key].EnumerateValuesInvariant(parse, styles);

        #endregion

        #region GetAllValues

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
            ParseSpan<T> parse, Range? range = null) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.GetValues(parse));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, T[]> GetAllValues<T>(this GroupCollection groups,
            ParseSpan<T, IFormatProvider> parse,
            Range? range = null, IFormatProvider? provider = null) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.GetValues(parse, provider));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, T[]> GetAllValues<T>(this GroupCollection groups,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            Range? range = null, IFormatProvider? provider = null, NumberStyles styles = 0) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.GetValues(parse, provider, styles));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, T[]> GetAllValues<T>(this GroupCollection groups,
            Range? range, CultureInfo? culture) =>
                GetAllValues<T>(groups, Util.GetConverter<T>(), range, culture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, T[]> GetAllValues<T>(this GroupCollection groups,
            TypeConverter converter, Range? range = null, CultureInfo? culture = null) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.GetValues<T>(converter, culture));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, T[]> GetAllValuesInvariant<T>(this GroupCollection groups,
            Range? range = null) =>
                GetAllValuesInvariant<T>(groups, Util.GetConverter<T>(), range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, T[]> GetAllValuesInvariant<T>(this GroupCollection groups,
            TypeConverter converter, Range? range = null) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.GetValuesInvariant<T>(converter));

        #endregion

        #region EnumerateAllValues

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, IEnumerable<string>> EnumerateAllValues(this GroupCollection groups,
            Range? range = null) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.EnumerateValues());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, IEnumerable<T>> EnumerateAllValues<T>(this GroupCollection groups,
            Range? range = null, IFormatProvider? provider = null)
                where T : IConvertible =>
                    groups.Skip(range)
                        .ToDictionary(g => g.Name, g => g.EnumerateValues<T>(provider));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, IEnumerable<T>> EnumerateAllValues<T>(this GroupCollection groups,
            ParseSpan<T> parse, Range? range = null) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.EnumerateValues(parse));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, IEnumerable<T>> EnumerateAllValues<T>(this GroupCollection groups,
            ParseSpan<T, IFormatProvider> parse,
            Range? range = null, IFormatProvider? provider = null) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.EnumerateValues(parse, provider));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, IEnumerable<T>> EnumerateAllValues<T>(this GroupCollection groups,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            Range? range = null, IFormatProvider? provider = null, NumberStyles styles = 0) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.EnumerateValues(parse, provider, styles));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, IEnumerable<T>> EnumerateAllValues<T>(this GroupCollection groups,
            Range? range = null, CultureInfo? culture = null) =>
                EnumerateAllValues<T>(groups, Util.GetConverter<T>(), range, culture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, IEnumerable<T>> EnumerateAllValues<T>(this GroupCollection groups,
            TypeConverter converter, Range? range = null, CultureInfo? culture = null) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.EnumerateValues<T>(converter, culture));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, IEnumerable<T>> EnumerateAllValuesInvariant<T>(this GroupCollection groups,
            Range? range = null) =>
                EnumerateAllValuesInvariant<T>(groups, Util.GetConverter<T>(), range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, IEnumerable<T>> EnumerateAllValuesInvariant<T>(this GroupCollection groups,
            TypeConverter converter, Range? range = null) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.EnumerateValuesInvariant<T>(converter));

        #endregion
    }
}
