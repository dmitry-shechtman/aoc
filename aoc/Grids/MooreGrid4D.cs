﻿using System.Collections.Generic;

namespace aoc.Grids
{
    public abstract class MooreGrid4D<TSelf> : Grid4D<TSelf>
        where TSelf : MooreGrid4D<TSelf>
    {
        internal abstract class MooreHelper<THelper, TGrid> : Helper<THelper, TGrid>
            where THelper : MooreHelper<THelper, TGrid>
            where TGrid : MooreGrid4D<TGrid>
        {
            protected MooreHelper()
            {
            }

            public override IEnumerable<Vector4D> GetNeighbors(Vector4D p)
            {
                for (var w = p.w - 1; w <= p.w + 1; ++w)
                    for (var z = p.z - 1; z <= p.z + 1; z++)
                        for (var y = p.y - 1; y <= p.y + 1; y++)
                            for (var x = p.x - 1; x <= p.x + 1; x++)
                                if (p != (x, y, z, w))
                                    yield return new(x, y, z, w);
            }

            public override IEnumerable<Vector4D> GetNeighborsAndSelf(Vector4D p)
            {
                for (var w = p.w - 1; w <= p.w + 1; ++w)
                    for (var z = p.z - 1; z <= p.z + 1; z++)
                        for (var y = p.y - 1; y <= p.y + 1; y++)
                            for (var x = p.x - 1; x <= p.x + 1; x++)
                                yield return new(x, y, z, w);
            }

            public override int CountNeighbors(TGrid grid, Vector4D p)
            {
                int count = 0;
                for (var w = p.w - 1; w <= p.w + 1; ++w)
                    for (var z = p.z - 1; z <= p.z + 1; z++)
                        for (var y = p.y - 1; y <= p.y + 1; y++)
                            for (var x = p.x - 1; x <= p.x + 1; x++)
                                count += p != (x, y, z, w) && grid.Points.Contains((x, y, z, w)) ? 1 : 0;
                return count;
            }

            public override int CountNeighborsAndSelf(TGrid grid, Vector4D p)
            {
                int count = 0;
                for (var w = p.w - 1; w <= p.w + 1; ++w)
                    for (var z = p.z - 1; z <= p.z + 1; z++)
                        for (var y = p.y - 1; y <= p.y + 1; y++)
                            for (var x = p.x - 1; x <= p.x + 1; x++)
                                count += grid.Points.Contains((x, y, z, w)) ? 1 : 0;
                return count;
            }
        }

        internal sealed class MooreHelper : MooreHelper<MooreHelper, MooreGrid4D>
        {
            private MooreHelper()
            {
            }

            protected override MooreGrid4D CreateGrid(HashSet<Vector4D> points) => new(points);
        }

        protected MooreGrid4D(HashSet<Vector4D> points)
            : base(points)
        {
        }

        protected MooreGrid4D(IEnumerable<Vector4D> points)
            : base(points)
        {
        }

        protected MooreGrid4D(IEnumerable<Vector3D> points)
            : base(points)
        {
        }

        protected MooreGrid4D(IEnumerable<Vector> points)
            : base(points)
        {
        }
    }

    public sealed class MooreGrid4D : MooreGrid4D<MooreGrid4D>, IGrid4D<MooreGrid4D, Vector4D>
    {
        static new MooreHelper Helper { get; } = MooreHelper.Instance;

        internal MooreGrid4D(HashSet<Vector4D> points)
            : base(points)
        {
        }

        public MooreGrid4D(params Vector4D[] points)
            : base(points)
        {
        }

        public MooreGrid4D(IEnumerable<Vector4D> points)
            : base(points)
        {
        }

        public MooreGrid4D(IEnumerable<Vector3D> points)
            : base(points)
        {
        }

        public MooreGrid4D(IEnumerable<Vector> points)
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

        public static Builders.IVectorBuilder<Vector4D>            Vector => Helper;
        public static Builders.IPathBuilder<Vector4D>              Path   => Helper;
        public static Builders.INextBuilder<MooreGrid4D, Vector4D> Next   => Helper;
    }
}
