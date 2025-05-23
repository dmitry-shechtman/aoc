﻿using System.Collections.Generic;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<(T Value, int Index)> Select<T>(this IEnumerable<T> source) =>
            source.Select((v, i) => (v, i));

        public static IEnumerable<TResult> Select1<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector) =>
            source.Select(selector);

        public static IEnumerable<TResult> Select2<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector) =>
            source.Select(selector);

        public static IEnumerable<TSource> Where1<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) =>
            source.Where(predicate);

        public static IEnumerable<TSource> Where2<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate) =>
            source.Where(predicate);

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

        public static Dictionary<TKey, TValue> ToDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, int, TKey> keySelector, Func<TSource, int, TValue> valueSelector)
            where TKey : notnull =>
                source.Select()
                    .ToDictionary(t => keySelector(t.Value, t.Index), t => valueSelector(t.Value, t.Index));

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

        public static int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate) =>
            source.Where(predicate).Count();

        public static long LongCount<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate) =>
            source.Where(predicate).LongCount();

        public static TResult? Min<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector) =>
            source.Select(selector).Min();

        public static TResult? Max<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector) =>
            source.Select(selector).Max();

        public static int Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int, int> selector) =>
            source.Select(selector).Sum();

        public static long Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int, long> selector) =>
            source.Select(selector).Sum();

        public static int Product(this IEnumerable<int> source) =>
            source.Aggregate(1, (a, v) => a * v);

        public static int Product<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector) =>
            source.Select(selector).Product();

        public static int Product<TSource>(this IEnumerable<TSource> source, Func<TSource, int, int> selector) =>
            source.Select(selector).Product();

        public static long Product(this IEnumerable<long> source) =>
            source.Aggregate(1L, (a, v) => a * v);

        public static long Product<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector) =>
            source.Select(selector).Product();

        public static long Product<TSource>(this IEnumerable<TSource> source, Func<TSource, int, long> selector) =>
            source.Select(selector).Product();

        public static TSource? MinOrDefault<TSource>(this IEnumerable<TSource> source, TSource result) =>
            source.Any() ? source.Min() : result;

        public static TResult? MinOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, TResult result) =>
            source.Any() ? source.Min(selector) : result;

        public static TResult? MinOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector, TResult result) =>
            source.Any() ? source.Min(selector) : result;

        public static TSource? MaxOrDefault<TSource>(this IEnumerable<TSource> source, TSource result) =>
            source.Any() ? source.Max() : result;

        public static TResult? MaxOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, TResult result) =>
            source.Any() ? source.Max(selector) : result;

        public static TResult? MaxOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector, TResult result) =>
            source.Any() ? source.Max(selector) : result;
    }
}
