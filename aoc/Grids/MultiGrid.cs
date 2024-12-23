using System;
using System.Collections;
using System.Collections.Generic;

namespace aoc.Grids
{
    using Helper = Internal.MultiGridParseHelper;

    public abstract class MultiGrid<TSelf, TGrid> : IReadOnlyList<TGrid>, IFormattableEx
        where TSelf : MultiGrid<TSelf, TGrid>
        where TGrid : Grid<TGrid>
    {
        protected MultiGrid(TGrid[] grids)
        {
            Grids = grids;
        }

        private TGrid[] Grids { get; }

        public TGrid this[int index] =>
            Grids[index];

        public IEnumerator<TGrid> GetEnumerator() =>
            ((IEnumerable<TGrid>)Grids).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public int Count => Grids.Length;

        public TGrid[] Slice(int start, int length) =>
            Grids[start..(start + length)];

        public abstract string ToString(IFormatProvider provider);
        public abstract string ToString(string format, IFormatProvider formatProvider);
    }

    public sealed class MultiGrid : MultiGrid<MultiGrid, Grid>
    {
        private static Helper Helper { get; } = Helper.Instance;

        public MultiGrid(Grid[] grids)
            : base(grids)
        {
        }

        public override string ToString() =>
            Helper.ToString(this);

        public override string ToString(IFormatProvider provider) =>
            Helper.ToString(this, provider);

        public override string ToString(string format, IFormatProvider provider = null) =>
            Helper.ToString(this, format, provider);

        public string ToString(Size size, string format, IFormatProvider provider = null) =>
            Helper.ToString(this, size, format, provider);

        public string ToString(VectorRange range, string format, IFormatProvider provider = null) =>
            Helper.ToString(this, range, format, provider);

        public static MultiGrid Parse(ReadOnlySpan<char> input) =>
            Helper.Parse(input);

        public static MultiGrid Parse(string input) =>
            Helper.Parse(input);

        public static MultiGrid Parse(ReadOnlySpan<char> input, out VectorRange range) =>
            Helper.Parse(input, out range);

        public static MultiGrid Parse(ReadOnlySpan<char> input, Func<char, bool> predicate) =>
            Helper.Parse(input, predicate);

        public static MultiGrid Parse(ReadOnlySpan<char> input, Func<char, bool> predicate, out VectorRange range) =>
            Helper.Parse(input, predicate, out range);

        public static MultiGrid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format) =>
            Helper.Parse(input, format);

        public static MultiGrid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out VectorRange range) =>
            Helper.Parse(input, format, out range);

        public static MultiGrid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, ReadOnlySpan<char> separator) =>
            Helper.Parse(input, format, separator);

        public static MultiGrid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, ReadOnlySpan<char> separator, out VectorRange range) =>
            Helper.Parse(input, format, separator, out range);
    }
}
