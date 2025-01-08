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
}
