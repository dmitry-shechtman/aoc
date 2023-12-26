using System;

using static aoc.ParseHelper;
using static aoc.ParticleParseHelper<aoc.LongParticle, aoc.LongVector, long>;

namespace aoc
{
    public struct LongParticle : IParticle<LongParticle, LongVector, long>
    {
        public readonly LongVector p;
        public readonly LongVector v;
        public readonly LongVector a;

        public LongParticle(LongVector p, LongVector v, LongVector a = default)
        {
            this.p = p;
            this.v = v;
            this.a = a;
        }

        public LongParticle(Particle p)
            : this(new(p.p), new(p.v), new(p.a))
        {
        }

        public readonly override bool Equals(object obj) =>
            obj is LongParticle other && Equals(other);

        public readonly bool Equals(LongParticle other) =>
            p.Equals(other.p) &&
            v.Equals(other.v) &&
            a.Equals(other.a);

        public readonly override int GetHashCode() =>
            HashCode.Combine(p, v, a);

        public readonly override string ToString() =>
            $"{p};{v};{a}";

        public readonly void Deconstruct(out LongVector p, out LongVector v, out LongVector a)
        {
            p = this.p;
            v = this.v;
            a = this.a;
        }

        public readonly void Deconstruct(out LongVector p, out LongVector v)
        {
            p = this.p;
            v = this.v;
        }

        public readonly LongParticle GetNext() =>
            new(p + v, v);

        public readonly LongParticle GetNextPV() =>
            new(p + v, v + a, a);

        public readonly LongParticle GetNextVP() =>
            new(p + v + a, v + a, a);

        public static LongParticle Parse(string s) =>
            Parse(s, ';');

        public static LongParticle Parse(string s, char separator, char separator2 = ',') =>
            Parse<LongParticle>(s, TryParse, separator, separator2);

        public static bool TryParse(string s, out LongParticle vector, char separator = ';', char separator2 = ',') =>
            TryParse<LongParticle>(s, TryParse, separator, separator2, out vector);

        public static LongParticle Parse(string[] ss) =>
            Parse(ss, ',');

        public static LongParticle Parse(string[] ss, char separator) =>
            Parse<LongParticle>(ss, TryParse, separator);

        public static bool TryParse(string[] ss, out LongParticle particle, char separator = ',') =>
            TryParseParticle(ss, LongVector.TryParse, FromArray, out particle, separator);

        private static LongParticle FromArray(LongVector[] values) =>
            new(values[0], values[1], values[2]);

        public static implicit operator (LongVector p, LongVector v, LongVector a)(LongParticle value) =>
            (value.p, value.v, value.a);

        public static implicit operator (LongVector p, LongVector v)(LongParticle value) =>
            (value.p, value.v);

        public static implicit operator LongParticle((LongVector p, LongVector v, LongVector a) value) =>
            new(value.p, value.v, value.a);

        public static implicit operator LongParticle((LongVector p, LongVector v) value) =>
            new(value.p, value.v);

        public static explicit operator Particle(LongParticle value) =>
            new((Vector)value.p, (Vector)value.v, (Vector)value.a);

        public static bool operator ==(LongParticle left, LongParticle right) =>
            left.Equals(right);

        public static bool operator !=(LongParticle left, LongParticle right) =>
            !left.Equals(right);
    }
}
