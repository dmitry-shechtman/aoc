using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc.Grids
{
    public abstract class Grid4D<TSelf> : Grid<TSelf, Vector4D, Size4D, Vector4DRange, int>
        where TSelf : Grid4D<TSelf>
    {
        internal abstract class Helper<THelper> : Internal.GridHelper<THelper, TSelf, Vector4D, Size4D, Vector4DRange, int>
            where THelper : Helper<THelper>
        {
            protected Helper()
            {
            }

            public override Vector4D[] Headings => new[]
            {
                Vector4D.North, Vector4D.East, Vector4D.South, Vector4D.West,
                Vector4D.Up,    Vector4D.Down, Vector4D.Ana,   Vector4D.Kata
            };

            protected override string[][] FormatStrings => new[]
            {
                new[] { "n", "e", "s", "w", "u", "d", "a", "k" }
            };

            public override IEnumerable<Vector4D> GetNeighbors(Vector4D p) => new Vector4D[]
            {
                new(p.x, p.y, p.z, p.w - 1),
                new(p.x, p.y, p.z - 1, p.w),
                new(p.x, p.y - 1, p.z, p.w),
                new(p.x - 1, p.y, p.z, p.w),
                new(p.x + 1, p.y, p.z, p.w),
                new(p.x, p.y + 1, p.z, p.w),
                new(p.x, p.y, p.z + 1, p.w),
                new(p.x, p.y, p.z, p.w + 1)
            };

            public override IEnumerable<Vector4D> GetNeighborsAndSelf(Vector4D p) => new Vector4D[]
            {
                new(p.x, p.y, p.z, p.w - 1),
                new(p.x, p.y, p.z - 1, p.w),
                new(p.x, p.y - 1, p.z, p.w),
                new(p.x - 1, p.y, p.z, p.w),
                new(p.x, p.y, p.z, p.w),
                new(p.x + 1, p.y, p.z, p.w),
                new(p.x, p.y + 1, p.z, p.w),
                new(p.x, p.y, p.z + 1, p.w),
                new(p.x, p.y, p.z, p.w + 1)
            };
        }

        internal sealed class Helper : Helper<Helper>
        {
            private Helper()
            {
            }
        }

        protected Grid4D(IEnumerable<Vector4D> points)
            : base(points)
        {
        }

        protected Grid4D(IEnumerable<Vector3D> points)
            : base(points.Select(p => new Vector4D(p)))
        {
        }

        protected Grid4D(IEnumerable<Vector> points)
            : base(points.Select(p => new Vector4D(p)))
        {
        }

        public bool Add(int x, int y, int z, int w) =>
            Points.Add(new(x, y, z, w));

        public bool Remove(int x, int y, int z, int w) =>
            Points.Remove(new(x, y, z, w));

        public bool AddRange(Size4D size) =>
            new Vector4DRange(size).All(Points.Add);

        public bool AddRange(IEnumerable<Vector4D> range) =>
            range.All(Points.Add);

        public bool RemoveRange(Size4D size) =>
            new Vector4DRange(size).All(Points.Remove);

        public bool RemoveRange(IEnumerable<Vector4D> range) =>
            range.All(Points.Remove);
    }

    public sealed class Grid4D : Grid4D<Grid4D>
    {
        static new Helper Helper { get; } = Helper.Instance;

        public Grid4D(params Vector4D[] points)
            : base(points)
        {
        }

        public Grid4D(IEnumerable<Vector4D> points)
            : base(points)
        {
        }

        public Grid4D(IEnumerable<Vector3D> points)
            : base(points)
        {
        }

        public Grid4D(IEnumerable<Vector> points)
            : base(points)
        {
        }

        public static IEnumerable<Vector4D> GetNeighbors(Vector4D p) =>
            Helper.GetNeighbors(p);

        public static IEnumerable<Vector4D> GetNeighborsAndSelf(Vector4D p) =>
            Helper.GetNeighborsAndSelf(p);

        public int CountNeighbors(Vector4D p) =>
            Helper.CountNeighbors(this, p);

        public int CountNeighborsAndSelf(Vector4D p) =>
            Helper.CountNeighborsAndSelf(this, p);

        public static Vector4D[] Headings =>
            Helper.Headings;

        public static IHeadingBuilder          Heading => Helper;
        public static IVectorBuilder<Vector4D> Vector  => Helper;
        public static IPathBuilder<Vector4D>   Path    => Helper;
    }
}
