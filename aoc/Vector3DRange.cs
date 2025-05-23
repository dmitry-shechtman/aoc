﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc
{
    using Helper = Internal.RangeHelper<Vector3DRange, Vector3D, int>;

    public readonly struct Vector3DRange : IIntegerRange<Vector3DRange, Vector3D, int>, IRange3D<Vector3DRange, Vector3D, int>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromSpan, Vector3D.Helper));

        private static Helper Helper => _helper.Value;

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
            : this((Vector3D)size - Vector3D.One)
        {
        }

        public Vector3DRange(int min, int max)
            : this((min, min, min), (max, max, max))
        {
        }

        public Vector3D Min { get; }
        public Vector3D Max { get; }

        public readonly int Width   => Max.x - Min.x + 1;
        public readonly int Height  => Max.y - Min.y + 1;
        public readonly int Depth   => Max.z - Min.z + 1;

        public readonly int Length =>
            Width * Height * Depth;

        public readonly long LongLength =>
            (long)Width * Height * Depth;

        public readonly override bool Equals(object? obj) =>
            obj is Vector3DRange other && Equals(other);

        public readonly bool Equals(Vector3DRange other) =>
            Min.Equals(other.Min) &&
            Max.Equals(other.Max);

        public readonly override int GetHashCode() =>
            HashCode.Combine(Min, Max);

        public readonly override string ToString() =>
            Helper.ToString(this);

        public readonly string ToString(IFormatProvider? provider) =>
            Helper.ToString(this, provider);

        public readonly string ToString(string? format, IFormatProvider? provider = null) =>
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

        readonly int IReadOnlyCollection<Vector3D>.Count =>
            Length;

        public readonly Vector3D this[int index] =>
            new(Min.x + index % Width, Min.y + index / Width % Height, Min.z + index / (Width * Height));

        public static Builders.IRangeBuilder<Vector3DRange, Vector3D> Builder =>
            Helper;

        public static Vector3DRange Parse(string? s) =>
            Helper.Parse(s);

        public static bool TryParse(string? s, out Vector3DRange range) =>
            Helper.TryParse(s, out range);

        public static Vector3DRange Parse(string? s, IFormatProvider? provider) =>
            Helper.Parse(s, provider);

        public static bool TryParse(string? s, IFormatProvider? provider, out Vector3DRange range) =>
            Helper.TryParse(s, provider, out range);

        public static Vector3DRange Parse(ReadOnlySpan<char> s) =>
            Helper.Parse(s);

        public static bool TryParse(ReadOnlySpan<char> s, out Vector3DRange range) =>
            Helper.TryParse(s, out range);

        public static Vector3DRange Parse(ReadOnlySpan<char> s, IFormatProvider? provider) =>
            Helper.Parse(s, provider);

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Vector3DRange range) =>
            Helper.TryParse(s, provider, out range);

        private static Vector3DRange FromSpan(ReadOnlySpan<Vector3D> values) =>
            new(values[0], values[1]);

        public readonly bool Contains(Vector3D vector) =>
            vector.x >= Min.x && vector.x <= Max.x &&
            vector.y >= Min.y && vector.y <= Max.y &&
            vector.z >= Min.z && vector.z <= Max.z;

        public readonly bool Contains(Vector3DRange other) =>
            other.Min.x >= Min.x && other.Max.x <= Max.x &&
            other.Min.y >= Min.y && other.Max.y <= Max.y &&
            other.Min.z >= Min.z && other.Max.z <= Max.z;

        public readonly bool Overlaps(Vector3DRange other) =>
            other.Min.x <= Max.x && other.Max.x >= Min.x &&
            other.Min.y <= Max.y && other.Max.y >= Min.y &&
            other.Min.z <= Max.z && other.Max.z >= Min.z;

        public readonly bool OverlapsOrAdjacentTo(Vector3DRange other) =>
            other.Min.x - 1 <= Max.x && other.Max.x + 1 >= Min.x &&
            other.Min.y - 1 <= Max.y && other.Max.y + 1 >= Min.y &&
            other.Min.z - 1 <= Max.z && other.Max.z + 1 >= Min.z;

        public readonly Vector3DRange Unify(Vector3DRange other) =>
            new(min: Vector3D.Min(Min, other.Min), max: Vector3D.Max(Max, other.Max));

        public readonly IEnumerable<Vector3DRange> Union(Vector3DRange other)
        {
            if (OverlapsOrAdjacentTo(other))
                throw new NotImplementedException();
            yield return (Vector3D.Min(Min, other.Min), Vector3D.Min(Max, other.Max));
            yield return (Vector3D.Max(Min, other.Min), Vector3D.Max(Max, other.Max));
        }

        public static IEnumerable<Vector3DRange> Union(Vector3DRange left, Vector3DRange right) =>
            left.Union(right);

        public static IEnumerable<Vector3DRange> operator |(Vector3DRange left, Vector3DRange right) =>
            left.Union(right);

        public readonly bool Intersect(Vector3DRange other, out Vector3DRange result)
        {
            result = new(Vector3D.Max(Min, other.Min), Vector3D.Min(Max, other.Max));
            return Overlaps(other);
        }

        public readonly IEnumerable<Vector3DRange> Intersect(Vector3DRange other)
        {
            if (Intersect(other, out Vector3DRange result))
                yield return result;
        }

        public static IEnumerable<Vector3DRange> Intersect(Vector3DRange left, Vector3DRange right) =>
            left.Intersect(right);

        public static IEnumerable<Vector3DRange> operator &(Vector3DRange left, Vector3DRange right) =>
            left.Intersect(right);

        public IEnumerable<Vector3DRange> Subtract(IEnumerable<Vector3DRange> ranges)
        {
            HashSet<int> setX = new() { Min.X, Max.X == int.MaxValue ? Max.X : Max.X + 1 };
            HashSet<int> setY = new() { Min.Y, Max.Y == int.MaxValue ? Max.Y : Max.Y + 1 };
            HashSet<int> setZ = new() { Min.Z, Max.Z == int.MaxValue ? Max.Z : Max.Z + 1 };
            foreach (var ((minX, minY, minZ), (maxX, maxY, maxZ)) in ranges)
            {
                setX.Add(minX);
                setX.Add(maxX == int.MaxValue ? maxX : maxX + 1);
                setY.Add(minY);
                setY.Add(maxY == int.MaxValue ? maxY : maxY + 1);
                setZ.Add(minZ);
                setZ.Add(maxZ == int.MaxValue ? maxZ : maxZ + 1);
            }
            var x = setX.ToArray();
            Array.Sort(x);
            var keyX = x.ToDictionary((v, _) => v, (_, i) => i);
            var width = x.Length - 1;
            var y = setY.ToArray();
            Array.Sort(y);
            var keyY = y.ToDictionary((v, _) => v, (_, i) => i);
            var height = y.Length - 1;
            var z = setZ.ToArray();
            Array.Sort(z);
            var keyZ = z.ToDictionary((v, _) => v, (_, i) => i);
            var depth = z.Length - 1;
            var grid = new bool[width * height * depth];
            foreach (var ((minX, minY, minZ), (maxX, maxY, maxZ)) in ranges)
            {
                var (i0, i1, j0, j1, k0, k1) = (keyX[minX], keyX[maxX == int.MaxValue ? maxX : maxX + 1], keyY[minY], keyY[maxY == int.MaxValue ? maxY : maxY + 1], keyZ[minZ], keyZ[maxZ == int.MaxValue ? maxZ : maxZ + 1]);
                for (int i = i0, a = i * height * depth; i < i1; i++, a += height * depth)
                {
                    for (int j = j0, b = a + j * depth; j < j1; j++, b += depth)
                    {
                        for (int k = k0, c = b + k; k < k1; k++, c++)
                        {
                            grid[c] = true;
                        }
                    }
                }
            }
            foreach (var range in Union(grid, x, keyX, y, keyY, z, keyZ))
            {
                yield return range;
            }
        }

        private IEnumerable<Vector3DRange> Union(bool[] grid, int[] x, Dictionary<int, int> keyX, int[] y, Dictionary<int, int> keyY, int[] z, Dictionary<int, int> keyZ)
        {
            var height = y.Length - 1;
            var depth = z.Length - 1;
            var (i0, i1, j0, j1, k0, k1) = (keyX[Min.X], keyX[Max.X == int.MaxValue ? Max.X : Max.X + 1], keyY[Min.Y], keyY[Max.Y == int.MaxValue ? Max.Y : Max.Y + 1], keyZ[Min.Z], keyZ[Max.Z == int.MaxValue ? Max.Z : Max.Z + 1]);
            for (int i = i0, a = i * height * depth; i < i1; i++, a += height * depth)
            {
                for (int j = j0, b = a + j * depth; j < j1; j++, b += depth)
                {
                    for (int k = k0, c = b + k; k < k1; k++, c++)
                    {
                        if (!grid[c])
                        {
                            yield return new(new Vector3D(x[i], y[j], z[k]), new(x[i + 1] == int.MaxValue ? x[i + 1] : x[i + 1] - 1, y[j + 1] == int.MaxValue ? y[j + 1] : y[j + 1] - 1, z[k + 1] == int.MaxValue ? z[k + 1] : z[k + 1] - 1));
                            grid[c] = true;
                        }
                    }
                }
            }
        }

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

        public readonly Vector3DRange Grow(int size) =>
            Grow((size, size, size));

        public static Vector3DRange Grow(Vector3DRange range, int size) =>
            range.Grow(size);

        public readonly Vector3DRange Shrink(Size3D size) =>
            new(Min + size, Max - size);

        public static Vector3DRange Shrink(Vector3DRange range, Size3D size) =>
            range.Shrink(size);

        public readonly Vector3DRange Shrink(int size) =>
            Shrink((size, size, size));

        public static Vector3DRange Shrink(Vector3DRange range, int size) =>
            range.Shrink(size);

        public readonly IEnumerable<Vector3D> Border(int size = 1) =>
            size > 0
                ? this.Except(Shrink(size))
                : Shrink(size).Except(this);

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

        public readonly int GetIndex(Vector3D vector) =>
            vector.x - Min.x + Width * (vector.y - Min.y + Height * (vector.z - Min.z));

        public readonly long GetLongIndex(Vector3D vector) =>
            vector.x - Min.x + (long)Width * (vector.y - Min.y + Height * (vector.z - Min.z));

        public static implicit operator (Vector3D min, Vector3D max)(Vector3DRange value) =>
            (value.Min, value.Max);

        public static implicit operator Vector3DRange((Vector3D min, Vector3D max) value) =>
            new(value.min, value.max);

        public static implicit operator Vector3DRange((int min, int max) value) =>
            new(value.min, value.max);

        public static explicit operator VectorRange(Vector3DRange range) =>
            new((Vector)range.Min, (Vector)range.Max);

        public static bool operator ==(Vector3DRange left, Vector3DRange right) =>
            left.Equals(right);

        public static bool operator !=(Vector3DRange left, Vector3DRange right) =>
            !left.Equals(right);
    }
}
