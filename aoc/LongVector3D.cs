using System;
using System.Linq;

namespace aoc
{
    public struct LongVector3D : IEquatable<LongVector3D>, IFormattable
    {
        private const int Cardinality = 3;

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

        public readonly override string ToString() =>
            $"{x},{y},{z}";

        private static readonly string[] FormatKeys    = { "x", "y", "z" };
        private static readonly string[] FormatStrings = { "neswud" };

        public readonly string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                return ToString();
            if (FormatKeys.Any(format.Contains))
                return ReplaceFormat(format);
            if (format.Length > 1)
                return ToString();
            int index = Headings.IndexOf(this);
            if (index < 0)
                return ToString();
            char c = char.ToLowerInvariant(format[0]);
            string s = FormatStrings.Find(s => s.Contains(c));
            if (s.Length == 0)
                return ToString();
            return char.IsUpper(format[0])
                ? char.ToUpperInvariant(s[index]).ToString()
                : s[index].ToString();
        }

        private readonly string ReplaceFormat(string format)
        {
            for (int i = 0; i < FormatKeys.Length; i++)
                format = format.Replace(FormatKeys[i], this[i].ToString());
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

        public readonly long this[int i] => i switch
        {
            0 => x,
            1 => y,
            2 => z,
            _ => throw new IndexOutOfRangeException(),
        };

        public static LongVector3D Parse(string s, char separator = ',') =>
            TryParse(s, out LongVector3D vector, separator)
                ? vector
                : throw new InvalidOperationException($"Incorrect string format: {s}");

        public static bool TryParse(string s, out LongVector3D vector, char separator = ',') =>
            TryParse(s.Trim().Split(separator), out vector);

        public static LongVector3D Parse(string[] ss) =>
            TryParse(ss, out LongVector3D vector)
                ? vector
                : throw new InvalidOperationException($"Input string was not in a correct format.");

        public static bool TryParse(string[] ss, out LongVector3D vector)
        {
            vector = default;
            if (ss.Length < Cardinality)
                return false;
            long[] values = new long[Cardinality];
            if (ss[..Cardinality].Any((s, i) => !long.TryParse(s, out values[i])))
                return false;
            vector = new(values);
            return true;
        }

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
