using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions
{
    internal static class Util
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TypeConverter GetConverter<T>()
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return converter.CanConvertTo(typeof(T))
                ? converter
                : throw new InvalidOperationException();
        }
    }
}
