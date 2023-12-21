using System;

namespace aoc
{
    public struct Range : IEquatable<Range>
    {
        public Range(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public int Min { get; }
        public int Max { get; }
        public int Length => Max - Min + 1;

        public readonly override bool Equals(object obj) =>
            obj is Range other && Equals(other);

        public readonly bool Equals(Range other) =>
            Min == other.Min && Max == other.Max;

        public readonly override int GetHashCode() =>
            HashCode.Combine(Min, Max);

        public readonly override string ToString() =>
            $"{Min},{Max}";

        public readonly void Deconstruct(out int min, out int max)
        {
            min = Min;
            max = Max;
        }

        public readonly bool IsMatch(int value) =>
            value >= Min && value <= Max;

        public readonly bool IsMatch(Range other) =>
            other.Min <= Max && other.Max >= Min;

        public static bool IsMatch(Range left, Range right) =>
            left.IsMatch(right);

        public readonly bool Contains(int value) =>
            IsMatch(value);

        public static bool Contains(LongRange range, int value) =>
            range.Contains(value);

        public readonly bool Contains(LongRange other) =>
            other.Min >= Min && other.Max <= Max;

        public static bool Contains(LongRange left, LongRange right) =>
            left.Contains(right);

        public readonly Range Union(Range other) =>
            new(Math.Min(Min, other.Min), Math.Max(Max, other.Max));

        public static Range Union(Range left, Range right) =>
            left.Union(right);

        public static Range operator |(Range left, Range right) =>
            left.Union(right);

        public readonly Range Intersect(Range other) =>
            new(Math.Max(Min, other.Min), Math.Min(Max, other.Max));

        public static Range Intersect(Range left, Range right) =>
            left.Intersect(right);

        public static Range operator &(Range left, Range right) =>
            left.Intersect(right);

        public readonly Range Add(int size) =>
            new(Min - size, Max + size);

        public static Range Add(Range range, int size) =>
            range.Add(size);

        public static Range operator +(Range range, int size) =>
            range.Add(size);

        public readonly Range Sub(int size) =>
            new(Min + size, Max - size);

        public static Range Sub(Range range, int size) =>
            range.Sub(size);

        public static Range operator -(Range range, int size) =>
            range.Sub(size);

        public static implicit operator (int min, int max)(Range value) =>
            (value.Min, value.Max);

        public static implicit operator Range((int min, int max) value) =>
            new(value.min, value.max);

        public static Range FromMinLength(params int[] values) =>
            new(values[0], values[0] + values[1] - 1);

        public static bool operator ==(Range left, Range right) =>
            left.Equals(right);

        public static bool operator !=(Range left, Range right) =>
            !left.Equals(right);
    }
}
