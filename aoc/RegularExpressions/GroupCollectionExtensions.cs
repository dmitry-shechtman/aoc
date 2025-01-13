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
            Range? range = null, CultureInfo? culture = null) =>
                GetGroupValues(groups, TypeConverter<T>.Instance, range, culture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetGroupValues<T>(this GroupCollection groups,
            TypeConverter converter, Range? range = null, CultureInfo? culture = null) =>
                GetGroupValues(groups, new TypeConverter<T>(converter), range, culture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T[] GetGroupValues<T>(this GroupCollection groups,
            TypeConverter<T> converter, Range? range, CultureInfo? culture) =>
                groups.Skip(range)
                    .Select(g => converter.ConvertFromString(g.Value, culture)).ToArray();

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
        public static T[] GetGroupValuesInvariant<T>(this GroupCollection groups,
            Range? range = null) =>
                GetGroupValuesInvariant(groups, TypeConverter<T>.Instance, range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetGroupValuesInvariant<T>(this GroupCollection groups,
            TypeConverter converter, Range? range = null) =>
                GetGroupValuesInvariant(groups, new TypeConverter<T>(converter), range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T[] GetGroupValuesInvariant<T>(this GroupCollection groups,
            TypeConverter<T> converter, Range? range) =>
                groups.Skip(range)
                    .Select(g => converter.ConvertFromInvariantString(g.Value)).ToArray();

        #endregion

        #region EnumerateGroupValues

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<string> EnumerateGroupValues(this GroupCollection groups,
            Range? range = null) =>
                groups.Skip(range)
                    .Select(g => g.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateGroupValues<T>(this GroupCollection groups,
            Range? range = null, CultureInfo? culture = null) =>
                EnumerateGroupValues(groups, TypeConverter<T>.Instance, range, culture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateGroupValues<T>(this GroupCollection groups,
            TypeConverter converter, Range? range = null, CultureInfo? culture = null) =>
                EnumerateGroupValues(groups, new TypeConverter<T>(converter), range, culture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IEnumerable<T> EnumerateGroupValues<T>(this GroupCollection groups,
            TypeConverter<T> converter, Range? range, CultureInfo? culture) =>
                groups.Skip(range)
                    .Select(g => converter.ConvertFromString(g.Value, culture));

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
        public static IEnumerable<T> EnumerateGroupValuesInvariant<T>(this GroupCollection groups,
            Range? range = null) =>
                EnumerateGroupValuesInvariant(groups, TypeConverter<T>.Instance, range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateGroupValuesInvariant<T>(this GroupCollection groups,
            TypeConverter converter, Range? range = null) =>
                EnumerateGroupValuesInvariant(groups, new TypeConverter<T>(converter), range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IEnumerable<T> EnumerateGroupValuesInvariant<T>(this GroupCollection groups,
            TypeConverter<T> converter, Range? range) =>
                groups.Skip(range)
                    .Select(g => converter.ConvertFromInvariantString(g.Value));

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
            Range? range = null, CultureInfo? culture = null) =>
                GetValues(groups, TypeConverter<T>.Instance, culture, range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            CultureInfo? culture, Range? range = null) =>
                GetValues(groups, TypeConverter<T>.Instance, culture, range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            CultureInfo? culture, params Index[] indices) =>
                GetValues(groups, TypeConverter<T>.Instance, culture, indices);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            CultureInfo? culture, params string[] keys) =>
                GetValues(groups, TypeConverter<T>.Instance, culture, keys);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            TypeConverter converter, CultureInfo? culture, Range? range = null) =>
                GetValues(groups, new TypeConverter<T>(converter), culture, range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            TypeConverter converter, CultureInfo? culture, params Index[] indices) =>
                GetValues(groups, new TypeConverter<T>(converter), culture, indices);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValues<T>(this GroupCollection groups,
            TypeConverter converter, CultureInfo? culture, params string[] keys) =>
                GetValues(groups, new TypeConverter<T>(converter), culture, keys);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T[][] GetValues<T>(this GroupCollection groups,
            TypeConverter<T> converter, CultureInfo? culture, Range? range) =>
                groups.Skip(range)
                    .Select(g => g.GetValues(converter, culture)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T[][] GetValues<T>(this GroupCollection groups,
            TypeConverter<T> converter, CultureInfo? culture, Index[] indices) =>
                indices.Select(i => groups[i].GetValues(converter, culture)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T[][] GetValues<T>(this GroupCollection groups,
            TypeConverter<T> converter, CultureInfo? culture, string[] keys) =>
                keys.Select(k => groups[k].GetValues(converter, culture)).ToArray();

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

        #endregion

        #region GetValuesInvariant

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValuesInvariant<T>(this GroupCollection groups,
            Range? range = null) =>
                GetValuesInvariant(groups, TypeConverter<T>.Instance, range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValuesInvariant<T>(this GroupCollection groups,
            params Index[] indices) =>
                GetValuesInvariant(groups, TypeConverter<T>.Instance, indices);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValuesInvariant<T>(this GroupCollection groups,
            params string[] keys) =>
                GetValuesInvariant(groups, TypeConverter<T>.Instance, keys);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValuesInvariant<T>(this GroupCollection groups,
            TypeConverter converter, Range? range = null) =>
                GetValuesInvariant(groups, new TypeConverter<T>(converter), range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValuesInvariant<T>(this GroupCollection groups,
            TypeConverter converter, params Index[] indices) =>
                GetValuesInvariant(groups, new TypeConverter<T>(converter), indices);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[][] GetValuesInvariant<T>(this GroupCollection groups,
            TypeConverter converter, params string[] keys) =>
                GetValuesInvariant(groups, new TypeConverter<T>(converter), keys);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T[][] GetValuesInvariant<T>(this GroupCollection groups,
            TypeConverter<T> converter, Range? range) =>
                groups.Skip(range)
                    .Select(g => g.GetValuesInvariant(converter)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T[][] GetValuesInvariant<T>(this GroupCollection groups,
            TypeConverter<T> converter, Index[] indices) =>
                indices.Select(i => groups[i].GetValuesInvariant(converter)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T[][] GetValuesInvariant<T>(this GroupCollection groups,
            TypeConverter<T> converter, string[] keys) =>
                keys.Select(k => groups[k].GetValuesInvariant(converter)).ToArray();

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
            Range? range = null, CultureInfo? culture = null) =>
                EnumerateValues(groups, TypeConverter<T>.Instance, culture, range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            CultureInfo? culture, Range? range = null) =>
                EnumerateValues(groups, TypeConverter<T>.Instance, culture, range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            CultureInfo? culture, params Index[] indices) =>
                EnumerateValues(groups, TypeConverter<T>.Instance, culture, indices);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            CultureInfo? culture, params string[] keys) =>
                EnumerateValues(groups, TypeConverter<T>.Instance, culture, keys);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            TypeConverter converter, CultureInfo? culture, Range? range = null) =>
                EnumerateValues(groups, new TypeConverter<T>(converter), culture, range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            TypeConverter converter, CultureInfo? culture, params Index[] indices) =>
                EnumerateValues(groups, new TypeConverter<T>(converter), culture, indices);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            TypeConverter converter, CultureInfo? culture, params string[] keys) =>
                EnumerateValues(groups, new TypeConverter<T>(converter), culture, keys);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            TypeConverter<T> converter, CultureInfo? culture, Range? range) =>
                groups.Skip(range)
                    .Select(g => g.EnumerateValues(converter, culture)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            TypeConverter<T> converter, CultureInfo? culture, Index[] indices) =>
                indices.Select(i => groups[i].EnumerateValues(converter, culture)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IEnumerable<T>[] EnumerateValues<T>(this GroupCollection groups,
            TypeConverter<T> converter, CultureInfo? culture, string[] keys) =>
                keys.Select(k => groups[k].EnumerateValues(converter, culture)).ToArray();

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

        #endregion

        #region EnumerateValuesInvariant

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValuesInvariant<T>(this GroupCollection groups,
            Range? range = null) =>
                EnumerateValuesInvariant(groups, TypeConverter<T>.Instance, range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValuesInvariant<T>(this GroupCollection groups,
            params Index[] indices) =>
                EnumerateValuesInvariant(groups, TypeConverter<T>.Instance, indices);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValuesInvariant<T>(this GroupCollection groups,
            params string[] keys) =>
                EnumerateValuesInvariant(groups, TypeConverter<T>.Instance, keys);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValuesInvariant<T>(this GroupCollection groups,
            TypeConverter converter, Range? range = null) =>
                EnumerateValuesInvariant(groups, new TypeConverter<T>(converter), range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValuesInvariant<T>(this GroupCollection groups,
            TypeConverter converter, params Index[] indices) =>
                EnumerateValuesInvariant(groups, new TypeConverter<T>(converter), indices);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T>[] EnumerateValuesInvariant<T>(this GroupCollection groups,
            TypeConverter converter, params string[] keys) =>
                EnumerateValuesInvariant(groups, new TypeConverter<T>(converter), keys);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IEnumerable<T>[] EnumerateValuesInvariant<T>(this GroupCollection groups,
            TypeConverter<T> converter, Range? range) =>
                groups.Skip(range)
                    .Select(g => g.EnumerateValuesInvariant(converter)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IEnumerable<T>[] EnumerateValuesInvariant<T>(this GroupCollection groups,
            TypeConverter<T> converter, Index[] indices) =>
                indices.Select(i => groups[i].EnumerateValuesInvariant(converter)).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IEnumerable<T>[] EnumerateValuesInvariant<T>(this GroupCollection groups,
            TypeConverter<T> converter, string[] keys) =>
                keys.Select(k => groups[k].EnumerateValuesInvariant(converter)).ToArray();

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
            Range? range = null, CultureInfo? culture = null) =>
                GetAllValues(groups, TypeConverter<T>.Instance, range, culture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, T[]> GetAllValues<T>(this GroupCollection groups,
            TypeConverter converter, Range? range = null, CultureInfo? culture = null) =>
                GetAllValues(groups, new TypeConverter<T>(converter), range, culture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Dictionary<string, T[]> GetAllValues<T>(this GroupCollection groups,
            TypeConverter<T> converter, Range? range, CultureInfo? culture) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.GetValues(converter, culture));

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
        public static Dictionary<string, T[]> GetAllValuesInvariant<T>(this GroupCollection groups,
            Range? range = null) =>
                GetAllValuesInvariant(groups, TypeConverter<T>.Instance, range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, T[]> GetAllValuesInvariant<T>(this GroupCollection groups,
            TypeConverter converter, Range? range = null) =>
                GetAllValuesInvariant(groups, new TypeConverter<T>(converter), range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Dictionary<string, T[]> GetAllValuesInvariant<T>(this GroupCollection groups,
            TypeConverter<T> converter, Range? range) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.GetValuesInvariant(converter));

        #endregion

        #region EnumerateAllValues

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, IEnumerable<string>> EnumerateAllValues(this GroupCollection groups,
            Range? range = null) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.EnumerateValues());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, IEnumerable<T>> EnumerateAllValues<T>(this GroupCollection groups,
            Range? range = null, CultureInfo? culture = null) =>
                EnumerateAllValues(groups, TypeConverter<T>.Instance, range, culture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, IEnumerable<T>> EnumerateAllValues<T>(this GroupCollection groups,
            TypeConverter converter, Range? range = null, CultureInfo? culture = null) =>
                EnumerateAllValues(groups, new TypeConverter<T>(converter), range, culture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Dictionary<string, IEnumerable<T>> EnumerateAllValues<T>(this GroupCollection groups,
            TypeConverter<T> converter, Range? range, CultureInfo? culture) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.EnumerateValues(converter, culture));

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
        public static Dictionary<string, IEnumerable<T>> EnumerateAllValuesInvariant<T>(this GroupCollection groups,
            Range? range = null) =>
                EnumerateAllValuesInvariant(groups, TypeConverter<T>.Instance, range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<string, IEnumerable<T>> EnumerateAllValuesInvariant<T>(this GroupCollection groups,
            TypeConverter converter, Range? range = null) =>
                EnumerateAllValuesInvariant(groups, new TypeConverter<T>(converter), range);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Dictionary<string, IEnumerable<T>> EnumerateAllValuesInvariant<T>(this GroupCollection groups,
            TypeConverter<T> converter, Range? range) =>
                groups.Skip(range)
                    .ToDictionary(g => g.Name, g => g.EnumerateValuesInvariant(converter));

        #endregion
    }
}
