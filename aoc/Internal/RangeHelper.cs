﻿using System;

namespace aoc.Internal
{
    sealed class RangeHelperStrategy<TRange, T> : ItemHelperStrategy<RangeHelperStrategy<TRange, T>, TRange, T>
        where TRange : struct, IRange<TRange, T>
        where T : struct, IFormattable
    {
        private RangeHelperStrategy()
            : base('~', " ~ ", "min", "max")
        {
        }

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
            : base(RangeHelperStrategy<TRange, T>.Instance, fromSpan, number)
        {
        }
    }
}
