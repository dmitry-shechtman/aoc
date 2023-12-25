using System;
using System.Linq;

namespace aoc
{
    public struct DoubleVector : IEquatable<DoubleVector>, IFormattable
    {
        private const int Cardinality = 2;

        public static readonly DoubleVector Zero      = default;

        public static readonly DoubleVector North     = ( 0, -1);
        public static readonly DoubleVector East      = ( 1,  0);
        public static readonly DoubleVector South     = ( 0,  1);
        public static readonly DoubleVector West      = (-1,  0);

        public static readonly DoubleVector[] Headings = { North, East, South, West };

        public readonly double x;
        public readonly double y;

        public DoubleVector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public DoubleVector(Vector v)
            : this(v.x, v.y)
        {
        }

        public DoubleVector(LongVector v)
            : this(v.x, v.y)
        {
        }

        public DoubleVector(double[] values)
            : this(values[0], values[1])
        {
        }

        public readonly override bool Equals(object obj) =>
            obj is DoubleVector other && Equals(other);

        public readonly bool Equals(DoubleVector other) =>
            x == other.x &&
            y == other.y;

        public readonly override int GetHashCode() =>
            HashCode.Combine(x, y);

        public readonly override string ToString() =>
            $"{x},{y}";

        private static readonly string[] FormatKeys    = { "x", "y" };
        private static readonly string[] FormatStrings = { "nesw", "urdl", "^>v<" };

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

        public readonly void Deconstruct(out double x, out double y)
        {
            x = this.x;
            y = this.y;
        }

        public readonly double this[int i] => i switch
        {
            0 => x,
            1 => y,
            _ => throw new IndexOutOfRangeException(),
        };

        public static DoubleVector Parse(string s, char separator = ',') =>
            TryParse(s, out DoubleVector vector, separator)
                ? vector
                : throw new InvalidOperationException($"Incorrect string format: {s}");

        public static bool TryParse(string s, out DoubleVector vector, char separator = ',') =>
            TryParse(s.Trim().Split(separator), out vector);

        public static DoubleVector Parse(string[] ss) =>
            TryParse(ss, out DoubleVector vector)
                ? vector
                : throw new InvalidOperationException($"Input string was not in a correct format.");

        public static bool TryParse(string[] ss, out DoubleVector vector)
        {
            vector = default;
            if (ss.Length < Cardinality)
                return false;
            double[] values = new double[Cardinality];
            if (ss[..Cardinality].Any((s, i) => !double.TryParse(s, out values[i])))
                return false;
            vector = new(values);
            return true;
        }

        public readonly DoubleVector Add(DoubleVector other) =>
            new(x + other.x, y + other.y);

        public static DoubleVector Add(DoubleVector left, DoubleVector right) =>
            left.Add(right);

        public static DoubleVector operator +(DoubleVector left, DoubleVector right) =>
            left.Add(right);

        public readonly DoubleVector Sub(DoubleVector other) =>
            new(x - other.x, y - other.y);

        public static DoubleVector Sub(DoubleVector left, DoubleVector right) =>
            left.Sub(right);

        public readonly DoubleVector Mul(DoubleMatrix m) =>
            new(x * m.m11 + y * m.m21 + m.m31,
                x * m.m12 + y * m.m22 + m.m32);

        public static DoubleVector Mul(DoubleVector vector, DoubleMatrix matrix) =>
            vector.Mul(matrix);

        public static DoubleVector operator *(DoubleVector vector, DoubleMatrix matrix) =>
            vector.Mul(matrix);

        public static DoubleVector operator -(DoubleVector left, DoubleVector right) =>
            left.Sub(right);

        public static implicit operator (double x, double y)(DoubleVector value) =>
            (value.x, value.y);

        public static implicit operator DoubleVector((double x, double y) value) =>
            new(value.x, value.y);

        public static implicit operator DoubleVector(double[] values) =>
            new(values);

        public static implicit operator DoubleVector(Vector value) =>
            new(value);

        public static explicit operator DoubleVector(LongVector value) =>
            new(value);

        public static explicit operator Vector(DoubleVector value) =>
            new((int)value.x, (int)value.y);

        public static explicit operator LongVector(DoubleVector value) =>
            new((long)value.x, (long)value.y);

        public static bool operator ==(DoubleVector left, DoubleVector right) =>
            left.Equals(right);

        public static bool operator !=(DoubleVector left, DoubleVector right) =>
            !left.Equals(right);
    }
}
