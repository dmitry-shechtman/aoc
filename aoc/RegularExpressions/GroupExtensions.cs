using System.Collections.Generic;
using System.Linq;

namespace System.Text.RegularExpressions
{
    public static class GroupExtensions
    {
        public static T[] GetValues<T>(this Group group,
            IFormatProvider? provider = null)
                where T : IConvertible =>
                    GetValues(group, StringExtensions.ConvertTo<T>, provider);

        public static T[] GetValues<T>(this Group group,
            Func<string, T> selector) =>
                SelectValues(group, selector).ToArray();

        public static T[] GetValues<T>(this Group group,
            Func<string, IFormatProvider?, T> selector,
            IFormatProvider? provider = null) =>
                SelectValues(group, selector, provider).ToArray();

        public static IEnumerable<T> SelectValues<T>(this Group group,
            IFormatProvider? provider = null)
                where T : IConvertible =>
                    SelectValues(group, StringExtensions.ConvertTo<T>, provider);

        public static IEnumerable<T> SelectValues<T>(this Group group,
            Func<string, T> selector) =>
                group.Captures.SelectValues(selector);

        public static IEnumerable<T> SelectValues<T>(this Group group,
            Func<string, IFormatProvider?, T> selector,
            IFormatProvider? provider = null) =>
                group.Captures.SelectValues(selector, provider);
    }
}
