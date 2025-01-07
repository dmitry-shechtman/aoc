using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<(T Value, int Index)> Select<T>(this IEnumerable<T> source) =>
            source.Select((v, i) => (v, i));

        public static IEnumerable<TResult> Select1<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector) =>
            source.Select(selector);

        public static IEnumerable<TResult> Select2<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector) =>
            source.Select(selector);

        public static IEnumerable<T> SelectMany<T>(this IEnumerable<IEnumerable<T>> source) =>
            source.SelectMany(v => v);

        public static IEnumerable<IGrouping<T, T>> Group<T>(this IEnumerable<T> source) =>
            source.GroupBy(v => v);

        public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate) =>
            source.Select()
                .All(t => predicate(t.Value, t.Index));

        public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate) =>
            source.Select()
                .Any(t => predicate(t.Value, t.Index));

        public static ValueTuple<TSource, int> First<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate) =>
            source.Select()
                .First(t => predicate(t.Value, t.Index));

        public static ValueTuple<TSource, int> FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate) =>
            source.Select()
                .FirstOrDefault(t => predicate(t.Value, t.Index));

        public static ValueTuple<TSource, int> Last<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate) =>
            source.Select()
                .Last(t => predicate(t.Value, t.Index));

        public static ValueTuple<TSource, int> LastOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate) =>
            source.Select()
                .LastOrDefault(t => predicate(t.Value, t.Index));

        public static ValueTuple<TSource, int> Single<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate) =>
            source.Select()
                .Single(t => predicate(t.Value, t.Index));

        public static ValueTuple<TSource, int> SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate) =>
            source.Select()
                .SingleOrDefault(t => predicate(t.Value, t.Index));

        public static TAccumulate Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, int, TAccumulate> selector) =>
            source.Select().Aggregate(seed, (a, v) => selector(a, v.Value, v.Index));

#if !NET7_0_OR_GREATER
        public static IOrderedEnumerable<T> Order<T>(this IEnumerable<T> source) =>
            source.OrderBy(v => v);

        public static IOrderedEnumerable<T> Order<T>(this IEnumerable<T> source, IComparer<T> comparer) =>
            source.OrderBy(v => v, comparer);

        public static IOrderedEnumerable<T> OrderDescending<T>(this IEnumerable<T> source) =>
            source.OrderByDescending(v => v);

        public static IOrderedEnumerable<T> OrderDescending<T>(this IEnumerable<T> source, IComparer<T> comparer) =>
            source.OrderByDescending(v => v, comparer);
#endif

#if !NET6_0_OR_GREATER && !NET6_0
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
            source.Where(predicate).Count();

        public static long LongCount<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate) =>
            source.Where(predicate).LongCount();

        public static TResult Min<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector) =>
            source.Select(selector).Min();

        public static TResult Max<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector) =>
            source.Select(selector).Max();

        public static int Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int, int> selector) =>
            source.Select(selector).Sum();

        public static long Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int, long> selector) =>
            source.Select(selector).Sum();

        public static int Product(this IEnumerable<int> source) =>
            source.Aggregate(1, (a, v) => a * v);

        public static int Product<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector) =>
            source.Aggregate(1, (a, v) => a * selector(v));

        public static int Product<TSource>(this IEnumerable<TSource> source, Func<TSource, int, int> selector) =>
            source.Aggregate(1, (a, v, i) => a * selector(v, i));

        public static long Product(this IEnumerable<long> source) =>
            source.Aggregate(1L, (a, v) => a * v);

        public static long Product<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector) =>
            source.Aggregate(1L, (a, v) => a * selector(v));

        public static long Product<TSource>(this IEnumerable<TSource> source, Func<TSource, int, long> selector) =>
            source.Aggregate(1L, (a, v, i) => a * selector(v, i));

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
    }
}
