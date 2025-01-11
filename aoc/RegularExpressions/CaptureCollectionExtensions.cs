using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions
{
    public static class CaptureCollectionExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[] GetValues(this CaptureCollection captures) =>
            SelectValues(captures).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>(this CaptureCollection captures,
            IFormatProvider? provider = null)
                where T : IConvertible =>
                    SelectValues<T>(captures, provider).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>(this CaptureCollection captures,
            ParseSpan<T> selector) =>
                SelectValues(captures, selector).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>(this CaptureCollection captures,
            ParseSpan<T, IFormatProvider> selector,
            IFormatProvider? provider = null) =>
                SelectValues(captures, selector, provider).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>(this CaptureCollection captures,
            ParseSpan<T, NumberStyles, IFormatProvider> selector,
            IFormatProvider? provider = null, NumberStyles styles = 0) =>
                SelectValues(captures, selector, provider, styles).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<string> SelectValues(this CaptureCollection captures) =>
            captures.Select(c => c.Value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> SelectValues<T>(this CaptureCollection captures,
            IFormatProvider? provider = null)
                where T : IConvertible =>
                    captures.Select(c => c.Value.ConvertTo<T>(provider));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> SelectValues<T>(this CaptureCollection captures,
            ParseSpan<T> selector) =>
                captures.Select(c => selector(c.ValueSpan));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> SelectValues<T>(this CaptureCollection captures,
            ParseSpan<T, IFormatProvider> selector,
            IFormatProvider? provider = null) =>
                captures.Select(c => selector(c.ValueSpan, provider));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> SelectValues<T>(this CaptureCollection captures,
            ParseSpan<T, NumberStyles, IFormatProvider> selector,
            IFormatProvider? provider = null, NumberStyles styles = 0) =>
                captures.Select(c => selector(c.ValueSpan, styles, provider));
    }
}
