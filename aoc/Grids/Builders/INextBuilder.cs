using System;

namespace aoc.Grids.Builders
{
    public interface INextBuilder<TGrid, TVector>
        where TGrid : IGrid<TVector>
        where TVector : struct, IVector<TVector>
    {
        TGrid Move(TGrid grid);
        TGrid Move<TSize>(TGrid grid, TSize size)
            where TSize : struct, ISize<TSize, TVector>;

        TGrid Move(TGrid grid, Func<TVector, bool> predicate, bool inclusive = false);
        TGrid Move<TSize>(TGrid grid, Func<TVector, bool> predicate, TSize size, bool inclusive = false)
            where TSize : struct, ISize<TSize, TVector>;

        TGrid Move(TGrid grid, Func<TVector, int, bool> filterInclusive);
        TGrid Move<TSize>(TGrid grid, Func<TVector, int, bool> filterInclusive, TSize size)
            where TSize : struct, ISize<TSize, TVector>;
    }
}
