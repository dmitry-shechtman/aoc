using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace aoc
{
    using Helper = Internal.RangeHelper<LongVectorRange, LongVector, long>;

    public readonly struct LongVectorRange : IIntegerRange<LongVectorRange, LongVector>, IRange2D<LongVectorRange, LongVector, long>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromSpan, LongVector.Helper));

        private static Helper Helper => _helper.Value;

        public LongVectorRange(LongVector min, LongVector max)
        {
            Min = min;
            Max = max;
        }

        public LongVectorRange(LongVector max)
            : this(LongVector.Zero, max)
        {
        }

        public LongVectorRange(long min, long max)
            : this((min, min), (max, max))
        {
        }

        public LongVector Min { get; }
        public LongVector Max { get; }

        public readonly long Width   => Max.x - Min.x + 1;
        public readonly long Height  => Max.y - Min.y + 1;

        public readonly long Length =>
            Width * Height;

        public readonly override bool Equals(object obj) =>
            obj is LongVectorRange other && Equals(other);

        public readonly bool Equals(LongVectorRange other) =>
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

        public readonly void Deconstruct(out LongVector min, out LongVector max)
        {
            min = Min;
            max = Max;
        }

        public readonly IEnumerator<LongVector> GetEnumerator()
        {
            for (var y = Min.y; y <= Max.y; y++)
                for (var x = Min.x; x <= Max.x; x++)
                    yield return new(x, y);
        }

        public readonly LongVector this[int index] =>
            new(Min.x + index % Width, Min.y + index / Width);

        readonly int IReadOnlyCollection<LongVector>.Count =>
            (int)Length;

        public static LongVectorRange Parse(string s) =>
            Helper.Parse(s);

        public static bool TryParse(string s, out LongVectorRange range) =>
            Helper.TryParse(s, out range);

        public static LongVectorRange Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out LongVectorRange range) =>
            Helper.TryParse(s, separator, out range);

        public static LongVectorRange Parse(string s, string separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, string separator, out LongVectorRange range) =>
            Helper.TryParse(s, separator, out range);

        public static LongVectorRange Parse(string s, Regex separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, Regex separator, out LongVectorRange range) =>
            Helper.TryParse(s, separator, out range);

        public static LongVectorRange Parse(string s, char separator, char separator2) =>
            Helper.Parse(s, separator, separator2);

        public static bool TryParse(string s, char separator, char separator2, out LongVectorRange range) =>
            Helper.TryParse(s, separator, separator2, out range);

        public static LongVectorRange ParseAny(string input) =>
            Helper.ParseAny(input);

        public static bool TryParseAny(string input, out LongVectorRange vector) =>
            Helper.TryParseAny(input, out vector);

        private static LongVectorRange FromSpan(ReadOnlySpan<LongVector> values) =>
            new(values[0], values[1]);

        public readonly bool Contains(LongVector vector) =>
            vector.x >= Min.x && vector.x <= Max.x &&
            vector.y >= Min.y && vector.y <= Max.y;

        public readonly bool Contains(LongVectorRange other) =>
            other.Min.x >= Min.x && other.Max.x <= Max.x &&
            other.Min.y >= Min.y && other.Max.y <= Max.y;

        public readonly bool Overlaps(LongVectorRange other) =>
            other.Min.x <= Max.x && other.Max.x >= Min.x &&
            other.Min.y <= Max.y && other.Max.y >= Min.y;

        public readonly bool OverlapsOrAdjacentTo(LongVectorRange other) =>
            other.Min.x - 1 <= Max.x && other.Max.x + 1 >= Min.x &&
            other.Min.y - 1 <= Max.y && other.Max.y + 1 >= Min.y;

        public readonly LongVectorRange Unify(LongVectorRange other) =>
            new(min: LongVector.Min(Min, other.Min), max: LongVector.Max(Max, other.Max));

        public readonly IEnumerable<LongVectorRange> Union(LongVectorRange other)
        {
            if (OverlapsOrAdjacentTo(other))
                throw new NotImplementedException();
            yield return (LongVector.Min(Min, other.Min), LongVector.Min(Max, other.Max));
            yield return (LongVector.Max(Min, other.Min), LongVector.Max(Max, other.Max));
        }

        public static IEnumerable<LongVectorRange> Union(LongVectorRange left, LongVectorRange right) =>
            left.Union(right);

        public static IEnumerable<LongVectorRange> operator |(LongVectorRange left, LongVectorRange right) =>
            left.Union(right);

        public readonly bool Intersect(LongVectorRange other, out LongVectorRange result)
        {
            result = new(LongVector.Max(Min, other.Min), LongVector.Min(Max, other.Max));
            return Overlaps(other);
        }

        public readonly IEnumerable<LongVectorRange> Intersect(LongVectorRange other)
        {
            if (Intersect(other, out LongVectorRange result))
                yield return result;
        }

        public static IEnumerable<LongVectorRange> Intersect(LongVectorRange left, LongVectorRange right) =>
            left.Intersect(right);

        public static IEnumerable<LongVectorRange> operator &(LongVectorRange left, LongVectorRange right) =>
            left.Intersect(right);

        public readonly long GetIndex(LongVector vector) =>
            vector.x - Min.x + Width * (vector.y - Min.y);

        readonly int IIntegerSize<LongVectorRange, LongVector>.GetIndex(LongVector vector) =>
            (int)GetIndex(vector);

        readonly long IIntegerSize<LongVectorRange, LongVector>.GetLongIndex(LongVector vector) =>
            GetIndex(vector);

        public static implicit operator (LongVector min, LongVector max)(LongVectorRange value) =>
            (value.Min, value.Max);

        public static implicit operator LongVectorRange((LongVector min, LongVector max) value) =>
            new(value.min, value.max);

        public static implicit operator LongVectorRange((long min, long max) value) =>
            new(value.min, value.max);

        public static bool operator ==(LongVectorRange left, LongVectorRange right) =>
            left.Equals(right);

        public static bool operator !=(LongVectorRange left, LongVectorRange right) =>
            !left.Equals(right);
    }
}
