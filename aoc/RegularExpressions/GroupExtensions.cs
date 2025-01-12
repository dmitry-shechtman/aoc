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
            IFormatProvider? provider = null)
                where T : IConvertible =>
                    EnumerateValues<T>(group, provider).ToArray();

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>(this Group group,
            CultureInfo? culture) =>
                EnumerateValues<T>(group, culture).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>(this Group group,
            TypeConverter converter, CultureInfo? culture = null) =>
                EnumerateValues<T>(group, converter, culture).ToArray();

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
            IFormatProvider? provider = null)
                where T : IConvertible =>
                    group.Captures.Select(c => c.Value.ConvertTo<T>(provider));

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateValues<T>(this Group group,
            CultureInfo? culture) =>
                EnumerateValues<T>(group, Util.GetConverter<T>(), culture);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateValues<T>(this Group group,
            TypeConverter converter, CultureInfo? culture = null) =>
                group.Captures.Select(c => converter.ConvertFromString<T>(c.Value, culture));

        #endregion

        #region EnumerateValuesInvariant

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateValuesInvariant<T>(this Group group) =>
            EnumerateValuesInvariant<T>(group, Util.GetConverter<T>());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateValuesInvariant<T>(this Group group,
            TypeConverter converter) =>
                group.Captures.Select(c => converter.ConvertFromInvariantString<T>(c.Value));

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
