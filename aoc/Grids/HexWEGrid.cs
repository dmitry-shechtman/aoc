using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace aoc.Grids
{
    public abstract class HexWEGrid<TSelf> : Grid2D<TSelf>
        where TSelf : HexWEGrid<TSelf>
    {
        internal abstract class Helper<THelper, TGrid> : Internal.Grid2DHelper2<THelper, TGrid>
            where THelper : Helper<THelper, TGrid>
            where TGrid : HexWEGrid<TGrid>
        {
            public override Vector[] Headings => new[]
            {
                Vector.East, Vector.SouthEast, Vector.SouthWest,
                Vector.West, Vector.NorthWest, Vector.NorthEast
            };

            protected override string[][] FormatStrings => new[]
            {
                new[] { "e", "se", "sw", "w", "nw", "ne" }
            };

            public override Vector[] GetNeighbors(Vector p) => new Vector[]
            {
                new(p.x - 2, p.y),
                new(p.x - 1, p.y - 1),
                new(p.x + 1, p.y - 1),
                new(p.x + 2, p.y),
                new(p.x + 1, p.y + 1),
                new(p.x - 1, p.y + 1),
            };

            public override Vector[] GetNeighborsAndSelf(Vector p) => new Vector[]
            {
                new(p.x, p.y),
                new(p.x - 2, p.y),
                new(p.x - 1, p.y - 1),
                new(p.x + 1, p.y - 1),
                new(p.x + 2, p.y),
                new(p.x + 1, p.y + 1),
                new(p.x - 1, p.y + 1),
            };
        }

        internal sealed class Helper : Helper<Helper, HexWEGrid>
        {
            private Helper()
            {
            }

            protected override HexWEGrid CreateGrid(HashSet<Vector> points) => new(points);
        }

        protected HexWEGrid(IEnumerable<Vector> points)
            : base(points)
        {
        }

        public static int Abs(Vector p) =>
            Math.Abs(p.y) + Math.Abs(Math.Abs(p.y) - Math.Abs(p.x)) / 2;
    }

    public sealed class HexWEGrid : HexWEGrid<HexWEGrid>, IGrid2D<HexWEGrid, Vector>
    {
        static new Helper Helper { get; } = Helper.Instance;

        public HexWEGrid(params Vector[] points)
            : base(points)
        {
        }

        public HexWEGrid(IEnumerable<Vector> points)
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

        public static Builders.IHeadingBuilder         Heading => Helper;
        public static Builders.IGridBuilder<HexWEGrid> Builder => Helper;
        public static Builders.IVectorBuilder<Vector>  Vector  => Helper;
        public static Builders.IPathBuilder<Vector>    Path    => Helper;

        public static HexWEGrid Parse(string input) =>
            Helper.Parse(input);

        public static bool TryParse(
            [NotNullWhen(true)] string? input,
            [MaybeNullWhen(false)] out HexWEGrid grid) =>
                Helper.TryParse(input, out grid);

        public static HexWEGrid Parse(ReadOnlySpan<char> input) =>
            Helper.Parse(input);

        public static bool TryParse(
            ReadOnlySpan<char> input,
            [MaybeNullWhen(false)] out HexWEGrid grid) =>
                Helper.TryParse(input, out grid);
    }
}
