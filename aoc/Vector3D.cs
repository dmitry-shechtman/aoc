using System;
using System.Collections.Generic;
using System.Linq;

using static aoc.ParseHelper;
using static aoc.Vector3DParseHelper<aoc.Vector3D, int>;

namespace aoc
{
    public struct Vector3D : IVector3D<Vector3D, Vector, int>
    {
        public static readonly Vector3D Zero = default;

        public static readonly Vector3D North = ( 0, -1,  0);
        public static readonly Vector3D East  = ( 1,  0,  0);
        public static readonly Vector3D South = ( 0,  1,  0);
        public static readonly Vector3D West  = (-1,  0,  0);
        public static readonly Vector3D Up    = ( 0,  0, -1);
        public static readonly Vector3D Down  = ( 0,  0,  1);

        public static readonly Vector3D[] Headings = { North, East, South, West, Up, Down };

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

        private const string DefaultFormat = "x,y,z";

        private static readonly string[] FormatKeys    = { "x", "y", "z" };
        private static readonly string[] FormatStrings = { "neswud" };

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

        public readonly IEnumerator<int> GetEnumerator()
        {
            yield return x;
            yield return y;
            yield return z;
        }

        public readonly int this[int i] => i switch
        {
            0 => x,
            1 => y,
            2 => z,
            _ => throw new IndexOutOfRangeException(),
        };

        public readonly int Abs() =>
            Math.Abs(x) + Math.Abs(y) + Math.Abs(z);

        public readonly Vector3D Abs2() =>
            new(Math.Abs(x), Math.Abs(y), Math.Abs(z));

        public readonly Vector3D Sign() =>
            new(Math.Sign(x), Math.Sign(y), Math.Sign(z));

        public readonly int X => x;
        public readonly int Y => y;
        public readonly int Z => z;

        public readonly int Length =>
            x * y * z;

        public readonly long LongLength =>
            (long)x * y * z;

        public static IEnumerable<Vector3D> Range(Vector3D toExclusive) =>
            Range(Zero, toExclusive);

        public static IEnumerable<Vector3D> Range(Vector3D fromInclusive, Vector3D toExclusive)
        {
            for (var z = fromInclusive.z; z < toExclusive.z; z++)
                for (var y = fromInclusive.y; y < toExclusive.y; y++)
                    for (var x = fromInclusive.x; x < toExclusive.x; x++)
                        yield return new(x, y, z);
        }

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

        public static Vector3D Parse(string s) =>
            Parse(s, ',');

        public static Vector3D Parse(string s, char separator) =>
            Parse<Vector3D>(s, TryParse, separator);

        public static bool TryParse(string s, out Vector3D vector, char separator = ',') =>
            TryParse<Vector3D>(s, TryParse, separator, out vector);

        public static Vector3D Parse(string[] ss) =>
            Parse<Vector3D>(ss, TryParse);

        public static bool TryParse(string[] ss, out Vector3D vector) =>
            TryParseVector(ss, int.TryParse, FromArray, out vector);

        private static Vector3D FromArray(int[] values) =>
            new(values);

        public readonly T GetValue<T>(T[] array, Vector3DRange range) =>
            array[GetIndex(range)];

        public static T GetValue<T>(Vector3D p, T[] array, Vector3DRange range) =>
            p.GetValue(array, range);

        public readonly T GetValue<T>(T[] array, Vector3D size) =>
            array[GetIndex(size)];

        public static T GetValue<T>(Vector3D p, T[] array, Vector3D size) =>
            p.GetValue(array, size);

        public readonly bool TryGetValue<T>(T[] array, Vector3DRange range, out T value)
        {
            if (!range.IsMatch(this))
            {
                value = default;
                return false;
            }
            value = GetValue(array, range);
            return true;
        }

        public static bool TryGetValue<T>(Vector3D p, T[] array, Vector3DRange range, out T value) =>
            p.TryGetValue(array, range, out value);

        public readonly bool TryGetValue<T>(T[] array, Vector3D size, out T value)
        {
            if (!size.Contains(this))
            {
                value = default;
                return false;
            }
            value = GetValue(array, size);
            return true;
        }

        public static bool TryGetValue<T>(Vector3D p, T[] array, Vector3D size, out T value) =>
            p.TryGetValue(array, size, out value);

        public readonly T SetValue<T>(T[] array, T value, Vector3DRange range) =>
            array[GetIndex(range)] = value;

        public static T SetValue<T>(Vector3D p, T[] array, T value, Vector3DRange range) =>
            p.SetValue(array, value, range);

        public readonly T SetValue<T>(T[] array, T value, Vector3D size) =>
            array[GetIndex(size)] = value;

        public static T SetValue<T>(Vector3D p, T[] array, T value, Vector3D size) =>
            p.SetValue(array, value, size);

        public readonly int GetIndex(Vector3DRange range) =>
            GetIndex(range.Width, range.Height);

        public readonly int GetIndex(Vector3D size) =>
            GetIndex(size.x, size.y);

        private readonly int GetIndex(int width, int height) =>
            x + width * (y + height * z);

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

        public readonly Vector3D Negate() =>
            new(-x, -y, -z);

        public static Vector3D Negate(Vector3D vector) =>
            vector.Negate();

        public static Vector3D operator -(Vector3D vector) =>
            vector.Negate();

        public readonly Vector3D Dot(Vector3D other) =>
            new(x * other.x, y * other.y, z * other.z);

        public static Vector3D Dot(Vector3D left, Vector3D right) =>
            left.Dot(right);

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

        public static Vector3D Min(Vector3D left, Vector3D right) =>
            new(Math.Min(left.x, right.x), Math.Min(left.y, right.y), Math.Min(left.z, right.z));

        public static Vector3D Max(Vector3D left, Vector3D right) =>
            new(Math.Max(left.x, right.x), Math.Max(left.y, right.y), Math.Max(left.z, right.z));

        public readonly bool Contains(Vector3D other) =>
            other.x >= 0 && other.x < x &&
            other.y >= 0 && other.y < y &&
            other.z >= 0 && other.z < z;

        public static bool Contains(Vector3D size, Vector3D vector) =>
            size.Contains(vector);

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
