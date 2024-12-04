using System.Collections.Generic;
using System.Linq;

namespace System.Text.RegularExpressions
{
    public static class RegexExtensions
    {
#if !NET6_0_OR_GREATER
        public static IEnumerable<TResult> Select<TResult>(this MatchCollection matches, Func<Match, TResult> selector) =>
            Enumerable.Select(matches, selector);
#endif

#if !NET8_0_OR_GREATER
        public static IEnumerable<TResult> Select<TResult>(this GroupCollection groups, Func<Group, TResult> selector) =>
            Enumerable.Select(groups, selector);

        public static Dictionary<TKey, TElement> ToDictionary<TKey, TElement>(this GroupCollection groups,
            Func<Group, TKey> keySelector,
            Func<Group, TElement> elementSelector)
            where TKey : notnull =>
                Enumerable.ToDictionary(groups, keySelector, elementSelector);
#endif
    }
}
