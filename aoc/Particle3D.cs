using System;

namespace aoc
{
    public struct Particle3D : IParticle<Particle3D, Vector3D, int>
    {
        private static readonly Lazy<ParticleHelper<Particle3D, Vector3D>> _helper =
            new(() => new(FromArray, Vector3D.TryParse));

        private static ParticleHelper<Particle3D, Vector3D> Helper => _helper.Value;

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
            Helper.ToString(this);

        public readonly string ToString(IFormatProvider provider) =>
            Helper.ToString(this, provider);

        public readonly string ToString(string format, IFormatProvider provider = null) =>
            Helper.ToString(this, format, provider);

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

        public readonly Vector3D P => p;
        public readonly Vector3D V => v;
        public readonly Vector3D A => a;

        public static Particle3D Parse(string s) =>
            Parse(s, ';');

        public static Particle3D Parse(string s, char separator, char separator2 = ',') =>
            Helper.Parse(s, separator, separator2);

        public static bool TryParse(string s, out Particle3D particle, char separator = ';', char separator2 = ',') =>
            Helper.TryParse(s, out particle, separator, separator2);

        public static Particle3D Parse(string[] ss) =>
            Parse(ss, ',');

        public static Particle3D Parse(string[] ss, char separator) =>
            Helper.Parse(ss, separator);

        public static bool TryParse(string[] ss, out Particle3D particle, char separator = ',') =>
            Helper.TryParse(ss, out particle, separator);

        private static Particle3D FromArray(Vector3D[] values) =>
            new(values[0], values[1], values[2]);

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
