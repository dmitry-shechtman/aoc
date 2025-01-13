using System.Runtime.CompilerServices;

namespace System
{
    public static class MemoryExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource Aggregate<TSource>(this ReadOnlySpan<TSource> source, Func<TSource, TSource, TSource> selector)
        {
            TSource acc = source[0];
            for (int i = 1; i < source.Length; i++)
                acc = selector(acc, source[i]);
            return acc;
        }

        public static TSource Aggregate<TSource>(this Span<TSource> source, Func<TSource, TSource, TSource> selector) =>
            Aggregate((ReadOnlySpan<TSource>)source, selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource Aggregate<TSource>(this ReadOnlySpan<TSource> source, Func<TSource, TSource, int, TSource> selector)
        {
            TSource acc = source[0];
            for (int i = 1; i < source.Length; i++)
                acc = selector(acc, source[i], i);
            return acc;
        }

        public static TSource Aggregate<TSource>(this Span<TSource> source, Func<TSource, TSource, int, TSource> selector) =>
            Aggregate((ReadOnlySpan<TSource>)source, selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TAccumulate Aggregate<TSource, TAccumulate>(this ReadOnlySpan<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> selector)
        {
            TAccumulate acc = seed;
            for (int i = 0; i < source.Length; i++)
                acc = selector(acc, source[i]);
            return acc;
        }

        public static TAccumulate Aggregate<TSource, TAccumulate>(this Span<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> selector) =>
            Aggregate((ReadOnlySpan<TSource>)source, seed, selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TAccumulate Aggregate<TSource, TAccumulate>(this ReadOnlySpan<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, int, TAccumulate> selector)
        {
            TAccumulate acc = seed;
            for (int i = 0; i < source.Length; i++)
                acc = selector(acc, source[i], i);
            return acc;
        }

        public static TAccumulate Aggregate<TSource, TAccumulate>(this Span<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, int, TAccumulate> selector) =>
            Aggregate((ReadOnlySpan<TSource>)source, seed, selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool All<TSource>(this ReadOnlySpan<TSource> source, Func<TSource, bool> predicate)
        {
            for (int i = 0; i < source.Length; i++)
                if (!predicate(source[i]))
                    return false;
            return true;
        }

        public static bool All<TSource>(this Span<TSource> source, Func<TSource, bool> predicate) =>
            All((ReadOnlySpan<TSource>)source, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool All<TSource>(this ReadOnlySpan<TSource> source, Func<TSource, int, bool> predicate)
        {
            for (int i = 0; i < source.Length; i++)
                if (!predicate(source[i], i))
                    return false;
            return true;
        }

        public static bool All<TSource>(this Span<TSource> source, Func<TSource, int, bool> predicate) =>
            All((ReadOnlySpan<TSource>)source, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<TSource>(this ReadOnlySpan<TSource> source, Func<TSource, bool> predicate)
        {
            for (int i = 0; i < source.Length; i++)
                if (predicate(source[i]))
                    return true;
            return false;
        }

        public static bool Any<TSource>(this Span<TSource> source, Func<TSource, bool> predicate) =>
            Any((ReadOnlySpan<TSource>)source, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<TSource>(this ReadOnlySpan<TSource> source, Func<TSource, int, bool> predicate)
        {
            for (int i = 0; i < source.Length; i++)
                if (predicate(source[i], i))
                    return true;
            return false;
        }

        public static bool Any<TSource>(this Span<TSource> source, Func<TSource, int, bool> predicate) =>
            Any((ReadOnlySpan<TSource>)source, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<TSource>(this ReadOnlySpan<TSource> source, Func<TSource, bool> predicate) =>
            source.Aggregate(0, (a, v) => predicate(v) ? a + 1 : a);

        public static int Count<TSource>(this Span<TSource> source, Func<TSource, bool> predicate) =>
            Count((ReadOnlySpan<TSource>)source, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<TSource>(this ReadOnlySpan<TSource> source, Func<TSource, int, bool> predicate) =>
            source.Aggregate(0, (a, v, i) => predicate(v, i) ? a + 1 : a);

        public static int Count<TSource>(this Span<TSource> source, Func<TSource, int, bool> predicate) =>
            Count((ReadOnlySpan<TSource>)source, predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sum(this ReadOnlySpan<int> source) =>
            source.Aggregate(0, (a, v) => a + v);

        public static int Sum(this Span<int> source) =>
            Sum((ReadOnlySpan<int>)source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sum<TSource>(this ReadOnlySpan<TSource> source, Func<TSource, int> selector) =>
            source.Aggregate(0, (a, v) => a + selector(v));

        public static int Sum<TSource>(this Span<TSource> source, Func<TSource, int> selector) =>
            Sum((ReadOnlySpan<TSource>)source, selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sum<TSource>(this ReadOnlySpan<TSource> source, Func<TSource, int, int> selector) =>
            source.Aggregate(0, (a, v, i) => a + selector(v, i));

        public static int Sum<TSource>(this Span<TSource> source, Func<TSource, int, int> selector) =>
            Sum((ReadOnlySpan<TSource>)source, selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Product(this ReadOnlySpan<int> source) =>
            source.Aggregate(1, (a, v) => a * v);

        public static int Product(this Span<int> source) =>
            Product((ReadOnlySpan<int>)source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Product<TSource>(this ReadOnlySpan<TSource> source, Func<TSource, int> selector) =>
            source.Aggregate(1, (a, v) => a * selector(v));

        public static int Product<TSource>(this Span<TSource> source, Func<TSource, int> selector) =>
            Product((ReadOnlySpan<TSource>)source, selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Product<TSource>(this ReadOnlySpan<TSource> source, Func<TSource, int, int> selector) =>
            source.Aggregate(1, (a, v, i) => a * selector(v, i));

        public static int Product<TSource>(this Span<TSource> source, Func<TSource, int, int> selector) =>
            Product((ReadOnlySpan<TSource>)source, selector);

#if !NET8_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Split(this ReadOnlySpan<char> source, Span<Range> destination, char separator, StringSplitOptions options = StringSplitOptions.None)
        {
            int index = 0, start = 0;
            for (int curr = 0; curr < source.Length; curr++)
            {
                if (source[curr] == separator)
                {
                    index += AddItem(source, destination, options, index, start, curr);
                    start = curr + 1;
                }
            }
            index += AddItem(source, destination, options, index, start, source.Length);
            return index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Split(this ReadOnlySpan<char> source, Span<Range> destination, ReadOnlySpan<char> separator, StringSplitOptions options = StringSplitOptions.None)
        {
            int index = 0, start = 0;
            for (int curr = 0; curr <= source.Length - separator.Length; curr++)
            {
                if (source[curr..].StartsWith(separator))
                {
                    index += AddItem(source, destination, options, index, start, curr);
                    start = curr + separator.Length;
                }
            }
            index += AddItem(source, destination, options, index, start, source.Length);
            return index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int AddItem(ReadOnlySpan<char> source, Span<Range> destination, StringSplitOptions options, int index, int start, int end)
        {
            if (options.HasFlag(StringSplitOptions.TrimEntries))
            {
                while (start < end && char.IsWhiteSpace(source[start]))
                    ++start;
                while (end > start && char.IsWhiteSpace(source[end - 1]))
                    --end;
            }
            if (options.HasFlag(StringSplitOptions.RemoveEmptyEntries) && start == end)
                return 0;
            destination[index] = new(start, end);
            return 1;
        }
#endif
    }
}
