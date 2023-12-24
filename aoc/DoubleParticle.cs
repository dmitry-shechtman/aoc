using System;

namespace aoc
{
    public struct DoubleParticle : IEquatable<DoubleParticle>
    {
        public readonly DoubleVector p;
        public readonly DoubleVector v;
        public readonly DoubleVector a;

        public DoubleParticle(DoubleVector p, DoubleVector v, DoubleVector a = default)
        {
            this.p = p;
            this.v = v;
            this.a = a;
        }

        public DoubleParticle(Particle p)
            : this(p.p, p.v, p.a)
        {
        }

        public DoubleParticle(LongParticle p)
            : this((DoubleVector)p.p, (DoubleVector)p.v, (DoubleVector)p.a)
        {
        }

        public readonly override bool Equals(object obj) =>
            obj is DoubleParticle other && Equals(other);

        public readonly bool Equals(DoubleParticle other) =>
            p.Equals(other.p) &&
            v.Equals(other.v) &&
            a.Equals(other.a);

        public readonly override int GetHashCode() =>
            HashCode.Combine(p, v, a);

        public readonly override string ToString() =>
            $"{p};{v};{a}";

        public readonly void Deconstruct(out DoubleVector p, out DoubleVector v, out DoubleVector a)
        {
            p = this.p;
            v = this.v;
            a = this.a;
        }

        public readonly void Deconstruct(out DoubleVector p, out DoubleVector v)
        {
            p = this.p;
            v = this.v;
        }

        public readonly DoubleParticle GetNext() =>
            new(p + v, v);

        public readonly DoubleParticle GetNextPV() =>
            new(p + v, v + a, a);

        public readonly DoubleParticle GetNextVP() =>
            new(p + v + a, v + a, a);

        public static DoubleParticle Parse(string s) =>
            Parse(s, ';');

        public static DoubleParticle Parse(string s, char separator, char separator2 = ',') =>
            TryParse(s, out DoubleParticle particle, separator, separator2)
                ? particle
                : throw new InvalidOperationException($"Incorrect string format: {s}");

        public static bool TryParse(string s, out DoubleParticle vector, char separator = ';', char separator2 = ',') =>
            TryParse(s.Trim().Split(separator), out vector, separator2);

        public static DoubleParticle Parse(string[] ss) =>
            Parse(ss, ',');

        public static DoubleParticle Parse(string[] ss, char separator) =>
            TryParse(ss, out DoubleParticle particle, separator)
                ? particle
                : throw new InvalidOperationException($"Input string was not in a correct format.");

        public static bool TryParse(string[] ss, out DoubleParticle particle, char separator = ',')
        {
            particle = default;
            DoubleVector a = default;
            if (ss.Length < 2 ||
                !DoubleVector.TryParse(ss[0], out DoubleVector p) ||
                !DoubleVector.TryParse(ss[1], out DoubleVector v) ||
                ss.Length > 2 && !DoubleVector.TryParse(ss[2], out a, separator))
                return false;
            particle = new(p, v, a);
            return true;
        }

        public static implicit operator (DoubleVector p, DoubleVector v, DoubleVector a)(DoubleParticle value) =>
            (value.p, value.v, value.a);

        public static implicit operator (DoubleVector p, DoubleVector v)(DoubleParticle value) =>
            (value.p, value.v);

        public static implicit operator DoubleParticle((DoubleVector p, DoubleVector v, DoubleVector a) value) =>
            new(value.p, value.v, value.a);

        public static implicit operator DoubleParticle((DoubleVector p, DoubleVector v) value) =>
            new(value.p, value.v);

        public static explicit operator Particle(DoubleParticle value) =>
            new((Vector)value.p, (Vector)value.v, (Vector)value.a);

        public static bool operator ==(DoubleParticle left, DoubleParticle right) =>
            left.Equals(right);

        public static bool operator !=(DoubleParticle left, DoubleParticle right) =>
            !left.Equals(right);
    }
}
