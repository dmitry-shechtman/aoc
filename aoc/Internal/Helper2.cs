using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace aoc.Internal
{
    delegate bool TryParse<T, TSeparator>(ReadOnlySpan<char> input, TSeparator separator, out T value);
    delegate bool TryParseMatches<T>(IEnumerator<Match> matches, int itemCount, out T value);

    abstract class Helper2<T, TItem, TStrategy> : Helper<T, TItem, TStrategy>
        where T : unmanaged, IReadOnlyCollection<TItem>
        where TItem : unmanaged, IFormattable
        where TStrategy : IHelperStrategy<T, TItem>
    {
        protected Helper2(FromSpan<T, TItem> fromSpan, TryParse<TItem> tryParse, TryParse<TItem, char> tryParseChar,
                TryParseMatches<TItem> tryParseMatches, int chunkSize)
            : base(fromSpan, tryParse, chunkSize)
        {
            TryParseItemChar = tryParseChar;
            TryParseItemMatches = tryParseMatches;
        }

        private TryParse<TItem, char> TryParseItemChar { get; }
        private TryParseMatches<TItem> TryParseItemMatches { get; }

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

        protected sealed override bool TryParse(IEnumerator<Match> matches, int chunkSize, Span<TItem> items)
        {
            for (int i = 0; i < items.Length; i++)
                if (!TryParseItemMatches(matches, chunkSize, out items[i]))
                    return false;
            return true;
        }
    }

    abstract class Helper2<T, TVector, TItem2, TStrategy, TVectorHelper> : Helper2<T, TVector, TStrategy>
        where T : unmanaged, IReadOnlyCollection<TVector>
        where TVectorHelper : IVectorHelper<TVector, TItem2>
        where TVector : unmanaged, IVector<TVector, TItem2>
        where TItem2 : struct, IFormattable
        where TStrategy : IHelperStrategy<T, TVector>
    {
        protected Helper2(FromSpan<T, TVector> fromSpan, TVectorHelper vector)
            : base(fromSpan, vector.TryParse, vector.TryParse, vector.TryParse, vector.MinCount)
        {
            Vector = vector;
        }

        protected TVectorHelper Vector { get; }

        protected sealed override IEnumerable<Match> GetMatches(string input, out int count) =>
            Vector.GetMatches(input, out count);
    }
}
