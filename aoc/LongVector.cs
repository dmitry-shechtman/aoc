using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace aoc
{
    using Helper = Internal.Vector2DHelper<LongVector, long>;

    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public readonly struct LongVector : IVector<LongVector, LongMatrix, long>, IVector2D<LongVector, long>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromSpan, Internal.Int64Helper.Instance));

        internal static Helper Helper => _helper.Value;

        public static readonly LongVector Zero      = default;

        public static readonly LongVector North     = Helper.North;
        public static readonly LongVector East      = Helper.East;
        public static readonly LongVector South     = Helper.South;
        public static readonly LongVector West      = Helper.West;

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

        private string GetDebuggerDisplay() =>
            ToString("(x,y)");

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

        public static LongVector Parse(string s) =>
            Helper.Parse(s);

        public static bool TryParse(string s, out LongVector vector) =>
            Helper.TryParse(s, out vector);

        public static LongVector Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out LongVector vector) =>
            Helper.TryParse(s, separator, out vector);

        public static LongVector Parse(string s, string separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, string separator, out LongVector vector) =>
            Helper.TryParse(s, separator, out vector);

        public static LongVector Parse(string s, Regex separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, Regex separator, out LongVector vector) =>
            Helper.TryParse(s, separator, out vector);

        public static LongVector ParseAny(string input) =>
            Helper.ParseAny(input);

        public static bool TryParseAny(string input, out LongVector vector) =>
            Helper.TryParseAny(input, out vector);

        public static LongVector[] ParseAll(string input) =>
            Helper.ParseAll(input);

        public static bool TryParseAll(string input, out LongVector[] vectors) =>
            Helper.TryParseAll(input, out vectors);

        private static LongVector FromSpan(ReadOnlySpan<long> values) =>
            new(values[0], values[1]);

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

        public readonly LongVector Mul(LongMatrix m) =>
            new(x * m.m11 + y * m.m21 + m.m31,
                x * m.m12 + y * m.m22 + m.m32);

        public static LongVector Mul(LongVector vector, LongMatrix matrix) =>
            vector.Mul(matrix);

        public static LongVector operator *(LongVector vector, LongMatrix matrix) =>
            vector.Mul(matrix);

        public readonly LongVector Div(long scalar) =>
            new(x / scalar, y / scalar);

        public static LongVector Div(LongVector vector, long scalar) =>
            vector.Div(scalar);

        public static LongVector operator /(LongVector vector, long scalar) =>
            vector.Div(scalar);

        public readonly LongVector Mod(LongVector other) =>
            new(x % other.x, y % other.y);

        public static LongVector Mod(LongVector left, LongVector right) =>
            left.Mod(right);

        public static LongVector operator %(LongVector left, LongVector right) =>
            left.Mod(right);

        public readonly long Dot(LongVector other) =>
            x * other.x + y * other.y;

        public static long Dot(LongVector left, LongVector right) =>
            left.Dot(right);

        public readonly LongVector Min(LongVector other) =>
            new(Math.Min(x, other.x), Math.Min(y, other.y));

        public static LongVector Min(LongVector left, LongVector right) =>
            left.Min(right);

        public readonly LongVector Max(LongVector other) =>
            new(Math.Max(x, other.x), Math.Max(y, other.y));

        public static LongVector Max(LongVector left, LongVector right) =>
            left.Max(right);

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
