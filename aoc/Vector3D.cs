using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc
{
    using Helper = Internal.Vector3DHelper<Vector3D, int>;

    public struct Vector3D : IVector3D<Vector3D, Vector, int>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromArray, int.TryParse, -1, 0, 1));

        private static Helper Helper => _helper.Value;

        public static readonly Vector3D Zero = default;

        public static readonly Vector3D North = ( 0, -1,  0);
        public static readonly Vector3D East  = ( 1,  0,  0);
        public static readonly Vector3D South = ( 0,  1,  0);
        public static readonly Vector3D West  = (-1,  0,  0);
        public static readonly Vector3D Up    = ( 0,  0, -1);
        public static readonly Vector3D Down  = ( 0,  0,  1);

        public static Vector3D[] Headings => Helper.Headings;

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

        public static int CountNeighbors(Vector3D p, IEnumerable<Vector3D> points)
        {
            int count = 0;
            for (var z = p.z - 1; z <= p.z + 1; z++)
                for (var y = p.y - 1; y <= p.y + 1; y++)
                    for (var x = p.x - 1; x <= p.x + 1; x++)
                        count += p != (x, y, z) && points.Contains((x, y, z)) ? 1 : 0;
            return count;
        }

        public static int CountNeighborsAndSelf(Vector3D p, IEnumerable<Vector3D> points)
        {
            int count = 0;
            for (var z = p.z - 1; z <= p.z + 1; z++)
                for (var y = p.y - 1; y <= p.y + 1; y++)
                    for (var x = p.x - 1; x <= p.x + 1; x++)
                        count += points.Contains((x, y, z)) ? 1 : 0;
            return count;
        }

        public static IEnumerable<Vector3D> GetNeighbors(Vector3D p)
        {
            for (var z = p.z - 1; z <= p.z + 1; z++)
                for (var y = p.y - 1; y <= p.y + 1; y++)
                    for (var x = p.x - 1; x <= p.x + 1; x++)
                        if (p != (x, y, z))
                            yield return new(x, y, z);
        }

        public static IEnumerable<Vector3D> GetNeighborsAndSelf(Vector3D p)
        {
            for (var z = p.z - 1; z <= p.z + 1; z++)
                for (var y = p.y - 1; y <= p.y + 1; y++)
                    for (var x = p.x - 1; x <= p.x + 1; x++)
                        yield return new(x, y, z);
        }

        public static Vector3D[] GetNeighborsJVN(Vector3D p) => new Vector3D[]
        {
            new(p.x, p.y, p.z - 1),
            new(p.x, p.y - 1, p.z),
            new(p.x + 1, p.y, p.z),
            new(p.x, p.y + 1, p.z),
            new(p.x - 1, p.y, p.z),
            new(p.x, p.y, p.z + 1)
        };

        public static Vector3D[] GetNeighborsJVNAndSelf(Vector3D p) => new Vector3D[]
        {
            new(p.x, p.y, p.z),
            new(p.x, p.y, p.z - 1),
            new(p.x, p.y - 1, p.z),
            new(p.x + 1, p.y, p.z),
            new(p.x, p.y + 1, p.z),
            new(p.x - 1, p.y, p.z),
            new(p.x, p.y, p.z + 1)
        };

        public static HashSet<Vector3D> GetNext(HashSet<Vector3D> pp) =>
            GetNext(pp, GetNeighborsAndSelf, FilterInclusive);

        public static HashSet<Vector3D> GetNext(HashSet<Vector3D> pp, Func<Vector3D, IEnumerable<Vector3D>> getNeighbors, Func<Vector3D, int, HashSet<Vector3D>, bool> filter) =>
            pp.SelectMany(getNeighbors).Distinct().AsParallel()
                .Select(p => (p, c: getNeighbors(p).Count(pp.Contains)))
                .Where(t => filter(t.p, t.c, pp))
                .Select(t => t.p)
                .ToHashSet();

        public static bool Filter(Vector3D p, int count, HashSet<Vector3D> pp) =>
            count == 3 || count == 2 && pp.Contains(p);

        public static bool FilterInclusive(Vector3D p, int count, HashSet<Vector3D> pp) =>
            count == 3 || count == 4 && pp.Contains(p);

        public static int GetHeading(char c) =>
            Helper.GetHeading(c);

        public static bool TryGetHeading(char c, out int heading) =>
            Helper.TryGetHeading(c, out heading);

        public static Vector3D Parse(char c) =>
            Helper.Parse(c);

        public static bool TryParse(char c, out Vector3D vector) =>
            Helper.TryParse(c, out vector);

        public static Vector3D Parse(string s) =>
            Helper.Parse(s);

        public static bool TryParse(string s, out Vector3D vector) =>
            Helper.TryParse(s, out vector);

        public static Vector3D Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out Vector3D vector) =>
            Helper.TryParse(s, separator, out vector);

        public static Vector3D Parse(string[] ss) =>
            Helper.Parse(ss);

        public static bool TryParse(string[] ss, out Vector3D vector) =>
            Helper.TryParse(ss, out vector);

        private static Vector3D FromArray(int[] values) =>
            new(values);

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

        public readonly int Dot(Vector3D other) =>
            x * other.x + y * other.y + z * other.z;

        public static int Dot(Vector3D left, Vector3D right) =>
            left.Dot(right);

        public static Vector3D Min(Vector3D left, Vector3D right) =>
            new(Math.Min(left.x, right.x), Math.Min(left.y, right.y), Math.Min(left.z, right.z));

        public static Vector3D Max(Vector3D left, Vector3D right) =>
            new(Math.Max(left.x, right.x), Math.Max(left.y, right.y), Math.Max(left.z, right.z));

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
