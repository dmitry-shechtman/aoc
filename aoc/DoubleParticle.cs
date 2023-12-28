using System;

namespace aoc
{
    using Helper = Internal.ParticleHelper<DoubleParticle, DoubleVector>;

    public struct DoubleParticle : IParticle<DoubleParticle, DoubleVector, double>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromArray, DoubleVector.TryParse));

        private static Helper Helper => _helper.Value;

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
            Helper.ToString(this);

        public readonly string ToString(IFormatProvider provider) =>
            Helper.ToString(this, provider);

        public readonly string ToString(string format, IFormatProvider provider = null) =>
            Helper.ToString(this, format, provider);

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

        public readonly DoubleVector P => p;
        public readonly DoubleVector V => v;
        public readonly DoubleVector A => a;

        public static DoubleParticle Parse(string s) =>
            Helper.Parse(s);

        public static bool TryParse(string s, out DoubleParticle particle) =>
            Helper.TryParse(s, out particle);

        public static DoubleParticle Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out DoubleParticle particle) =>
            Helper.TryParse(s, separator, out particle);

        public static DoubleParticle Parse(string s, char separator, char separator2) =>
            Helper.Parse(s, separator, separator2);

        public static bool TryParse(string s, char separator, char separator2, out DoubleParticle particle) =>
            Helper.TryParse(s, separator, separator2, out particle);

        public static DoubleParticle Parse(string[] ss) =>
            Helper.Parse(ss);

        public static bool TryParse(string[] ss, out DoubleParticle particle) =>
            Helper.TryParse(ss, out particle);

        public static DoubleParticle Parse(string[] ss, char separator) =>
            Helper.Parse(ss, separator);

        public static bool TryParse(string[] ss, char separator, out DoubleParticle particle) =>
            Helper.TryParse(ss, separator, out particle);

        private static DoubleParticle FromArray(DoubleVector[] values) =>
            new(values[0], values[1], values[2]);

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
