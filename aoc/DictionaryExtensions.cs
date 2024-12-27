using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace aoc
{
    public static class DictionaryExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> vertices, TKey key, TValue value)
        {
            if (!vertices.TryGetValue(key, out var result))
                vertices.Add(key, result = value);
            return result;
        }
    }
}
