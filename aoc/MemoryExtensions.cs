using System;
using System.Runtime.CompilerServices;

namespace aoc
{
    public static class MemoryExtensions
    {
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
