using System;
using System.Collections.Generic;

namespace aoc
{
    public struct LongVectorRange : IIntegerRange<LongVectorRange, LongVector>, ISize2D<LongVectorRange, LongVector, long>
    {
        private static readonly Lazy<VectorRangeHelper<LongVectorRange, LongVector>> _helper =
            new(() => new(FromArray, LongVector.TryParse));

        private static VectorRangeHelper<LongVectorRange, LongVector> Helper => _helper.Value;

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

        public readonly long Width  => Max.x - Min.x + 1;
        public readonly long Height => Max.y - Min.y + 1;
        public readonly long Count  => Width * Height;

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
            (int)Count;

        readonly long ISize<LongVectorRange, LongVector, long>.Length =>
            Count;

        public static LongVectorRange Parse(string s) =>
            Parse(s, '~');

        public static LongVectorRange Parse(string s, char separator, char separator2 = ',') =>
            Helper.Parse(s, separator, separator2);

        public static bool TryParse(string s, out LongVectorRange range, char separator = '~', char separator2 = ',') =>
            Helper.TryParse(s, out range, separator, separator2);

        public static LongVectorRange Parse(string[] ss) =>
            Parse(ss, ',');

        public static LongVectorRange Parse(string[] ss, char separator) =>
            Helper.Parse(ss, separator);

        public static bool TryParse(string[] ss, out LongVectorRange range, char separator = ',') =>
            Helper.TryParse(ss, out range, separator);

        private static LongVectorRange FromArray(LongVector[] values) =>
            new(values[0], values[1]);

        public readonly bool IsMatch(LongVector vector) =>
            vector.x >= Min.x && vector.x <= Max.x &&
            vector.y >= Min.y && vector.y <= Max.y;

        public readonly bool IsMatch(LongVectorRange other) =>
            other.Min.x <= Max.x && other.Max.x >= Min.x &&
            other.Min.y <= Max.y && other.Max.y >= Min.y;

        public readonly bool Contains(LongVector vector) =>
            IsMatch(vector);

        public readonly bool Contains(LongVectorRange other) =>
            other.Min.x >= Min.x && other.Max.x <= Max.x &&
            other.Min.y >= Min.y && other.Max.y <= Max.y;

        public static implicit operator (LongVector min, LongVector max)(LongVectorRange value) =>
            (value.Min, value.Max);

        public static implicit operator LongVectorRange((LongVector min, LongVector max) value) =>
            new(value.min, value.max);

        public static bool operator ==(LongVectorRange left, LongVectorRange right) =>
            left.Equals(right);

        public static bool operator !=(LongVectorRange left, LongVectorRange right) =>
            !left.Equals(right);
    }
}
