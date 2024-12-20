using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace aoc
{
    using Helper = Internal.Vector2DHelper<Vector, int>;

    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public readonly struct Vector : IVector<Vector, Matrix, int>, IVector2D<Vector, int>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromSpan, int.TryParse, -1, 0, 1));

        internal static Helper Helper => _helper.Value;

        public static readonly Vector Zero      = default;

        public static readonly Vector North     = Helper.North;
        public static readonly Vector East      = Helper.East;
        public static readonly Vector South     = Helper.South;
        public static readonly Vector West      = Helper.West;

        public static readonly Vector North2    = ( 0, -2);
        public static readonly Vector East2     = ( 2,  0);
        public static readonly Vector South2    = ( 0,  2);
        public static readonly Vector West2     = (-2,  0);

        public static readonly Vector NorthWest = (-1, -1);
        public static readonly Vector NorthEast = ( 1, -1);
        public static readonly Vector SouthWest = (-1,  1);
        public static readonly Vector SouthEast = ( 1,  1);

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

        public readonly override bool Equals(object obj) =>
            obj is Vector other && Equals(other);

        public readonly bool Equals(Vector other) =>
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

        public static Vector Parse(string s) =>
            Helper.Parse(s);

        public static bool TryParse(string s, out Vector vector) =>
            Helper.TryParse(s, out vector);

        public static Vector Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out Vector vector) =>
            Helper.TryParse(s, separator, out vector);

        public static Vector Parse(string s, string separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, string separator, out Vector vector) =>
            Helper.TryParse(s, separator, out vector);

        public static Vector Parse(string s, Regex separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, Regex separator, out Vector vector) =>
            Helper.TryParse(s, separator, out vector);

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
