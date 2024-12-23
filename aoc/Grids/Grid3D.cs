using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc.Grids
{
    public abstract class Grid3D<TSelf> : Grid<TSelf, Vector3D, Size3D, Vector3DRange, int>
        where TSelf : Grid3D<TSelf>
    {
        internal sealed class Helper : Internal.GridHelper<Helper, Grid3D, Vector3D>
        {
            private Helper()
            {
            }

            public override Vector3D[] Headings => new[]
            {
                Vector3D.North, Vector3D.East, Vector3D.South, Vector3D.West,
                Vector3D.Up,    Vector3D.Down
            };

            protected override string[][] FormatStrings => new[]
            {
                new[] { "n", "e", "s", "w", "u", "d" }
            };
        }

        protected Grid3D(IEnumerable<Vector3D> points)
            : base(points)
        {
        }

        protected Grid3D(IEnumerable<Vector> points)
            : base(points.Select(p => new Vector3D(p)))
        {
        }

        public bool Add(int x, int y, int z) =>
            Points.Add(new(x, y, z));

        public bool Remove(int x, int y, int z) =>
            Points.Remove(new(x, y, z));

        public bool AddRange(Size3D size) =>
            new Vector3DRange(size).All(Points.Add);

        public bool AddRange(IEnumerable<Vector3D> range) =>
            range.All(Points.Add);

        public bool RemoveRange(Size3D size) =>
            new Vector3DRange(size).All(Points.Remove);

        public bool RemoveRange(IEnumerable<Vector3D> range) =>
            range.All(Points.Remove);

        public override IEnumerable<Vector3D> GetNeighbors(Vector3D p) => new Vector3D[]
        {
            new(p.x, p.y, p.z - 1),
            new(p.x, p.y - 1, p.z),
            new(p.x + 1, p.y, p.z),
            new(p.x, p.y + 1, p.z),
            new(p.x - 1, p.y, p.z),
            new(p.x, p.y, p.z + 1)
        };

        public override IEnumerable<Vector3D> GetNeighborsAndSelf(Vector3D p) => new Vector3D[]
        {
            new(p.x, p.y, p.z),
            new(p.x, p.y, p.z - 1),
            new(p.x, p.y - 1, p.z),
            new(p.x + 1, p.y, p.z),
            new(p.x, p.y + 1, p.z),
            new(p.x - 1, p.y, p.z),
            new(p.x, p.y, p.z + 1)
        };
    }

    public sealed class Grid3D : Grid3D<Grid3D>
    {
        static new Helper Helper { get; } = Helper.Instance;

        public Grid3D(params Vector3D[] points)
            : base(points)
        {
        }

        public Grid3D(IEnumerable<Vector3D> points)
            : base(points)
        {
        }

        public Grid3D(IEnumerable<Vector> points)
            : base(points)
        {
        }

        public static Vector3D[] Headings =>
            Helper.Headings;

        public static int GetHeading(ReadOnlySpan<char> s) =>
            Helper.GetHeading(s);

        public static bool TryGetHeading(ReadOnlySpan<char> s, out int heading) =>
            Helper.TryGetHeading(s, out heading);

        public static string ToString(Vector3D vector, char format) =>
            Helper.ToString(vector, format);

        public static Vector3D ParseVector(ReadOnlySpan<char> s) =>
            Helper.ParseVector(s);

        public static bool TryParseVector(ReadOnlySpan<char> s, out Vector3D vector) =>
            Helper.TryParseVector(s, out vector);

        public static IEnumerable<Vector3D> ParseVectors(ReadOnlySpan<char> s, params char[] skip) =>
            Helper.ParseVectors(s, skip);

        public static bool TryParseVectors(ReadOnlySpan<char> s, ReadOnlySpan<char> skip, out IEnumerable<Vector3D> vectors) =>
            Helper.TryParseVectors(s, skip, out vectors);

        public static IEnumerable<PathSegment<Vector3D>> ParsePath(ReadOnlySpan<char> s) =>
            Helper.ParsePath(s);

        public static IEnumerable<PathSegment<Vector3D>> ParsePath(string s, char separator) =>
            Helper.ParsePath(s, separator);

        public static IEnumerable<PathSegment<Vector3D>> ParsePath(string s, string separator) =>
            Helper.ParsePath(s, separator);

        public static IEnumerable<PathSegment<Vector3D>> ParsePath(string[] ss) =>
            Helper.ParsePath(ss);

        public static IEnumerable<PathSegment<Vector3D>> ParsePath(string[] ss, char separator) =>
            Helper.ParsePath(ss, separator);

        public static IEnumerable<PathSegment<Vector3D>> ParsePath(string[] ss, string separator) =>
            Helper.ParsePath(ss, separator);
    }
}
