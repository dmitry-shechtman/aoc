using System;

namespace aoc
{
    public interface IParticle<TSelf, TVector> : IEquatable<TSelf>
        where TSelf : struct, IParticle<TSelf, TVector>
        where TVector : struct
    {
        void Deconstruct(out TVector p, out TVector v);
        void Deconstruct(out TVector p, out TVector v, out TVector a);
    }

    public interface IParticle<TSelf, TVector, T> : IParticle<TSelf, TVector>
        where TSelf : struct, IParticle<TSelf, TVector, T>
        where TVector : struct, IVector<T>
        where T : struct
    {
        TSelf GetNext();
        TSelf GetNextPV();
        TSelf GetNextVP();
    }
}
