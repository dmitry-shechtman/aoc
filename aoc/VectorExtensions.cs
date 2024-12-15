using System.Collections.Generic;
using System.Linq;

namespace aoc
{
    public static class VectorExtensions
    {
        public static Vector Sum(this IEnumerable<Vector> pp) =>
            pp.Aggregate((x, y) => x + y);

        public static Vector Min(this IEnumerable<Vector> pp) =>
            new(pp.Min(p => p.x), pp.Min(p => p.y));

        public static Vector Max(this IEnumerable<Vector> pp) =>
            new(pp.Max(p => p.x), pp.Max(p => p.y));

        public static Vector Center(this IEnumerable<Vector> pp) =>
            (pp.Min() + pp.Max()) / 2;

        public static int Width(this IEnumerable<Vector> pp) =>
            pp.Max(p => p.x) - pp.Min(p => p.x) + 1;

        public static int Height(this IEnumerable<Vector> pp) =>
            pp.Max(p => p.y) - pp.Min(p => p.y) + 1;

        public static VectorRange Range(this IEnumerable<Vector> pp) =>
            new(Min(pp), Max(pp));

        public static bool Contains(this IEnumerable<Vector> pp, int x, int y) =>
            pp.Contains(new(x, y));

        public static void Add(this ICollection<Vector> pp, int x, int y) =>
            pp.Add(new(x, y));

        public static void Remove(this ICollection<Vector> pp, int x, int y) =>
            pp.Remove(new(x, y));
    }
}
