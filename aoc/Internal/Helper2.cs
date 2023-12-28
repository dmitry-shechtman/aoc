﻿using System;
using System.Collections.Generic;

namespace aoc.Internal
{
    interface IHelper2Strategy : IHelperStrategy
    {
        int  MinCount          { get; }
        int  MaxCount          { get; }
        char DefaultSeparator2 { get; }
    }

    abstract class Helper2Strategy<TSelf> : HelperStrategy<TSelf>, IHelper2Strategy
        where TSelf : Helper2Strategy<TSelf>
    {
        protected Helper2Strategy(params string[] formatKeys)
            : base(formatKeys)
        {
        }

        public abstract int MinCount { get; }
        public abstract int MaxCount { get; }

        public char DefaultSeparator2 => ',';

        protected override string SeparatorString =>
            $" {DefaultSeparator} ";
    }

    delegate bool TryParse2<T>(string s, char separator, out T value);

    abstract class Helper2<T, TItem, TStrategy> : Helper<T, TItem, TryParse2<TItem>, TStrategy>
        where T : IReadOnlyList<TItem>
        where TItem : IFormattable
        where TStrategy : IHelper2Strategy
    {
        protected Helper2(Func<TItem[], T> fromArray, TryParse2<TItem> tryParse)
            : base(fromArray, tryParse)
        {
            MinCount = Strategy.MinCount;
            MaxCount = Strategy.MaxCount;
            DefaultSeparator2 = Strategy.DefaultSeparator2;
        }

        private int  MinCount          { get; }
        private int  MaxCount          { get; }
        private char DefaultSeparator2 { get; }

        public override T Parse(string s, char separator) =>
            Parse(s, separator, DefaultSeparator2);

        public override bool TryParse(string s, char separator, out T value) =>
            TryParse(s, separator, DefaultSeparator2, out value);

        public override T Parse(string[] ss) =>
            Parse(ss, DefaultSeparator2);

        public override bool TryParse(string[] ss, out T value) =>
            TryParse(ss, DefaultSeparator2, out value);

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
                    !TryParseItem(ss[i], separator, out values[i]))
                        return false;
            return true;
        }
    }
}