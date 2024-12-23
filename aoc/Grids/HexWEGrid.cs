using System;
using System.Collections.Generic;

namespace aoc.Grids
{
    public abstract class HexWEGrid<TSelf> : Grid<TSelf>
        where TSelf : HexWEGrid<TSelf>
    {
        internal sealed class Helper : Internal.GridHelper2<Helper, TSelf>
        {
            private Helper()
            {
            }

            public override Vector[] Headings => new[]
            {
                Vector.East, Vector.SouthEast, Vector.SouthWest,
                Vector.West, Vector.NorthWest, Vector.NorthEast
            };

            protected override string[][] FormatStrings => new[]
            {
                new[] { "e", "se", "sw", "w", "nw", "ne" }
            };
        }

        protected HexWEGrid(IEnumerable<Vector> points)
            : base(points)
        {
        }

        public static int Abs(Vector p) =>
            Math.Abs(p.y) + Math.Abs(Math.Abs(p.y) - Math.Abs(p.x)) / 2;

        public override Vector[] GetNeighbors(Vector p) => new Vector[]
        {
                new(p.x - 2, p.y),
                new(p.x - 1, p.y - 1),
                new(p.x + 1, p.y - 1),
                new(p.x + 2, p.y),
                new(p.x + 1, p.y + 1),
                new(p.x - 1, p.y + 1),
        };

        public override Vector[] GetNeighborsAndSelf(Vector p) => new Vector[]
        {
                new(p.x, p.y),
                new(p.x - 2, p.y),
                new(p.x - 1, p.y - 1),
                new(p.x + 1, p.y - 1),
                new(p.x + 2, p.y),
                new(p.x + 1, p.y + 1),
                new(p.x - 1, p.y + 1),
        };
    }

    public sealed class HexWEGrid : HexWEGrid<HexWEGrid>
    {
        static new Helper Helper { get; } = Helper.Instance;

        public HexWEGrid(params Vector[] points)
            : base(points)
        {
        }

        public HexWEGrid(IEnumerable<Vector> points)
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
