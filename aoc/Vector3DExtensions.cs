using System.Collections.Generic;
using System.Linq;

namespace aoc
{
    public static class Vector3DExtensions
    {
        public static Vector3D Min(this IEnumerable<Vector3D> pp) =>
            new(pp.Min(p => p.x), pp.Min(p => p.y), pp.Min(p => p.z));

        public static Vector3D Max(this IEnumerable<Vector3D> pp) =>
            new(pp.Max(p => p.x), pp.Max(p => p.y), pp.Max(p => p.z));

        public static Vector3D Center(this IEnumerable<Vector3D> pp) =>
            (pp.Min() + pp.Max()) / 2;

        public static int Width(this IEnumerable<Vector3D> pp) =>
            pp.Max(p => p.x) - pp.Min(p => p.x) + 1;

        public static int Height(this IEnumerable<Vector3D> pp) =>
            pp.Max(p => p.y) - pp.Min(p => p.y) + 1;

        public static int Depth(this IEnumerable<Vector3D> pp) =>
            pp.Max(p => p.z) - pp.Min(p => p.z) + 1;

        public static Vector3DRange Range(this IEnumerable<Vector3D> pp) =>
            new(Min(pp), Max(pp));

        public static bool Contains(this IEnumerable<Vector3D> pp, int x, int y, int z) =>
            pp.Contains(new(x, y, z));

        public static void Add(this ICollection<Vector3D> pp, int x, int y, int z) =>
            pp.Add(new(x, y, z));

        public static void Remove(this ICollection<Vector3D> pp, int x, int y, int z) =>
            pp.Remove(new(x, y, z));

        public static bool Add(this HashSet<Vector3D> pp, int x, int y, int z) =>
            pp.Add(new(x, y, z));

        public static bool Remove(this HashSet<Vector3D> pp, int x, int y, int z) =>
            pp.Remove(new(x, y, z));

        public static bool AddRange(this HashSet<Vector3D> pp, IEnumerable<Vector3D> range) =>
            range.All(pp.Add);

        public static bool AddRange(this HashSet<Vector3D> pp, Size3D size) =>
            new Vector3DRange(size).All(pp.Add);

        public static bool RemoveRange(this HashSet<Vector3D> pp, IEnumerable<Vector3D> range) =>
            range.All(pp.Remove);

        public static bool RemoveRange(this HashSet<Vector3D> pp, Size3D size) =>
            new Vector3DRange(size).All(pp.Remove);
    }
}
