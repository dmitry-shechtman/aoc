using System;
using System.Collections.Generic;

namespace aoc.Internal
{
    delegate bool TryParse2<T>(string s, char separator, out T value);

    abstract class Helper2<T, TItem, TStrategy> : Helper<T, TItem, TStrategy>
        where T : IReadOnlyList<TItem>
        where TItem : IFormattable
        where TStrategy : IHelperStrategy
    {
        protected Helper2(Func<TItem[], T> fromArray, TryParse<TItem> tryParse, TryParse2<TItem> tryParse2)
            : base(fromArray, tryParse)
        {
            TryParseItem2 = tryParse2;
        }

        private TryParse2<TItem> TryParseItem2 { get; }

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
                    !TryParseItem2(ss[i], separator, out values[i]))
                        return false;
            return true;
        }
    }

    abstract class Helper2<T, TItem, TStrategy, TVectorHelper> : Helper2<T, TItem, TStrategy>
        where T : IReadOnlyList<TItem>
        where TVectorHelper : IVectorHelper<TItem>
        where TItem : struct, IFormattable
        where TStrategy : IHelperStrategy
    {
        protected Helper2(Func<TItem[], T> fromArray, TVectorHelper vector)
            : base(fromArray, vector.TryParse, vector.TryParse)
        {
        }
    }
}
