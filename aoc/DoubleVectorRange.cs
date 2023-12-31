using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace aoc
{
    using Helper = Internal.VectorRangeHelper<DoubleVectorRange, DoubleVector>;

    public struct DoubleVectorRange : IRange2D<DoubleVectorRange, DoubleVector, double>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromArray, DoubleVector.Helper));

        private static Helper Helper => _helper.Value;

        public DoubleVectorRange(DoubleVector min, DoubleVector max)
        {
            Min = min;
            Max = max;
        }

        public DoubleVectorRange(DoubleVector max)
            : this(DoubleVector.Zero, max)
        {
        }

        public DoubleVectorRange(double min, double max)
            : this((min, min), (max, max))
        {
        }

        public DoubleVector Min { get; }
        public DoubleVector Max { get; }

        public readonly double Width  => Max.x - Min.x;
        public readonly double Height => Max.y - Min.y;
        public readonly double Length => Width * Height;

        public readonly override bool Equals(object obj) =>
            obj is DoubleVectorRange other && Equals(other);

        public readonly bool Equals(DoubleVectorRange other) =>
            Min.Equals(other.Min) &&
            Max.Equals(other.Max);

        public readonly override int GetHashCode() =>
            HashCode.Combine(Min, Max);

        public readonly override string ToString() =>
            Helper.ToString(this);

        public readonly string ToString(IFormatProvider provider) =>
            Helper.ToString(this, provider);

        public readonly string ToString(string format, IFormatProvider provider = null) =>
            Helper.ToString(this, format, provider);

        public readonly void Deconstruct(out DoubleVector min, out DoubleVector max)
        {
            min = Min;
            max = Max;
        }

        public static DoubleVectorRange Parse(string s) =>
            Helper.Parse(s);

        public static bool TryParse(string s, out DoubleVectorRange range) =>
            Helper.TryParse(s, out range);

        public static DoubleVectorRange Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out DoubleVectorRange range) =>
            Helper.TryParse(s, separator, out range);

        public static DoubleVectorRange Parse(string s, string separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, string separator, out DoubleVectorRange range) =>
            Helper.TryParse(s, separator, out range);

        public static DoubleVectorRange Parse(string s, Regex separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, Regex separator, out DoubleVectorRange range) =>
            Helper.TryParse(s, separator, out range);

        public static DoubleVectorRange Parse(string s, char separator, char separator2) =>
            Helper.Parse(s, separator, separator2);

        public static bool TryParse(string s, char separator, char separator2, out DoubleVectorRange range) =>
            Helper.TryParse(s, separator, separator2, out range);

        public static DoubleVectorRange Parse(string[] ss) =>
            Helper.Parse(ss);

        public static bool TryParse(string[] ss, out DoubleVectorRange range) =>
            Helper.TryParse(ss, out range);

        public static DoubleVectorRange Parse(string[] ss, char separator) =>
            Helper.Parse(ss, separator);

        public static bool TryParse(string[] ss, char separator, out DoubleVectorRange range) =>
            Helper.TryParse(ss, separator, out range);

        private static DoubleVectorRange FromArray(DoubleVector[] values) =>
            new(values[0], values[1]);

        public readonly bool Contains(DoubleVector vector) =>
            vector.x >= Min.x && vector.x <= Max.x &&
            vector.y >= Min.y && vector.y <= Max.y;

        public readonly bool Contains(DoubleVectorRange other) =>
            other.Min.x >= Min.x && other.Max.x <= Max.x &&
            other.Min.y >= Min.y && other.Max.y <= Max.y;

        public readonly bool Overlaps(DoubleVectorRange other) =>
            other.Min.x <= Max.x && other.Max.x >= Min.x &&
            other.Min.y <= Max.y && other.Max.y >= Min.y;

        public readonly DoubleVectorRange Unify(DoubleVectorRange other) =>
            new(min: DoubleVector.Min(Min, other.Min), max: DoubleVector.Max(Max, other.Max));

        public readonly IEnumerable<DoubleVectorRange> Union(DoubleVectorRange other)
        {
            if (Overlaps(other))
                throw new NotImplementedException();
            else
            {
                yield return (DoubleVector.Min(Min, other.Min), DoubleVector.Min(Max, other.Max));
                yield return (DoubleVector.Max(Min, other.Min), DoubleVector.Max(Max, other.Max));
            }
        }

        public static IEnumerable<DoubleVectorRange> Union(DoubleVectorRange left, DoubleVectorRange right) =>
            left.Union(right);

        public static IEnumerable<DoubleVectorRange> operator |(DoubleVectorRange left, DoubleVectorRange right) =>
            left.Union(right);

        public readonly bool Intersect(DoubleVectorRange other, out DoubleVectorRange result)
        {
            result = new(DoubleVector.Max(Min, other.Min), DoubleVector.Min(Max, other.Max));
            return Overlaps(other);
        }

        public readonly IEnumerable<DoubleVectorRange> Intersect(DoubleVectorRange other)
        {
            if (Intersect(other, out DoubleVectorRange result))
                yield return result;
        }

        public static IEnumerable<DoubleVectorRange> Intersect(DoubleVectorRange left, DoubleVectorRange right) =>
            left.Intersect(right);

        public static IEnumerable<DoubleVectorRange> operator &(DoubleVectorRange left, DoubleVectorRange right) =>
            left.Intersect(right);

        public static implicit operator (DoubleVector min, DoubleVector max)(DoubleVectorRange value) =>
            (value.Min, value.Max);

        public static implicit operator DoubleVectorRange((DoubleVector min, DoubleVector max) value) =>
            new(value.min, value.max);

        public static implicit operator DoubleVectorRange((double min, double max) value) =>
            new(value.min, value.max);

        public static bool operator ==(DoubleVectorRange left, DoubleVectorRange right) =>
            left.Equals(right);

        public static bool operator !=(DoubleVectorRange left, DoubleVectorRange right) =>
            !left.Equals(right);
    }
}
