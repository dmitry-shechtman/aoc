using System;

namespace aoc
{
    public interface ISize<T>
        where T : struct
    {
        bool Contains(T value);
    }

    public interface ISize<TSelf, T> : ISize<T>, IEquatable<TSelf>
        where TSelf : struct, ISize<TSelf, T>
        where T : struct
    {
    }

    public interface ISize2D<TSelf, TVector, T> : ISize<TSelf, TVector>
        where TSelf : struct, ISize2D<TSelf, TVector, T>
        where TVector : struct, IVector2D<TVector, T>
        where T : struct
    {
        T Width   { get; }
        T Height  { get; }
        T Length  { get; }
    }

    public interface ISize3D<TSelf, TVector, T> : ISize<TSelf, TVector>
        where TSelf : struct, ISize3D<TSelf, TVector, T>
        where TVector : struct, IVector3D<TVector, T>
        where T : struct
    {
        T Width   { get; }
        T Height  { get; }
        T Depth   { get; }
        T Length  { get; }
    }
}
