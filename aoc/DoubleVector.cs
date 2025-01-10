using System;
using System.Diagnostics;

namespace aoc
{
    using Helper = Internal.Vector2DHelper<DoubleVector, double>;

    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public readonly struct DoubleVector : IVector<DoubleVector, DoubleMatrix, double>, IVector2D<DoubleVector, double>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromSpan, Internal.DoubleHelper.Instance));

        internal static Helper Helper => _helper.Value;

        public static DoubleVector NegativeOne => Helper.NegativeOne;
        public static DoubleVector Zero        => default;
        public static DoubleVector One         => Helper.One;

        public static DoubleVector North       => Helper.North;
        public static DoubleVector East        => Helper.East;
        public static DoubleVector South       => Helper.South;
        public static DoubleVector West        => Helper.West;

        public static DoubleVector AdditiveIdentity       => Zero;
        public static DoubleVector MultiplicativeIdentity => One;

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

        public readonly override bool Equals(object? obj) =>
            obj is DoubleVector other && Equals(other);

        public readonly bool Equals(DoubleVector other) =>
            x == other.x &&
            y == other.y;

        public readonly override int GetHashCode() =>
            HashCode.Combine(x, y);

        public readonly override string ToString() =>
            Helper.ToString(this);

        public readonly string ToString(IFormatProvider? provider) =>
            Helper.ToString(this, provider);

        public readonly string ToString(string? format, IFormatProvider? provider = null) =>
            Helper.ToString(this, format, provider);

        private string GetDebuggerDisplay() =>
            ToString("(x,y)");

        public readonly void Deconstruct(out double x, out double y)
        {
            x = this.x;
            y = this.y;
        }

        public readonly double Abs() =>
            Math.Abs(x) + Math.Abs(y);

        public readonly DoubleVector Abs2() =>
            new(Math.Abs(x), Math.Abs(y));

        public readonly DoubleVector Sign() =>
            new(Math.Sign(x), Math.Sign(y));

        public readonly double X => x;
        public readonly double Y => y;

        public static Builders.IBuilder<DoubleVector> Builder => Helper;

        public static DoubleVector Parse(string? s) =>
            Helper.Parse(s);

        public static bool TryParse(string? s, out DoubleVector vector) =>
            Helper.TryParse(s, out vector);

        public static DoubleVector Parse(string? s, IFormatProvider? provider) =>
            Helper.Parse(s, provider);

        public static bool TryParse(string? s, IFormatProvider? provider, out DoubleVector vector) =>
            Helper.TryParse(s, provider, out vector);

        public static DoubleVector Parse(ReadOnlySpan<char> s) =>
            Helper.Parse(s);

        public static bool TryParse(ReadOnlySpan<char> s, out DoubleVector vector) =>
            Helper.TryParse(s, out vector);

        public static DoubleVector Parse(ReadOnlySpan<char> s, IFormatProvider? provider) =>
            Helper.Parse(s, provider);

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out DoubleVector vector) =>
            Helper.TryParse(s, provider, out vector);

        private static DoubleVector FromSpan(ReadOnlySpan<double> values) =>
            new(values[0], values[1]);

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

        public readonly DoubleVector Min(DoubleVector other) =>
            new(Math.Min(x, other.x), Math.Min(y, other.y));

        public static DoubleVector Min(DoubleVector left, DoubleVector right) =>
            left.Min(right);

        public readonly DoubleVector Max(DoubleVector other) =>
            new(Math.Max(x, other.x), Math.Max(y, other.y));

        public static DoubleVector Max(DoubleVector left, DoubleVector right) =>
            left.Max(right);

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
