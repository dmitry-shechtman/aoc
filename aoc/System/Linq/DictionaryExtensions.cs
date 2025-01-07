using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Linq
{
    public static class DictionaryExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            if (!dic.TryGetValue(key, out var result))
                dic.Add(key, result = value);
            return result;
        }

        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, Func<TKey, TValue> factory)
        {
            if (!dic.TryGetValue(key, out var result))
                dic.Add(key, result = factory(key));
            return result;
        }
    }
}
