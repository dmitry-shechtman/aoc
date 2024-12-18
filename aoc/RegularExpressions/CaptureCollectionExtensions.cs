using System.Collections.Generic;
using System.Linq;

namespace System.Text.RegularExpressions
{
    public static class CaptureCollectionExtensions
    {
        public static T[] GetValues<T>(this CaptureCollection captures)
            where T : IConvertible =>
                GetValues(captures, StringExtensions.ConvertTo<T>);

        public static T[] GetValues<T>(this CaptureCollection captures,
            Func<string, T> selector) =>
                SelectValues(captures, selector).ToArray();

        public static IEnumerable<T> SelectValues<T>(this CaptureCollection captures)
            where T : IConvertible =>
                SelectValues(captures, StringExtensions.ConvertTo<T>);

        public static IEnumerable<T> SelectValues<T>(this CaptureCollection captures,
            Func<string, T> selector) =>
                captures.Select(c => selector(c.Value));
    }
}
