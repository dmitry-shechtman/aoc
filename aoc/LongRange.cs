using System;
using System.Collections.Generic;

namespace aoc
{
    public struct LongRange : IIntegerRange<LongRange, long>
    {
        public LongRange(long min, long max)
        {
            Min = min;
            Max = max;
        }

        public long Min { get; }
        public long Max { get; }

        public readonly long Count => Max - Min + 1;

        public readonly override bool Equals(object obj) =>
            obj is LongRange other && Equals(other);

        public readonly bool Equals(LongRange other) =>
            Min == other.Min && Max == other.Max;

        public readonly override int GetHashCode() =>
            HashCode.Combine(Min, Max);

        public readonly override string ToString() =>
            $"{Min}~{Max}";

        public readonly void Deconstruct(out long min, out long max)
        {
            min = Min;
            max = Max;
        }

        public readonly IEnumerator<long> GetEnumerator()
        {
            for (var i = Min; i <= Max; i++)
                yield return i;
        }

        public readonly long this[int index] =>
            Min + index;

        readonly int IReadOnlyCollection<long>.Count =>
            (int)Count;

        public static LongRange Parse(string s) =>
            Parse(s, '~');

        public static LongRange Parse(string s, char separator) =>
            TryParse(s, out LongRange range, separator)
                ? range
                : throw new InvalidOperationException($"Incorrect string format: {s}");

        public static bool TryParse(string s, out LongRange range, char separator = '~') =>
            TryParse(s.Trim().Split(separator), out range);

        public static LongRange Parse(string[] ss) =>
            TryParse(ss, out LongRange range)
                ? range
                : throw new InvalidOperationException($"Input string was not in a correct format.");

        public static bool TryParse(string[] ss, out LongRange range)
        {
            range = default;
            if (ss.Length < 2 ||
                !long.TryParse(ss[0], out long min) ||
                !long.TryParse(ss[1], out long max))
                return false;
            range = new(min, max);
            return true;
        }

        public readonly bool IsMatch(long value) =>
            value >= Min && value <= Max;

        public readonly bool IsMatch(LongRange other) =>
            other.Min <= Max && other.Max >= Min;

        public static bool IsMatch(LongRange left, LongRange right) =>
            left.IsMatch(right);

        public readonly bool Contains(long value) =>
            IsMatch(value);

        public static bool Contains(LongRange range, long value) =>
            range.Contains(value);

        public readonly bool Contains(LongRange other) =>
            other.Min >= Min && other.Max <= Max;

        public static bool Contains(LongRange left, LongRange right) =>
            left.Contains(right);

        public readonly LongRange Union(LongRange other) =>
            new(Math.Min(Min, other.Min), Math.Max(Max, other.Max));

        public static LongRange Union(LongRange left, LongRange right) =>
            left.Union(right);

        public static LongRange operator |(LongRange left, LongRange right) =>
            left.Union(right);

        public readonly LongRange Intersect(LongRange other) =>
            new(Math.Max(Min, other.Min), Math.Min(Max, other.Max));

        public static LongRange Intersect(LongRange left, LongRange right) =>
            left.Intersect(right);

        public static LongRange operator &(LongRange left, LongRange right) =>
            left.Intersect(right);

        public readonly LongRange Add(long value) =>
            new(Min + value, Max + value);

        public static LongRange Add(LongRange range, long value) =>
            range.Add(value);

        public static LongRange operator +(LongRange range, long value) =>
            range.Add(value);

        public readonly LongRange Sub(long value) =>
            new(Min - value, Max - value);

        public static LongRange Sub(LongRange range, long value) =>
            range.Sub(value);

        public static LongRange operator -(LongRange range, long value) =>
            Sub(range, value);

        public readonly LongRange Grow(long size) =>
            new(Min - size, Max + size);

        public static LongRange Grow(LongRange range, long size) =>
            range.Grow(size);

        public readonly LongRange Shrink(long size) =>
            new(Min + size, Max - size);

        public static LongRange Shrink(LongRange range, long size) =>
            range.Shrink(size);

        public readonly IEnumerable<long> Border(long size = 1) =>
            System.Linq.Enumerable.Except(this, Shrink(size));

        public static IEnumerable<long> Border(LongRange range, long size = 1) =>
            range.Border(size);

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
