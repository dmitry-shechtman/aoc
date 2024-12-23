using System;
using System.Collections.Generic;

namespace aoc.Grids
{
    public abstract class MooreGrid<TSelf> : Grid<TSelf>
        where TSelf : MooreGrid<TSelf>
    {
        internal sealed class Helper : Internal.GridHelper2<Helper, TSelf>
        {
            private Helper()
            {
            }

            public override Vector[] Headings => new[]
            {
                Vector.North, Vector.NorthEast, Vector.East, Vector.SouthEast,
                Vector.South, Vector.SouthWest, Vector.West, Vector.NorthWest,
            };

            protected override string[][] FormatStrings => new[]
            {
                new[] { "n", "ne", "e", "se", "s", "sw", "w", "nw" }
            };
        }

        protected MooreGrid(IEnumerable<Vector> points)
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
    }

    public sealed class MooreGrid : MooreGrid<MooreGrid>
    {
        static new Helper Helper { get; } = Helper.Instance;

        public MooreGrid(params Vector[] points)
            : base(points)
        {
        }

        public MooreGrid(IEnumerable<Vector> points)
            : base(points)
        {
        }

        public override string ToString() =>
            Helper.ToString(this);

        public override string ToString(string format, IFormatProvider provider = null) =>
            Helper.ToString(this, format, provider);

        public override string ToString(IFormatProvider provider) =>
            Helper.ToString(this, provider);

        public string ToString(Size size, ReadOnlySpan<char> format = default, IFormatProvider provider = null) =>
            Helper.ToString(this, size, format, provider);

        public string ToString(VectorRange range, ReadOnlySpan<char> format = default, IFormatProvider provider = null) =>
            Helper.ToString(this, range, format, provider);

        public static Vector[] Headings =>
            Helper.Headings;

        public static int GetHeading(ReadOnlySpan<char> s) =>
            Helper.GetHeading(s);

        public static bool TryGetHeading(ReadOnlySpan<char> s, out int heading) =>
            Helper.TryGetHeading(s, out heading);

        public static string ToString(Vector vector, char format) =>
            Helper.ToString(vector, format);

        public static Vector ParseVector(ReadOnlySpan<char> s) =>
            Helper.ParseVector(s);

        public static bool TryParseVector(ReadOnlySpan<char> s, out Vector vector) =>
            Helper.TryParseVector(s, out vector);

        public static IEnumerable<Vector> ParseVectors(ReadOnlySpan<char> s, params char[] skip) =>
            Helper.ParseVectors(s, skip);

        public static bool TryParseVectors(ReadOnlySpan<char> s, ReadOnlySpan<char> skip, out IEnumerable<Vector> vectors) =>
            Helper.TryParseVectors(s, skip, out vectors);

        public static IEnumerable<PathSegment<Vector>> ParsePath(ReadOnlySpan<char> s) =>
            Helper.ParsePath(s);

        public static IEnumerable<PathSegment<Vector>> ParsePath(string s, char separator) =>
            Helper.ParsePath(s, separator);

        public static IEnumerable<PathSegment<Vector>> ParsePath(string s, string separator) =>
            Helper.ParsePath(s, separator);

        public static IEnumerable<PathSegment<Vector>> ParsePath(string[] ss) =>
            Helper.ParsePath(ss);

        public static IEnumerable<PathSegment<Vector>> ParsePath(string[] ss, char separator) =>
            Helper.ParsePath(ss, separator);

        public static IEnumerable<PathSegment<Vector>> ParsePath(string[] ss, string separator) =>
            Helper.ParsePath(ss, separator);
    }
}
