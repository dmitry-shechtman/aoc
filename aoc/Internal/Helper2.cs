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
        protected Helper2(FromSpan<T, TItem> fromSpan, TryParse<TItem> tryParse, TryParse<TItem, char> tryParseChar)
            : base(fromSpan, tryParse)
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
        where TItem2 : struct, IFormattable
        where TStrategy : IHelperStrategy<T, TVector>
    {
        protected Helper2(FromSpan<T, TVector> fromSpan, TVectorHelper vector)
            : base(fromSpan, vector.TryParse, vector.TryParse)
        {
            Vector = vector;
        }

        protected TVectorHelper Vector { get; }

        protected sealed override MatchCollection GetMatches(string input) =>
            Vector.GetMatches(input);
    }
}
