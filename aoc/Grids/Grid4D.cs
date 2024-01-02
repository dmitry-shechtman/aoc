using System.Collections.Generic;
using System.Linq;

namespace aoc.Grids
{
    using Helper = Internal.Grid4DHelper;

    public abstract class Grid4D<TSelf> : Grid<TSelf, Vector4D, Size4D, Vector4DRange, int>
        where TSelf : Grid4D<TSelf>
    {
        protected Grid4D(Vector4D[] points)
            : base(points)
        {
        }

        protected Grid4D(IEnumerable<Vector4D> points)
            : base(points)
        {
        }

        protected Grid4D(IEnumerable<Vector3D> points)
            : base(points.Select(p => new Vector4D(p)))
        {
        }

        protected Grid4D(IEnumerable<Vector> points)
            : base(points.Select(p => new Vector4D(p)))
        {
        }

        public bool Add(int x, int y, int z, int w) =>
            Points.Add(new(x, y, z, w));

        public bool Remove(int x, int y, int z, int w) =>
            Points.Remove(new(x, y, z, w));

        public bool AddRange(Size4D size) =>
            new Vector4DRange(size).All(Points.Add);

        public bool AddRange(IEnumerable<Vector4D> range) =>
            range.All(Points.Add);

        public bool RemoveRange(Size4D size) =>
            new Vector4DRange(size).All(Points.Remove);

        public bool RemoveRange(IEnumerable<Vector4D> range) =>
            range.All(Points.Remove);
    }

    public sealed class Grid4D : Grid4D<Grid4D>
    {
        static Helper Helper { get; } = Helper.Instance;

        public Grid4D(params Vector4D[] points)
            : base(points)
        {
        }

        public Grid4D(IEnumerable<Vector4D> points)
            : base(points)
        {
        }

        public Grid4D(IEnumerable<Vector3D> points)
            : base(points)
        {
        }

        public Grid4D(IEnumerable<Vector> points)
            : base(points)
        {
        }

        public override IEnumerable<Vector4D> GetNeighbors(Vector4D p) => new Vector4D[]
        {
            new(p.x, p.y, p.z, p.w - 1),
            new(p.x, p.y, p.z - 1, p.w),
            new(p.x, p.y - 1, p.z, p.w),
            new(p.x - 1, p.y, p.z, p.w),
            new(p.x + 1, p.y, p.z, p.w),
            new(p.x, p.y + 1, p.z, p.w),
            new(p.x, p.y, p.z + 1, p.w),
            new(p.x, p.y, p.z, p.w + 1)
        };

        public override IEnumerable<Vector4D> GetNeighborsAndSelf(Vector4D p) => new Vector4D[]
        {
            new(p.x, p.y, p.z, p.w - 1),
            new(p.x, p.y, p.z - 1, p.w),
            new(p.x, p.y - 1, p.z, p.w),
            new(p.x - 1, p.y, p.z, p.w),
            new(p.x, p.y, p.z, p.w),
            new(p.x + 1, p.y, p.z, p.w),
            new(p.x, p.y + 1, p.z, p.w),
            new(p.x, p.y, p.z + 1, p.w),
            new(p.x, p.y, p.z, p.w + 1)
        };

        public static Vector4D[] Headings =>
            Helper.Headings;

        public static int GetHeading(string s) =>
            Helper.GetHeading(s);

        public static bool TryGetHeading(string s, out int heading) =>
            Helper.TryGetHeading(s, out heading);

        public static string ToString(Vector4D vector, char format) =>
            Helper.ToString(vector, format);

        public static Vector4D ParseVector(string s) =>
            Helper.ParseVector(s);

        public static bool TryParseVector(string s, out Vector4D vector) =>
            Helper.TryParseVector(s, out vector);

        public static IEnumerable<Vector4D> ParseVectors(string s) =>
            Helper.ParseVectors(s);

        public static bool TryParseVectors(string s, out IEnumerable<Vector4D> vectors) =>
            Helper.TryParseVectors(s, out vectors);

        public static IEnumerable<(Vector4D v, int d)> ParsePath(string s) =>
            Helper.ParsePath(s);

        public static IEnumerable<(Vector4D v, int d)> ParsePath(string s, char separator) =>
            Helper.ParsePath(s, separator);

        public static IEnumerable<(Vector4D v, int d)> ParsePath(string s, string separator) =>
            Helper.ParsePath(s, separator);

        public static IEnumerable<(Vector4D v, int d)> ParsePath(string[] ss) =>
            Helper.ParsePath(ss);

        public static IEnumerable<(Vector4D v, int d)> ParsePath(string[] ss, char separator) =>
            Helper.ParsePath(ss, separator);

        public static IEnumerable<(Vector4D v, int d)> ParsePath(string[] ss, string separator) =>
            Helper.ParsePath(ss, separator);
    }
}
