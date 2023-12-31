﻿using System;
using System.Collections.Generic;

namespace aoc.Grids
{
    using Helper = Internal.HexNSGridHelper;

    public sealed class HexNSGrid : Grid<HexNSGrid>
    {
        static Helper Helper { get; } = Helper.Instance;

        public HexNSGrid(params Vector[] points)
            : base(points)
        {
        }

        public HexNSGrid(IEnumerable<Vector> points)
            : base(points)
        {
        }

        public static int Abs(Vector p) =>
            Math.Abs(p.x) + Math.Abs(Math.Abs(p.x) - Math.Abs(p.y)) / 2;

        public override Vector[] GetNeighbors(Vector p) => new Vector[]
        {
            new(p.x, p.y + 2),
            new(p.x + 1, p.y + 1),
            new(p.x - 1, p.y + 1),
            new(p.x, p.y - 2),
            new(p.x - 1, p.y - 1),
            new(p.x + 1, p.y - 1)
        };

        public override Vector[] GetNeighborsAndSelf(Vector p) => new Vector[]
        {
            new(p.x, p.y),
            new(p.x, p.y + 2),
            new(p.x + 1, p.y + 1),
            new(p.x - 1, p.y + 1),
            new(p.x, p.y - 2),
            new(p.x - 1, p.y - 1),
            new(p.x + 1, p.y - 1)
        };

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
