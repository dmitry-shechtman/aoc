using System;
using System.Collections.Generic;

namespace aoc.Grids
{
    using Helper = Internal.HexWEGridParseHelper;

    public sealed class HexWEGrid : Grid<HexWEGrid>
    {
        static Helper Helper { get; } = Helper.Instance;

        public HexWEGrid(params Vector[] points)
            : base(points)
        {
        }

        public HexWEGrid(IEnumerable<Vector> points)
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

        public static IEnumerable<(Vector v, int d)> ParsePath(string[] ss, string separator) =>
            Helper.ParsePath(ss, separator);

        public static IEnumerable<(Vector v, int d)> ParsePath(string[] ss, char separator) =>
            Helper.ParsePath(ss, separator);
    }
}
