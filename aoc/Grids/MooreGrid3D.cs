using System.Collections.Generic;

namespace aoc.Grids
{
    public abstract class MooreGrid3D<TSelf> : Grid3D<TSelf>
        where TSelf : MooreGrid3D<TSelf>
    {
        internal sealed class MooreHelper : Helper<MooreHelper>
        {
            private MooreHelper()
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

            public override int CountNeighbors(TSelf grid, Vector3D p)
            {
                int count = 0;
                for (var z = p.z - 1; z <= p.z + 1; z++)
                    for (var y = p.y - 1; y <= p.y + 1; y++)
                        for (var x = p.x - 1; x <= p.x + 1; x++)
                            count += p != (x, y, z) && grid.Points.Contains((x, y, z)) ? 1 : 0;
                return count;
            }

            public override int CountNeighborsAndSelf(TSelf grid, Vector3D p)
            {
                int count = 0;
                for (var z = p.z - 1; z <= p.z + 1; z++)
                    for (var y = p.y - 1; y <= p.y + 1; y++)
                        for (var x = p.x - 1; x <= p.x + 1; x++)
                            count += grid.Points.Contains((x, y, z)) ? 1 : 0;
                return count;
            }
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

    public sealed class MooreGrid3D : MooreGrid3D<MooreGrid3D>
    {
        static new MooreHelper Helper { get; } = MooreHelper.Instance;

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

        public static IVectorBuilder<Vector3D> Vector => Helper;
        public static IPathBuilder<Vector3D>   Path   => Helper;
    }
}
