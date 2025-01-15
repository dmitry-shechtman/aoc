using System.Collections.Generic;

namespace aoc.Grids
{
    public interface IGrid<TVector>
        where TVector : struct, IVector<TVector>
    {
        HashSet<TVector> Points { get; }
    }

    internal interface IGrid2D<TSelf, TVector> : IGrid<TVector>
        where TSelf : IGrid2D<TSelf, TVector>
        where TVector : struct, IVector<TVector>
    {
    }

    internal interface IGrid3D<TSelf, TVector> : IGrid<TVector>
        where TSelf : IGrid3D<TSelf, TVector>
        where TVector : struct, IVector<TVector>
    {
    }

    internal interface IGrid4D<TSelf, TVector> : IGrid<TVector>
        where TSelf : IGrid4D<TSelf, TVector>
        where TVector : struct, IVector<TVector>
    {
    }
}
