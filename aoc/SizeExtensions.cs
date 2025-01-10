using System;
using System.Diagnostics.CodeAnalysis;

namespace aoc
{
    public static class SizeExtensions
    {
        public static TValue GetValue<TSize, T, TValue>(this TSize size, TValue[] array, T key)
            where TSize : struct, IIntegerSize<TSize, T>
            where T : struct =>
                array[size.GetLongIndex(key)];

        public static TValue GetValue<TSize, T, TValue>(this TSize size, ReadOnlySpan<TValue> span, T key)
            where TSize : struct, IIntegerSize<TSize, T>
            where T : struct =>
                span[size.GetIndex(key)];

        public static TValue SetValue<TSize, T, TValue>(this TSize size, TValue[] array, T key, TValue value)
            where TSize : struct, IIntegerSize<TSize, T>
            where T : struct =>
                array[size.GetLongIndex(key)] = value;

        public static TValue SetValue<TSize, T, TValue>(this TSize size, Span<TValue> span, T key, TValue value)
            where TSize : struct, IIntegerSize<TSize, T>
            where T : struct =>
                span[size.GetIndex(key)] = value;

        public static bool TryGetValue<TSize, T, TValue>(this TSize size, TValue[] array, T key,
            [MaybeNullWhen(false)] out TValue value)
                where TSize : struct, IIntegerSize<TSize, T>
                where T : struct
        {
            if (!size.Contains(key))
            {
                value = default;
                return false;
            }
            value = GetValue(size, array, key);
            return true;
        }

        public static bool TryGetValue<TSize, T, TValue>(this TSize size, ReadOnlySpan<TValue> span, T key,
            [MaybeNullWhen(false)] out TValue value)
                where TSize : struct, IIntegerSize<TSize, T>
                where T : struct
        {
            if (!size.Contains(key))
            {
                value = default;
                return false;
            }
            value = GetValue(size, span, key);
            return true;
        }
    }
}
