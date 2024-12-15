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

        public override string ToString() =>
            Helper.ToString(this);

        public string ToString(Size size) =>
            Helper.ToString(this, size);

        public string ToString(VectorRange range) =>
            Helper.ToString(this, range);

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
