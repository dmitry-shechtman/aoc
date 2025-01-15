using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace aoc.Grids
{
    public abstract class DiagGrid<TSelf> : Grid2D<TSelf>
        where TSelf : DiagGrid<TSelf>
    {
        internal abstract class Helper<THelper, TGrid> : Internal.Grid2DHelper2<THelper, TGrid>
            where THelper : Helper<THelper, TGrid>
            where TGrid : DiagGrid<TGrid>
        {
            public override Vector[] Headings => new[]
            {
                Vector.NorthEast, Vector.SouthEast, Vector.SouthWest, Vector.NorthWest
            };

            protected override string[][] FormatStrings => new[]
            {
                new[] { "ne", "se", "sw", "nw" }
            };

            public override IEnumerable<Vector> GetNeighbors(Vector p) => new Vector[]
            {
                new(p.x - 1, p.y - 1),
                new(p.x + 1, p.y - 1),
                new(p.x - 1, p.y + 1),
                new(p.x + 1, p.y + 1)
            };

            public override IEnumerable<Vector> GetNeighborsAndSelf(Vector p) => new Vector[]
            {
                new(p.x, p.y),
                new(p.x - 1, p.y - 1),
                new(p.x + 1, p.y - 1),
                new(p.x - 1, p.y + 1),
                new(p.x + 1, p.y + 1)
            };
        }

        internal sealed class Helper : Helper<Helper, DiagGrid>
        {
            private Helper()
            {
            }

            protected override DiagGrid CreateGrid(HashSet<Vector> points) => new(points);
        }

        protected DiagGrid(HashSet<Vector> points)
            : base(points)
        {
        }

        protected DiagGrid(IEnumerable<Vector> points)
            : base(points)
        {
        }
    }

    public sealed class DiagGrid : DiagGrid<DiagGrid>, IGrid2D<DiagGrid, Vector>
    {
        static new Helper Helper { get; } = Helper.Instance;

        internal DiagGrid(HashSet<Vector> points)
            : base(points)
        {
        }

        public DiagGrid(params Vector[] points)
            : base(points)
        {
        }

        public DiagGrid(IEnumerable<Vector> points)
            : base(points)
        {
        }

        public override string ToString() =>
            Helper.ToString(this);

        public override string ToString(IFormatProvider? provider) =>
            Helper.ToString(this, provider);

        public override string ToString(string? format, IFormatProvider? provider = null) =>
            Helper.ToString(this, format, provider);

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
        public static Builders.IGridBuilder<DiagGrid>  Builder => Helper;
        public static Builders.IVectorBuilder<Vector>  Vector  => Helper;
        public static Builders.IPathBuilder<Vector>    Path    => Helper;

        public static DiagGrid Parse(string input) =>
            Helper.Parse(input);

        public static bool TryParse(
            [NotNullWhen(true)] string? input,
            [MaybeNullWhen(false)] out DiagGrid grid) =>
                Helper.TryParse(input, out grid);

        public static DiagGrid Parse(ReadOnlySpan<char> input) =>
            Helper.Parse(input);

        public static bool TryParse(
            ReadOnlySpan<char> input,
            [MaybeNullWhen(false)] out DiagGrid grid) =>
                Helper.TryParse(input, out grid);
    }
}
