using System.Collections.Generic;
using System.Linq;

namespace System.Text.RegularExpressions
{
    public static class CaptureCollectionExtensions
    {
#if !NET6_0_OR_GREATER
        public static IEnumerable<TResult> Select<TResult>(this CaptureCollection captures, Func<Capture, TResult> selector) =>
            Enumerable.Select(captures, selector);
#endif

        public static string[] GetStrings(this CaptureCollection captures) =>
            GetValues(captures, s => s);

        public static int[] GetInts(this CaptureCollection captures) =>
            GetValues(captures, int.Parse);

        public static long[] GetLongs(this CaptureCollection captures) =>
            GetValues(captures, long.Parse);

        public static T[] GetValues<T>(this CaptureCollection captures,
            Func<string, T> selector) =>
                captures.Select(c => selector(c.Value)).ToArray();
    }
}
