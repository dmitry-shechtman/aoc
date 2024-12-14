using System.Collections.Generic;
using System.Linq;

namespace aoc
{
    public static class Vector4DExtensions
    {
        public static Vector4D Sum(this IEnumerable<Vector4D> pp) =>
            pp.Aggregate((x, y) => x + y);

        public static Vector4D Min(this IEnumerable<Vector4D> pp) =>
            new(pp.Min(p => p.x), pp.Min(p => p.y), pp.Min(p => p.z), pp.Min(p => p.w));

        public static Vector4D Max(this IEnumerable<Vector4D> pp) =>
            new(pp.Max(p => p.x), pp.Max(p => p.y), pp.Max(p => p.z), pp.Max(p => p.w));

        public static Vector4D Center(this IEnumerable<Vector4D> pp) =>
            (pp.Min() + pp.Max()) / 2;

        public static int Width(this IEnumerable<Vector4D> pp) =>
            pp.Max(p => p.x) - pp.Min(p => p.x) + 1;

        public static int Height(this IEnumerable<Vector4D> pp) =>
            pp.Max(p => p.y) - pp.Min(p => p.y) + 1;

        public static int Depth(this IEnumerable<Vector4D> pp) =>
            pp.Max(p => p.z) - pp.Min(p => p.z) + 1;

        public static int Anakata(this IEnumerable<Vector4D> pp) =>
            pp.Max(p => p.w) - pp.Min(p => p.w) + 1;

        public static Vector4DRange Range(this IEnumerable<Vector4D> pp) =>
            new(Min(pp), Max(pp));

        public static bool Contains(this IEnumerable<Vector4D> pp, int x, int y, int z, int w) =>
            pp.Contains(new(x, y, z, w));

        public static void Add(this ICollection<Vector4D> pp, int x, int y, int z, int w) =>
            pp.Add(new(x, y, z, w));

        public static void Remove(this ICollection<Vector4D> pp, int x, int y, int z, int w) =>
            pp.Remove(new(x, y, z, w));
    }
}
