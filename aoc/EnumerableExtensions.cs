using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc
{
    public static class EnumerableExtensions
    {
        public static long Product(this IEnumerable<long> collection) =>
            collection.Aggregate(1L, (a, v) => a * v);

        public static long Product<T>(this IEnumerable<T> collection, Func<T, long> selector) =>
            collection.Aggregate(1L, (a, v) => a * selector(v));
    }
}
