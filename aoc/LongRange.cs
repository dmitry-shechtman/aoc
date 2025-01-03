﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc
{
    using Helper = Internal.RangeHelper<LongRange, long>;

    public readonly struct LongRange : IIntegerRange<LongRange, long>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromSpan, Internal.Int64Helper.Instance));

        private static Helper Helper => _helper.Value;

        public LongRange(long min, long max)
        {
            Min = min;
            Max = max;
        }

        public LongRange(Range range)
            : this(range.Min, range.Max)
        {
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
            Helper.ToString(this);

        public readonly string ToString(IFormatProvider provider) =>
            Helper.ToString(this, provider);

        public readonly string ToString(string format, IFormatProvider provider = null) =>
            Helper.ToString(this, format, provider);

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
            Helper.Parse(s);

        public static bool TryParse(string s, out LongRange range) =>
            Helper.TryParse(s, out range);

        public static LongRange Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out LongRange range) =>
            Helper.TryParse(s, separator, out range);

        public static LongRange Parse(string s, string separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, string separator, out LongRange range) =>
            Helper.TryParse(s, separator, out range);

        public static LongRange Parse(string s, Regex separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, Regex separator, out LongRange range) =>
            Helper.TryParse(s, separator, out range);

        public static LongRange ParseAny(string input) =>
            Helper.ParseAny(input);

        public static bool TryParseAny(string input, out LongRange range) =>
            Helper.TryParseAny(input, out range);

        public static LongRange[] ParseAll(string input) =>
            Helper.ParseAll(input);

        public static bool TryParseAll(string input, out LongRange[] ranges) =>
            Helper.TryParseAll(input, out ranges);

        private static LongRange FromSpan(ReadOnlySpan<long> values) =>
            new(values[0], values[1]);

        public readonly bool Contains(long value) =>
            value >= Min && value <= Max;

        public static bool Contains(LongRange range, long value) =>
            range.Contains(value);

        public readonly bool Contains(LongRange other) =>
            other.Min >= Min && other.Max <= Max;

        public static bool Contains(LongRange left, LongRange right) =>
            left.Contains(right);

        public readonly bool Overlaps(LongRange other) =>
            other.Min <= Max && other.Max >= Min;

        public static bool Overlaps(LongRange left, LongRange right) =>
            left.Overlaps(right);

        public readonly bool OverlapsOrAdjacentTo(LongRange other) =>
            other.Min - 1 <= Max && other.Max + 1 >= Min;

        public readonly LongRange Unify(LongRange other) =>
            new(Math.Min(Min, other.Min), Math.Max(Max, other.Max));

        public readonly IEnumerable<LongRange> Union(LongRange other)
        {
            if (OverlapsOrAdjacentTo(other))
                yield return Unify(other);
            yield return (Math.Min(Min, other.Min), Math.Min(Max, other.Max));
            yield return (Math.Max(Min, other.Min), Math.Max(Max, other.Max));
        }

        public static IEnumerable<LongRange> Union(LongRange left, LongRange right) =>
            left.Union(right);

        public static IEnumerable<LongRange> operator |(LongRange left, LongRange right) =>
            left.Union(right);

        public readonly bool Intersect(LongRange other, out LongRange result)
        {
            result = new(Math.Max(Min, other.Min), Math.Min(Max, other.Max));
            return Overlaps(other);
        }

        public readonly IEnumerable<LongRange> Intersect(LongRange other)
        {
            if (Intersect(other, out LongRange result))
                yield return result;
        }

        public static IEnumerable<LongRange> Intersect(LongRange left, LongRange right) =>
            left.Intersect(right);

        public static IEnumerable<LongRange> operator &(LongRange left, LongRange right) =>
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
            size > 0
                ? this.Except(Shrink(size))
                : Shrink(size).Except(this);

        public static IEnumerable<long> Border(LongRange range, long size = 1) =>
            range.Border(size);

        public readonly long GetIndex(long value) =>
            value - Min;

        readonly int IIntegerSize<LongRange, long>.GetIndex(long value) =>
            (int)GetIndex(value);

        readonly long IIntegerSize<LongRange, long>.GetLongIndex(long value) =>
            GetIndex(value);

        readonly int IComparer<long>.Compare(long x, long y) =>
            (int)(x - y);

        public static implicit operator (long min, long max)(LongRange value) =>
            (value.Min, value.Max);

        public static implicit operator LongRange((long min, long max) value) =>
            new(value.min, value.max);

        public static implicit operator LongRange(Range value) =>
            new(value);

        public static explicit operator Range(LongRange value) =>
            new((int)value.Min, (int)value.Max);

        public static LongRange FromMinLength(params long[] values) =>
            new(values[0], values[0] + values[1] - 1);

        public static bool operator ==(LongRange left, LongRange right) =>
            left.Equals(right);

        public static bool operator !=(LongRange left, LongRange right) =>
            !left.Equals(right);
    }
}
