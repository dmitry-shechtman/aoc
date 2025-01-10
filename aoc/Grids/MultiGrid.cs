using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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

        public abstract string ToString(IFormatProvider? provider);
        public abstract string ToString(string? format, IFormatProvider? formatProvider);
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

        public override string ToString(IFormatProvider? provider) =>
            Helper.ToString(this, provider);

        public override string ToString(string? format, IFormatProvider? provider = null) =>
            Helper.ToString(this, format, provider);

        public static Builders.IMultiGridBuilder<MultiGrid, Grid> Builder => Helper;

        public static MultiGrid Parse(string? input) =>
            Helper.Parse(input);

        public static bool TryParse(
            [NotNullWhen(true)] string? input,
            [MaybeNullWhen(false)] out MultiGrid multi) =>
                Helper.TryParse(input, out multi);

        public static MultiGrid Parse(ReadOnlySpan<char> input) =>
            Helper.Parse(input);

        public static bool TryParse(
            ReadOnlySpan<char> input,
            [MaybeNullWhen(false)] out MultiGrid multi) =>
                Helper.TryParse(input, out multi);
    }
}
