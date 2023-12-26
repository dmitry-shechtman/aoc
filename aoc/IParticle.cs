using System;

namespace aoc
{
    public interface IParticle<T>
        where T : struct
    {
        internal const int MinCardinality = 2;
        internal const int MaxCardinality = 3;

        void Deconstruct(out T p, out T v);
        void Deconstruct(out T p, out T v, out T a);
    }

    public interface IParticle<TSelf, TVector, T> : IParticle<TVector>, IEquatable<TSelf>
        where TSelf : struct, IParticle<TSelf, TVector, T>
        where TVector : struct, IVector<T>
        where T : struct
    {
        TSelf GetNext();
        TSelf GetNextPV();
        TSelf GetNextVP();
    }
}
