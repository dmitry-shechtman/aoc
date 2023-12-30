using System.Collections.Generic;
using System.Linq;

namespace aoc.Grids
{
    using Helper = Internal.Grid3DParseHelper;

    public abstract class Grid3D<TSelf> : Grid<TSelf, Vector3D, Size3D, Vector3DRange, int>
        where TSelf : Grid3D<TSelf>
    {
        protected Grid3D(Vector3D[] points)
            : base(points)
        {
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
    }

    public sealed class Grid3D : Grid3D<Grid3D>
    {
        static Helper Helper { get; } = Helper.Instance;

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

        public override Vector3D[] GetNeighbors(Vector3D p) => new Vector3D[]
        {
            new(p.x, p.y, p.z - 1),
            new(p.x, p.y - 1, p.z),
            new(p.x + 1, p.y, p.z),
            new(p.x, p.y + 1, p.z),
            new(p.x - 1, p.y, p.z),
            new(p.x, p.y, p.z + 1)
        };

        public override Vector3D[] GetNeighborsAndSelf(Vector3D p) => new Vector3D[]
        {
            new(p.x, p.y, p.z),
            new(p.x, p.y, p.z - 1),
            new(p.x, p.y - 1, p.z),
            new(p.x + 1, p.y, p.z),
            new(p.x, p.y + 1, p.z),
            new(p.x - 1, p.y, p.z),
            new(p.x, p.y, p.z + 1)
        };

        public static Vector3D ParseVector(string s) =>
            Helper.ParseVector(s);

        public static bool TryParseVector(string s, out Vector3D vector) =>
            Helper.TryParseVector(s, out vector);

        public static IEnumerable<Vector3D> ParseVectors(string s) =>
            Helper.ParseVectors(s);

        public static bool TryParseVectors(string s, out IEnumerable<Vector3D> vectors) =>
            Helper.TryParseVectors(s, out vectors);

        public static IEnumerable<(Vector3D v, int d)> ParsePath(string s) =>
            Helper.ParsePath(s);

        public static IEnumerable<(Vector3D v, int d)> ParsePath(string s, char separator) =>
            Helper.ParsePath(s, separator);

        public static IEnumerable<(Vector3D v, int d)> ParsePath(string s, string separator) =>
            Helper.ParsePath(s, separator);

        public static IEnumerable<(Vector3D v, int d)> ParsePath(string[] ss) =>
            Helper.ParsePath(ss);

        public static IEnumerable<(Vector3D v, int d)> ParsePath(string[] ss, char separator) =>
            Helper.ParsePath(ss, separator);

        public static IEnumerable<(Vector3D v, int d)> ParsePath(string[] ss, string separator) =>
            Helper.ParsePath(ss, separator);
    }
}
