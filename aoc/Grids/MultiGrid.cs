namespace aoc.Grids
{
    using Helper = Internal.MultiGridParseHelper;

    public abstract class MultiGrid<TSelf, TGrid>
        where TSelf : MultiGrid<TSelf, TGrid>
        where TGrid : Grid<TGrid>
    {
        protected MultiGrid(TGrid[] grids)
        {
            Grids = grids;
        }

        public TGrid[] Grids { get; }
    }

    public sealed class MultiGrid : MultiGrid<MultiGrid, Grid>
    {
        private static Helper Helper { get; } = Helper.Instance;

        public MultiGrid(Grid[] grids)
            : base(grids)
        {
        }

        public static MultiGrid Parse(string s, string cc) =>
            Helper.Parse(s, cc);

        public static MultiGrid Parse(string s, char separator, string cc) =>
            Helper.Parse(s, separator, cc);

        public static MultiGrid Parse(string[] ss, string cc) =>
            Helper.Parse(ss, cc);
    }
}
