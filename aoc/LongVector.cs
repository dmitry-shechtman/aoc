using System;

namespace aoc
{
    using Helper = Internal.Vector2DHelper<LongVector, long>;

    public struct LongVector : IVector2D<LongVector, long>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromArray, long.TryParse, -1, 0, 1));

        private static Helper Helper => _helper.Value;

        public static readonly LongVector Zero      = default;

        public static readonly LongVector North     = ( 0, -1);
        public static readonly LongVector East      = ( 1,  0);
        public static readonly LongVector South     = ( 0,  1);
        public static readonly LongVector West      = (-1,  0);

        public static LongVector[] Headings => Helper.Headings;

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

        public readonly override string ToString() =>
            Helper.ToString(this);

        public readonly string ToString(IFormatProvider provider) =>
            Helper.ToString(this, provider);

        public readonly string ToString(string format, IFormatProvider provider = null) =>
            Helper.ToString(this, format, provider);

        public readonly void Deconstruct(out long x, out long y)
        {
            x = this.x;
            y = this.y;
        }

        public readonly long Abs() =>
            Math.Abs(x) + Math.Abs(y);

        public readonly LongVector Abs2() =>
            new(Math.Abs(x), Math.Abs(y));

        public readonly LongVector Sign() =>
            new(Math.Sign(x), Math.Sign(y));

        public readonly long X => x;
        public readonly long Y => y;

        public static LongVector Parse(char c) =>
            Helper.Parse(c);

        public static bool TryParse(char c, out LongVector vector) =>
            Helper.TryParse(c, out vector);

        public static LongVector Parse(string s) =>
            Helper.Parse(s);

        public static bool TryParse(string s, out LongVector vector) =>
            Helper.TryParse(s, out vector);

        public static LongVector Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out LongVector vector) =>
            Helper.TryParse(s, separator, out vector);

        public static LongVector Parse(string[] ss) =>
            Helper.Parse(ss);

        public static bool TryParse(string[] ss, out LongVector vector) =>
            Helper.TryParse(ss, out vector);

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
