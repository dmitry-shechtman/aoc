using System.Collections.Generic;

namespace aoc.Grids
{
    public sealed class MooreGrid4D : Grid4D<MooreGrid4D>
    {
        public MooreGrid4D(params Vector4D[] points)
            : base(points)
        {
        }

        public MooreGrid4D(IEnumerable<Vector4D> points)
            : base(points)
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

        public override int CountNeighbors(Vector4D p)
        {
            int count = 0;
            for (var w = p.w - 1; w <= p.w + 1; ++w)
                for (var z = p.z - 1; z <= p.z + 1; z++)
                    for (var y = p.y - 1; y <= p.y + 1; y++)
                        for (var x = p.x - 1; x <= p.x + 1; x++)
                            count += p != (x, y, z, w) && Points.Contains((x, y, z, w)) ? 1 : 0;
            return count;
        }

        public override int CountNeighborsAndSelf(Vector4D p)
        {
            int count = 0;
            for (var w = p.w - 1; w <= p.w + 1; ++w)
                for (var z = p.z - 1; z <= p.z + 1; z++)
                    for (var y = p.y - 1; y <= p.y + 1; y++)
                        for (var x = p.x - 1; x <= p.x + 1; x++)
                            count += Points.Contains((x, y, z, w)) ? 1 : 0;
            return count;
        }
    }
}
