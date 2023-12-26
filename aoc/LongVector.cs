using System;
using System.Collections.Generic;
using System.Linq;

using static aoc.ParseHelper;
using static aoc.Vector2DParseHelper<aoc.LongVector, long>;

namespace aoc
{
    public struct LongVector : IVector2D<LongVector, long>
    {
        public static readonly LongVector Zero      = default;

        public static readonly LongVector North     = ( 0, -1);
        public static readonly LongVector East      = ( 1,  0);
        public static readonly LongVector South     = ( 0,  1);
        public static readonly LongVector West      = (-1,  0);

        public static readonly LongVector[] Headings = { North, East, South, West };

        public readonly long x;
        public readonly long y;

        public LongVector(long x, long y)
        {
            this.x = x;
            this.y = y;
        }

        public LongVector(Vector v)
            : this(v.x, v.y)
        {
        }

        public LongVector(long[] values)
            : this(values[0], values[1])
        {
        }

        public readonly override bool Equals(object obj) =>
            obj is LongVector other && Equals(other);

        public readonly bool Equals(LongVector other) =>
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

        public readonly void Deconstruct(out long x, out long y)
        {
            x = this.x;
            y = this.y;
        }

        public readonly IEnumerator<long> GetEnumerator()
        {
            yield return x;
            yield return y;
        }

        public readonly long this[int i] => i switch
        {
            0 => x,
            1 => y,
            _ => throw new IndexOutOfRangeException(),
        };

        public readonly long Abs() =>
            Math.Abs(x) + Math.Abs(y);

        public readonly LongVector Abs2() =>
            new(Math.Abs(x), Math.Abs(y));

        public readonly LongVector Sign() =>
            new(Math.Sign(x), Math.Sign(y));

        public readonly long X => x;
        public readonly long Y => y;

        public readonly long Length =>
            x * y;

        public static LongVector Parse(string s) =>
            Parse(s, ',');

        public static LongVector Parse(string s, char separator) =>
            Parse<LongVector>(s, TryParse, separator);

        public static bool TryParse(string s, out LongVector vector, char separator = ',') =>
            TryParse<LongVector>(s, TryParse, separator, out vector);

        public static LongVector Parse(string[] ss) =>
            Parse<LongVector>(ss, TryParse);

        public static bool TryParse(string[] ss, out LongVector vector) =>
            TryParseVector(ss, long.TryParse, FromArray, out vector);

        private static LongVector FromArray(long[] values) =>
            new(values);

        public static LongVector operator +(LongVector vector) =>
            vector;

        public readonly LongVector Neg() =>
            new(-x, -y);

        public static LongVector Neg(LongVector vector) =>
            vector.Neg();

        public static LongVector operator -(LongVector vector) =>
            vector.Neg();

        public readonly LongVector Add(LongVector other) =>
            new(x + other.x, y + other.y);

        public static LongVector Add(LongVector left, LongVector right) =>
            left.Add(right);

        public static LongVector operator +(LongVector left, LongVector right) =>
            left.Add(right);

        public readonly LongVector Sub(LongVector other) =>
            new(x - other.x, y - other.y);

        public static LongVector Sub(LongVector left, LongVector right) =>
            left.Sub(right);

        public static LongVector operator -(LongVector left, LongVector right) =>
            left.Sub(right);

        public readonly LongVector Mul(long scalar) =>
            new(x * scalar, y * scalar);

        public static LongVector Mul(LongVector vector, long scalar) =>
            vector.Mul(scalar);

        public static LongVector operator *(LongVector vector, long scalar) =>
            vector.Mul(scalar);

        public static LongVector operator *(long scalar, LongVector vector) =>
            vector.Mul(scalar);

        public readonly LongVector Div(long scalar) =>
            new(x / scalar, y / scalar);

        public static LongVector Div(LongVector vector, long scalar) =>
            vector.Div(scalar);

        public static LongVector operator /(LongVector vector, long scalar) =>
            vector.Div(scalar);

        public readonly long Dot(LongVector other) =>
            x * other.x + y * other.y;

        public static long Dot(LongVector left, LongVector right) =>
            left.Dot(right);

        public static LongVector Min(LongVector left, LongVector right) =>
            new(Math.Min(left.x, right.x), Math.Min(left.y, right.y));

        public static LongVector Max(LongVector left, LongVector right) =>
            new(Math.Max(left.x, right.x), Math.Max(left.y, right.y));

        public static implicit operator (long x, long y)(LongVector value) =>
            (value.x, value.y);

        public static implicit operator LongVector((long x, long y) value) =>
            new(value.x, value.y);

        public static implicit operator LongVector(long[] values) =>
            new(values);

        public static implicit operator LongVector(Vector value) =>
            new(value);

        public static explicit operator Vector(LongVector value) =>
            new((int)value.x, (int)value.y);

        public static bool operator ==(LongVector left, LongVector right) =>
            left.Equals(right);

        public static bool operator !=(LongVector left, LongVector right) =>
            !left.Equals(right);
    }
}
