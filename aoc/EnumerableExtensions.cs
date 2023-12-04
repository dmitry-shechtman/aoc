using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace aoc
{
    public static class EnumerableExtensions
    {
        public static int Product(this IEnumerable<int> collection) =>
            collection.Aggregate(1, (a, v) => a * v);

        public static int Product<T>(this IEnumerable<T> collection, Func<T, int> selector) =>
            collection.Aggregate(1, (a, v) => a * selector(v));

        public static long Product(this IEnumerable<long> collection) =>
            collection.Aggregate(1L, (a, v) => a * v);

        public static long Product<T>(this IEnumerable<T> collection, Func<T, long> selector) =>
            collection.Aggregate(1L, (a, v) => a * selector(v));

        public static BigInteger Product(this IEnumerable<BigInteger> collection) =>
            collection.Aggregate(BigInteger.One, (a, v) => a * v);

        public static BigInteger Product<T>(this IEnumerable<T> collection, Func<T, BigInteger> selector) =>
            collection.Aggregate(BigInteger.One, (a, v) => a * selector(v));
    }
}
