using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace aoc
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<(T Value, int Index)> Select<T>(this IEnumerable<T> source) =>
            source.Select((v, i) => (v, i));

        public static IEnumerable<T> SelectMany<T>(this IEnumerable<IEnumerable<T>> source) =>
            source.SelectMany(v => v);

        public static IOrderedEnumerable<T> Order<T>(this IEnumerable<T> source) =>
            source.OrderBy(v => v);

        public static IOrderedEnumerable<T> Order<T>(this IEnumerable<T> source, IComparer<T> comparer) =>
            source.OrderBy(v => v, comparer);

        public static IOrderedEnumerable<T> OrderDescending<T>(this IEnumerable<T> source) =>
            source.OrderByDescending(v => v);

        public static IOrderedEnumerable<T> OrderDescending<T>(this IEnumerable<T> source, IComparer<T> comparer) =>
            source.OrderByDescending(v => v, comparer);

#if !NET_6_0_OR_GREATER
        public static IEnumerable<T[]> Chunk<T>(this IEnumerable<T> source, int size) =>
            source.Select()
                .GroupBy(t => t.Index / size)
                .Select(g => g.Select(t => t.Value).ToArray());

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            source.OrderBy(keySelector).First();

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) =>
            source.OrderByDescending(keySelector).First();
#endif

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

        public static int TryMin(this IEnumerable<int> collection, int value) =>
            collection.Any() ? collection.Min() : value;

        public static int TryMin<T>(this IEnumerable<T> collection, Func<T, int> selector, int value) =>
            collection.Any() ? collection.Min(selector) : value;

        public static int TryMax(this IEnumerable<int> collection, int value) =>
            collection.Any() ? collection.Max() : value;

        public static int TryMax<T>(this IEnumerable<T> collection, Func<T, int> selector, int value) =>
            collection.Any() ? collection.Max(selector) : value;

        public static long TryMin(this IEnumerable<long> collection, long value) =>
            collection.Any() ? collection.Min() : value;

        public static long TryMin<T>(this IEnumerable<T> collection, Func<T, long> selector, long value) =>
            collection.Any() ? collection.Min(selector) : value;

        public static long TryMax(this IEnumerable<long> collection, long value) =>
            collection.Any() ? collection.Max() : value;

        public static long TryMax<T>(this IEnumerable<T> collection, Func<T, long> selector, long value) =>
            collection.Any() ? collection.Max(selector) : value;
    }
}
