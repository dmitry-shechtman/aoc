using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions
{
    readonly struct TypeConverter<T>
    {
        private static readonly Lazy<TypeConverter<T>> _instance =
            new(() => new(TypeDescriptor.GetConverter(typeof(T))));

        public static TypeConverter<T> Instance => _instance.Value;

        private readonly TypeConverter converter;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TypeConverter(TypeConverter converter) =>
            this.converter = converter.CanConvertTo(typeof(T))
                ? converter
                : throw new InvalidOperationException($"Cannot convert to {typeof(T)}.");

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T ConvertFromString(string value, CultureInfo? culture) =>
            (T)converter.ConvertFromString(null, culture, value)!;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T ConvertFromInvariantString(string value) =>
            (T)converter.ConvertFromInvariantString(value)!;
    }
}
