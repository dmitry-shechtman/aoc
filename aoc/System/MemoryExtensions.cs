using System.Runtime.CompilerServices;

namespace System
{
    public static class MemoryExtensions
    {
        public static bool All<TSource>(this ReadOnlySpan<TSource> source, Func<TSource, bool> predicate)
        {
            for (int i = 0; i < source.Length; i++)
                if (!predicate(source[i]))
                    return false;
            return true;
        }

        public static bool All<TSource>(this Span<TSource> source, Func<TSource, bool> predicate) =>
            ((ReadOnlySpan<TSource>)source).All(predicate);

        public static bool All<TSource>(this ReadOnlySpan<TSource> source, Func<TSource, int, bool> predicate)
        {
            for (int i = 0; i < source.Length; i++)
                if (!predicate(source[i], i))
                    return false;
            return true;
        }

        public static bool All<TSource>(this Span<TSource> source, Func<TSource, int, bool> predicate) =>
            ((ReadOnlySpan<TSource>)source).All(predicate);

        public static bool Any<TSource>(this ReadOnlySpan<TSource> source, Func<TSource, bool> predicate)
        {
            for (int i = 0; i < source.Length; i++)
                if (predicate(source[i]))
                    return true;
            return false;
        }

        public static bool Any<TSource>(this Span<TSource> source, Func<TSource, bool> predicate) =>
            ((ReadOnlySpan<TSource>)source).Any(predicate);

        public static bool Any<TSource>(this ReadOnlySpan<TSource> source, Func<TSource, int, bool> predicate)
        {
            for (int i = 0; i < source.Length; i++)
                if (predicate(source[i], i))
                    return true;
            return false;
        }

        public static bool Any<TSource>(this Span<TSource> source, Func<TSource, int, bool> predicate) =>
            ((ReadOnlySpan<TSource>)source).Any(predicate);

        public static int Count<TSource>(this ReadOnlySpan<TSource> source, Func<TSource, bool> predicate)
        {
            int count = 0;
            for (int i = 0; i < source.Length; i++)
                count += predicate(source[i]) ? 1 : 0;
            return count;
        }

        public static int Count<TSource>(this Span<TSource> source, Func<TSource, bool> predicate) =>
            ((ReadOnlySpan<TSource>)source).Count(predicate);

        public static int Count<TSource>(this ReadOnlySpan<TSource> source, Func<TSource, int, bool> predicate)
        {
            int count = 0;
            for (int i = 0; i < source.Length; i++)
                count += predicate(source[i], i) ? 1 : 0;
            return count;
        }

        public static int Count<TSource>(this Span<TSource> source, Func<TSource, int, bool> predicate) =>
            ((ReadOnlySpan<TSource>)source).Count(predicate);

        public static int Sum(this ReadOnlySpan<int> source)
        {
            int sum = 0;
            for (int i = 0; i < source.Length; i++)
                sum += source[i];
            return sum;
        }

        public static int Sum(this Span<int> source) =>
            ((ReadOnlySpan<int>)source).Sum();

        public static int Sum<TSource>(this ReadOnlySpan<TSource> source, Func<TSource, int> selector)
        {
            int sum = 0;
            for (int i = 0; i < source.Length; i++)
                sum += selector(source[i]);
            return sum;
        }

        public static int Sum<TSource>(this Span<TSource> source, Func<TSource, int> selector) =>
            ((ReadOnlySpan<TSource>)source).Sum(selector);

        public static int Sum<TSource>(this ReadOnlySpan<TSource> source, Func<TSource, int, int> selector)
        {
            int sum = 0;
            for (int i = 0; i < source.Length; i++)
                sum += selector(source[i], i);
            return sum;
        }

        public static int Sum<TSource>(this Span<TSource> source, Func<TSource, int, int> selector) =>
            ((ReadOnlySpan<TSource>)source).Sum(selector);

        public static int Product(this ReadOnlySpan<int> source)
        {
            int product = 1;
            for (int i = 0; i < source.Length; i++)
                product *= source[i];
            return product;
        }

        public static int Product(this Span<int> source) =>
            ((ReadOnlySpan<int>)source).Product();

        public static int Product<TSource>(this ReadOnlySpan<TSource> source, Func<TSource, int> selector)
        {
            int product = 1;
            for (int i = 0; i < source.Length; i++)
                product *= selector(source[i]);
            return product;
        }

        public static int Product<TSource>(this Span<TSource> source, Func<TSource, int> selector) =>
            ((ReadOnlySpan<TSource>)source).Product(selector);

        public static int Product<TSource>(this ReadOnlySpan<TSource> source, Func<TSource, int, int> selector)
        {
            int product = 1;
            for (int i = 0; i < source.Length; i++)
                product *= selector(source[i], i);
            return product;
        }

        public static int Product<TSource>(this Span<TSource> source, Func<TSource, int, int> selector) =>
            ((ReadOnlySpan<TSource>)source).Product(selector);

#if !NET8_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Split(this ReadOnlySpan<char> source, Span<System.Range> destination, char separator, StringSplitOptions options = StringSplitOptions.None)
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
        public static int Split(this ReadOnlySpan<char> source, Span<System.Range> destination, ReadOnlySpan<char> separator, StringSplitOptions options = StringSplitOptions.None)
        {
            int index = 0, start = 0;
            for (int curr = 0; curr <= source.Length - separator.Length; curr++)
            {
                if (source[curr..(curr + separator.Length)].SequenceEqual(separator))
                {
                    index += AddItem(source, destination, options, index, start, curr);
                    start = curr + separator.Length;
                }
            }
            index += AddItem(source, destination, options, index, start, source.Length);
            return index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int AddItem(ReadOnlySpan<char> source, Span<System.Range> destination, StringSplitOptions options, int index, int start, int end)
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
