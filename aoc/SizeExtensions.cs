namespace aoc
{
    public static class SizeExtensions
    {
        public static TValue GetValue<TSize, T, TValue>(this TSize size, TValue[] array, T key)
            where TSize : struct, IIntegerSize<TSize, T>
            where T : struct =>
                array[size.GetLongIndex(key)];

        public static TValue SetValue<TSize, T, TValue>(this TSize size, TValue[] array, T key, TValue value)
            where TSize : struct, IIntegerSize<TSize, T>
            where T : struct =>
                array[size.GetLongIndex(key)] = value;

        public static bool TryGetValue<TSize, T, TValue>(this TSize size, TValue[] array, T key, out TValue value)
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
    }
}
