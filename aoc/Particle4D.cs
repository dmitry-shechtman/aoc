using System;

namespace aoc
{
    using Helper = Internal.ParticleHelper<Particle4D, Vector4D>;

    public struct Particle4D : IParticle<Particle4D, Vector4D, int>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromArray, Vector4D.TryParse));

        private static Helper Helper => _helper.Value;

        public readonly Vector4D p;
        public readonly Vector4D v;
        public readonly Vector4D a;

        public Particle4D(Vector4D p, Vector4D v, Vector4D a = default)
        {
            this.p = p;
            this.v = v;
            this.a = a;
        }

        public Particle4D(Particle particle)
            : this(new(particle.p), new(particle.v), new(particle.a))
        {
        }

        public Particle4D(Particle3D particle)
            : this(new(particle.p), new(particle.v), new(particle.a))
        {
        }

        public readonly override bool Equals(object obj) =>
            obj is Particle4D other && Equals(other);

        public readonly bool Equals(Particle4D other) =>
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

        public readonly void Deconstruct(out Vector4D p, out Vector4D v, out Vector4D a)
        {
            p = this.p;
            v = this.v;
            a = this.a;
        }

        public readonly void Deconstruct(out Vector4D p, out Vector4D v)
        {
            p = this.p;
            v = this.v;
        }

        public readonly Particle4D GetNext() =>
            new(p + v, v);

        public readonly Particle4D GetNextPV() =>
            new(p + v, v + a, a);

        public readonly Particle4D GetNextVP() =>
            new(p + v + a, v + a, a);

        public readonly Vector4D P => p;
        public readonly Vector4D V => v;
        public readonly Vector4D A => a;

        public static Particle4D Parse(string s) =>
            Helper.Parse(s);

        public static bool TryParse(string s, out Particle4D particle) =>
            Helper.TryParse(s, out particle);

        public static Particle4D Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out Particle4D particle) =>
            Helper.TryParse(s, separator, out particle);

        public static Particle4D Parse(string s, char separator, char separator2) =>
            Helper.Parse(s, separator, separator2);

        public static bool TryParse(string s, char separator, char separator2, out Particle4D particle) =>
            Helper.TryParse(s, separator, separator2, out particle);

        public static Particle4D Parse(string[] ss) =>
            Helper.Parse(ss);

        public static bool TryParse(string[] ss, out Particle4D particle) =>
            Helper.TryParse(ss, out particle);

        public static Particle4D Parse(string[] ss, char separator) =>
            Helper.Parse(ss, separator);

        public static bool TryParse(string[] ss, char separator, out Particle4D particle) =>
            Helper.TryParse(ss, separator, out particle);

        private static Particle4D FromArray(Vector4D[] values) =>
            new(values[0], values[1], values[2]);

        public static implicit operator (Vector4D p, Vector4D v, Vector4D a)(Particle4D value) =>
            (value.p, value.v, value.a);

        public static implicit operator (Vector4D p, Vector4D v)(Particle4D value) =>
            (value.p, value.v);

        public static implicit operator Particle4D((Vector4D p, Vector4D v, Vector4D a) value) =>
            new(value.p, value.v, value.a);

        public static implicit operator Particle4D((Vector4D p, Vector4D v) value) =>
            new(value.p, value.v);

        public static explicit operator Particle(Particle4D particle) =>
            new((Vector)particle.p, (Vector)particle.v, (Vector)particle.a);

        public static explicit operator Particle3D(Particle4D particle) =>
            new((Vector3D)particle.p, (Vector3D)particle.v, (Vector3D)particle.a);

        public static bool operator ==(Particle4D left, Particle4D right) =>
            left.Equals(right);

        public static bool operator !=(Particle4D left, Particle4D right) =>
            !left.Equals(right);
    }
}
