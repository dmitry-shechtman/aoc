using System;
using System.Collections.Generic;
using System.Linq;

using static aoc.ParseHelper;
using static aoc.Vector3DParseHelper<aoc.LongVector3D, long>;

namespace aoc
{
    public struct LongVector3D : IVector3D<LongVector3D, LongVector, long>
    {
        public static readonly LongVector3D Zero  = default;

        public static readonly LongVector3D North = ( 0, -1,  0);
        public static readonly LongVector3D East  = ( 1,  0,  0);
        public static readonly LongVector3D South = ( 0,  1,  0);
        public static readonly LongVector3D West  = (-1,  0,  0);
        public static readonly LongVector3D Up    = ( 0,  0, -1);
        public static readonly LongVector3D Down  = ( 0,  0,  1);

        public static readonly LongVector3D[] Headings = { North, East, South, West, Up, Down };

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

        private const string DefaultFormat = "x,y,z";

        private static readonly string[] FormatKeys    = { "x", "y", "z" };
        private static readonly string[] FormatStrings = { "neswud" };

        public readonly override string ToString() =>
            ToStringInner(DefaultFormat, null);

        public readonly string ToString(IFormatProvider provider) =>
            ToStringInner(DefaultFormat, provider);

        public readonly string ToString(string format, IFormatProvider provider = null)
        {
            if (string.IsNullOrEmpty(format))
                format = DefaultFormat;
            if (FormatKeys.Any(format.Contains) || format.Length > 1)
                return ToStringInner(format, provider);
            int index = Headings.IndexOf(this);
            if (index < 0)
                return ToStringInner(DefaultFormat, provider);
            char c = char.ToLowerInvariant(format[0]);
            string s = FormatStrings.Find(s => s.Contains(c));
            if (s.Length == 0)
                return ToStringInner(DefaultFormat, provider);
            return char.IsUpper(format[0])
                ? char.ToUpperInvariant(s[index]).ToString()
                : s[index].ToString();
        }

        private readonly string ToStringInner(string format, IFormatProvider provider)
        {
            for (int i = 0; i < FormatKeys.Length; i++)
                format = format.Replace(FormatKeys[i], this[i].ToString(provider));
            return format;
        }

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

        public readonly IEnumerator<long> GetEnumerator()
        {
            yield return x;
            yield return y;
            yield return z;
        }

        public readonly long this[int i] => i switch
        {
            0 => x,
            1 => y,
            2 => z,
            _ => throw new IndexOutOfRangeException(),
        };

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
            Parse<LongVector3D>(s, TryParse, separator);

        public static bool TryParse(string s, out LongVector3D vector, char separator = ',') =>
            TryParse<LongVector3D>(s, TryParse, separator, out vector);

        public static LongVector3D Parse(string[] ss) =>
            Parse<LongVector3D>(ss, TryParse);

        public static bool TryParse(string[] ss, out LongVector3D vector) =>
            TryParseVector(ss, long.TryParse, FromArray, out vector);

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
