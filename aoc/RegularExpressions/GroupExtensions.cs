using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions
{
    public static class GroupExtensions
    {
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
            ParseSpan<T> selector) =>
                EnumerateValues(group, selector).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>(this Group group,
            ParseSpan<T, IFormatProvider> selector,
            IFormatProvider? provider = null) =>
                EnumerateValues(group, selector, provider).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>(this Group group,
            ParseSpan<T, NumberStyles, IFormatProvider> selector,
            IFormatProvider? provider = null, NumberStyles styles = 0) =>
                EnumerateValues(group, selector, provider, styles).ToArray();

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
            ParseSpan<T> selector) =>
                group.Captures.Select(c => selector(c.ValueSpan));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateValues<T>(this Group group,
            ParseSpan<T, IFormatProvider> selector,
            IFormatProvider? provider = null) =>
                group.Captures.Select(c => selector(c.ValueSpan, provider));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EnumerateValues<T>(this Group group,
            ParseSpan<T, NumberStyles, IFormatProvider> selector,
            IFormatProvider? provider = null, NumberStyles styles = 0) =>
                group.Captures.Select(c => selector(c.ValueSpan, styles, provider));
    }
}
