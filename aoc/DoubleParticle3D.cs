﻿using System;

namespace aoc
{
    using Helper = Internal.ParticleHelper<DoubleParticle3D, DoubleVector3D>;

    public struct DoubleParticle3D : IParticle<DoubleParticle3D, DoubleVector3D>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromArray, DoubleVector3D.TryParse));

        private static Helper Helper => _helper.Value;

        public readonly DoubleVector3D p;
        public readonly DoubleVector3D v;
        public readonly DoubleVector3D a;

        public DoubleParticle3D(DoubleVector3D p, DoubleVector3D v, DoubleVector3D a = default)
        {
            this.p = p;
            this.v = v;
            this.a = a;
        }

        public DoubleParticle3D(Particle3D p)
            : this(p.p, p.v, p.a)
        {
        }

        public DoubleParticle3D(LongParticle3D p)
            : this((DoubleVector3D)p.p, (DoubleVector3D)p.v, (DoubleVector3D)p.a)
        {
        }

        public readonly override bool Equals(object obj) =>
            obj is DoubleParticle3D other && Equals(other);

        public readonly bool Equals(DoubleParticle3D other) =>
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

        public readonly void Deconstruct(out DoubleVector3D p, out DoubleVector3D v, out DoubleVector3D a)
        {
            p = this.p;
            v = this.v;
            a = this.a;
        }

        public readonly void Deconstruct(out DoubleVector3D p, out DoubleVector3D v)
        {
            p = this.p;
            v = this.v;
        }

        public readonly DoubleParticle3D GetNext() =>
            new(p + v, v);

        public readonly DoubleParticle3D GetNextPV() =>
            new(p + v, v + a, a);

        public readonly DoubleParticle3D GetNextVP() =>
            new(p + v + a, v + a, a);

        public readonly DoubleVector3D P => p;
        public readonly DoubleVector3D V => v;
        public readonly DoubleVector3D A => a;

        public static DoubleParticle3D Parse(string s) =>
            Helper.Parse(s);

        public static bool TryParse(string s, out DoubleParticle3D particle) =>
            Helper.TryParse(s, out particle);

        public static DoubleParticle3D Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out DoubleParticle3D particle) =>
            Helper.TryParse(s, separator, out particle);

        public static DoubleParticle3D Parse(string s, char separator, char separator2) =>
            Helper.Parse(s, separator, separator2);

        public static bool TryParse(string s, char separator, char separator2, out DoubleParticle3D particle) =>
            Helper.TryParse(s, separator, separator2, out particle);

        public static DoubleParticle3D Parse(string[] ss) =>
            Helper.Parse(ss);

        public static bool TryParse(string[] ss, out DoubleParticle3D particle) =>
            Helper.TryParse(ss, out particle);

        public static DoubleParticle3D Parse(string[] ss, char separator) =>
            Helper.Parse(ss, separator);

        public static bool TryParse(string[] ss, char separator, out DoubleParticle3D particle) =>
            Helper.TryParse(ss, separator, out particle);

        private static DoubleParticle3D FromArray(DoubleVector3D[] values) =>
            new(values[0], values[1], values[2]);

        public static implicit operator (DoubleVector3D p, DoubleVector3D v, DoubleVector3D a)(DoubleParticle3D value) =>
            (value.p, value.v, value.a);

        public static implicit operator (DoubleVector3D p, DoubleVector3D v)(DoubleParticle3D value) =>
            (value.p, value.v);

        public static implicit operator DoubleParticle3D((DoubleVector3D p, DoubleVector3D v, DoubleVector3D a) value) =>
            new(value.p, value.v, value.a);

        public static implicit operator DoubleParticle3D((DoubleVector3D p, DoubleVector3D v) value) =>
            new(value.p, value.v);

        public static explicit operator Particle3D(DoubleParticle3D value) =>
            new((Vector3D)value.p, (Vector3D)value.v, (Vector3D)value.a);

        public static explicit operator LongParticle3D(DoubleParticle3D value) =>
            new((LongVector3D)value.p, (LongVector3D)value.v, (LongVector3D)value.a);

        public static bool operator ==(DoubleParticle3D left, DoubleParticle3D right) =>
            left.Equals(right);

        public static bool operator !=(DoubleParticle3D left, DoubleParticle3D right) =>
            !left.Equals(right);
    }
}
