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

        public static IEnumerable<IGrouping<T, T>> Group<T>(this IEnumerable<T> source) =>
            source.GroupBy(v => v);

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

        public static int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate) =>
            source.Select(predicate).Count();

        public static long LongCount<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate) =>
            source.Select(predicate).LongCount();

        public static TResult Min<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector) =>
            source.Select(selector).Min();

        public static TResult Max<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector) =>
            source.Select(selector).Max();

        public static int Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int, int> selector) =>
            source.Select(selector).Sum();

        public static long Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int, long> selector) =>
            source.Select(selector).Sum();

        public static int Product(this IEnumerable<int> collection) =>
            collection.Aggregate(1, (a, v) => a * v);

        public static int Product<T>(this IEnumerable<T> collection, Func<T, int> selector) =>
            collection.Aggregate(1, (a, v) => a * selector(v));

        public static int Product<TSource>(this IEnumerable<TSource> source, Func<TSource, int, int> selector) =>
            source.Select(selector).Product();

        public static long Product(this IEnumerable<long> collection) =>
            collection.Aggregate(1L, (a, v) => a * v);

        public static long Product<T>(this IEnumerable<T> collection, Func<T, long> selector) =>
            collection.Aggregate(1L, (a, v) => a * selector(v));

        public static long Product<TSource>(this IEnumerable<TSource> source, Func<TSource, int, long> selector) =>
            source.Select(selector).Product();

        public static BigInteger Product(this IEnumerable<BigInteger> collection) =>
            collection.Aggregate(BigInteger.One, (a, v) => a * v);

        public static BigInteger Product<T>(this IEnumerable<T> collection, Func<T, BigInteger> selector) =>
            collection.Aggregate(BigInteger.One, (a, v) => a * selector(v));

        public static BigInteger Product<TSource>(this IEnumerable<TSource> source, Func<TSource, int, BigInteger> selector) =>
            source.Select(selector).Product();

        public static TSource TryMin<TSource>(this IEnumerable<TSource> source, TSource result) =>
            source.Any() ? source.Min() : result;

        public static TResult TryMin<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, TResult result) =>
            source.Any() ? source.Min(selector) : result;

        public static TResult TryMin<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector, TResult result) =>
            source.Any() ? source.Min(selector) : result;

        public static TSource TryMax<TSource>(this IEnumerable<TSource> source, TSource result) =>
            source.Any() ? source.Max() : result;

        public static TResult TryMax<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, TResult result) =>
            source.Any() ? source.Max(selector) : result;

        public static TResult TryMax<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector, TResult result) =>
            source.Any() ? source.Max(selector) : result;

        public static int Gcd(this IEnumerable<int> source) =>
            source.Aggregate(Gcd);

        public static int Gcd<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector) =>
            source.Select(selector).Aggregate(Gcd);

        public static int Gcd<TSource>(this IEnumerable<TSource> source, Func<TSource, int, int> selector) =>
            source.Select(selector).Aggregate(Gcd);

        private static int Gcd(int a, int b) =>
            b == 0 ? a : Gcd(b, a % b);

        public static long Gcd(this IEnumerable<long> source) =>
            source.Aggregate(Gcd);

        public static long Gcd<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector) =>
            source.Select(selector).Aggregate(Gcd);

        public static long Gcd<TSource>(this IEnumerable<TSource> source, Func<TSource, int, long> selector) =>
            source.Select(selector).Aggregate(Gcd);

        private static long Gcd(long a, long b) =>
            b == 0 ? a : Gcd(b, a % b);

        public static int Lcm(this IEnumerable<int> source) =>
            source.Aggregate(Lcm);

        public static int Lcm<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector) =>
            source.Select(selector).Aggregate(Lcm);

        public static int Lcm<TSource>(this IEnumerable<TSource> source, Func<TSource, int, int> selector) =>
            source.Select(selector).Aggregate(Lcm);

        private static int Lcm(int a, int b) =>
            a / Gcd(a, b) * b;

        public static long Lcm(this IEnumerable<long> source) =>
            source.Aggregate(Lcm);

        public static long Lcm<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector) =>
            source.Select(selector).Aggregate(Lcm);

        public static long Lcm<TSource>(this IEnumerable<TSource> source, Func<TSource, int, long> selector) =>
            source.Select(selector).Aggregate(Lcm);

        private static long Lcm(long a, long b) =>
            a / Gcd(a, b) * b;
    }
}
