﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc
{
    using Helper = Internal.RangeHelper<Vector4DRange, Vector4D, int>;

    public readonly struct Vector4DRange : IIntegerRange<Vector4DRange, Vector4D, int>, IRange4D<Vector4DRange, Vector4D, int>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromSpan, Vector4D.Helper));

        private static Helper Helper => _helper.Value;

        public Vector4DRange(Vector4D min, Vector4D max)
        {
            Min = min;
            Max = max;
        }

        public Vector4DRange(Vector4D max)
            : this(Vector4D.Zero, max)
        {
        }

        public Vector4DRange(Size4D size)
            : this((Vector4D)size - Vector4D.One)
        {
        }

        public Vector4DRange(int min, int max)
            : this((min, min, min, min), (max, max, max, max))
        {
        }

        public Vector4D Min { get; }
        public Vector4D Max { get; }

        public readonly int Width   => Max.x - Min.x + 1;
        public readonly int Height  => Max.y - Min.y + 1;
        public readonly int Depth   => Max.z - Min.z + 1;
        public readonly int Anakata => Max.w - Min.w + 1;

        public readonly int Length =>
            Width * Height * Depth * Anakata;

        public readonly long LongLength =>
            (long)Width * Height * Depth * Anakata;

        public readonly override bool Equals(object? obj) =>
            obj is Vector4DRange other && Equals(other);

        public readonly bool Equals(Vector4DRange other) =>
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

        public readonly void Deconstruct(out Vector4D min, out Vector4D max)
        {
            min = Min;
            max = Max;
        }

        public readonly IEnumerator<Vector4D> GetEnumerator()
        {
            for (var w = Min.w; w <= Max.w; w++)
                for (var z = Min.z; z <= Max.z; z++)
                    for (var y = Min.y; y <= Max.y; y++)
                        for (var x = Min.x; x <= Max.x; x++)
                            yield return new(x, y, z, w);
        }

        readonly int IReadOnlyCollection<Vector4D>.Count =>
            Length;

        public readonly Vector4D this[int index] =>
            new(Min.x + index % Width,
                Min.y + index / Width % Height,
                Min.z + index / (Width * Height) % Depth,
                Min.w + index / (Width * Height * Depth));

        public static Builders.IRangeBuilder<Vector4DRange, Vector4D> Builder =>
            Helper;

        public static Vector4DRange Parse(string? s) =>
            Helper.Parse(s);

        public static bool TryParse(string? s, out Vector4DRange range) =>
            Helper.TryParse(s, out range);

        public static Vector4DRange Parse(string? s, IFormatProvider? provider) =>
            Helper.Parse(s, provider);

        public static bool TryParse(string? s, IFormatProvider? provider, out Vector4DRange range) =>
            Helper.TryParse(s, provider, out range);

        public static Vector4DRange Parse(ReadOnlySpan<char> s) =>
            Helper.Parse(s);

        public static bool TryParse(ReadOnlySpan<char> s, out Vector4DRange range) =>
            Helper.TryParse(s, out range);

        public static Vector4DRange Parse(ReadOnlySpan<char> s, IFormatProvider? provider) =>
            Helper.Parse(s, provider);

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Vector4DRange range) =>
            Helper.TryParse(s, provider, out range);

        private static Vector4DRange FromSpan(ReadOnlySpan<Vector4D> values) =>
            new(values[0], values[1]);

        public readonly bool Contains(Vector4D vector) =>
            vector.x >= Min.x && vector.x <= Max.x &&
            vector.y >= Min.y && vector.y <= Max.y &&
            vector.z >= Min.z && vector.z <= Max.z &&
            vector.w >= Min.w && vector.w <= Max.w;

        public readonly bool Contains(Vector4DRange other) =>
            other.Min.x >= Min.x && other.Max.x <= Max.x &&
            other.Min.y >= Min.y && other.Max.y <= Max.y &&
            other.Min.z >= Min.z && other.Max.z <= Max.z &&
            other.Min.w >= Min.w && other.Max.w <= Max.w;

        public readonly bool Overlaps(Vector4DRange other) =>
            other.Min.x <= Max.x && other.Max.x >= Min.x &&
            other.Min.y <= Max.y && other.Max.y >= Min.y &&
            other.Min.z <= Max.z && other.Max.z >= Min.z &&
            other.Min.w <= Max.w && other.Max.w >= Min.w;

        public readonly bool OverlapsOrAdjacentTo(Vector4DRange other) =>
            other.Min.x - 1 <= Max.x && other.Max.x + 1 >= Min.x &&
            other.Min.y - 1 <= Max.y && other.Max.y + 1 >= Min.y &&
            other.Min.z - 1 <= Max.z && other.Max.z + 1 >= Min.z &&
            other.Min.w - 1 <= Max.w && other.Max.w + 1 >= Min.w;

        public readonly Vector4DRange Unify(Vector4DRange other) =>
            new(min: Vector4D.Min(Min, other.Min), max: Vector4D.Max(Max, other.Max));

        public readonly IEnumerable<Vector4DRange> Union(Vector4DRange other)
        {
            if (OverlapsOrAdjacentTo(other))
                throw new NotImplementedException();
            yield return (Vector4D.Min(Min, other.Min), Vector4D.Min(Max, other.Max));
            yield return (Vector4D.Max(Min, other.Min), Vector4D.Max(Max, other.Max));
        }

        public static IEnumerable<Vector4DRange> Union(Vector4DRange left, Vector4DRange right) =>
            left.Union(right);

        public static IEnumerable<Vector4DRange> operator |(Vector4DRange left, Vector4DRange right) =>
            left.Union(right);

        public readonly bool Intersect(Vector4DRange other, out Vector4DRange result)
        {
            result = new(Vector4D.Max(Min, other.Min), Vector4D.Min(Max, other.Max));
            return Overlaps(other);
        }

        public readonly IEnumerable<Vector4DRange> Intersect(Vector4DRange other)
        {
            if (Intersect(other, out Vector4DRange result))
                yield return result;
        }

        public static IEnumerable<Vector4DRange> Intersect(Vector4DRange left, Vector4DRange right) =>
            left.Intersect(right);

        public static IEnumerable<Vector4DRange> operator &(Vector4DRange left, Vector4DRange right) =>
            left.Intersect(right);

        public IEnumerable<Vector4DRange> Subtract(IEnumerable<Vector4DRange> ranges)
        {
            HashSet<int> setX = new() { Min.X, Max.X == int.MaxValue ? Max.X : Max.X + 1 };
            HashSet<int> setY = new() { Min.Y, Max.Y == int.MaxValue ? Max.Y : Max.Y + 1 };
            HashSet<int> setZ = new() { Min.Z, Max.Z == int.MaxValue ? Max.Z : Max.Z + 1 };
            HashSet<int> setW = new() { Min.W, Max.W == int.MaxValue ? Max.W : Max.W + 1 };
            foreach (var ((minX, minY, minZ, minW), (maxX, maxY, maxZ, maxW)) in ranges)
            {
                setX.Add(minX);
                setX.Add(maxX == int.MaxValue ? maxX : maxX + 1);
                setY.Add(minY);
                setY.Add(maxY == int.MaxValue ? maxY : maxY + 1);
                setZ.Add(minZ);
                setZ.Add(maxZ == int.MaxValue ? maxZ : maxZ + 1);
                setW.Add(minW);
                setW.Add(maxW == int.MaxValue ? maxW : maxW + 1);
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
            var w = setW.ToArray();
            Array.Sort(w);
            var keyW = w.ToDictionary((v, _) => v, (_, i) => i);
            var anakata = w.Length - 1;
            var grid = new bool[width * height * depth * anakata];
            foreach (var ((minX, minY, minZ, minW), (maxX, maxY, maxZ, maxW)) in ranges)
            {
                var (i0, i1, j0, j1, k0, k1, l0, l1) = (keyX[minX], keyX[maxX == int.MaxValue ? maxX : maxX + 1], keyY[minY], keyY[maxY == int.MaxValue ? maxY : maxY + 1], keyZ[minZ], keyZ[maxZ == int.MaxValue ? maxZ : maxZ + 1], keyW[minW], keyW[maxW == int.MaxValue ? maxW : maxW + 1]);
                for (int i = i0, a = i * height * depth * anakata; i < i1; i++, a += height * depth * anakata)
                {
                    for (int j = j0, b = a + j * depth * anakata; j < j1; j++, b += depth * anakata)
                    {
                        for (int k = k0, c = b + k * anakata; k < k1; k++, c += anakata)
                        {
                            for (int l = l0, d = c + l; l < l1; l++, d++)
                            {
                                grid[d] = true;
                            }
                        }
                    }
                }
            }
            foreach (var range in Union(grid, x, keyX, y, keyY, z, keyZ, w, keyW))
            {
                yield return range;
            }
        }

        private IEnumerable<Vector4DRange> Union(bool[] grid, int[] x, Dictionary<int, int> keyX, int[] y, Dictionary<int, int> keyY, int[] z, Dictionary<int, int> keyZ, int[] w, Dictionary<int, int> keyW)
        {
            var height = y.Length - 1;
            var depth = z.Length - 1;
            var anakata = w.Length - 1;
            var (i0, i1, j0, j1, k0, k1, l0, l1) = (keyX[Min.X], keyX[Max.X == int.MaxValue ? Max.X : Max.X + 1], keyY[Min.Y], keyY[Max.Y == int.MaxValue ? Max.Y : Max.Y + 1], keyZ[Min.Z], keyZ[Max.Z == int.MaxValue ? Max.Z : Max.Z + 1], keyW[Min.W], keyW[Max.W == int.MaxValue ? Max.W : Max.W + 1]);
            for (int i = i0, a = i * height * depth * anakata; i < i1; i++, a += height * depth * anakata)
            {
                for (int j = j0, b = a + j * depth * anakata; j < j1; j++, b += depth * anakata)
                {
                    for (int k = k0, c = b + k * anakata; k < k1; k++, c += anakata)
                    {
                        for (int l = l0, d = c + l; l < l1; l++, d++)
                        {
                            if (!grid[d])
                            {
                                yield return new(new Vector4D(x[i], y[j], z[k], w[l]), new(x[i + 1] == int.MaxValue ? x[i + 1] : x[i + 1] - 1, y[j + 1] == int.MaxValue ? y[j + 1] : y[j + 1] - 1, z[k + 1] == int.MaxValue ? z[k + 1] : z[k + 1] - 1, w[l + 1] == int.MaxValue ? w[l + 1] : w[l + 1] - 1));
                                grid[d] = true;
                            }
                        }
                    }
                }
            }
        }

        public readonly Vector4DRange Add(Vector4D vector) =>
            (Min + vector, Max + vector);

        public static Vector4DRange Add(Vector4DRange range, Vector4D vector) =>
            range.Add(vector);

        public static Vector4DRange operator +(Vector4DRange range, Vector4D vector) =>
            range.Add(vector);

        public readonly Vector4DRange Sub(Vector4D vector) =>
            (Min - vector, Max - vector);

        public static Vector4DRange Sub(Vector4DRange range, Vector4D vector) =>
            range.Sub(vector);

        public static Vector4DRange operator -(Vector4DRange range, Vector4D vector) =>
            Sub(range, vector);

        public readonly Vector4DRange Grow(Size4D size) =>
            new(Min - size, Max + size);

        public static Vector4DRange Grow(Vector4DRange range, Size4D size) =>
            range.Grow(size);

        public readonly Vector4DRange Grow(int size) =>
            Grow((size, size, size, size));

        public static Vector4DRange Grow(Vector4DRange range, int size) =>
            range.Grow(size);

        public readonly Vector4DRange Shrink(Size4D size) =>
            new(Min + size, Max - size);

        public static Vector4DRange Shrink(Vector4DRange range, Size4D size) =>
            range.Shrink(size);

        public readonly Vector4DRange Shrink(int size) =>
            Shrink((size, size, size, size));

        public static Vector4DRange Shrink(Vector4DRange range, int size) =>
            range.Shrink(size);

        public readonly IEnumerable<Vector4D> Border(int size = 1) =>
            size > 0
                ? this.Except(Shrink(size))
                : Shrink(size).Except(this);

        public static IEnumerable<Vector4D> Border(Vector4DRange range, int size = 1) =>
            range.Border(size);

        public readonly Vector4DRange SplitWest(int width) =>
            new(Min, new(Min.x + width, Max.y, Max.z, Max.w));

        public readonly Vector4DRange SplitEast(int width) =>
            new(new(Min.x + width, Min.y, Min.z, Min.w), Max);

        public readonly Vector4DRange SplitNorth(int height) =>
            new(Min, new(Max.x, Min.y + height, Max.z, Max.w));

        public readonly Vector4DRange SplitSouth(int height) =>
            new(new(Min.x, Min.y + height, Min.z, Min.w), Max);

        public readonly Vector4DRange SplitUp(int depth) =>
            new(Min, new(Max.x, Max.y, Min.z + depth, Max.w));

        public readonly Vector4DRange SplitDown(int depth) =>
            new(new(Min.x, Min.y, Min.z + depth, Min.w), Max);

        public readonly Vector4DRange SplitAna(int anakata) =>
            new(Min, new(Max.x, Max.y, Max.z, Min.w + anakata));

        public readonly Vector4DRange SplitKata(int anakata) =>
            new(new(Min.x, Min.y, Min.z, Min.w + anakata), Max);

        public readonly int GetIndex(Vector4D vector) =>
            vector.x - Min.x + Width * (vector.y - Min.y + Height * (vector.z - Min.z + Depth * (vector.w - Min.w)));

        public readonly long GetLongIndex(Vector4D vector) =>
            vector.x - Min.x + (long)Width * (vector.y - Min.y + Height * (vector.z - Min.z + Depth * (vector.w - Min.w)));

        public static implicit operator (Vector4D min, Vector4D max)(Vector4DRange value) =>
            (value.Min, value.Max);

        public static implicit operator Vector4DRange((Vector4D min, Vector4D max) value) =>
            new(value.min, value.max);

        public static implicit operator Vector4DRange((int min, int max) value) =>
            new(value.min, value.max);

        public static explicit operator VectorRange(Vector4DRange range) =>
            new((Vector)range.Min, (Vector)range.Max);

        public static bool operator ==(Vector4DRange left, Vector4DRange right) =>
            left.Equals(right);

        public static bool operator !=(Vector4DRange left, Vector4DRange right) =>
            !left.Equals(right);
    }
}
