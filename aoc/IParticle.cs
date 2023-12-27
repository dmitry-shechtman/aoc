using System;
using System.Collections;
using System.Collections.Generic;

namespace aoc
{
    public interface IParticle<TSelf, TVector> : IEquatable<TSelf>, IReadOnlyList<TVector>
        where TSelf : struct, IParticle<TSelf, TVector>
        where TVector : struct
    {
        private const int Cardinality = 3;

        TVector P { get; }
        TVector V { get; }
        TVector A { get; }

        void Deconstruct(out TVector p, out TVector v);
        void Deconstruct(out TVector p, out TVector v, out TVector a);

        IEnumerator<TVector> IEnumerable<TVector>.GetEnumerator()
        {
            yield return P;
            yield return V;
            yield return A;
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        TVector IReadOnlyList<TVector>.this[int i] => i switch
        {
            0 => P,
            1 => V,
            2 => A,
            _ => throw new IndexOutOfRangeException(),
        };

        int IReadOnlyCollection<TVector>.Count =>
            Cardinality;
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
