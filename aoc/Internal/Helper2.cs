using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace aoc.Internal
{
    delegate bool TryParse<T, TSeparator>(ReadOnlySpan<char> input, TSeparator separator, out T value);

    abstract class Helper2<T, TItem, TStrategy> : Helper<T, TItem, TStrategy>
        where T : unmanaged, IReadOnlyCollection<TItem>
        where TItem : unmanaged, IFormattable
        where TStrategy : IHelperStrategy<T, TItem>
    {
        protected Helper2(FromSpan<T, TItem> fromSpan, TryParse<TItem> tryParse, TryParse<TItem, char> tryParseChar, int chunkSize)
            : base(fromSpan, tryParse, chunkSize)
        {
            TryParseItemChar = tryParseChar;
        }

        private TryParse<TItem, char> TryParseItemChar { get; }

        public T Parse(ReadOnlySpan<char> input, char separator, char separator2) =>
            TryParse(input, separator, separator2, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(ReadOnlySpan<char> input, char separator, char separator2, out T value)
        {
            Span<System.Range> split = stackalloc System.Range[MaxCount];
            int count = input.Split(split, separator, StringSplitOptions.TrimEntries);
            return TryParse(input, split[..count], separator2, out value);
        }

        private bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<System.Range> split, char separator, out T value)
        {
            value = default;
            if (split.Length < MinCount || split.Length > MaxCount)
                return false;
            Span<TItem> values = stackalloc TItem[split.Length];
            if (!TryParse(input, split, separator, values))
                return false;
            value = FromSpan(values);
            return true;
        }

        private bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<System.Range> split, char separator, Span<TItem> values)
        {
            for (int i = 0; i < split.Length; i++)
                if (!TryParseItemChar(input[split[i]], separator, out values[i]))
                    return false;
            return true;
        }
    }

    abstract class Helper2<T, TVector, TItem2, TStrategy, TVectorHelper> : Helper2<T, TVector, TStrategy>
        where T : unmanaged, IReadOnlyCollection<TVector>
        where TVectorHelper : IVectorHelper<TVector, TItem2>
        where TVector : unmanaged, IVector<TVector, TItem2>
        where TItem2 : unmanaged, IFormattable
        where TStrategy : IHelperStrategy<T, TVector>
    {
        protected Helper2(FromSpan<T, TVector> fromSpan, TVectorHelper vector)
            : base(fromSpan, vector.TryParse, vector.TryParse, vector.MinCount)
        {
            Vector = vector;
        }

        protected TVectorHelper Vector { get; }

        public T[] ParseAll(string input, int chunkCount, int chunkSize) =>
            TryParseAll(input, chunkCount, chunkSize, out var value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public sealed override bool TryParseAll(string input, int chunkSize, out T[] values) =>
            TryParseAll(input, MinCount, chunkSize, out values);

        public bool TryParseAll(string input, int chunkCount, int chunkSize, out T[] values) =>
            TryParseAll(input, chunkCount, chunkSize, FromSpan, out values);

        protected sealed override IEnumerable<Match> GetMatches(string input, out int count) =>
            Vector.GetMatches(input, out count);

        protected bool TryParseAny(string input, FromSpan<T, TVector> fromSpan, out T value)
        {
            value = default;
            return TryGetMatches(input, out var matches, out var matchCount, out var chunkSize)
                && TryParse(matches.GetEnumerator(), matchCount, chunkSize, fromSpan, out value);
        }

        protected bool TryParseAll(string input, int chunkCount, int chunkSize, FromSpan<T, TVector> fromSpan, out T[] values)
        {
            values = default;
            var matches = GetMatches(input, out var matchCount);
            var itemSize = chunkCount * chunkSize;
            if (matchCount == 0 || matchCount % itemSize != 0)
                return false;
            values = new T[matchCount / itemSize];
            var enumerator = matches.GetEnumerator();
            for (int i = 0; i < values.Length; i++)
                if (!TryParse(enumerator, itemSize, chunkSize, fromSpan, out values[i]))
                    return false;
            return true;
        }

        private bool TryParse(IEnumerator<Match> matches, int itemSize, int chunkSize, FromSpan<T, TVector> fromSpan, out T value)
        {
            value = default;
            Span<TItem2> items = stackalloc TItem2[itemSize];
            if (!Vector.TryParse(matches, items))
                return false;
            value = FromItems(items, itemSize, chunkSize, fromSpan);
            return true;
        }

        protected T FromItems(ReadOnlySpan<TItem2> items, int itemSize, int chunkSize, FromSpan<T, TVector> fromSpan)
        {
            Span<TVector> vectors = stackalloc TVector[itemSize / chunkSize];
            for (int i = 0; i < vectors.Length; ++i)
                vectors[i] = Vector.FromSpan(items[(i * chunkSize)..((i + 1) * chunkSize)]);
            return fromSpan(vectors);
        }
    }
}
