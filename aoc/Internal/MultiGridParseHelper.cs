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
        private const char DefaultSeparatorChar = '\n';

        public TMulti Parse(string s) =>
            Parse(s, out _);

        public TMulti Parse(string s, out Size size) =>
            Parse(s, s.Distinct().ToArray(), out size);

        public TMulti Parse(string s, Func<char, bool> predicate) =>
            Parse(s, predicate, out _);

        public TMulti Parse(string s, Func<char, bool> predicate, out Size size) =>
            Parse(s, s.Where(predicate).Distinct().ToArray(), out size);

        public TMulti Parse(string s, ReadOnlySpan<char> cc) =>
            Parse(s, cc, out _);

        public TMulti Parse(string s, ReadOnlySpan<char> cc, out Size size) =>
            Parse(s, DefaultSeparatorChar, cc, out size);

        public TMulti Parse(string s, char separator, ReadOnlySpan<char> cc) =>
            Parse(s, separator, cc, out _);

        public TMulti Parse(string s, char separator, ReadOnlySpan<char> cc, out Size size) =>
            Parse(s.Split(separator), cc, out size);

        public TMulti Parse(string[] ss, ReadOnlySpan<char> cc) =>
            Parse(ss, cc, out _);

        public TMulti Parse(string[] ss, ReadOnlySpan<char> cc, out Size size)
        {
            int height = 0, i;
            var points = new HashSet<Vector>[cc.Length + 1];
            for (i = 0; i < points.Length; i++)
                points[i] = new();
            for (int y = 0; y < ss.Length; y++)
            {
                if (ss[y].Length > 0)
                    ++height;
                for (int x = 0; x < ss[y].Length; x++)
                {
                    if ((i = cc.IndexOf(ss[y][x])) >= 0)
                    {
                        points[i].Add((x, y));
                        points[^1].Add((x, y));
                    }
                }
            }
            size = new(ss[0].Length, height);
            var grids = new TGrid[points.Length];
            for (i = 0; i < points.Length; i++)
                grids[i] = CreateGrid(points[i]);
            return CreateMulti(grids);
        }

        protected abstract TGrid CreateGrid(HashSet<Vector> points);
        protected abstract TMulti CreateMulti(TGrid[] grids);
    }

    sealed class MultiGridParseHelper : MultiGridParseHelper<MultiGridParseHelper, MultiGrid, Grid>
    {
        private MultiGridParseHelper()
        {
        }

        protected override Grid CreateGrid(HashSet<Vector> points) =>
            new(points);

        protected override MultiGrid CreateMulti(Grid[] grids) =>
            new(grids);
    }
}
