using System;

namespace aoc
{
    public struct LongVector3D : IVector3D<LongVector3D, LongVector, long>
    {
        private static readonly Lazy<Vector3DHelper<LongVector3D, long>> _helper =
            new(() => new(FromArray, long.TryParse, -1, 0, 1));

        private static Vector3DHelper<LongVector3D, long> Helper => _helper.Value;

        public static readonly LongVector3D Zero  = default;

        public static readonly LongVector3D North = ( 0, -1,  0);
        public static readonly LongVector3D East  = ( 1,  0,  0);
        public static readonly LongVector3D South = ( 0,  1,  0);
        public static readonly LongVector3D West  = (-1,  0,  0);
        public static readonly LongVector3D Up    = ( 0,  0, -1);
        public static readonly LongVector3D Down  = ( 0,  0,  1);

        public static LongVector3D[] Headings => Helper.Headings;

        public readonly long x;
        public readonly long y;
        public readonly long z;

        public LongVector3D(long x, long y, long z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public LongVector3D(LongVector v, long z = 0)
            : this(v.x, v.y, z)
        {
        }

        public LongVector3D(Vector3D v)
            : this(v.x, v.y, v.z)
        {
        }

        public LongVector3D(long[] values)
            : this(values[0], values[1], values[2])
        {
        }

        public readonly override bool Equals(object obj) =>
            obj is LongVector3D other && Equals(other);

        public readonly bool Equals(LongVector3D other) =>
            x == other.x &&
            y == other.y &&
            z == other.z;

        public readonly override int GetHashCode() =>
            HashCode.Combine(x, y, z);

        public readonly override string ToString() =>
            Helper.ToString(this);

        public readonly string ToString(IFormatProvider provider) =>
            Helper.ToString(this, provider);

        public readonly string ToString(string format, IFormatProvider provider = null) =>
            Helper.ToString(this, format, provider);

        public readonly void Deconstruct(out long x, out long y, out long z)
        {
            x = this.x;
            y = this.y;
            z = this.z;
        }

        public readonly void Deconstruct(out LongVector vector, out long z)
        {
            vector = new(x, y);
            z = this.z;
        }

        public readonly long Abs() =>
            Math.Abs(x) + Math.Abs(y) + Math.Abs(z);

        public readonly LongVector3D Abs2() =>
            new(Math.Abs(x), Math.Abs(y), Math.Abs(z));

        public readonly LongVector3D Sign() =>
            new(Math.Sign(x), Math.Sign(y), Math.Sign(z));

        public readonly long X => x;
        public readonly long Y => y;
        public readonly long Z => z;

        public static LongVector3D Parse(string s) =>
            Parse(s, ',');

        public static LongVector3D Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, out LongVector3D vector, char separator = ',') =>
            Helper.TryParse(s, out vector, separator);

        public static LongVector3D Parse(string[] ss) =>
            Helper.Parse(ss);

        public static bool TryParse(string[] ss, out LongVector3D vector) =>
            Helper.TryParse(ss, out vector);

        private static LongVector3D FromArray(long[] values) =>
            new(values);

        public static LongVector3D operator +(LongVector3D vector) =>
            vector;

        public readonly LongVector3D Neg() =>
            new(-x, -y, -z);

        public static LongVector3D Neg(LongVector3D vector) =>
            vector.Neg();

        public static LongVector3D operator -(LongVector3D vector) =>
            vector.Neg();

        public readonly LongVector3D Add(LongVector3D other) =>
            new(x + other.x, y + other.y, z + other.z);

        public static LongVector3D Add(LongVector3D left, LongVector3D right) =>
            left.Add(right);

        public static LongVector3D operator +(LongVector3D left, LongVector3D right) =>
            left.Add(right);

        public readonly LongVector3D Sub(LongVector3D other) =>
            new(x - other.x, y - other.y, z - other.z);

        public static LongVector3D Sub(LongVector3D left, LongVector3D right) =>
            left.Sub(right);

        public static LongVector3D operator -(LongVector3D left, LongVector3D right) =>
            left.Sub(right);

        public readonly LongVector3D Mul(long scalar) =>
            new(x * scalar, y * scalar, z * scalar);

        public static LongVector3D Mul(LongVector3D vector, long scalar) =>
            vector.Mul(scalar);

        public static LongVector3D operator *(LongVector3D vector, long scalar) =>
            vector.Mul(scalar);

        public static LongVector3D operator *(long scalar, LongVector3D vector) =>
            vector.Mul(scalar);

        public readonly LongVector3D Div(long scalar) =>
            new(x / scalar, y / scalar, z / scalar);

        public static LongVector3D Div(LongVector3D vector, long scalar) =>
            vector.Div(scalar);

        public static LongVector3D operator /(LongVector3D vector, long scalar) =>
            vector.Div(scalar);

        public readonly long Dot(LongVector3D other) =>
            x * other.x + y * other.y + z * other.z;

        public static long Dot(LongVector3D left, LongVector3D right) =>
            left.Dot(right);

        public static LongVector3D Min(LongVector3D left, LongVector3D right) =>
            new(Math.Min(left.x, right.x), Math.Min(left.y, right.y), Math.Min(left.z, right.z));

        public static LongVector3D Max(LongVector3D left, LongVector3D right) =>
            new(Math.Max(left.x, right.x), Math.Max(left.y, right.y), Math.Max(left.z, right.z));

        public static implicit operator (long x, long y, long z)(LongVector3D value) =>
            (value.x, value.y, value.z);

        public static implicit operator LongVector3D((long x, long y, long z) value) =>
            new(value.x, value.y, value.z);

        public static implicit operator LongVector3D((LongVector vector, long z) value) =>
            new(value.vector, value.z);

        public static implicit operator LongVector3D(long[] values) =>
            new(values);

        public static implicit operator LongVector3D(Vector3D value) =>
            new(value);

        public static explicit operator LongVector3D(LongVector value) =>
            new(value);

        public static explicit operator Vector3D(LongVector3D value) =>
            new((int)value.x, (int)value.y, (int)value.z);

        public static explicit operator LongVector(LongVector3D value) =>
            new(value.x, value.y);

        public static bool operator ==(LongVector3D left, LongVector3D right) =>
            left.Equals(right);

        public static bool operator !=(LongVector3D left, LongVector3D right) =>
            !left.Equals(right);
    }
}
