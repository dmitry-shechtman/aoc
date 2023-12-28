using System;
using System.Collections.Generic;

namespace aoc
{
    interface IHelper1Strategy : IHelperStrategy
    {
        int Count { get; }
    }

    abstract class Helper1Strategy<TSelf> : HelperStrategy<TSelf>, IHelper1Strategy
        where TSelf : Helper1Strategy<TSelf>
    {
        protected Helper1Strategy(string[] formatKeys)
            : base(formatKeys)
        {
        }

        public int Count => FormatKeys.Length;

        protected override string SeparatorString =>
            $"{DefaultSeparator}";
    }

    delegate bool TryParseValue1<T>(string s, out T value);

    abstract class Helper1<T, TValue, TStrategy> : Helper<T, TValue, TryParseValue1<TValue>, TStrategy>
        where T : IReadOnlyList<TValue>
        where TValue : IFormattable
        where TStrategy : IHelper1Strategy
    {
        protected Helper1(Func<TValue[], T> fromArray, TryParseValue1<TValue> tryParse)
            : base(fromArray, tryParse)
        {
            Count = Strategy.Count;
        }

        private int Count { get; }

        public override T Parse(string s, char separator) =>
            TryParse(s, separator, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public override bool TryParse(string s, char separator, out T value) =>
            TryParse(s.Trim().Split(separator, StringSplitOptions.TrimEntries), out value);

        public override T Parse(string[] ss) =>
            TryParse(ss, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public override bool TryParse(string[] ss, out T value)
        {
            value = default;
            if (ss.Length < Count ||
                !TryParse(ss, out TValue[] values))
                    return false;
            value = FromArray(values);
            return true;
        }

        private bool TryParse(string[] ss, out TValue[] values)
        {
            values = new TValue[Count];
            for (int i = 0; i < Count; i++)
                if (!TryParseValue(ss[i], out values[i]))
                    return false;
            return true;
        }
    }
}
