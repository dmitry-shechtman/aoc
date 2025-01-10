using System.Collections.Generic;
using System.Linq;

namespace System.Text.RegularExpressions
{
    public static class CaptureCollectionExtensions
    {
        public static T[] GetValues<T>(this CaptureCollection captures,
            IFormatProvider? provider = null)
                where T : IConvertible =>
                    GetValues(captures, StringExtensions.ConvertTo<T>, provider);

        public static T[] GetValues<T>(this CaptureCollection captures,
            Func<string, T> selector) =>
                SelectValues(captures, selector).ToArray();

        public static T[] GetValues<T>(this CaptureCollection captures,
            Func<string, IFormatProvider?, T> selector,
            IFormatProvider? provider = null) =>
                SelectValues(captures, selector, provider).ToArray();

        public static IEnumerable<T> SelectValues<T>(this CaptureCollection captures,
            IFormatProvider? provider = null)
                where T : IConvertible =>
                    SelectValues(captures, StringExtensions.ConvertTo<T>, provider);

        public static IEnumerable<T> SelectValues<T>(this CaptureCollection captures,
            Func<string, T> selector) =>
                captures.Select(c => selector(c.Value));

        public static IEnumerable<T> SelectValues<T>(this CaptureCollection captures,
            Func<string, IFormatProvider?, T> selector,
            IFormatProvider? provider = null) =>
                captures.Select(c => selector(c.Value, provider));
    }
}
