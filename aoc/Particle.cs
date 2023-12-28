using System;

namespace aoc
{
    using Helper = Internal.ParticleHelper<Particle, Vector>;

    public struct Particle : IParticle<Particle, Vector, int>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromArray, Vector.TryParse));

        private static Helper Helper => _helper.Value;

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
            Helper.ToString(this);

        public readonly string ToString(IFormatProvider provider) =>
            Helper.ToString(this, provider);

        public readonly string ToString(string format, IFormatProvider provider = null) =>
            Helper.ToString(this, format, provider);

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

        public readonly Vector P => p;
        public readonly Vector V => v;
        public readonly Vector A => a;

        public static Particle Parse(string s) =>
            Helper.Parse(s);

        public static bool TryParse(string s, out Particle particle) =>
            Helper.TryParse(s, out particle);

        public static Particle Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out Particle particle) =>
            Helper.TryParse(s, separator, out particle);

        public static Particle Parse(string s, char separator, char separator2) =>
            Helper.Parse(s, separator, separator2);

        public static bool TryParse(string s, char separator, char separator2, out Particle particle) =>
            Helper.TryParse(s, separator, separator2, out particle);

        public static Particle Parse(string[] ss) =>
            Helper.Parse(ss);

        public static bool TryParse(string[] ss, out Particle particle) =>
            Helper.TryParse(ss, out particle);

        public static Particle Parse(string[] ss, char separator) =>
            Helper.Parse(ss, separator);

        public static bool TryParse(string[] ss, char separator, out Particle particle) =>
            Helper.TryParse(ss, separator, out particle);

        private static Particle FromArray(Vector[] values) =>
            new(values[0], values[1], values[2]);

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
