using System;
using System.Collections;
using System.Collections.Generic;

namespace aoc
{
    public struct DoubleVectorRange : IEquatable<DoubleVectorRange>, IReadOnlyList<DoubleVector>
    {
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

        public readonly double Width  => Max.x - Min.x + 1;
        public readonly double Height => Max.y - Min.y + 1;
        public readonly double Count  => Width * Height;

        public readonly override bool Equals(object obj) =>
            obj is DoubleVectorRange other && Equals(other);

        public readonly bool Equals(DoubleVectorRange other) =>
            Min.Equals(other.Min) &&
            Max.Equals(other.Max);

        public readonly override int GetHashCode() =>
            HashCode.Combine(Min, Max);

        public readonly override string ToString() =>
            $"{Min}~{Max}";

        public readonly void Deconstruct(out DoubleVector min, out DoubleVector max)
        {
            min = Min;
            max = Max;
        }

        public readonly IEnumerator<DoubleVector> GetEnumerator()
        {
            for (var y = Min.y; y <= Max.y; y++)
                for (var x = Min.x; x <= Max.x; x++)
                    yield return new(x, y);
        }

        readonly IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public readonly DoubleVector this[int index] =>
            new(Min.x + index % Width, Min.y + index / Width);

        readonly int IReadOnlyCollection<DoubleVector>.Count =>
            (int)Count;

        public static DoubleVectorRange Parse(string s) =>
            Parse(s, '~');

        public static DoubleVectorRange Parse(string s, char separator, char separator2 = ',') =>
            TryParse(s, out DoubleVectorRange range, separator, separator2)
                ? range
                : throw new InvalidOperationException($"Incorrect string format: {s}");

        public static bool TryParse(string s, out DoubleVectorRange range, char separator = '~', char separator2 = ',') =>
            TryParse(s.Trim().Split(separator), out range, separator2);

        public static DoubleVectorRange Parse(string[] ss) =>
            Parse(ss, ',');

        public static DoubleVectorRange Parse(string[] ss, char separator) =>
            TryParse(ss, out DoubleVectorRange range, separator)
                ? range
                : throw new InvalidOperationException($"Input string was not in a correct format.");

        public static bool TryParse(string[] ss, out DoubleVectorRange range, char separator = ',')
        {
            range = default;
            if (ss.Length < 2 ||
                !DoubleVector.TryParse(ss[0], out DoubleVector min, separator) ||
                !DoubleVector.TryParse(ss[1], out DoubleVector max, separator))
                return false;
            range = new(min, max);
            return true;
        }

        public readonly bool IsMatch(DoubleVector vector) =>
            vector.x >= Min.x && vector.x <= Max.x &&
            vector.y >= Min.y && vector.y <= Max.y;

        public readonly bool IsMatch(DoubleVectorRange other) =>
            other.Min.x <= Max.x && other.Max.x >= Min.x &&
            other.Min.y <= Max.y && other.Max.y >= Min.y;

        public readonly bool Contains(DoubleVector vector) =>
            IsMatch(vector);

        public readonly bool Contains(DoubleVectorRange other) =>
            other.Min.x >= Min.x && other.Max.x <= Max.x &&
            other.Min.y >= Min.y && other.Max.y <= Max.y;

        public static implicit operator (DoubleVector min, DoubleVector max)(DoubleVectorRange value) =>
            (value.Min, value.Max);

        public static implicit operator DoubleVectorRange((DoubleVector min, DoubleVector max) value) =>
            new(value.min, value.max);

        public static bool operator ==(DoubleVectorRange left, DoubleVectorRange right) =>
            left.Equals(right);

        public static bool operator !=(DoubleVectorRange left, DoubleVectorRange right) =>
            !left.Equals(right);
    }
}
