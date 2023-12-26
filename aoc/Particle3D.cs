using System;

namespace aoc
{
    public struct Particle3D : IParticle<Particle3D, Vector3D, int>
    {
        public readonly Vector3D p;
        public readonly Vector3D v;
        public readonly Vector3D a;

        public Particle3D(Vector3D p, Vector3D v, Vector3D a = default)
        {
            this.p = p;
            this.v = v;
            this.a = a;
        }

        public Particle3D(Particle particle)
            : this(new(particle.p), new(particle.v), new(particle.a))
        {
        }

        public readonly override bool Equals(object obj) =>
            obj is Particle3D other && Equals(other);

        public readonly bool Equals(Particle3D other) =>
            p.Equals(other.p) &&
            v.Equals(other.v) &&
            a.Equals(other.a);

        public readonly override int GetHashCode() =>
            HashCode.Combine(p, v, a);

        public readonly override string ToString() =>
            $"{p};{v};{a}";

        public readonly void Deconstruct(out Vector3D p, out Vector3D v, out Vector3D a)
        {
            p = this.p;
            v = this.v;
            a = this.a;
        }

        public readonly void Deconstruct(out Vector3D p, out Vector3D v)
        {
            p = this.p;
            v = this.v;
        }

        public readonly Particle3D GetNext() =>
            new(p + v, v);

        public readonly Particle3D GetNextPV() =>
            new(p + v, v + a, a);

        public readonly Particle3D GetNextVP() =>
            new(p + v + a, v + a, a);

        public static Particle3D Parse(string s) =>
            Parse(s, ';');

        public static Particle3D Parse(string s, char separator, char separator2 = ',') =>
            TryParse(s, out Particle3D particle, separator, separator2)
                ? particle
                : throw new InvalidOperationException($"Incorrect string format: {s}");

        public static bool TryParse(string s, out Particle3D vector, char separator = ';', char separator2 = ',') =>
            TryParse(s.Trim().Split(separator), out vector, separator2);

        public static Particle3D Parse(string[] ss) =>
            Parse(ss, ',');

        public static Particle3D Parse(string[] ss, char separator) =>
            TryParse(ss, out Particle3D particle, separator)
                ? particle
                : throw new InvalidOperationException($"Input string was not in a correct format.");

        public static bool TryParse(string[] ss, out Particle3D particle, char separator = ',')
        {
            particle = default;
            Vector3D a = default;
            if (ss.Length < 2 ||
                !Vector3D.TryParse(ss[0], out Vector3D p) ||
                !Vector3D.TryParse(ss[1], out Vector3D v) ||
                ss.Length > 2 && !Vector3D.TryParse(ss[2], out a, separator))
                return false;
            particle = new(p, v, a);
            return true;
        }

        public static implicit operator (Vector3D p, Vector3D v, Vector3D a)(Particle3D value) =>
            (value.p, value.v, value.a);

        public static implicit operator (Vector3D p, Vector3D v)(Particle3D value) =>
            (value.p, value.v);

        public static implicit operator Particle3D((Vector3D p, Vector3D v, Vector3D a) value) =>
            new(value.p, value.v, value.a);

        public static implicit operator Particle3D((Vector3D p, Vector3D v) value) =>
            new(value.p, value.v);

        public static explicit operator Particle(Particle3D particle) =>
            new((Vector)particle.p, (Vector)particle.v, (Vector)particle.a);

        public static bool operator ==(Particle3D left, Particle3D right) =>
            left.Equals(right);

        public static bool operator !=(Particle3D left, Particle3D right) =>
            !left.Equals(right);
    }
}
