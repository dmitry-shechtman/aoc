using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace aoc.Internal
{
    abstract class Helper2<T, TItem, TStrategy, TItemHelper> : Helper<T, TItem, TStrategy, TItemHelper>
        where T : unmanaged, IReadOnlyCollection<TItem>
        where TItem : unmanaged, IFormattable
        where TStrategy : IHelperStrategy<T, TItem>
        where TItemHelper : IItemHelper<TItem>, IParseHelper<TItem, char>
    {
        protected Helper2(TStrategy strategy, FromSpan<T, TItem> fromSpan, TItemHelper item, int chunkCount, int chunkSize)
            : base(strategy, fromSpan, item, chunkCount, chunkSize)
        {
        }

        public T Parse(ReadOnlySpan<char> input, char separator, char separator2, IFormatProvider provider) =>
            TryParse(input, separator, separator2, provider, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(ReadOnlySpan<char> input, char separator, char separator2, out T value) =>
            TryParse(input, separator, separator2, null, out value);

        public bool TryParse(ReadOnlySpan<char> input, char separator, char separator2, IFormatProvider provider, out T value)
        {
            Span<System.Range> split = stackalloc System.Range[MaxCount];
            int count = input.Split(split, separator, StringSplitOptions.TrimEntries);
            return TryParse(input, split[..count], separator2, provider, out value);
        }

        private bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<System.Range> split, char separator, IFormatProvider provider, out T value)
        {
            value = default;
            if (split.Length < MinCount || split.Length > MaxCount)
                return false;
            Span<TItem> values = stackalloc TItem[split.Length];
            if (!TryParse(input, split, separator, values, provider))
                return false;
            value = FromSpan(values);
            return true;
        }

        private bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<System.Range> split, char separator, Span<TItem> values, IFormatProvider provider)
        {
            for (int i = 0; i < split.Length; i++)
                if (!Item.TryParse(input[split[i]], separator, provider, out values[i]))
                    return false;
            return true;
        }
    }

    abstract class Helper2<T, TVector, TItem2, TStrategy, TVectorHelper> : Helper2<T, TVector, TStrategy, TVectorHelper>
        where T : unmanaged, IReadOnlyCollection<TVector>
        where TVectorHelper : IVectorHelper<TVector, TItem2>
        where TVector : unmanaged, IVector<TVector, TItem2>
        where TItem2 : unmanaged, IFormattable
        where TStrategy : IHelperStrategy<T, TVector>
    {
        protected Helper2(TStrategy strategy, FromSpan<T, TVector> fromSpan, TVectorHelper vector)
            : base(strategy, fromSpan, vector, strategy.MinCount, vector.MinCount)
        {
        }

        protected TVectorHelper Vector => Item;

        public sealed override T[] ParseAll(string input, IFormatProvider provider, int chunkCount) =>
            ParseAll(input, provider, chunkCount, ChunkSize);

        public T[] ParseAll(string input, IFormatProvider provider, int chunkCount, int chunkSize) =>
            ParseAll(input, 0, provider, chunkCount, chunkSize);

        public T[] ParseAll(string input, NumberStyles styles, IFormatProvider provider, int chunkCount, int chunkSize) =>
            TryParseAll(input, styles, provider, chunkCount, chunkSize, out var value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParseAll(string input, int chunkCount, int chunkSize, out T[] values) =>
            TryParseAll(input, null, chunkCount, chunkSize, out values);

        public bool TryParseAll(string input, IFormatProvider provider, int chunkCount, int chunkSize, out T[] values) =>
            TryParseAll(input, 0, provider, chunkCount, chunkSize, out values);

        public bool TryParseAll(string input, NumberStyles styles, int chunkCount, int chunkSize, out T[] values) =>
            TryParseAll(input, styles, null, chunkCount, chunkSize, out values);

        protected override bool TryParse(IEnumerator<Match> matches, NumberStyles styles, IFormatProvider provider, int itemSize, int chunkSize, out T value)
        {
            value = default;
            Span<TItem2> items = stackalloc TItem2[itemSize];
            if (!Vector.TryParse(matches, styles, provider, items))
                return false;
            value = FromItems(items, itemSize, chunkSize);
            return true;
        }

        protected T FromItems(ReadOnlySpan<TItem2> items, int itemSize, int chunkSize)
        {
            Span<TVector> vectors = stackalloc TVector[itemSize / chunkSize];
            for (int i = 0; i < vectors.Length; ++i)
                vectors[i] = Vector.FromSpan(items[(i * chunkSize)..((i + 1) * chunkSize)]);
            return FromSpan(vectors);
        }
    }
}
