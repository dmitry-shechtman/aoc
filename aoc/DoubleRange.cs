using System;

namespace aoc
{
    public struct DoubleRange : IRange<DoubleRange, double>
    {
        private static readonly Lazy<RangeHelper<DoubleRange, double>> _helper =
            new(() => new(FromArray, double.TryParse));

        private static RangeHelper<DoubleRange, double> Helper => _helper.Value;

        public DoubleRange(double min, double max)
        {
            Min = min;
            Max = max;
        }

        public DoubleRange(Range range)
            : this(range.Min, range.Max)
        {
        }

        public DoubleRange(LongRange range)
            : this(range.Min, range.Max)
        {
        }

        public double Min { get; }
        public double Max { get; }

        public readonly double Count => Max - Min;

        public readonly override bool Equals(object obj) =>
            obj is DoubleRange other && Equals(other);

        public readonly bool Equals(DoubleRange other) =>
            Min == other.Min && Max == other.Max;

        public readonly override int GetHashCode() =>
            HashCode.Combine(Min, Max);

        public readonly override string ToString() =>
            Helper.ToString(this);

        public readonly string ToString(IFormatProvider provider) =>
            Helper.ToString(this, provider);

        public readonly string ToString(string format, IFormatProvider provider = null) =>
            Helper.ToString(this, format, provider);

        public readonly void Deconstruct(out double min, out double max)
        {
            min = Min;
            max = Max;
        }

        public static DoubleRange Parse(string s) =>
            Helper.Parse(s);

        public static bool TryParse(string s, out DoubleRange range) =>
            Helper.TryParse(s, out range);

        public static DoubleRange Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out DoubleRange range) =>
            Helper.TryParse(s, separator, out range);

        public static DoubleRange Parse(string[] ss) =>
            Helper.Parse(ss);

        public static bool TryParse(string[] ss, out DoubleRange range) =>
            Helper.TryParse(ss, out range);

        private static DoubleRange FromArray(double[] values) =>
            new(values[0], values[1]);

        public readonly bool IsMatch(double value) =>
            value >= Min && value <= Max;

        public static bool IsMatch(DoubleRange range, double value) =>
            range.IsMatch(value);

        public readonly bool IsMatch(DoubleRange other) =>
            other.Min <= Max && other.Max >= Min;

        public static bool IsMatch(DoubleRange left, DoubleRange right) =>
            left.IsMatch(right);

        public readonly bool Contains(double value) =>
            IsMatch(value);

        public static bool Contains(DoubleRange range, double value) =>
            range.Contains(value);

        public readonly bool Contains(DoubleRange other) =>
            other.Min >= Min && other.Max <= Max;

        public static bool Contains(DoubleRange left, DoubleRange right) =>
            left.Contains(right);

        public readonly DoubleRange Union(DoubleRange other) =>
            new(Math.Min(Min, other.Min), Math.Max(Max, other.Max));

        public static DoubleRange Union(DoubleRange left, DoubleRange right) =>
            left.Union(right);

        public static DoubleRange operator |(DoubleRange left, DoubleRange right) =>
            left.Union(right);

        public readonly DoubleRange Intersect(DoubleRange other) =>
            new(Math.Max(Min, other.Min), Math.Min(Max, other.Max));

        public static DoubleRange Intersect(DoubleRange left, DoubleRange right) =>
            left.Intersect(right);

        public static DoubleRange operator &(DoubleRange left, DoubleRange right) =>
            left.Intersect(right);

        public readonly DoubleRange Add(double value) =>
            new(Min + value, Max + value);

        public static DoubleRange Add(DoubleRange range, double value) =>
            range.Add(value);

        public static DoubleRange operator +(DoubleRange range, double value) =>
            range.Add(value);

        public readonly DoubleRange Sub(double value) =>
            new(Min - value, Max - value);

        public static DoubleRange Sub(DoubleRange range, double value) =>
            range.Sub(value);

        public static DoubleRange operator -(DoubleRange range, double value) =>
            Sub(range, value);

        public readonly DoubleRange Grow(double size) =>
            new(Min - size, Max + size);

        public static DoubleRange Grow(DoubleRange range, double size) =>
            range.Grow(size);

        public readonly DoubleRange Shrink(double size) =>
            new(Min + size, Max - size);

        public static DoubleRange Shrink(DoubleRange range, double size) =>
            range.Shrink(size);

        public static implicit operator (double min, double max)(DoubleRange value) =>
            (value.Min, value.Max);

        public static implicit operator DoubleRange((double min, double max) value) =>
            new(value.min, value.max);

        public static implicit operator DoubleRange(Range value) =>
            new(value);

        public static explicit operator DoubleRange(LongRange value) =>
            new(value);

        public static explicit operator Range(DoubleRange value) =>
            new((int)value.Min, (int)value.Max);

        public static explicit operator LongRange(DoubleRange value) =>
            new((long)value.Min, (long)value.Max);

        public static DoubleRange FromMinLength(params double[] values) =>
            new(values[0], values[0] + values[1]);

        public static bool operator ==(DoubleRange left, DoubleRange right) =>
            left.Equals(right);

        public static bool operator !=(DoubleRange left, DoubleRange right) =>
            !left.Equals(right);
    }
}
