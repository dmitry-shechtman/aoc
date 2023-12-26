using System;

namespace aoc
{
    public struct LongParticle3D : IParticle<LongParticle3D, LongVector3D, long>
    {
        public readonly LongVector3D p;
        public readonly LongVector3D v;
        public readonly LongVector3D a;

        public LongParticle3D(LongVector3D p, LongVector3D v, LongVector3D a = default)
        {
            this.p = p;
            this.v = v;
            this.a = a;
        }

        public LongParticle3D(Particle3D p)
            : this(new(p.p), new(p.v), new(p.a))
        {
        }

        public LongParticle3D(LongParticle p)
            : this(new(p.p), new(p.v), new(p.a))
        {
        }

        public readonly override bool Equals(object obj) =>
            obj is LongParticle3D other && Equals(other);

        public readonly bool Equals(LongParticle3D other) =>
            p.Equals(other.p) &&
            v.Equals(other.v) &&
            a.Equals(other.a);

        public readonly override int GetHashCode() =>
            HashCode.Combine(p, v, a);

        public readonly override string ToString() =>
            $"{p};{v};{a}";

        public readonly void Deconstruct(out LongVector3D p, out LongVector3D v, out LongVector3D a)
        {
            p = this.p;
            v = this.v;
            a = this.a;
        }

        public readonly void Deconstruct(out LongVector3D p, out LongVector3D v)
        {
            p = this.p;
            v = this.v;
        }

        public readonly LongParticle3D GetNext() =>
            new(p + v, v);

        public readonly LongParticle3D GetNextPV() =>
            new(p + v, v + a, a);

        public readonly LongParticle3D GetNextVP() =>
            new(p + v + a, v + a, a);

        public static LongParticle3D Parse(string s) =>
            Parse(s, ';');

        public static LongParticle3D Parse(string s, char separator, char separator2 = ',') =>
            TryParse(s, out LongParticle3D particle, separator, separator2)
                ? particle
                : throw new InvalidOperationException($"Incorrect string format: {s}");

        public static bool TryParse(string s, out LongParticle3D vector, char separator = ';', char separator2 = ',') =>
            TryParse(s.Trim().Split(separator), out vector, separator2);

        public static LongParticle3D Parse(string[] ss) =>
            Parse(ss, ',');

        public static LongParticle3D Parse(string[] ss, char separator) =>
            TryParse(ss, out LongParticle3D particle, separator)
                ? particle
                : throw new InvalidOperationException($"Input string was not in a correct format.");

        public static bool TryParse(string[] ss, out LongParticle3D particle, char separator = ',')
        {
            particle = default;
            LongVector3D a = default;
            if (ss.Length < 2 ||
                !LongVector3D.TryParse(ss[0], out LongVector3D p) ||
                !LongVector3D.TryParse(ss[1], out LongVector3D v) ||
                ss.Length > 2 && !LongVector3D.TryParse(ss[2], out a, separator))
                return false;
            particle = new(p, v, a);
            return true;
        }

        public static implicit operator (LongVector3D p, LongVector3D v, LongVector3D a)(LongParticle3D value) =>
            (value.p, value.v, value.a);

        public static implicit operator (LongVector3D p, LongVector3D v)(LongParticle3D value) =>
            (value.p, value.v);

        public static implicit operator LongParticle3D((LongVector3D p, LongVector3D v, LongVector3D a) value) =>
            new(value.p, value.v, value.a);

        public static implicit operator LongParticle3D((LongVector3D p, LongVector3D v) value) =>
            new(value.p, value.v);

        public static explicit operator Particle3D(LongParticle3D value) =>
            new((Vector3D)value.p, (Vector3D)value.v, (Vector3D)value.a);

        public static explicit operator LongParticle(LongParticle3D value) =>
            new((LongVector)value.p, (LongVector)value.v, (LongVector)value.a);

        public static bool operator ==(LongParticle3D left, LongParticle3D right) =>
            left.Equals(right);

        public static bool operator !=(LongParticle3D left, LongParticle3D right) =>
            !left.Equals(right);
    }
}
