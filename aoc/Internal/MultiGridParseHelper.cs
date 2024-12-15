using aoc.Grids;
using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc.Internal
{
    abstract class MultiGridParseHelper<TSelf, TMulti, TGrid> : Singleton<TSelf>
        where TSelf : MultiGridParseHelper<TSelf, TMulti, TGrid>
        where TMulti : MultiGrid<TMulti, TGrid>
        where TGrid : Grid<TGrid>
    {
        private const char DefaultEmptyChar     = '.';
        private const char DefaultSeparatorChar = '\n';

        public static string ToString(TMulti multi) =>
            ToString(multi, multi[^1].Range());

        public static string ToString(TMulti multi, Size size) =>
            ToString(multi, range: new(size));

        public static string ToString(TMulti multi, VectorRange range)
        {
            var chars = new char[(range.Width + 1) * range.Height];
            for (int y = range.Min.Y, i = 0, j = 0; y <= range.Max.Y; y++, chars[j++] = DefaultSeparatorChar)
            {
                for (int x = range.Min.X; x <= range.Max.X; x++, i++)
                {
                    var index = multi[..^1].FindIndex(g => g.Contains((x, y)));
                    chars[j++] = index >= 0
                        ? multi.Chars[index]
                        : DefaultEmptyChar;
                }
            }
            return new(chars);
        }

        public TMulti Parse(ReadOnlySpan<char> s) =>
            Parse(s, out _);

        public TMulti Parse(string s) =>
            Parse(s, out _);

        public TMulti Parse(ReadOnlySpan<char> s, out Size size) =>
            Parse(s.ToString(), out size);

        public TMulti Parse(string s, out Size size) =>
            Parse(s, s.Distinct().ToArray(), out size);

        public TMulti Parse(ReadOnlySpan<char> s, Func<char, bool> predicate) =>
            Parse(s, predicate, out _);

        public TMulti Parse(string s, Func<char, bool> predicate) =>
            Parse(s, predicate, out _);

        public TMulti Parse(ReadOnlySpan<char> s, Func<char, bool> predicate, out Size size) =>
            Parse(s.ToString(), predicate, out size);

        public TMulti Parse(string s, Func<char, bool> predicate, out Size size) =>
            Parse(s, s.Where(predicate).Distinct().ToArray(), out size);

        public TMulti Parse(ReadOnlySpan<char> s, ReadOnlySpan<char> cc) =>
            Parse(s, cc, out _);

        public TMulti Parse(ReadOnlySpan<char> s, ReadOnlySpan<char> cc, out Size size) =>
            Parse(s, DefaultSeparatorChar, cc, out size);

        public TMulti Parse(ReadOnlySpan<char> s, char separator, ReadOnlySpan<char> cc) =>
            Parse(s, separator, cc, out _);

        public TMulti Parse(ReadOnlySpan<char> s, char separator, ReadOnlySpan<char> cc, out Size size)
        {
            int width = 0, height = 1, x = 0, y = 0, i;
            var points = new HashSet<Vector>[cc.Length + 1];
            for (i = 0; i < points.Length; i++)
                points[i] = new();
            for (int j = 0; j < s.Length; ++j, ++x)
            {
                if (s[j] == separator)
                {
                    width = x > width ? x : width;
                    if (x > 0)
                        ++height;
                    (x, y) = (-1, ++y);
                }
                else if ((i = cc.IndexOf(s[j])) >= 0)
                {
                    points[i].Add((x, y));
                    points[^1].Add((x, y));
                }
            }
            width = x > width ? x : width;
            size = new(width, height);
            var grids = new TGrid[points.Length];
            for (i = 0; i < points.Length; i++)
                grids[i] = CreateGrid(points[i]);
            return CreateMulti(grids, cc);
        }

        protected abstract TGrid CreateGrid(HashSet<Vector> points);
        protected abstract TMulti CreateMulti(TGrid[] grids, ReadOnlySpan<char> cc);
    }

    sealed class MultiGridParseHelper : MultiGridParseHelper<MultiGridParseHelper, MultiGrid, Grid>
    {
        private MultiGridParseHelper()
        {
        }

        protected override Grid CreateGrid(HashSet<Vector> points) =>
            new(points);

        protected override MultiGrid CreateMulti(Grid[] grids, ReadOnlySpan<char> cc) =>
            new(grids, cc);
    }
}
