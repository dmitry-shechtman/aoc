using System.Collections.Generic;
using System.Linq;

namespace System.Text.RegularExpressions
{
    public static class MatchCollectionExtensions
    {
#if !NET6_0_OR_GREATER
        public static IEnumerable<TResult> Select<TResult>(this MatchCollection matches, Func<Match, TResult> selector) =>
            Enumerable.Select(matches, selector);
#endif

        public static string[] GetStrings(this MatchCollection matches) =>
            GetValues(matches, s => s);

        public static int[] GetInts(this MatchCollection matches) =>
            GetValues(matches, int.Parse);

        public static long[] GetLongs(this MatchCollection matches) =>
            GetValues(matches, long.Parse);

        public static T[] GetValues<T>(this MatchCollection matches,
            Func<string, T> selector) =>
                matches.Select(c => selector(c.Value)).ToArray();
    }
}
