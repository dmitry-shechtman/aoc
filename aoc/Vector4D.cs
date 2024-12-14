using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace aoc
{
    using Helper = Internal.Vector4DHelper<Vector4D, int>;

    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public readonly struct Vector4D : IVector4D<Vector4D, Vector3D, Vector, int>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromArray, int.TryParse, -1, 0, 1));

        internal static Helper Helper => _helper.Value;

        public static readonly Vector4D Zero = default;

        public static readonly Vector4D North = Helper.North;
        public static readonly Vector4D East  = Helper.East;
        public static readonly Vector4D South = Helper.South;
        public static readonly Vector4D West  = Helper.West;
        public static readonly Vector4D Up    = Helper.Up;
        public static readonly Vector4D Down  = Helper.Down;
        public static readonly Vector4D Ana   = Helper.Ana;
        public static readonly Vector4D Kata  = Helper.Kata;

        public readonly int x;
        public readonly int y;
        public readonly int z;
        public readonly int w;

        public Vector4D(int x, int y, int z, int w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector4D(Vector v, int z = 0, int w = 0)
            : this(v.x, v.y, z, w)
        {
        }

        public Vector4D(Vector3D v, int w = 0)
            : this(v.x, v.y, v.z, w)
        {
        }

        public Vector4D(int[] values)
            : this(values[0], values[1], values[2], values[3])
        {
        }

        public readonly override bool Equals(object obj) =>
            obj is Vector4D other && Equals(other);

        public readonly bool Equals(Vector4D other) =>
            x == other.x &&
            y == other.y &&
            z == other.z &&
            w == other.w;

        public readonly override int GetHashCode() =>
            HashCode.Combine(x, y, z, w);

        public readonly override string ToString() =>
            Helper.ToString(this);

        public readonly string ToString(IFormatProvider provider) =>
            Helper.ToString(this, provider);

        public readonly string ToString(string format, IFormatProvider provider = null) =>
            Helper.ToString(this, format, provider);

        private string GetDebuggerDisplay() =>
            ToString("(x,y,z,w)");

        public readonly void Deconstruct(out int x, out int y, out int z, out int w)
        {
            x = this.x;
            y = this.y;
            z = this.z;
            w = this.w;
        }

        public readonly void Deconstruct(out Vector vector, out int z, out int w)
        {
            vector = new(x, y);
            z = this.z;
            w = this.w;
        }

        public readonly void Deconstruct(out Vector3D vector, out int w)
        {
            vector = new(x, y, z);
            w = this.w;
        }

        public readonly int Abs() =>
            Math.Abs(x) + Math.Abs(y) + Math.Abs(z) + Math.Abs(w);

        public readonly Vector4D Abs2() =>
            new(Math.Abs(x), Math.Abs(y), Math.Abs(z), Math.Abs(w));

        public readonly Vector4D Sign() =>
            new(Math.Sign(x), Math.Sign(y), Math.Sign(z), Math.Sign(w));

        public readonly int X => x;
        public readonly int Y => y;
        public readonly int Z => z;
        public readonly int W => w;

        public static Vector4D Parse(string s) =>
            Helper.Parse(s);

        public static bool TryParse(string s, out Vector4D vector) =>
            Helper.TryParse(s, out vector);

        public static Vector4D Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out Vector4D vector) =>
            Helper.TryParse(s, separator, out vector);

        public static Vector4D Parse(string s, string separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, string separator, out Vector4D vector) =>
            Helper.TryParse(s, separator, out vector);

        public static Vector4D Parse(string s, Regex separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, Regex separator, out Vector4D vector) =>
            Helper.TryParse(s, separator, out vector);

        public static Vector4D Parse(string[] ss) =>
            Helper.Parse(ss);

        public static bool TryParse(string[] ss, out Vector4D vector) =>
            Helper.TryParse(ss, out vector);

        private static Vector4D FromArray(int[] values) =>
            new(values);

        public static Vector4D operator +(Vector4D vector) =>
            vector;

        public readonly Vector4D Neg() =>
            new(-x, -y, -z, -w);

        public static Vector4D Neg(Vector4D vector) =>
            vector.Neg();

        public static Vector4D operator -(Vector4D vector) =>
            vector.Neg();

        public readonly Vector4D Add(Vector4D other) =>
            new(x + other.x, y + other.y, z + other.z, w + other.w);

        public static Vector4D Add(Vector4D left, Vector4D right) =>
            left.Add(right);

        public static Vector4D operator +(Vector4D left, Vector4D right) =>
            left.Add(right);

        public readonly Vector4D Sub(Vector4D other) =>
            new(x - other.x, y - other.y, z - other.z, w - other.w);

        public static Vector4D Sub(Vector4D left, Vector4D right) =>
            left.Sub(right);

        public static Vector4D operator -(Vector4D left, Vector4D right) =>
            left.Sub(right);

        public readonly Vector4D Mul(int scalar) =>
            new(x * scalar, y * scalar, z * scalar, w * scalar);

        public static Vector4D Mul(Vector4D vector, int scalar) =>
            vector.Mul(scalar);

        public static Vector4D operator *(Vector4D vector, int scalar) =>
            vector.Mul(scalar);

        public static Vector4D operator *(int scalar, Vector4D vector) =>
            vector.Mul(scalar);

        public readonly Vector4D Div(int scalar) =>
            new(x / scalar, y / scalar, z / scalar, w / scalar);

        public static Vector4D Div(Vector4D vector, int scalar) =>
            vector.Div(scalar);

        public static Vector4D operator /(Vector4D vector, int scalar) =>
            vector.Div(scalar);

        public readonly Vector4D Mod(Vector4D other) =>
            new(x % other.x, y % other.y, z % other.z, w % other.w);

        public static Vector4D Mod(Vector4D left, Vector4D right) =>
            left.Mod(right);

        public static Vector4D operator %(Vector4D left, Vector4D right) =>
            left.Mod(right);

        public readonly int Dot(Vector4D other) =>
            x * other.x + y * other.y + z * other.z + w * other.w;

        public static int Dot(Vector4D left, Vector4D right) =>
            left.Dot(right);

        public readonly Vector4D Min(Vector4D other) =>
            new(Math.Min(x, other.x), Math.Min(y, other.y), Math.Min(z, other.z), Math.Min(w, other.w));

        public static Vector4D Min(Vector4D left, Vector4D right) =>
            left.Min(right);

        public readonly Vector4D Max(Vector4D other) =>
            new(Math.Max(x, other.x), Math.Max(y, other.y), Math.Max(z, other.z), Math.Max(z, other.z));

        public static Vector4D Max(Vector4D left, Vector4D right) =>
            left.Max(right);

        public static implicit operator (int x, int y, int z, int w)(Vector4D value) =>
            (value.x, value.y, value.z, value.w);

        public static implicit operator Vector4D((int x, int y, int z, int w) value) =>
            new(value.x, value.y, value.z, value.w);

        public static implicit operator Vector4D((Vector v, int z, int w) value) =>
            new(value.v, value.z, value.w);

        public static implicit operator Vector4D((Vector3D v, int w) value) =>
            new(value.v, value.w);

        public static implicit operator Vector4D(int[] values) =>
            new(values);

        public static explicit operator Vector(Vector4D value) =>
            new(value.x, value.y);

        public static explicit operator Vector3D(Vector4D value) =>
            new(value.x, value.y, value.z);

        public static bool operator ==(Vector4D left, Vector4D right) =>
            left.Equals(right);

        public static bool operator !=(Vector4D left, Vector4D right) =>
            !left.Equals(right);
    }
}
