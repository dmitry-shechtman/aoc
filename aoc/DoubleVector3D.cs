using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace aoc
{
    using Helper = Internal.Vector3DHelper<DoubleVector3D, double>;

    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public readonly struct DoubleVector3D : IVector<DoubleVector3D, DoubleMatrix3D, double>, IVector3D<DoubleVector3D, DoubleVector, double>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromSpan, Internal.DoubleHelper.Instance));

        internal static Helper Helper => _helper.Value;

        public static DoubleVector3D NegativeOne => Helper.NegativeOne;
        public static DoubleVector3D Zero        => default;
        public static DoubleVector3D One         => Helper.One;

        public static DoubleVector3D North       => Helper.North;
        public static DoubleVector3D East        => Helper.East;
        public static DoubleVector3D South       => Helper.South;
        public static DoubleVector3D West        => Helper.West;
        public static DoubleVector3D Up          => Helper.Up;
        public static DoubleVector3D Down        => Helper.Down;

        public static DoubleVector3D AdditiveIdentity       => Zero;
        public static DoubleVector3D MultiplicativeIdentity => One;

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

        private string GetDebuggerDisplay() =>
            ToString("(x,y,z)");

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

        public static DoubleVector3D Parse(string s, IFormatProvider provider = null) =>
            Helper.Parse(s, provider);

        public static bool TryParse(string s, out DoubleVector3D vector) =>
            Helper.TryParse(s, out vector);

        public static bool TryParse(string s, IFormatProvider provider, out DoubleVector3D vector) =>
            Helper.TryParse(s, provider, out vector);

        public static DoubleVector3D Parse(ReadOnlySpan<char> s, IFormatProvider provider = null) =>
            Helper.Parse(s, provider);

        public static bool TryParse(ReadOnlySpan<char> s, out DoubleVector3D vector) =>
            Helper.TryParse(s, out vector);

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out DoubleVector3D vector) =>
            Helper.TryParse(s, provider, out vector);

        public static DoubleVector3D Parse(string s, char separator, IFormatProvider provider = null) =>
            Helper.Parse(s, separator, provider);

        public static bool TryParse(string s, char separator, out DoubleVector3D vector) =>
            Helper.TryParse(s, separator, out vector);

        public static bool TryParse(string s, char separator, IFormatProvider provider, out DoubleVector3D vector) =>
            Helper.TryParse(s, separator, provider, out vector);

        public static DoubleVector3D Parse(string s, string separator, IFormatProvider provider = null) =>
            Helper.Parse(s, separator, provider);

        public static bool TryParse(string s, string separator, out DoubleVector3D vector) =>
            Helper.TryParse(s, separator, out vector);

        public static bool TryParse(string s, string separator, IFormatProvider provider, out DoubleVector3D vector) =>
            Helper.TryParse(s, separator, provider, out vector);

        public static DoubleVector3D Parse(string s, Regex separator, IFormatProvider provider = null) =>
            Helper.Parse(s, separator, provider);

        public static bool TryParse(string s, Regex separator, out DoubleVector3D vector) =>
            Helper.TryParse(s, separator, out vector);

        public static bool TryParse(string s, Regex separator, IFormatProvider provider, out DoubleVector3D vector) =>
            Helper.TryParse(s, separator, provider, out vector);

        public static DoubleVector3D ParseAny(string input, IFormatProvider provider = null) =>
            Helper.ParseAny(input, provider);

        public static bool TryParseAny(string input, out DoubleVector3D vector) =>
            Helper.TryParseAny(input, out vector);

        public static bool TryParseAny(string input, IFormatProvider provider, out DoubleVector3D vector) =>
            Helper.TryParseAny(input, provider, out vector);

        public static DoubleVector3D[] ParseAll(string input, IFormatProvider provider = null) =>
            Helper.ParseAll(input, provider);

        public static bool TryParseAll(string input, out DoubleVector3D[] vectors) =>
            Helper.TryParseAll(input, out vectors);

        public static bool TryParseAll(string input, IFormatProvider provider, out DoubleVector3D[] vectors) =>
            Helper.TryParseAll(input, provider, out vectors);

        private static DoubleVector3D FromSpan(ReadOnlySpan<double> values) =>
            new(values[0], values[1], values[2]);

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

        public readonly DoubleVector3D Mul(DoubleMatrix3D m) =>
            new(x * m.m11 + y * m.m21 + z * m.m31 + m.m41,
                x * m.m12 + y * m.m22 + z * m.m32 + m.m42,
                x * m.m13 + y * m.m23 + z * m.m33 + m.m43);

        public static DoubleVector3D Mul(DoubleVector3D vector, DoubleMatrix3D matrix) =>
            vector.Mul(matrix);

        public static DoubleVector3D operator *(DoubleVector3D vector, DoubleMatrix3D matrix) =>
            vector.Mul(matrix);

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

        public readonly DoubleVector3D Min(DoubleVector3D other) =>
            new(Math.Min(x, other.x), Math.Min(y, other.y), Math.Min(z, other.z));

        public static DoubleVector3D Min(DoubleVector3D left, DoubleVector3D right) =>
            left.Min(right);

        public readonly DoubleVector3D Max(DoubleVector3D other) =>
            new(Math.Max(x, other.x), Math.Max(y, other.y), Math.Max(z, other.z));

        public static DoubleVector3D Max(DoubleVector3D left, DoubleVector3D right) =>
            left.Max(right);

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
