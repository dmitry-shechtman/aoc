﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace aoc
{
    public struct VectorRange : IEquatable<VectorRange>, IEnumerable<Vector>
    {
        public VectorRange(Vector min, Vector max)
        {
            Min = min;
            Max = max;
        }

        public VectorRange(Vector max)
            : this(Vector.Zero, max)
        {
        }

        public VectorRange(int x, int y)
            : this(new(x, y))
        {
        }

        public Vector Min { get; }
        public Vector Max { get; }

        public int Width  => Max.x - Min.x + 1;
        public int Height => Max.y - Min.y + 1;
        public int Count  => Width * Height;

        public long LongCount =>
            (long)Width * Height;

        public readonly override bool Equals(object obj) =>
            obj is VectorRange other && Equals(other);

        public readonly bool Equals(VectorRange other) =>
            Min.Equals(other.Min) &&
            Max.Equals(other.Max);

        public readonly override int GetHashCode() =>
            HashCode.Combine(Min, Max);

        public readonly override string ToString() =>
            $"{Min},{Max}";

        public readonly void Deconstruct(out Vector min, out Vector max)
        {
            min = Min;
            max = Max;
        }

        public readonly IEnumerator<Vector> GetEnumerator()
        {
            for (var y = Min.y; y <= Max.y; y++)
                for (var x = Min.x; x <= Max.x; x++)
                    yield return new(x, y);
        }

        readonly IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public readonly bool IsMatch(Vector vector) =>
            vector.x >= Min.x && vector.x <= Max.x &&
            vector.y >= Min.y && vector.y <= Max.y;

        public readonly bool IsMatch(VectorRange other) =>
            other.Min.x <= Max.x && other.Max.x >= Min.x &&
            other.Min.y <= Max.y && other.Max.y >= Min.y;

        public readonly bool Contains(Vector vector) =>
            IsMatch(vector);

        public readonly bool Contains(VectorRange other) =>
            other.Min.x >= Min.x && other.Max.x <= Max.x &&
            other.Min.y >= Min.y && other.Max.y <= Max.y;

        public readonly VectorRange Union(VectorRange other) =>
            new(min: Vector.Min(Min, other.Min), max: Vector.Max(Max, other.Max));

        public static VectorRange Union(VectorRange left, VectorRange right) =>
            left.Union(right);

        public static VectorRange operator |(VectorRange left, VectorRange right) =>
            left.Union(right);

        public readonly VectorRange Intersect(VectorRange other) =>
            new(min: Vector.Max(Min, other.Min), max: Vector.Min(Max, other.Max));

        public static VectorRange Intersect(VectorRange left, VectorRange right) =>
            left.Intersect(right);

        public static VectorRange operator &(VectorRange left, VectorRange right) =>
            left.Intersect(right);

        public readonly VectorRange Add(Vector size) =>
            new(Min - size, Max + size);

        public static VectorRange Add(VectorRange range, Vector size) =>
            range.Add(size);

        public static VectorRange operator +(VectorRange range, Vector size) =>
            range.Add(size);

        public readonly VectorRange Add(int size) =>
            Add((size, size));

        public static VectorRange Add(VectorRange range, int size) =>
            range.Add(size);

        public static VectorRange operator +(VectorRange range, int size) =>
            range.Add(size);

        public readonly VectorRange Sub(Vector size) =>
            new(Min + size, Max - size);

        public static VectorRange Sub(VectorRange range, Vector size) =>
            range.Sub(size);

        public static VectorRange operator -(VectorRange range, Vector size) =>
            range.Sub(size);

        public readonly VectorRange Sub(int size) =>
            Sub((size, size));

        public static VectorRange Sub(VectorRange range, int size) =>
            range.Sub(size);

        public static VectorRange operator -(VectorRange range, int size) =>
            range.Sub(size);

        public readonly VectorRange SplitWest(int width) =>
            new(Min, new(Min.x + width, Max.y));

        public readonly VectorRange SplitEast(int width) =>
            new(new(Min.x + width, Min.y), Max);

        public readonly VectorRange SplitNorth(int height) =>
            new(Min, new(Max.x, Min.y + height));

        public readonly VectorRange SplitSouth(int height) =>
            new(new(Min.x, Min.y + height), Max);

        public static VectorRange FromField(string s) =>
            FromField(s, Vector.GetFieldWidth(s));

        private static VectorRange FromField(string s, int width) =>
            new(width - 1, Vector.GetFieldHeight(s, width) - 1);

        public static implicit operator (Vector min, Vector max)(VectorRange value) =>
            (value.Min, value.Max);

        public static implicit operator VectorRange((Vector min, Vector max) value) =>
            new(value.min, value.max);

        public static bool operator ==(VectorRange left, VectorRange right) =>
            left.Equals(right);

        public static bool operator !=(VectorRange left, VectorRange right) =>
            !left.Equals(right);
    }
}
