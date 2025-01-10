using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
    public static class DictionaryExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value)
            where TKey : notnull
        {
            if (!dic.TryGetValue(key, out var result))
                dic.Add(key, result = value);
            return result;
        }

        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, Func<TKey, TValue> factory)
            where TKey : notnull
        {
            if (!dic.TryGetValue(key, out var result))
                dic.Add(key, result = factory(key));
            return result;
        }
    }
}
