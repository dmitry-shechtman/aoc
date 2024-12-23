using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc.Grids
{
    public abstract class Grid4D<TSelf> : Grid<TSelf, Vector4D, Size4D, Vector4DRange, int>
        where TSelf : Grid4D<TSelf>
    {
        internal sealed class Helper : Internal.GridHelper<Helper, TSelf, Vector4D>
        {
            private Helper()
            {
            }

            public override Vector4D[] Headings => new[]
            {
                Vector4D.North, Vector4D.East, Vector4D.South, Vector4D.West,
                Vector4D.Up,    Vector4D.Down, Vector4D.Ana,   Vector4D.Kata
            };

            protected override string[][] FormatStrings => new[]
            {
                new[] { "n", "e", "s", "w", "u", "d", "a", "k" }
            };
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
    }

    public sealed class Grid4D : Grid4D<Grid4D>
    {
        static new Helper Helper { get; } = Helper.Instance;

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

        public static Vector4D[] Headings =>
            Helper.Headings;

        public static int GetHeading(ReadOnlySpan<char> s) =>
            Helper.GetHeading(s);

        public static bool TryGetHeading(ReadOnlySpan<char> s, out int heading) =>
            Helper.TryGetHeading(s, out heading);

        public static string ToString(Vector4D vector, char format) =>
            Helper.ToString(vector, format);

        public static Vector4D ParseVector(ReadOnlySpan<char> s) =>
            Helper.ParseVector(s);

        public static bool TryParseVector(ReadOnlySpan<char> s, out Vector4D vector) =>
            Helper.TryParseVector(s, out vector);

        public static IEnumerable<Vector4D> ParseVectors(ReadOnlySpan<char> s, params char[] skip) =>
            Helper.ParseVectors(s, skip);

        public static bool TryParseVectors(ReadOnlySpan<char> s, ReadOnlySpan<char> skip, out IEnumerable<Vector4D> vectors) =>
            Helper.TryParseVectors(s, skip, out vectors);

        public static IEnumerable<PathSegment<Vector4D>> ParsePath(ReadOnlySpan<char> s) =>
            Helper.ParsePath(s);

        public static IEnumerable<PathSegment<Vector4D>> ParsePath(string s, char separator) =>
            Helper.ParsePath(s, separator);

        public static IEnumerable<PathSegment<Vector4D>> ParsePath(string s, string separator) =>
            Helper.ParsePath(s, separator);

        public static IEnumerable<PathSegment<Vector4D>> ParsePath(string[] ss) =>
            Helper.ParsePath(ss);

        public static IEnumerable<PathSegment<Vector4D>> ParsePath(string[] ss, char separator) =>
            Helper.ParsePath(ss, separator);

        public static IEnumerable<PathSegment<Vector4D>> ParsePath(string[] ss, string separator) =>
            Helper.ParsePath(ss, separator);
    }
}
