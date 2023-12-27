using System;

namespace aoc
{
    public struct DoubleVector3D : IVector3D<DoubleVector3D, DoubleVector, double>
    {
        private static readonly Lazy<Vector3DHelper<DoubleVector3D, double>> _helper =
            new(() => new(FromArray, double.TryParse, -1, 0, 1));

        private static Vector3DHelper<DoubleVector3D, double> Helper => _helper.Value;

        public static readonly DoubleVector3D Zero  = default;

        public static readonly DoubleVector3D North = ( 0, -1,  0);
        public static readonly DoubleVector3D East  = ( 1,  0,  0);
        public static readonly DoubleVector3D South = ( 0,  1,  0);
        public static readonly DoubleVector3D West  = (-1,  0,  0);
        public static readonly DoubleVector3D Up    = ( 0,  0, -1);
        public static readonly DoubleVector3D Down  = ( 0,  0,  1);

        public static DoubleVector3D[] Headings => Helper.Headings;

        public readonly double x;
        public readonly double y;
        public readonly double z;

        public DoubleVector3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public DoubleVector3D(DoubleVector v, double z = 0)
            : this(v.x, v.y, z)
        {
        }

        public DoubleVector3D(Vector3D v)
            : this(v.x, v.y, v.z)
        {
        }

        public DoubleVector3D(LongVector3D v)
            : this(v.x, v.y, v.z)
        {
        }

        public DoubleVector3D(double[] values)
            : this(values[0], values[1], values[2])
        {
        }

        public readonly override bool Equals(object obj) =>
            obj is DoubleVector3D other && Equals(other);

        public readonly bool Equals(DoubleVector3D other) =>
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

        public readonly void Deconstruct(out double x, out double y, out double z)
        {
            x = this.x;
            y = this.y;
            z = this.z;
        }

        public readonly void Deconstruct(out DoubleVector vector, out double z)
        {
            vector = new(x, y);
            z = this.z;
        }

        public readonly double Abs() =>
            Math.Abs(x) + Math.Abs(y) + Math.Abs(z);

        public readonly DoubleVector3D Abs2() =>
            new(Math.Abs(x), Math.Abs(y), Math.Abs(z));

        public readonly DoubleVector3D Sign() =>
            new(Math.Sign(x), Math.Sign(y), Math.Sign(z));

        public readonly double X => x;
        public readonly double Y => y;
        public readonly double Z => z;

        public static DoubleVector3D Parse(string s) =>
            Parse(s, ',');

        public static DoubleVector3D Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, out DoubleVector3D vector, char separator = ',') =>
            Helper.TryParse(s, out vector, separator);

        public static DoubleVector3D Parse(string[] ss) =>
            Helper.Parse(ss);

        public static bool TryParse(string[] ss, out DoubleVector3D vector) =>
            Helper.TryParse(ss, out vector);

        private static DoubleVector3D FromArray(double[] values) =>
            new(values);

        public static DoubleVector3D operator +(DoubleVector3D vector) =>
            vector;

        public readonly DoubleVector3D Neg() =>
            new(-x, -y, -z);

        public static DoubleVector3D Neg(DoubleVector3D vector) =>
            vector.Neg();

        public static DoubleVector3D operator -(DoubleVector3D vector) =>
            vector.Neg();

        public readonly DoubleVector3D Add(DoubleVector3D other) =>
            new(x + other.x, y + other.y, z + other.z);

        public static DoubleVector3D Add(DoubleVector3D left, DoubleVector3D right) =>
            left.Add(right);

        public static DoubleVector3D operator +(DoubleVector3D left, DoubleVector3D right) =>
            left.Add(right);

        public readonly DoubleVector3D Sub(DoubleVector3D other) =>
            new(x - other.x, y - other.y, z - other.z);

        public static DoubleVector3D Sub(DoubleVector3D left, DoubleVector3D right) =>
            left.Sub(right);

        public static DoubleVector3D operator -(DoubleVector3D left, DoubleVector3D right) =>
            left.Sub(right);

        public readonly DoubleVector3D Mul(double scalar) =>
            new(x * scalar, y * scalar, z * scalar);

        public static DoubleVector3D Mul(DoubleVector3D vector, double scalar) =>
            vector.Mul(scalar);

        public static DoubleVector3D operator *(DoubleVector3D vector, double scalar) =>
            vector.Mul(scalar);

        public static DoubleVector3D operator *(double scalar, DoubleVector3D vector) =>
            vector.Mul(scalar);

        public readonly DoubleVector3D Div(double scalar) =>
            new(x / scalar, y / scalar, z / scalar);

        public static DoubleVector3D Div(DoubleVector3D vector, double scalar) =>
            vector.Div(scalar);

        public static DoubleVector3D operator /(DoubleVector3D vector, double scalar) =>
            vector.Div(scalar);

        public readonly double Dot(DoubleVector3D other) =>
            x * other.x + y * other.y + z * other.z;

        public static double Dot(DoubleVector3D left, DoubleVector3D right) =>
            left.Dot(right);

        public static DoubleVector3D Min(DoubleVector3D left, DoubleVector3D right) =>
            new(Math.Min(left.x, right.x), Math.Min(left.y, right.y), Math.Min(left.z, right.z));

        public static DoubleVector3D Max(DoubleVector3D left, DoubleVector3D right) =>
            new(Math.Max(left.x, right.x), Math.Max(left.y, right.y), Math.Max(left.z, right.z));

        public static implicit operator (double x, double y, double z)(DoubleVector3D value) =>
            (value.x, value.y, value.z);

        public static implicit operator DoubleVector3D((double x, double y, double z) value) =>
            new(value.x, value.y, value.z);

        public static implicit operator DoubleVector3D((DoubleVector vector, double z) value) =>
            new(value.vector, value.z);

        public static implicit operator DoubleVector3D(double[] values) =>
            new(values);

        public static implicit operator DoubleVector3D(Vector3D value) =>
            new(value);

        public static explicit operator DoubleVector3D(LongVector3D value) =>
            new(value);

        public static explicit operator DoubleVector3D(DoubleVector value) =>
            new(value);

        public static explicit operator Vector3D(DoubleVector3D value) =>
            new((int)value.x, (int)value.y, (int)value.z);

        public static explicit operator LongVector3D(DoubleVector3D value) =>
            new((long)value.x, (long)value.y, (long)value.z);

        public static explicit operator DoubleVector(DoubleVector3D value) =>
            new(value.x, value.y);

        public static bool operator ==(DoubleVector3D left, DoubleVector3D right) =>
            left.Equals((object)right);

        public static bool operator !=(DoubleVector3D left, DoubleVector3D right) =>
            !left.Equals((object)right);
    }
}
