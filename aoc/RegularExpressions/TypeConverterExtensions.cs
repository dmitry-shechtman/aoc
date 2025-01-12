using System.ComponentModel;
using System.Globalization;
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
    }
}
