using System;
using System.Collections.Generic;

namespace aoc
{
    public struct Vector3DRange : IIntegerRange<Vector3DRange, Vector3D>, IRange3D<Vector3DRange, Vector3D, int>
    {
        private static readonly Lazy<VectorRangeHelper<Vector3DRange, Vector3D>> _helper =
            new(() => new(FromArray, Vector3D.TryParse));

        private static VectorRangeHelper<Vector3DRange, Vector3D> Helper => _helper.Value;

        public Vector3DRange(Vector3D min, Vector3D max)
        {
            Min = min;
            Max = max;
        }

        public Vector3DRange(Vector3D max)
            : this(Vector3D.Zero, max)
        {
        }

        public Vector3DRange(Size3D size)
            : this((Vector3D)size - (1, 1, 1))
        {
        }

        public Vector3DRange(int x, int y, int z)
            : this(new Vector3D(x, y, z))
        {
        }

        public Vector3D Min { get; }
        public Vector3D Max { get; }

        public readonly int Width  => Max.x - Min.x + 1;
        public readonly int Height => Max.y - Min.y + 1;
        public readonly int Depth  => Max.z - Min.z + 1;
        public readonly int Count  => Width * Height * Depth;

        public readonly long LongCount =>
            (long)Width * Height * Depth;

        public readonly override bool Equals(object obj) =>
            obj is Vector3DRange other && Equals(other);

        public readonly bool Equals(Vector3DRange other) =>
            Min.Equals(other.Min) &&
            Max.Equals(other.Max);

        public readonly override int GetHashCode() =>
            HashCode.Combine(Min, Max);

        public readonly override string ToString() =>
            Helper.ToString(this);

        public readonly string ToString(IFormatProvider provider) =>
            Helper.ToString(this, provider);

        public readonly string ToString(string format, IFormatProvider provider = null) =>
            Helper.ToString(this, format, provider);

        public readonly void Deconstruct(out Vector3D min, out Vector3D max)
        {
            min = Min;
            max = Max;
        }

        public readonly IEnumerator<Vector3D> GetEnumerator()
        {
            for (var z = Min.z; z <= Max.z; z++)
                for (var y = Min.y; y <= Max.y; y++)
                    for (var x = Min.x; x <= Max.x; x++)
                        yield return new(x, y, z);
        }

        public readonly Vector3D this[int index] =>
            new(Min.x + index % Width, Min.y + index / Width % Height, Min.z + index / (Width * Height));

        public static Vector3DRange Parse(string s) =>
            Parse(s, '~');

        public static Vector3DRange Parse(string s, char separator, char separator2 = ',') =>
            Helper.Parse(s, separator, separator2);

        public static bool TryParse(string s, out Vector3DRange range, char separator = '~', char separator2 = ',') =>
            Helper.TryParse(s, out range, separator, separator2);

        public static Vector3DRange Parse(string[] ss) =>
            Parse(ss, ',');

        public static Vector3DRange Parse(string[] ss, char separator) =>
            Helper.Parse(ss, separator);

        public static bool TryParse(string[] ss, out Vector3DRange range, char separator = ',') =>
            Helper.TryParse(ss, out range, separator);

        private static Vector3DRange FromArray(Vector3D[] values) =>
            new(values[0], values[1]);

        public readonly bool IsMatch(Vector3D vector) =>
            vector.x >= Min.x && vector.x <= Max.x &&
            vector.y >= Min.y && vector.y <= Max.y &&
            vector.z >= Min.z && vector.z <= Max.z;

        public readonly bool IsMatch(Vector3DRange other) =>
            other.Min.x <= Max.x && other.Max.x >= Min.x &&
            other.Min.y <= Max.y && other.Max.y >= Min.y &&
            other.Min.z <= Max.z && other.Max.z >= Min.z;

        public readonly bool Contains(Vector3D vector) =>
            IsMatch(vector);

        public readonly bool Contains(Vector3DRange other) =>
            other.Min.x >= Min.x && other.Max.x <= Max.x &&
            other.Min.y >= Min.y && other.Max.y <= Max.y &&
            other.Min.z >= Min.z && other.Max.z <= Max.z;

        public readonly Vector3DRange Union(Vector3DRange other) =>
            new(min: Vector3D.Min(Min, other.Min), max: Vector3D.Max(Max, other.Max));

        public static Vector3DRange Union(Vector3DRange left, Vector3DRange right) =>
            left.Union(right);

        public static Vector3DRange operator |(Vector3DRange left, Vector3DRange right) =>
            left.Union(right);

        public readonly Vector3DRange Intersect(Vector3DRange other) =>
            new(min: Vector3D.Max(Min, other.Min), max: Vector3D.Min(Max, other.Max));

        public static Vector3DRange Intersect(Vector3DRange left, Vector3DRange right) =>
            left.Intersect(right);

        public static Vector3DRange operator &(Vector3DRange left, Vector3DRange right) =>
            left.Intersect(right);

        public readonly Vector3DRange Add(Vector3D vector) =>
            (Min + vector, Max + vector);

        public static Vector3DRange Add(Vector3DRange range, Vector3D vector) =>
            range.Add(vector);

        public static Vector3DRange operator +(Vector3DRange range, Vector3D vector) =>
            range.Add(vector);

        public readonly Vector3DRange Sub(Vector3D vector) =>
            (Min - vector, Max - vector);

        public static Vector3DRange Sub(Vector3DRange range, Vector3D vector) =>
            range.Sub(vector);

        public static Vector3DRange operator -(Vector3DRange range, Vector3D vector) =>
            Sub(range, vector);

        public readonly Vector3DRange Grow(Size3D size) =>
            new(Min - size, Max + size);

        public static Vector3DRange Grow(Vector3DRange range, Size3D size) =>
            range.Grow(size);

        public static Vector3DRange operator +(Vector3DRange range, Size3D size) =>
            range.Grow(size);

        public readonly Vector3DRange Grow(int size) =>
            Grow((size, size, size));

        public static Vector3DRange Grow(Vector3DRange range, int size) =>
            range.Grow(size);

        public readonly Vector3DRange Shrink(Size3D size) =>
            new(Min + size, Max - size);

        public static Vector3DRange Shrink(Vector3DRange range, Size3D size) =>
            range.Shrink(size);

        public static Vector3DRange operator -(Vector3DRange range, Size3D size) =>
            range.Shrink(size);

        public readonly Vector3DRange Shrink(int size) =>
            Shrink((size, size, size));

        public static Vector3DRange Shrink(Vector3DRange range, int size) =>
            range.Shrink(size);

        public readonly IEnumerable<Vector3D> Border(int size = 1) =>
            System.Linq.Enumerable.Except(this, Shrink(size));

        public static IEnumerable<Vector3D> Border(Vector3DRange range, int size = 1) =>
            range.Border(size);

        public readonly Vector3DRange Mul(Matrix3D matrix) =>
            new(Min * matrix, Max * matrix);

        public static Vector3DRange Mul(Vector3DRange range, Matrix3D matrix) =>
            range.Mul(matrix);

        public static Vector3DRange operator *(Vector3DRange range, Matrix3D matrix) =>
            range.Mul(matrix);

        public readonly Vector3DRange SplitWest(int width) =>
            new(Min, new(Min.x + width, Max.y, Max.z));

        public readonly Vector3DRange SplitEast(int width) =>
            new(new(Min.x + width, Min.y, Min.z), Max);

        public readonly Vector3DRange SplitNorth(int height) =>
            new(Min, new(Max.x, Min.y + height, Max.z));

        public readonly Vector3DRange SplitSouth(int height) =>
            new(new(Min.x, Min.y + height, Min.z), Max);

        public readonly Vector3DRange SplitUp(int depth) =>
            new(Min, new(Max.x, Max.y, Min.z + depth));

        public readonly Vector3DRange SplitDown(int depth) =>
            new(new(Min.x, Min.y, Min.z + depth), Max);

        public readonly T GetValue<T>(T[] array, Vector3D vector) =>
            array[GetIndex(vector)];

        public readonly bool TryGetValue<T>(T[] array, Vector3D vector, out T value)
        {
            if (!IsMatch(vector))
            {
                value = default;
                return false;
            }
            value = GetValue(array, vector);
            return true;
        }

        public readonly T SetValue<T>(T[] array, Vector3D vector, T value) =>
            array[GetIndex(vector)] = value;

        public readonly int GetIndex(Vector3D vector) =>
            vector.x + Width * (vector.y + Height * vector.z);

        public static implicit operator (Vector3D min, Vector3D max)(Vector3DRange value) =>
            (value.Min, value.Max);

        public static implicit operator Vector3DRange((Vector3D min, Vector3D max) value) =>
            new(value.min, value.max);

        public static explicit operator VectorRange(Vector3DRange range) =>
            new((Vector)range.Min, (Vector)range.Max);

        public static bool operator ==(Vector3DRange left, Vector3DRange right) =>
            left.Equals(right);

        public static bool operator !=(Vector3DRange left, Vector3DRange right) =>
            !left.Equals(right);
    }
}
