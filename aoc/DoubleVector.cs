using System;
using System.Collections.Generic;
using System.Linq;

using static aoc.ParseHelper;
using static aoc.Vector2DParseHelper<aoc.DoubleVector, double>;

namespace aoc
{
    public struct DoubleVector : IVector2D<DoubleVector, double>
    {
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

        private const string DefaultFormat = "x,y";

        private static readonly string[] FormatKeys    = { "x", "y" };
        private static readonly string[] FormatStrings = { "nesw", "urdl", "^>v<" };

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

        public readonly void Deconstruct(out double x, out double y)
        {
            x = this.x;
            y = this.y;
        }

        public readonly IEnumerator<double> GetEnumerator()
        {
            yield return x;
            yield return y;
        }

        public readonly double this[int i] => i switch
        {
            0 => x,
            1 => y,
            _ => throw new IndexOutOfRangeException(),
        };

        public readonly double Abs() =>
            Math.Abs(x) + Math.Abs(y);

        public readonly DoubleVector Abs2() =>
            new(Math.Abs(x), Math.Abs(y));

        public readonly DoubleVector Sign() =>
            new(Math.Sign(x), Math.Sign(y));

        public readonly double X => x;
        public readonly double Y => y;

        public static DoubleVector Parse(string s) =>
            Parse(s, ',');

        public static DoubleVector Parse(string s, char separator) =>
            Parse<DoubleVector>(s, TryParse, separator);

        public static bool TryParse(string s, out DoubleVector vector, char separator = ',') =>
            TryParse<DoubleVector>(s, TryParse, separator, out vector);

        public static DoubleVector Parse(string[] ss) =>
            Parse<DoubleVector>(ss, TryParse);

        public static bool TryParse(string[] ss, out DoubleVector vector) =>
            TryParseVector(ss, double.TryParse, FromArray, out vector);

        private static DoubleVector FromArray(double[] values) =>
            new(values);

        public static DoubleVector operator +(DoubleVector vector) =>
            vector;

        public readonly DoubleVector Neg() =>
            new(-x, -y);

        public static DoubleVector Neg(DoubleVector vector) =>
            vector.Neg();

        public static DoubleVector operator -(DoubleVector vector) =>
            vector.Neg();

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

        public static DoubleVector operator -(DoubleVector left, DoubleVector right) =>
            left.Sub(right);

        public readonly DoubleVector Mul(double scalar) =>
            new(x * scalar, y * scalar);

        public static DoubleVector Mul(DoubleVector vector, double scalar) =>
            vector.Mul(scalar);

        public static DoubleVector operator *(DoubleVector vector, double scalar) =>
            vector.Mul(scalar);

        public static DoubleVector operator *(double scalar, DoubleVector vector) =>
            vector.Mul(scalar);

        public readonly DoubleVector Mul(DoubleMatrix m) =>
            new(x * m.m11 + y * m.m21 + m.m31,
                x * m.m12 + y * m.m22 + m.m32);

        public static DoubleVector Mul(DoubleVector vector, DoubleMatrix matrix) =>
            vector.Mul(matrix);

        public static DoubleVector operator *(DoubleVector vector, DoubleMatrix matrix) =>
            vector.Mul(matrix);

        public readonly DoubleVector Div(double scalar) =>
            new(x / scalar, y / scalar);

        public static DoubleVector Div(DoubleVector vector, double scalar) =>
            vector.Div(scalar);

        public static DoubleVector operator /(DoubleVector vector, double scalar) =>
            vector.Div(scalar);

        public readonly double Dot(DoubleVector other) =>
            x * other.x + y * other.y;

        public static double Dot(DoubleVector left, DoubleVector right) =>
            left.Dot(right);

        public static DoubleVector Min(DoubleVector left, DoubleVector right) =>
            new(Math.Min(left.x, right.x), Math.Min(left.y, right.y));

        public static DoubleVector Max(DoubleVector left, DoubleVector right) =>
            new(Math.Max(left.x, right.x), Math.Max(left.y, right.y));

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
