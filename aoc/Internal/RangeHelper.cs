﻿using System;
using System.Text.RegularExpressions;

namespace aoc.Internal
{
    sealed class RangeHelperStrategy<TRange, T> : ItemHelperStrategy<RangeHelperStrategy<TRange, T>, TRange, T>
        where TRange : struct, IRange<TRange, T>
        where T : struct, IFormattable
    {
        private RangeHelperStrategy()
            : base("min", "max")
        {
        }

        public override int MinCount => 2;
        public override int MaxCount => 2;

        public override char DefaultSeparator => '~';

        protected override string SeparatorString =>
            $" {DefaultSeparator} ";

        protected override T GetItem(TRange range, int i) => i switch
        {
            0 => range.Min,
            1 => range.Max,
            _ => throw new IndexOutOfRangeException(),
        };
    }

    sealed class RangeHelper<TRange, T> : Helper<TRange, T, RangeHelperStrategy<TRange, T>>
        where TRange : unmanaged, IRange<TRange, T>
        where T : unmanaged, IFormattable
    {
        public RangeHelper(FromSpan<TRange, T> fromSpan, INumberHelper<T> number)
            : base(fromSpan, number.TryParse)
        {
            Number = number;
        }

        private INumberHelper<T> Number { get; }

        protected override MatchCollection GetMatches(string input, out int count) =>
            Number.GetMatches(input, out count);

        protected override RangeHelperStrategy<TRange, T> Strategy =>
            RangeHelperStrategy<TRange, T>.Instance;
    }
}
