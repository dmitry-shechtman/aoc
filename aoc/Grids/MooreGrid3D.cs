using System.Collections.Generic;

namespace aoc.Grids
{
    public abstract class MooreGrid3D<TSelf> : Grid3D<TSelf>
        where TSelf : MooreGrid3D<TSelf>
    {
        internal abstract class MooreHelper<THelper, TGrid> : Helper<THelper, TGrid>
            where THelper : MooreHelper<THelper, TGrid>
            where TGrid : MooreGrid3D<TGrid>
        {
            protected MooreHelper()
            {
            }

            public override IEnumerable<Vector3D> GetNeighbors(Vector3D p)
            {
                for (var z = p.z - 1; z <= p.z + 1; z++)
                    for (var y = p.y - 1; y <= p.y + 1; y++)
                        for (var x = p.x - 1; x <= p.x + 1; x++)
                            if (p != (x, y, z))
                                yield return new(x, y, z);
            }

            public override IEnumerable<Vector3D> GetNeighborsAndSelf(Vector3D p)
            {
                for (var z = p.z - 1; z <= p.z + 1; z++)
                    for (var y = p.y - 1; y <= p.y + 1; y++)
                        for (var x = p.x - 1; x <= p.x + 1; x++)
                            yield return new(x, y, z);
            }

            public override int CountNeighbors(TGrid grid, Vector3D p)
            {
                int count = 0;
                for (var z = p.z - 1; z <= p.z + 1; z++)
                    for (var y = p.y - 1; y <= p.y + 1; y++)
                        for (var x = p.x - 1; x <= p.x + 1; x++)
                            count += p != (x, y, z) && grid.Points.Contains((x, y, z)) ? 1 : 0;
                return count;
            }

            public override int CountNeighborsAndSelf(TGrid grid, Vector3D p)
            {
                int count = 0;
                for (var z = p.z - 1; z <= p.z + 1; z++)
                    for (var y = p.y - 1; y <= p.y + 1; y++)
                        for (var x = p.x - 1; x <= p.x + 1; x++)
                            count += grid.Points.Contains((x, y, z)) ? 1 : 0;
                return count;
            }
        }

        internal sealed class MooreHelper : MooreHelper<MooreHelper, MooreGrid3D>
        {
            private MooreHelper()
            {
            }

            protected override MooreGrid3D CreateGrid(HashSet<Vector3D> points) => new(points);
        }

        protected MooreGrid3D(IEnumerable<Vector3D> points)
            : base(points)
        {
        }

        protected MooreGrid3D(IEnumerable<Vector> points)
            : base(points)
        {
        }
    }

    public sealed class MooreGrid3D : MooreGrid3D<MooreGrid3D>, IGrid3D<Grid3D, Vector3D>
    {
        static new MooreHelper Helper { get; } = MooreHelper.Instance;

        internal MooreGrid3D(HashSet<Vector3D> points)
            : base(points)
        {
        }

        public MooreGrid3D(params Vector3D[] points)
            : base(points)
        {
        }

        public MooreGrid3D(IEnumerable<Vector3D> points)
            : base(points)
        {
        }

        public MooreGrid3D(IEnumerable<Vector> points)
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

        public static Builders.IVectorBuilder<Vector3D>            Vector => Helper;
        public static Builders.IPathBuilder<Vector3D>              Path   => Helper;
        public static Builders.INextBuilder<MooreGrid3D, Vector3D> Next   => Helper;
    }
}
