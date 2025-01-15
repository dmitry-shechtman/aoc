using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace aoc.Grids
{
    public abstract class HexNSGrid<TSelf> : Grid2D<TSelf>
        where TSelf : HexNSGrid<TSelf>
    {
        internal abstract class Helper<THelper, TGrid> : Internal.Grid2DHelper2<THelper, TGrid>
            where THelper : Helper<THelper, TGrid>
            where TGrid : HexNSGrid<TGrid>
        {
            public override Vector[] Headings => new[]
            {
                Vector.North, Vector.NorthEast, Vector.SouthEast,
                Vector.South, Vector.SouthWest, Vector.NorthWest
            };

            protected override string[][] FormatStrings => new[]
            {
                new[] { "n", "ne", "se", "s", "sw", "nw" }
            };

            public override Vector[] GetNeighbors(Vector p) => new Vector[]
            {
                new(p.x, p.y + 2),
                new(p.x + 1, p.y + 1),
                new(p.x - 1, p.y + 1),
                new(p.x, p.y - 2),
                new(p.x - 1, p.y - 1),
                new(p.x + 1, p.y - 1)
            };

            public override Vector[] GetNeighborsAndSelf(Vector p) => new Vector[]
            {
                new(p.x, p.y),
                new(p.x, p.y + 2),
                new(p.x + 1, p.y + 1),
                new(p.x - 1, p.y + 1),
                new(p.x, p.y - 2),
                new(p.x - 1, p.y - 1),
                new(p.x + 1, p.y - 1)
            };
        }

        internal sealed class Helper : Helper<Helper, HexNSGrid>
        {
            private Helper()
            {
            }

            protected override HexNSGrid CreateGrid(HashSet<Vector> points) => new(points);
        }

        protected HexNSGrid(HashSet<Vector> points)
            : base(points)
        {
        }

        protected HexNSGrid(IEnumerable<Vector> points)
            : base(points)
        {
        }

        public static int Abs(Vector p) =>
            Math.Abs(p.x) + Math.Abs(Math.Abs(p.x) - Math.Abs(p.y)) / 2;
    }

    public sealed class HexNSGrid : HexNSGrid<HexNSGrid>, IGrid2D<HexNSGrid, Vector>
    {
        static new Helper Helper { get; } = Helper.Instance;

        internal HexNSGrid(HashSet<Vector> points)
            : base(points)
        {
        }

        public HexNSGrid(params Vector[] points)
            : base(points)
        {
        }

        public HexNSGrid(IEnumerable<Vector> points)
            : base(points)
        {
        }

        public override string ToString() =>
            Helper.ToString(this);

        public override string ToString(string? format, IFormatProvider? provider = null) =>
            Helper.ToString(this, format, provider);

        public override string ToString(IFormatProvider? provider) =>
            Helper.ToString(this, provider);

        public static IEnumerable<Vector> GetNeighbors(Vector p) =>
            Helper.GetNeighbors(p);

        public static IEnumerable<Vector> GetNeighborsAndSelf(Vector p) =>
            Helper.GetNeighborsAndSelf(p);

        public int CountNeighbors(Vector p) =>
            Helper.CountNeighbors(this, p);

        public int CountNeighborsAndSelf(Vector p) =>
            Helper.CountNeighborsAndSelf(this, p);

        public static Vector[] Headings =>
            Helper.Headings;

        public static Builders.IHeadingBuilder                 Heading => Helper;
        public static Builders.IGridBuilder<HexNSGrid>         Builder => Helper;
        public static Builders.IVectorBuilder<Vector>          Vector  => Helper;
        public static Builders.IPathBuilder<Vector>            Path    => Helper;
        public static Builders.INextBuilder<HexNSGrid, Vector> Next    => Helper;

        public static HexNSGrid Parse(string input) =>
            Helper.Parse(input);

        public static bool TryParse(
            [NotNullWhen(true)] string? input,
            [MaybeNullWhen(false)] out HexNSGrid grid) =>
                Helper.TryParse(input, out grid);

        public static HexNSGrid Parse(ReadOnlySpan<char> input) =>
            Helper.Parse(input);

        public static bool TryParse(
            ReadOnlySpan<char> input,
            [MaybeNullWhen(false)] out HexNSGrid grid) =>
                Helper.TryParse(input, out grid);
    }
}
