﻿using System;
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

        public TGrid[] Slice(int start, int length)
        {
            var slice = new TGrid[length];
            Array.Copy(Grids, start, slice, 0, length);
            return slice;
        }
    }

    public sealed class MultiGrid : MultiGrid<MultiGrid, Grid>
    {
        private static Helper Helper { get; } = Helper.Instance;

        public MultiGrid(Grid[] grids)
            : base(grids)
        {
        }

        public static MultiGrid Parse(string s, ReadOnlySpan<char> cc) =>
            Helper.Parse(s, cc);

        public static MultiGrid Parse(string s, ReadOnlySpan<char> cc, out Size size) =>
            Helper.Parse(s, cc, out size);

        public static MultiGrid Parse(string s, char separator, ReadOnlySpan<char> cc) =>
            Helper.Parse(s, separator, cc);

        public static MultiGrid Parse(string s, char separator, ReadOnlySpan<char> cc, out Size size) =>
            Helper.Parse(s, separator, cc, out size);

        public static MultiGrid Parse(string[] ss, ReadOnlySpan<char> cc) =>
            Helper.Parse(ss, cc);

        public static MultiGrid Parse(string[] ss, ReadOnlySpan<char> cc, out Size size) =>
            Helper.Parse(ss, cc, out size);
    }
}
