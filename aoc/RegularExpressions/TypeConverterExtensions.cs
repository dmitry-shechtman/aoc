using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions
{
    internal static class TypeConverterExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ConvertFromString<T>(this TypeConverter converter, string value, CultureInfo? culture) =>
            (T)converter.ConvertFromString(null, culture, value)!;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ConvertFromInvariantString<T>(this TypeConverter converter, string value) =>
            (T)converter.ConvertFromInvariantString(value)!;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> ConvertFromStrings<T>(this TypeConverter converter, Group group, CultureInfo? culture) =>
            group.Captures.Select(c => ConvertFromString<T>(converter, c.Value, culture));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> ConvertFromInvariantStrings<T>(this TypeConverter converter, Group group) =>
            group.Captures.Select(c => ConvertFromInvariantString<T>(converter, c.Value));
    }
}
