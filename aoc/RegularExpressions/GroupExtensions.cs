using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions
{
    public static class GroupExtensions
    {
        #region GetValues

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[] GetValues(this Group group) =>
            EnumerateValues(group).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>(this Group group,
            CultureInfo? culture = null) =>
                EnumerateValues<T>(group, culture).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>(this Group group,
            TypeConverter converter, CultureInfo? culture = null) =>
                EnumerateValues<T>(group, converter, culture).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T[] GetValues<T>(this Group group,
            TypeConverter<T> converter, CultureInfo? culture = null) =>
                EnumerateValues(group, converter, culture).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>(this Group group,
            ParseSpan<T> parse) =>
                EnumerateValues(group, parse).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>(this Group group,
            ParseSpan<T, IFormatProvider> parse,
            IFormatProvider? provider = null) =>
                EnumerateValues(group, parse, provider).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>(this Group group,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            IFormatProvider? provider = null, NumberStyles styles = 0) =>
                EnumerateValues(group, parse, provider, styles).ToArray();

        #endregion

        #region GetValuesInvariant

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValuesInvariant<T>(this Group group) =>
            EnumerateValuesInvariant<T>(group).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValuesInvariant<T>(this Group group,
            TypeConverter converter) =>
                EnumerateValuesInvariant<T>(group, converter).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T[] GetValuesInvariant<T>(this Group group,
            TypeConverter<T> converter) =>
                EnumerateValuesInvariant(group, converter).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValuesInvariant<T>(this Group group,
            ParseSpan<T, IFormatProvider> parse) =>
                EnumerateValuesInvariant(group, parse).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValuesInvariant<T>(this Group group,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            NumberStyles styles = 0) =>
                EnumerateValuesInvariant(group, parse, styles).ToArray();

        #endregion

        #region EnumerateValues

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<string> EnumerateValues(this Group group) =>
            group.Captures.Select(c => c.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateValues<T>(this Group group,
            CultureInfo? culture = null) =>
                EnumerateValues(group, TypeConverter<T>.Instance, culture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateValues<T>(this Group group,
            TypeConverter converter, CultureInfo? culture = null) =>
                EnumerateValues(group, new TypeConverter<T>(converter), culture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IEnumerable<T> EnumerateValues<T>(this Group group,
            TypeConverter<T> converter, CultureInfo? culture = null) =>
                group.Captures.Select(c => converter.ConvertFromString(c.Value, culture));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateValues<T>(this Group group,
            ParseSpan<T> parse) =>
                group.Captures.Select(c => parse(c.ValueSpan));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateValues<T>(this Group group,
            ParseSpan<T, IFormatProvider> parse,
            IFormatProvider? provider = null) =>
                group.Captures.Select(c => parse(c.ValueSpan, provider));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateValues<T>(this Group group,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            IFormatProvider? provider = null, NumberStyles styles = 0) =>
                group.Captures.Select(c => parse(c.ValueSpan, styles, provider));

        #endregion

        #region EnumerateValuesInvariant

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateValuesInvariant<T>(this Group group) =>
            EnumerateValuesInvariant(group, TypeConverter<T>.Instance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateValuesInvariant<T>(this Group group,
            TypeConverter converter) =>
                EnumerateValuesInvariant(group, new TypeConverter<T>(converter));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IEnumerable<T> EnumerateValuesInvariant<T>(this Group group,
            TypeConverter<T> converter) =>
                group.Captures.Select(c => converter.ConvertFromInvariantString(c.Value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateValuesInvariant<T>(this Group group,
            ParseSpan<T, IFormatProvider> parse) =>
                group.Captures.Select(c => parse(c.ValueSpan, CultureInfo.InvariantCulture));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateValuesInvariant<T>(this Group group,
            ParseSpan<T, NumberStyles, IFormatProvider> parse,
            NumberStyles styles = 0) =>
                group.Captures.Select(c => parse(c.ValueSpan, styles, CultureInfo.InvariantCulture));

        #endregion
    }
}
