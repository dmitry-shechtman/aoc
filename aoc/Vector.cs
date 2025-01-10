using System;
using System.Diagnostics;

namespace aoc
{
    using Helper = Internal.Vector2DHelper<Vector, int>;

    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public readonly struct Vector : IIntegerVector<Vector, Matrix, int>, IVector2D<Vector, int>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromSpan, Internal.Int32Helper.Instance));

        internal static Helper Helper => _helper.Value;

        public static Vector NegativeOne       => Helper.NegativeOne;
        public static Vector Zero              => default;
        public static Vector One               => Helper.One;

        public static Vector North             => Helper.North;
        public static Vector East              => Helper.East;
        public static Vector South             => Helper.South;
        public static Vector West              => Helper.West;

        public static Vector North2    { get; } = ( 0, -2);
        public static Vector East2     { get; } = ( 2,  0);
        public static Vector South2    { get; } = ( 0,  2);
        public static Vector West2     { get; } = (-2,  0);
        public static Vector NorthWest { get; } = (-1, -1);
        public static Vector NorthEast { get; } = ( 1, -1);
        public static Vector SouthWest { get; } = (-1,  1);
        public static Vector SouthEast { get; } = ( 1,  1);

        public static Vector AdditiveIdentity       => Zero;
        public static Vector MultiplicativeIdentity => One;

        public readonly int x;
        public readonly int y;

        public Vector(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector(int[] values)
            : this(values[0], values[1])
        {
        }

        public readonly override bool Equals(object? obj) =>
            obj is Vector other && Equals(other);

        public readonly bool Equals(Vector other) =>
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

        public readonly void Deconstruct(out int x, out int y)
        {
            x = this.x;
            y = this.y;
        }

        public readonly int Abs() =>
            Math.Abs(x) + Math.Abs(y);

        public readonly Vector Abs2() =>
            new(Math.Abs(x), Math.Abs(y));

        public readonly Vector Sign() =>
            new(Math.Sign(x), Math.Sign(y));

        public readonly int X => x;
        public readonly int Y => y;

        public static Builders.IBuilder<Vector> Builder => Helper;

        public static Vector Parse(string? s) =>
            Helper.Parse(s);

        public static bool TryParse(string? s, out Vector vector) =>
            Helper.TryParse(s, out vector);

        public static Vector Parse(string? s, IFormatProvider? provider) =>
            Helper.Parse(s, provider);

        public static bool TryParse(string? s, IFormatProvider? provider, out Vector vector) =>
            Helper.TryParse(s, provider, out vector);

        public static Vector Parse(ReadOnlySpan<char> s) =>
            Helper.Parse(s);

        public static bool TryParse(ReadOnlySpan<char> s, out Vector vector) =>
            Helper.TryParse(s, out vector);

        public static Vector Parse(ReadOnlySpan<char> s, IFormatProvider? provider) =>
            Helper.Parse(s, provider);

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Vector vector) =>
            Helper.TryParse(s, provider, out vector);

        private static Vector FromSpan(ReadOnlySpan<int> values) =>
            new(values[0], values[1]);

        public static Vector operator +(Vector vector) =>
            vector;

        public readonly Vector Neg() =>
            new(-x, -y);

        public static Vector Neg(Vector vector) =>
            vector.Neg();

        public static Vector operator -(Vector vector) =>
            vector.Neg();

        public readonly Vector Add(Vector other) =>
            new(x + other.x, y + other.y);

        public static Vector Add(Vector left, Vector right) =>
            left.Add(right);

        public static Vector operator +(Vector left, Vector right) =>
            left.Add(right);

        public readonly Vector Sub(Vector other) =>
            new(x - other.x, y - other.y);

        public static Vector Sub(Vector left, Vector right) =>
            left.Sub(right);

        public static Vector operator -(Vector left, Vector right) =>
            left.Sub(right);

        public readonly Vector Mul(int scalar) =>
            new(x * scalar, y * scalar);

        public static Vector Mul(Vector vector, int scalar) =>
            vector.Mul(scalar);

        public static Vector operator *(Vector vector, int scalar) =>
            vector.Mul(scalar);

        public static Vector operator *(int scalar, Vector vector) =>
            vector.Mul(scalar);

        public readonly Vector Mul(Matrix m) =>
            new(x * m.m11 + y * m.m21 + m.m31,
                x * m.m12 + y * m.m22 + m.m32);

        public static Vector Mul(Vector vector, Matrix matrix) =>
            vector.Mul(matrix);

        public static Vector operator *(Vector vector, Matrix matrix) =>
            vector.Mul(matrix);

        public readonly Vector Div(int scalar) =>
            new(x / scalar, y / scalar);

        public static Vector Div(Vector vector, int scalar) =>
            vector.Div(scalar);

        public static Vector operator /(Vector vector, int scalar) =>
            vector.Div(scalar);

        public readonly Vector Mod(Vector other) =>
            new(x % other.x, y % other.y);

        public static Vector Mod(Vector left, Vector right) =>
            left.Mod(right);

        public static Vector operator %(Vector left, Vector right) =>
            left.Mod(right);

        public readonly int Dot(Vector other) =>
            x * other.x + y * other.y;

        public static int Dot(Vector left, Vector right) =>
            left.Dot(right);

        public readonly Vector Min(Vector other) =>
            new(Math.Min(x, other.x), Math.Min(y, other.y));

        public static Vector Min(Vector left, Vector right) =>
            left.Min(right);

        public readonly Vector Max(Vector other) =>
            new(Math.Max(x, other.x), Math.Max(y, other.y));

        public static Vector Max(Vector left, Vector right) =>
            left.Max(right);

        public static implicit operator (int x, int y)(Vector value) =>
            (value.x, value.y);

        public static implicit operator Vector((int x, int y) value) =>
            new(value.x, value.y);

        public static implicit operator Vector(int[] values) =>
            new(values);

        public static bool operator ==(Vector left, Vector right) =>
            left.Equals(right);

        public static bool operator !=(Vector left, Vector right) =>
            !left.Equals(right);
    }
}
