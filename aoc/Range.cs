﻿using System;
using System.Collections.Generic;

namespace aoc
{
    public struct Range : IIntegerRange<Range, int>
    {
        private static readonly Lazy<RangeHelper<Range, int>> _helper =
            new(() => new(FromArray, int.TryParse));

        private static RangeHelper<Range, int> Helper => _helper.Value;

        public Range(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public int Min { get; }
        public int Max { get; }

        public readonly int Count => Max - Min + 1;

        public readonly override bool Equals(object obj) =>
            obj is Range other && Equals(other);

        public readonly bool Equals(Range other) =>
            Min == other.Min && Max == other.Max;

        public readonly override int GetHashCode() =>
            HashCode.Combine(Min, Max);

        public readonly override string ToString() =>
            $"{Min}~{Max}";

        public readonly void Deconstruct(out int min, out int max)
        {
            min = Min;
            max = Max;
        }

        public readonly IEnumerator<int> GetEnumerator()
        {
            for (var i = Min; i <= Max; i++)
                yield return i;
        }

        public readonly int this[int index] =>
            Min + index;

        public static Range Parse(string s) =>
            Parse(s, '~');

        public static Range Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, out Range range, char separator = '~') =>
            Helper.TryParse(s, out range, separator);

        public static Range Parse(string[] ss) =>
            Helper.Parse(ss);

        public static bool TryParse(string[] ss, out Range range) =>
            Helper.TryParse(ss, out range);

        private static Range FromArray(int[] values) =>
            new(values[0], values[1]);

        public readonly bool IsMatch(int value) =>
            value >= Min && value <= Max;

        public readonly bool IsMatch(Range other) =>
            other.Min <= Max && other.Max >= Min;

        public static bool IsMatch(Range left, Range right) =>
            left.IsMatch(right);

        public readonly bool Contains(int value) =>
            IsMatch(value);

        public static bool Contains(Range range, int value) =>
            range.Contains(value);

        public readonly bool Contains(Range other) =>
            other.Min >= Min && other.Max <= Max;

        public static bool Contains(Range left, Range right) =>
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

        public readonly Range Add(int value) =>
            new(Min + value, Max + value);

        public static Range Add(Range range, int value) =>
            range.Add(value);

        public static Range operator +(Range range, int value) =>
            range.Add(value);

        public readonly Range Sub(int value) =>
            new(Min - value, Max - value);

        public static Range Sub(Range range, int value) =>
            range.Sub(value);

        public static Range operator -(Range range, int value) =>
            Sub(range, value);

        public readonly Range Grow(int size) =>
            new(Min - size, Max + size);

        public static Range Grow(Range range, int size) =>
            range.Grow(size);

        public readonly Range Shrink(int size) =>
            new(Min + size, Max - size);

        public static Range Shrink(Range range, int size) =>
            range.Shrink(size);

        public readonly IEnumerable<int> Border(int size = 1) =>
            System.Linq.Enumerable.Except(this, Shrink(size));

        public static IEnumerable<int> Border(Range range, int size = 1) =>
            range.Border(size);

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
