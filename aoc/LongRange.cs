using System;

namespace aoc
{
    public struct LongRange : IEquatable<LongRange>
    {
        public LongRange(long min, long max)
        {
            Min = min;
            Max = max;
        }

        public long Min { get; }
        public long Max { get; }
        public long Length => Max - Min + 1;

        public readonly override bool Equals(object obj) =>
            obj is LongRange other && Equals(other);

        public readonly bool Equals(LongRange other) =>
            Min == other.Min && Max == other.Max;

        public readonly override int GetHashCode() =>
            HashCode.Combine(Min, Max);

        public readonly override string ToString() =>
            $"{Min},{Max}";

        public readonly void Deconstruct(out long min, out long max)
        {
            min = Min;
            max = Max;
        }

        public readonly bool Match(long value) =>
            value >= Min && value <= Max;

        public readonly bool Match(LongRange other) =>
            other.Min <= Max && other.Max >= Min;

        public static bool Match(LongRange left, LongRange right) =>
            left.Match(right);

        public readonly LongRange Union(LongRange other) =>
            new(Math.Min(Min, other.Min), Math.Max(Max, other.Max));

        public static LongRange Union(LongRange left, LongRange right) =>
            left.Union(right);

        public readonly LongRange Intersect(LongRange other) =>
            new(Math.Max(Min, other.Min), Math.Min(Max, other.Max));

        public static LongRange Intersect(LongRange left, LongRange right) =>
            left.Intersect(right);

        public static implicit operator (long min, long max)(LongRange value) =>
            (value.Min, value.Max);

        public static implicit operator LongRange((long min, long max) value) =>
            new(value.min, value.max);

        public static LongRange FromMinLength(params long[] values) =>
            new(values[0], values[0] + values[1] - 1);

        public static bool operator ==(LongRange left, LongRange right) =>
            left.Equals(right);

        public static bool operator !=(LongRange left, LongRange right) =>
            !left.Equals(right);
    }
}
