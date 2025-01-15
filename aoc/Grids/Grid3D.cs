using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc.Grids
{
    public abstract class Grid3D<TSelf> : Grid<TSelf, Vector3D>
        where TSelf : Grid3D<TSelf>
    {
        internal abstract class Helper<THelper, TGrid> : Internal.GridHelper<THelper, TGrid, Vector3D>
            where THelper : Helper<THelper, TGrid>
            where TGrid : Grid3D<TGrid>
        {
            protected Helper()
            {
            }

            public override Vector3D[] Headings => new[]
            {
                Vector3D.North, Vector3D.East, Vector3D.South, Vector3D.West,
                Vector3D.Up,    Vector3D.Down
            };

            protected override string[][] FormatStrings => new[]
            {
                new[] { "n", "e", "s", "w", "u", "d" }
            };

            public override IEnumerable<Vector3D> GetNeighbors(Vector3D p) => new Vector3D[]
            {
                new(p.x, p.y, p.z - 1),
                new(p.x, p.y - 1, p.z),
                new(p.x + 1, p.y, p.z),
                new(p.x, p.y + 1, p.z),
                new(p.x - 1, p.y, p.z),
                new(p.x, p.y, p.z + 1)
            };

            public override IEnumerable<Vector3D> GetNeighborsAndSelf(Vector3D p) => new Vector3D[]
            {
                new(p.x, p.y, p.z),
                new(p.x, p.y, p.z - 1),
                new(p.x, p.y - 1, p.z),
                new(p.x + 1, p.y, p.z),
                new(p.x, p.y + 1, p.z),
                new(p.x - 1, p.y, p.z),
                new(p.x, p.y, p.z + 1)
            };
        }

        internal sealed class Helper : Helper<Helper, Grid3D>
        {
            private Helper()
            {
            }

            protected override Grid3D CreateGrid(HashSet<Vector3D> points) => new(points);
        }

        protected Grid3D(HashSet<Vector3D> points)
            : base(points)
        {
        }

        protected Grid3D(IEnumerable<Vector3D> points)
            : base(points)
        {
        }

        protected Grid3D(IEnumerable<Vector> points)
            : base(points.Select(p => new Vector3D(p)))
        {
        }

        public bool Add(int x, int y, int z) =>
            Points.Add(new(x, y, z));

        public bool Remove(int x, int y, int z) =>
            Points.Remove(new(x, y, z));

        public bool AddRange(Size3D size) =>
            new Vector3DRange(size).All(Points.Add);

        public bool AddRange(IEnumerable<Vector3D> range) =>
            range.All(Points.Add);

        public bool RemoveRange(Size3D size) =>
            new Vector3DRange(size).All(Points.Remove);

        public bool RemoveRange(IEnumerable<Vector3D> range) =>
            range.All(Points.Remove);
    }

    public sealed class Grid3D : Grid3D<Grid3D>, IGrid3D<Grid3D, Vector3D>
    {
        static new Helper Helper { get; } = Helper.Instance;

        internal Grid3D(HashSet<Vector3D> points)
            : base(points)
        {
        }

        public Grid3D(params Vector3D[] points)
            : base(points)
        {
        }

        public Grid3D(IEnumerable<Vector3D> points)
            : base(points)
        {
        }

        public Grid3D(IEnumerable<Vector> points)
            : base(points)
        {
        }

        public static IEnumerable<Vector3D> GetNeighbors(Vector3D p) =>
            Helper.GetNeighbors(p);

        public static IEnumerable<Vector3D> GetNeighborsAndSelf(Vector3D p) =>
            Helper.GetNeighborsAndSelf(p);

        public int CountNeighbors(Vector3D p) =>
            Helper.CountNeighbors(this, p);

        public int CountNeighborsAndSelf(Vector3D p) =>
            Helper.CountNeighborsAndSelf(this, p);

        public static Vector3D[] Headings =>
            Helper.Headings;

        public static Builders.IHeadingBuilder                Heading => Helper;
        public static Builders.IVectorBuilder<Vector3D>       Vector  => Helper;
        public static Builders.IPathBuilder<Vector3D>         Path    => Helper;
        public static Builders.INextBuilder<Grid3D, Vector3D> Next    => Helper;
    }
}
