using System;
using System.Collections.Generic;

namespace aoc.Grids
{
    public abstract class MooreGrid<TSelf> : Grid<TSelf>
        where TSelf : MooreGrid<TSelf>
    {
        internal abstract class Helper<THelper, TGrid> : Internal.GridHelper2<THelper, TGrid>
            where THelper : Helper<THelper, TGrid>
            where TGrid : MooreGrid<TGrid>
        {
            public override Vector[] Headings => new[]
            {
                Vector.North, Vector.NorthEast, Vector.East, Vector.SouthEast,
                Vector.South, Vector.SouthWest, Vector.West, Vector.NorthWest,
            };

            protected override string[][] FormatStrings => new[]
            {
                new[] { "n", "ne", "e", "se", "s", "sw", "w", "nw" }
            };

            public override IEnumerable<Vector> GetNeighbors(Vector p)
            {
                for (var y = p.y - 1; y <= p.y + 1; y++)
                    for (var x = p.x - 1; x <= p.x + 1; x++)
                        if (p != (x, y))
                            yield return new(x, y);
            }

            public override IEnumerable<Vector> GetNeighborsAndSelf(Vector p)
            {
                for (var y = p.y - 1; y <= p.y + 1; y++)
                    for (var x = p.x - 1; x <= p.x + 1; x++)
                        yield return new(x, y);
            }

            public override int CountNeighbors(TGrid grid, Vector p)
            {
                int count = 0;
                for (var y = p.y - 1; y <= p.y + 1; y++)
                    for (var x = p.x - 1; x <= p.x + 1; x++)
                        count += p != (x, y) && grid.Points.Contains((x, y)) ? 1 : 0;
                return count;
            }

            public override int CountNeighborsAndSelf(TGrid grid, Vector p)
            {
                int count = 0;
                for (var y = p.y - 1; y <= p.y + 1; y++)
                    for (var x = p.x - 1; x <= p.x + 1; x++)
                        count += grid.Points.Contains((x, y)) ? 1 : 0;
                return count;
            }
        }

        internal sealed class Helper : Helper<Helper, MooreGrid>
        {
            private Helper()
            {
            }

            protected override MooreGrid CreateGrid(HashSet<Vector> points) => new(points);
        }

        protected MooreGrid(IEnumerable<Vector> points)
            : base(points)
        {
        }
    }

    public sealed class MooreGrid : MooreGrid<MooreGrid>
    {
        static new Helper Helper { get; } = Helper.Instance;

        public MooreGrid(params Vector[] points)
            : base(points)
        {
        }

        public MooreGrid(IEnumerable<Vector> points)
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

        public static IHeadingBuilder         Heading => Helper;
        public static IGridBuilder<MooreGrid> Builder => Helper;
        public static IVectorBuilder<Vector>  Vector  => Helper;
        public static IPathBuilder<Vector>    Path    => Helper;
    }
}
