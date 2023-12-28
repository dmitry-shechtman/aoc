using System;

namespace aoc
{
    public interface IMatrix<TSelf, TVector, T> : IEquatable<TSelf>
        where TSelf : struct, IMatrix<TSelf, TVector, T>
        where TVector : struct, IVector<TVector, TSelf, T>
        where T : struct
    {
        TVector Mul(TVector vector);
    }
}
