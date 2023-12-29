using System.Collections.Generic;

namespace aoc.Grids
{
    public sealed class MooreGrid3D : Grid3D<MooreGrid3D>
    {
        public MooreGrid3D(params Vector3D[] points)
            : base(points)
        {
        }

        public MooreGrid3D(IEnumerable<Vector3D> points)
            : base(points)
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

        public override int CountNeighbors(Vector3D p)
        {
            int count = 0;
            for (var z = p.z - 1; z <= p.z + 1; z++)
                for (var y = p.y - 1; y <= p.y + 1; y++)
                    for (var x = p.x - 1; x <= p.x + 1; x++)
                        count += p != (x, y, z) && Points.Contains((x, y, z)) ? 1 : 0;
            return count;
        }

        public override int CountNeighborsAndSelf(Vector3D p)
        {
            int count = 0;
            for (var z = p.z - 1; z <= p.z + 1; z++)
                for (var y = p.y - 1; y <= p.y + 1; y++)
                    for (var x = p.x - 1; x <= p.x + 1; x++)
                        count += Points.Contains((x, y, z)) ? 1 : 0;
            return count;
        }
    }
}
