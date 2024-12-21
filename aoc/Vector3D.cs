using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace aoc
{
    using Helper = Internal.Vector3DHelper<Vector3D, int>;

    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public readonly struct Vector3D : IVector<Vector3D, Matrix3D, int>, IVector3D<Vector3D, Vector, int>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromSpan, int.TryParse, -1, 0, 1, @"[-+]?\d+"));

        internal static Helper Helper => _helper.Value;

        public static readonly Vector3D Zero = default;

        public static readonly Vector3D North = Helper.North;
        public static readonly Vector3D East  = Helper.East;
        public static readonly Vector3D South = Helper.South;
        public static readonly Vector3D West  = Helper.West;
        public static readonly Vector3D Up    = Helper.Up;
        public static readonly Vector3D Down  = Helper.Down;

        public readonly int x;
        public readonly int y;
        public readonly int z;

        public Vector3D(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3D(Vector v, int z = 0)
            : this(v.x, v.y, z)
        {
        }

        public Vector3D(int[] values)
            : this(values[0], values[1], values[2])
        {
        }

        public readonly override bool Equals(object obj) =>
            obj is Vector3D other && Equals(other);

        public readonly bool Equals(Vector3D other) =>
            x == other.x &&
            y == other.y &&
            z == other.z;

        public readonly override int GetHashCode() =>
            HashCode.Combine(x, y, z);

        public readonly override string ToString() =>
            Helper.ToString(this);

        public readonly string ToString(IFormatProvider provider) =>
            Helper.ToString(this, provider);

        public readonly string ToString(string format, IFormatProvider provider = null) =>
            Helper.ToString(this, format, provider);

        private string GetDebuggerDisplay() =>
            ToString("(x,y,z)");

        public readonly void Deconstruct(out int x, out int y, out int z)
        {
            x = this.x;
            y = this.y;
            z = this.z;
        }

        public readonly void Deconstruct(out Vector vector, out int z)
        {
            vector = new(x, y);
            z = this.z;
        }

        public readonly int Abs() =>
            Math.Abs(x) + Math.Abs(y) + Math.Abs(z);

        public readonly Vector3D Abs2() =>
            new(Math.Abs(x), Math.Abs(y), Math.Abs(z));

        public readonly Vector3D Sign() =>
            new(Math.Sign(x), Math.Sign(y), Math.Sign(z));

        public readonly int X => x;
        public readonly int Y => y;
        public readonly int Z => z;

        public static Vector3D Parse(string s) =>
            Helper.Parse(s);

        public static bool TryParse(string s, out Vector3D vector) =>
            Helper.TryParse(s, out vector);

        public static Vector3D Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out Vector3D vector) =>
            Helper.TryParse(s, separator, out vector);

        public static Vector3D Parse(string s, string separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, string separator, out Vector3D vector) =>
            Helper.TryParse(s, separator, out vector);

        public static Vector3D Parse(string s, Regex separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, Regex separator, out Vector3D vector) =>
            Helper.TryParse(s, separator, out vector);

        public static Vector3D ParseAny(string input) =>
            Helper.ParseAny(input);

        public static bool TryParseAny(string input, out Vector3D vector) =>
            Helper.TryParseAny(input, out vector);

        private static Vector3D FromSpan(ReadOnlySpan<int> values) =>
            new(values[0], values[1], values[2]);

        public static Vector3D operator +(Vector3D vector) =>
            vector;

        public readonly Vector3D Neg() =>
            new(-x, -y, -z);

        public static Vector3D Neg(Vector3D vector) =>
            vector.Neg();

        public static Vector3D operator -(Vector3D vector) =>
            vector.Neg();

        public readonly Vector3D Add(Vector3D other) =>
            new(x + other.x, y + other.y, z + other.z);

        public static Vector3D Add(Vector3D left, Vector3D right) =>
            left.Add(right);

        public static Vector3D operator +(Vector3D left, Vector3D right) =>
            left.Add(right);

        public readonly Vector3D Sub(Vector3D other) =>
            new(x - other.x, y - other.y, z - other.z);

        public static Vector3D Sub(Vector3D left, Vector3D right) =>
            left.Sub(right);

        public static Vector3D operator -(Vector3D left, Vector3D right) =>
            left.Sub(right);

        public readonly Vector3D Mul(int scalar) =>
            new(x * scalar, y * scalar, z * scalar);

        public static Vector3D Mul(Vector3D vector, int scalar) =>
            vector.Mul(scalar);

        public static Vector3D operator *(Vector3D vector, int scalar) =>
            vector.Mul(scalar);

        public static Vector3D operator *(int scalar, Vector3D vector) =>
            vector.Mul(scalar);

        public readonly Vector3D Mul(Matrix3D m) =>
            new(x * m.m11 + y * m.m21 + z * m.m31 + m.m41,
                x * m.m12 + y * m.m22 + z * m.m32 + m.m42,
                x * m.m13 + y * m.m23 + z * m.m33 + m.m43);

        public static Vector3D Mul(Vector3D vector, Matrix3D matrix) =>
            vector.Mul(matrix);

        public static Vector3D operator *(Vector3D vector, Matrix3D matrix) =>
            vector.Mul(matrix);

        public readonly Vector3D Div(int scalar) =>
            new(x / scalar, y / scalar, z / scalar);

        public static Vector3D Div(Vector3D vector, int scalar) =>
            vector.Div(scalar);

        public static Vector3D operator /(Vector3D vector, int scalar) =>
            vector.Div(scalar);

        public readonly Vector3D Mod(Vector3D other) =>
            new(x % other.x, y % other.y, z % other.z);

        public static Vector3D Mod(Vector3D left, Vector3D right) =>
            left.Mod(right);

        public static Vector3D operator %(Vector3D left, Vector3D right) =>
            left.Mod(right);

        public readonly int Dot(Vector3D other) =>
            x * other.x + y * other.y + z * other.z;

        public static int Dot(Vector3D left, Vector3D right) =>
            left.Dot(right);

        public readonly Vector3D Min(Vector3D other) =>
            new(Math.Min(x, other.x), Math.Min(y, other.y), Math.Min(z, other.z));

        public static Vector3D Min(Vector3D left, Vector3D right) =>
            left.Min(right);

        public readonly Vector3D Max(Vector3D other) =>
            new(Math.Max(x, other.x), Math.Max(y, other.y), Math.Max(z, other.z));

        public static Vector3D Max(Vector3D left, Vector3D right) =>
            left.Max(right);

        public static implicit operator (int x, int y, int z)(Vector3D value) =>
            (value.x, value.y, value.z);

        public static implicit operator Vector3D((int x, int y, int z) value) =>
            new(value.x, value.y, value.z);

        public static implicit operator Vector3D((Vector vector, int z) value) =>
            new(value.vector, value.z);

        public static implicit operator Vector3D(int[] values) =>
            new(values);

        public static explicit operator Vector(Vector3D value) =>
            new(value.x, value.y);

        public static bool operator ==(Vector3D left, Vector3D right) =>
            left.Equals(right);

        public static bool operator !=(Vector3D left, Vector3D right) =>
            !left.Equals(right);
    }
}
