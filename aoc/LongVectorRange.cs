using System;
using System.Collections.Generic;

namespace aoc
{
    public struct LongVectorRange : IIntegerRange<LongVectorRange, LongVector>
    {
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
            $"{Min}~{Max}";

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

        public static LongVectorRange Parse(string s) =>
            Parse(s, '~');

        public static LongVectorRange Parse(string s, char separator, char separator2 = ',') =>
            TryParse(s, out LongVectorRange range, separator, separator2)
                ? range
                : throw new InvalidOperationException($"Incorrect string format: {s}");

        public static bool TryParse(string s, out LongVectorRange range, char separator = '~', char separator2 = ',') =>
            TryParse(s.Trim().Split(separator), out range, separator2);

        public static LongVectorRange Parse(string[] ss) =>
            Parse(ss, ',');

        public static LongVectorRange Parse(string[] ss, char separator) =>
            TryParse(ss, out LongVectorRange range, separator)
                ? range
                : throw new InvalidOperationException($"Input string was not in a correct format.");

        public static bool TryParse(string[] ss, out LongVectorRange range, char separator = ',')
        {
            range = default;
            if (ss.Length < 2 ||
                !LongVector.TryParse(ss[0], out LongVector min, separator) ||
                !LongVector.TryParse(ss[1], out LongVector max, separator))
                return false;
            range = new(min, max);
            return true;
        }

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
