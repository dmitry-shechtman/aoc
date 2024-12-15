using System;
using System.Collections;
using System.Collections.Generic;

namespace aoc.Grids
{
    using Helper = Internal.MultiGridParseHelper;

    public abstract class MultiGrid<TSelf, TGrid> : IReadOnlyList<TGrid>
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
    }

    public sealed class MultiGrid : MultiGrid<MultiGrid, Grid>
    {
        private static Helper Helper { get; } = Helper.Instance;

        public MultiGrid(params Grid[] grids)
            : base(grids)
        {
        }

        public static MultiGrid Parse(ReadOnlySpan<char> s) =>
            Helper.Parse(s);

        public static MultiGrid Parse(string s) =>
            Helper.Parse(s);

        public static MultiGrid Parse(ReadOnlySpan<char> s, out Size size) =>
            Helper.Parse(s, out size);

        public static MultiGrid Parse(string s, out Size size) =>
            Helper.Parse(s, out size);

        public static MultiGrid Parse(ReadOnlySpan<char> s, Func<char, bool> predicate) =>
            Helper.Parse(s, predicate);

        public static MultiGrid Parse(string s, Func<char, bool> predicate) =>
            Helper.Parse(s, predicate);

        public static MultiGrid Parse(ReadOnlySpan<char> s, Func<char, bool> predicate, out Size size) =>
            Helper.Parse(s, predicate, out size);

        public static MultiGrid Parse(string s, Func<char, bool> predicate, out Size size) =>
            Helper.Parse(s, predicate, out size);

        public static MultiGrid Parse(ReadOnlySpan<char> s, ReadOnlySpan<char> cc) =>
            Helper.Parse(s, cc);

        public static MultiGrid Parse(ReadOnlySpan<char> s, ReadOnlySpan<char> cc, out Size size) =>
            Helper.Parse(s, cc, out size);

        public static MultiGrid Parse(ReadOnlySpan<char> s, char separator, ReadOnlySpan<char> cc) =>
            Helper.Parse(s, separator, cc);

        public static MultiGrid Parse(ReadOnlySpan<char> s, char separator, ReadOnlySpan<char> cc, out Size size) =>
            Helper.Parse(s, separator, cc, out size);
    }
}
