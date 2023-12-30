using aoc.Grids;
using System;

namespace aoc.Internal
{
    abstract class MultiGridParseHelper<TSelf, TMulti, TGrid> : Singleton<TSelf>
        where TSelf : MultiGridParseHelper<TSelf, TMulti, TGrid>
        where TMulti : MultiGrid<TMulti, TGrid>
        where TGrid : Grid<TGrid>
    {
        private const char DefaultSeparatorChar = '\n';

        public TMulti Parse(string s, ReadOnlySpan<char> cc) =>
            Parse(s, DefaultSeparatorChar, cc);

        public TMulti Parse(string s, char separator, ReadOnlySpan<char> cc) =>
            Parse(s.Split(separator), cc);

        public TMulti Parse(string[] ss, ReadOnlySpan<char> cc)
        {
            var grids = new TGrid[cc.Length + 1];
            int i;
            for (i = 0; i <= cc.Length; i++)
                grids[i] = CreateGrid();
            for (int y = 0; y < ss.Length; y++)
            {
                for (int x = 0; x < ss[y].Length; x++)
                {
                    if ((i = cc.IndexOf(ss[y][x])) >= 0)
                    {
                        grids[i].Add(x, y);
                        grids[^1].Add(x, y);
                    }
                }
            }
            return CreateMulti(grids);
        }

        protected abstract TGrid CreateGrid();
        protected abstract TMulti CreateMulti(TGrid[] grids);
    }

    sealed class MultiGridParseHelper : MultiGridParseHelper<MultiGridParseHelper, MultiGrid, Grid>
    {
        private MultiGridParseHelper()
        {
        }

        protected override Grid CreateGrid() =>
            new();

        protected override MultiGrid CreateMulti(Grid[] grids) =>
            new(grids);
    }
}
