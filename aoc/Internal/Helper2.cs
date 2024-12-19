using System;
using System.Collections.Generic;

namespace aoc.Internal
{
    delegate bool TryParse<T, TSeparator>(string s, TSeparator separator, out T value);

    abstract class Helper2<T, TItem, TStrategy> : Helper<T, TItem, TStrategy>
        where T : IReadOnlyCollection<TItem>
        where TItem : IFormattable
        where TStrategy : IHelperStrategy<T, TItem>
    {
        protected Helper2(FromArray<T, TItem> fromArray, TryParse<TItem> tryParse, TryParse<TItem, char> tryParseChar)
            : base(fromArray, tryParse)
        {
            TryParseItemChar = tryParseChar;
        }

        private TryParse<TItem, char> TryParseItemChar { get; }

        public T Parse(string s, char separator, char separator2) =>
            TryParse(s, separator, separator2, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(string s, char separator, char separator2, out T value) =>
            TryParse(s.Trim().Split(separator, StringSplitOptions.TrimEntries), separator2, out value);

        public T Parse(string[] ss, char separator) =>
            TryParse(ss, separator, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(string[] ss, char separator, out T value)
        {
            value = default;
            if (ss.Length < MinCount ||
                !TryParse(ss, separator, out TItem[] values))
                    return false;
            value = FromArray(values);
            return true;
        }

        private bool TryParse(string[] ss, char separator, out TItem[] values)
        {
            values = new TItem[MaxCount];
            for (int i = 0; i < MaxCount; i++)
                if (i < ss.Length &&
                    !TryParseItemChar(ss[i], separator, out values[i]))
                        return false;
            return true;
        }
    }

    abstract class Helper2<T, TVector, TItem2, TStrategy, TVectorHelper> : Helper2<T, TVector, TStrategy>
        where T : IReadOnlyCollection<TVector>
        where TVectorHelper : IVectorHelper<TVector, TItem2>
        where TVector : struct, IVector<TVector, TItem2>
        where TItem2 : struct, IFormattable
        where TStrategy : IHelperStrategy<T, TVector>
    {
        protected Helper2(FromArray<T, TVector> fromArray, TVectorHelper vector)
            : base(fromArray, vector.TryParse, vector.TryParse)
        {
        }
    }
}
