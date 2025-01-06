using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc
{
    public static class MathEx
    {
        public static int Gcd(int a, int b) =>
            b == 0 ? a : Gcd(b, a % b);

        public static int Gcd(this IEnumerable<int> source) =>
            source.Aggregate(Gcd);

        public static int Gcd<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector) =>
            source.Select(selector).Aggregate(Gcd);

        public static int Gcd<TSource>(this IEnumerable<TSource> source, Func<TSource, int, int> selector) =>
            source.Select(selector).Aggregate(Gcd);

        public static long Gcd(long a, long b) =>
            b == 0 ? a : Gcd(b, a % b);

        public static long Gcd(this IEnumerable<long> source) =>
            source.Aggregate(Gcd);

        public static long Gcd<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector) =>
            source.Select(selector).Aggregate(Gcd);

        public static long Gcd<TSource>(this IEnumerable<TSource> source, Func<TSource, int, long> selector) =>
            source.Select(selector).Aggregate(Gcd);

        public static int Lcm(int a, int b) =>
            a / Gcd(a, b) * b;

        public static int Lcm(this IEnumerable<int> source) =>
            source.Aggregate(Lcm);

        public static int Lcm<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector) =>
            source.Select(selector).Aggregate(Lcm);

        public static int Lcm<TSource>(this IEnumerable<TSource> source, Func<TSource, int, int> selector) =>
            source.Select(selector).Aggregate(Lcm);

        public static long Lcm(long a, long b) =>
            a / Gcd(a, b) * b;

        public static long Lcm(this IEnumerable<long> source) =>
            source.Aggregate(Lcm);

        public static long Lcm<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector) =>
            source.Select(selector).Aggregate(Lcm);

        public static long Lcm<TSource>(this IEnumerable<TSource> source, Func<TSource, int, long> selector) =>
            source.Select(selector).Aggregate(Lcm);

        public static int Median(this int[] source)
        {
            source.Sort();
            return (source[source.Length / 2] + source[(source.Length + 1) / 2]) / 2;
        }

        public static int Median(this IEnumerable<int> source) =>
            Median(source.ToArray());
    }
}
