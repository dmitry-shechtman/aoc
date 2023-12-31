using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace aoc
{
    using Helper = Internal.RangeHelper<Range, int>;

    public struct Range : IIntegerRange<Range, int>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromArray, int.TryParse));

        private static Helper Helper => _helper.Value;

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
            Helper.ToString(this);

        public readonly string ToString(IFormatProvider provider) =>
            Helper.ToString(this, provider);

        public readonly string ToString(string format, IFormatProvider provider = null) =>
            Helper.ToString(this, format, provider);

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
            Helper.Parse(s);

        public static bool TryParse(string s, out Range range) =>
            Helper.TryParse(s, out range);

        public static Range Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out Range range) =>
            Helper.TryParse(s, separator, out range);

        public static Range Parse(string s, string separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, string separator, out Range range) =>
            Helper.TryParse(s, separator, out range);

        public static Range Parse(string s, Regex separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, Regex separator, out Range range) =>
            Helper.TryParse(s, separator, out range);

        public static Range Parse(string[] ss) =>
            Helper.Parse(ss);

        public static bool TryParse(string[] ss, out Range range) =>
            Helper.TryParse(ss, out range);

        private static Range FromArray(int[] values) =>
            new(values[0], values[1]);

        public readonly bool Contains(int value) =>
            value >= Min && value <= Max;

        public static bool Contains(Range range, int value) =>
            range.Contains(value);

        public readonly bool Contains(Range other) =>
            other.Min >= Min && other.Max <= Max;

        public static bool Contains(Range left, Range right) =>
            left.Contains(right);

        public readonly bool Overlaps(Range other) =>
            other.Min <= Max && other.Max >= Min;

        public static bool Overlaps(Range left, Range right) =>
            left.Overlaps(right);

        public readonly bool OverlapsOrAdjacentTo(Range other) =>
            other.Min - 1 <= Max && other.Max + 1 >= Min;

        public readonly Range Unify(Range other) =>
            new(Math.Min(Min, other.Min), Math.Max(Max, other.Max));

        public readonly IEnumerable<Range> Union(Range other)
        {
            if (OverlapsOrAdjacentTo(other))
                yield return Unify(other);
            else
            {
                yield return (Math.Min(Min, other.Min), Math.Min(Max, other.Max));
                yield return (Math.Max(Min, other.Min), Math.Max(Max, other.Max));
            }
        }

        public static IEnumerable<Range> Union(Range left, Range right) =>
            left.Union(right);

        public static IEnumerable<Range> operator |(Range left, Range right) =>
            left.Union(right);

        public readonly bool Intersect(Range other, out Range result)
        {
            result = new(Math.Max(Min, other.Min), Math.Min(Max, other.Max));
            return Overlaps(other);
        }

        public readonly IEnumerable<Range> Intersect(Range other)
        {
            if (Intersect(other, out Range result))
                yield return result;
        }

        public static IEnumerable<Range> Intersect(Range left, Range right) =>
            left.Intersect(right);

        public static IEnumerable<Range> operator &(Range left, Range right) =>
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

        public readonly T GetValue<T>(T[] array, int key) =>
            array[GetIndex(key)];

        public readonly bool TryGetValue<T>(T[] array, int key, out T value)
        {
            if (!Contains(key))
            {
                value = default;
                return false;
            }
            value = GetValue(array, key);
            return true;
        }

        public readonly T SetValue<T>(T[] array, int key, T value) =>
            array[GetIndex(key)] = value;

        public readonly int GetIndex(int value) =>
            value - Min;

        public readonly long GetLongIndex(int value) =>
            GetIndex(value);

        readonly int IComparer<int>.Compare(int x, int y) =>
            x - y;

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
