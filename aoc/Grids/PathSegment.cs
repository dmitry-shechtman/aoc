using System;

namespace aoc.Grids
{
    public readonly struct PathSegment<TVector> : IEquatable<PathSegment<TVector>>
        where TVector : struct, IEquatable<TVector>
    {
        public readonly TVector v;
        public readonly int d;

        public PathSegment(TVector v, int d)
        {
            this.v = v;
            this.d = d;
        }

        public override bool Equals(object? obj) =>
            obj is PathSegment<TVector> other &&
                Equals(other);

        public bool Equals(PathSegment<TVector> other) =>
            v.Equals(other.v) &&
            d == other.d;

        public override int GetHashCode() =>
            HashCode.Combine(v, d);

        public override string ToString() =>
            $"({v},{d})";

        public void Deconstruct(out TVector v, out int d)
        {
            v = this.v;
            d = this.d;
        }

        public static implicit operator (TVector v, int d)(PathSegment<TVector> value) =>
            (value.v, value.d);

        public static implicit operator PathSegment<TVector>((TVector v, int d) value) =>
            new(value.v, value.d);

        public static bool operator ==(PathSegment<TVector> left, PathSegment<TVector> right) =>
            left.Equals(right);

        public static bool operator !=(PathSegment<TVector> left, PathSegment<TVector> right) =>
            !left.Equals(right);
    }
}
