using System.Collections.Generic;
using System.Linq;

namespace System.Text.RegularExpressions
{
    public static class GroupExtensions
    {
        public static T[] GetValues<T>(this Group group)
            where T : IConvertible =>
                GetValues(group, StringExtensions.ConvertTo<T>);

        public static T[] GetValues<T>(this Group group,
            Func<string, T> selector) =>
                SelectValues(group, selector).ToArray();

        public static IEnumerable<T> SelectValues<T>(this Group group)
            where T : IConvertible =>
                SelectValues(group, StringExtensions.ConvertTo<T>);

        public static IEnumerable<T> SelectValues<T>(this Group group,
            Func<string, T> selector) =>
                group.Captures.SelectValues(selector);
    }
}
