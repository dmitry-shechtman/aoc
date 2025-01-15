using System.Collections.Generic;
using System.Linq;

namespace aoc
{
    public static class VectorExtensions
    {
        public static T Sum<T>(this IEnumerable<T> pp)
            where T : IVector<T> =>
                pp.Aggregate((a, v) => a.Add(v));

        public static T Min<T>(this IEnumerable<T> pp)
            where T : IVector<T> =>
                pp.Aggregate((a, v) => a.Min(v));

        public static T Max<T>(this IEnumerable<T> pp)
            where T : IVector<T> =>
                pp.Aggregate((a, v) => a.Max(v));

        public static (T, T) Range<T>(this IEnumerable<T> pp)
            where T : IVector<T> =>
                new(pp.Min(), pp.Max());
    }

    public static class Vector2DExtensions
    {
        public static VectorRange Range(this IEnumerable<Vector> pp) =>
            new(pp.Min(), pp.Max());
    }

    public static class Vector3DExtensions
    {
        public static Vector3DRange Range(this IEnumerable<Vector3D> pp) =>
            new(pp.Min(), pp.Max());
    }

    public static class Vector4DExtensions
    {
        public static Vector4DRange Range(this IEnumerable<Vector4D> pp) =>
            new(pp.Min(), pp.Max());
    }
}
