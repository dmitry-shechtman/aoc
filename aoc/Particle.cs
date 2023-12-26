using System;

namespace aoc
{
    public struct Particle : IParticle<Particle, Vector, int>
    {
        public readonly Vector p;
        public readonly Vector v;
        public readonly Vector a;

        public Particle(Vector p, Vector v, Vector a = default)
        {
            this.p = p;
            this.v = v;
            this.a = a;
        }

        public readonly override bool Equals(object obj) =>
            obj is Particle other && Equals(other);

        public readonly bool Equals(Particle other) =>
            p.Equals(other.p) &&
            v.Equals(other.v) &&
            a.Equals(other.a);

        public readonly override int GetHashCode() =>
            HashCode.Combine(p, v, a);

        public readonly override string ToString() =>
            $"{p};{v};{a}";

        public readonly void Deconstruct(out Vector p, out Vector v, out Vector a)
        {
            p = this.p;
            v = this.v;
            a = this.a;
        }

        public readonly void Deconstruct(out Vector p, out Vector v)
        {
            p = this.p;
            v = this.v;
        }

        public readonly Particle GetNext() =>
            new(p + v, v);

        public readonly Particle GetNextPV() =>
            new(p + v, v + a, a);

        public readonly Particle GetNextVP() =>
            new(p + v + a, v + a, a);

        public static Particle Parse(string s) =>
            Parse(s, ';');

        public static Particle Parse(string s, char separator, char separator2 = ',') =>
            TryParse(s, out Particle particle, separator, separator2)
                ? particle
                : throw new InvalidOperationException($"Incorrect string format: {s}");

        public static bool TryParse(string s, out Particle vector, char separator = ';', char separator2 = ',') =>
            TryParse(s.Trim().Split(separator), out vector, separator2);

        public static Particle Parse(string[] ss) =>
            Parse(ss, ',');

        public static Particle Parse(string[] ss, char separator) =>
            TryParse(ss, out Particle particle, separator)
                ? particle
                : throw new InvalidOperationException($"Input string was not in a correct format.");

        public static bool TryParse(string[] ss, out Particle particle, char separator = ',')
        {
            particle = default;
            Vector a = default;
            if (ss.Length < 2 ||
                !Vector.TryParse(ss[0], out Vector p) ||
                !Vector.TryParse(ss[1], out Vector v) ||
                ss.Length > 2 && !Vector.TryParse(ss[2], out a, separator))
                return false;
            particle = new(p, v, a);
            return true;
        }

        public static implicit operator (Vector p, Vector v, Vector a)(Particle value) =>
            (value.p, value.v, value.a);

        public static implicit operator (Vector p, Vector v)(Particle value) =>
            (value.p, value.v);

        public static implicit operator Particle((Vector p, Vector v, Vector a) value) =>
            new(value.p, value.v, value.a);

        public static implicit operator Particle((Vector p, Vector v) value) =>
            new(value.p, value.v);

        public static bool operator ==(Particle left, Particle right) =>
            left.Equals(right);

        public static bool operator !=(Particle left, Particle right) =>
            !left.Equals(right);
    }
}
