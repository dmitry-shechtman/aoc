using System.Collections.Generic;

namespace aoc.Grids
{
    using Helper = Internal.MooreGridHelper;

    public sealed class MooreGrid : Grid<MooreGrid>
    {
        static Helper Helper { get; } = Helper.Instance;

        public MooreGrid(params Vector[] points)
            : base(points)
        {
        }

        public MooreGrid(IEnumerable<Vector> points)
            : base(points)
        {
        }

        public override IEnumerable<Vector> GetNeighbors(Vector p)
        {
            for (var y = p.y - 1; y <= p.y + 1; y++)
                for (var x = p.x - 1; x <= p.x + 1; x++)
                    if (p != (x, y))
                        yield return new(x, y);
        }

        public override IEnumerable<Vector> GetNeighborsAndSelf(Vector p)
        {
            for (var y = p.y - 1; y <= p.y + 1; y++)
                for (var x = p.x - 1; x <= p.x + 1; x++)
                    yield return new(x, y);
        }

        public override int CountNeighbors(Vector p)
        {
            int count = 0;
            for (var y = p.y - 1; y <= p.y + 1; y++)
                for (var x = p.x - 1; x <= p.x + 1; x++)
                    count += p != (x, y) && Points.Contains((x, y)) ? 1 : 0;
            return count;
        }

        public override int CountNeighborsAndSelf(Vector p)
        {
            int count = 0;
            for (var y = p.y - 1; y <= p.y + 1; y++)
                for (var x = p.x - 1; x <= p.x + 1; x++)
                    count += Points.Contains((x, y)) ? 1 : 0;
            return count;
        }

        public static Vector[] Headings =>
            Helper.Headings;

        public static int GetHeading(string s) =>
            Helper.GetHeading(s);

        public static bool TryGetHeading(string s, out int heading) =>
            Helper.TryGetHeading(s, out heading);

        public static string ToString(Vector vector, char format) =>
            Helper.ToString(vector, format);

        public static Vector ParseVector(string s) =>
            Helper.ParseVector(s);

        public static bool TryParseVector(string s, out Vector vector) =>
            Helper.TryParseVector(s, out vector);

        public static IEnumerable<Vector> ParseVectors(string s) =>
            Helper.ParseVectors(s);

        public static bool TryParseVectors(string s, out IEnumerable<Vector> vectors) =>
            Helper.TryParseVectors(s, out vectors);

        public static IEnumerable<(Vector v, int d)> ParsePath(string s) =>
            Helper.ParsePath(s);

        public static IEnumerable<(Vector v, int d)> ParsePath(string s, char separator) =>
            Helper.ParsePath(s, separator);

        public static IEnumerable<(Vector v, int d)> ParsePath(string s, string separator) =>
            Helper.ParsePath(s, separator);

        public static IEnumerable<(Vector v, int d)> ParsePath(string[] ss) =>
            Helper.ParsePath(ss);

        public static IEnumerable<(Vector v, int d)> ParsePath(string[] ss, char separator) =>
            Helper.ParsePath(ss, separator);

        public static IEnumerable<(Vector v, int d)> ParsePath(string[] ss, string separator) =>
            Helper.ParsePath(ss, separator);
    }
}
