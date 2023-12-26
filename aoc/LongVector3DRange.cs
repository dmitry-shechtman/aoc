using System;
using System.Collections.Generic;

using static aoc.ParseHelper;
using static aoc.RangeParseHelper<aoc.LongVector3DRange, aoc.LongVector3D>;

namespace aoc
{
    public struct LongVector3DRange : IIntegerRange<LongVector3DRange, LongVector3D>, IRange3D<LongVector3DRange, LongVector3D, long>
    {
        public LongVector3DRange(LongVector3D min, LongVector3D max)
        {
            Min = min;
            Max = max;
        }

        public LongVector3DRange(LongVector3D max)
            : this(LongVector3D.Zero, max)
        {
        }

        public LongVector3DRange(long x, long y, long z)
            : this(new LongVector3D(x, y, z))
        {
        }

        public LongVector3DRange(LongVectorRange range)
            : this(new(range.Min), new(range.Max))
        {
        }

        public LongVector3D Min { get; }
        public LongVector3D Max { get; }

        public readonly long Width  => Max.x - Min.x + 1;
        public readonly long Height => Max.y - Min.y + 1;
        public readonly long Depth  => Max.z - Min.z + 1;
        public readonly long Count  => Width * Height * Depth;

        public readonly override bool Equals(object obj) =>
            obj is LongVector3DRange other && Equals(other);

        public readonly bool Equals(LongVector3DRange other) =>
            Min.Equals(other.Min) &&
            Max.Equals(other.Max);

        public readonly override int GetHashCode() =>
            HashCode.Combine(Min, Max);

        public readonly override string ToString() =>
            $"{Min}~{Max}";

        public readonly void Deconstruct(out LongVector3D min, out LongVector3D max)
        {
            min = Min;
            max = Max;
        }

        public readonly IEnumerator<LongVector3D> GetEnumerator()
        {
            for (var z = Min.z; z <= Max.z; z++)
                for (var y = Min.y; y <= Max.y; y++)
                    for (var x = Min.x; x <= Max.x; x++)
                        yield return new(x, y, z);
        }

        public readonly LongVector3D this[int index] =>
            new(Min.x + index % Width, Min.y + index / Width % Height, Min.z + index / (Width * Height));

        readonly int IReadOnlyCollection<LongVector3D>.Count =>
            (int)Count;

        readonly long ISize3D<LongVector3DRange, LongVector3D, long>.Length =>
            Count;

        public static LongVector3DRange Parse(string s) =>
            Parse(s, '~');

        public static LongVector3DRange Parse(string s, char separator, char separator2 = ',') =>
            Parse<LongVector3DRange>(s, TryParse, separator, separator2);

        public static bool TryParse(string s, out LongVector3DRange range, char separator = '~', char separator2 = ',') =>
            TryParse<LongVector3DRange>(s, TryParse, separator, separator2, out range);

        public static LongVector3DRange Parse(string[] ss) =>
            Parse(ss, ',');

        public static LongVector3DRange Parse(string[] ss, char separator) =>
            Parse<LongVector3DRange>(ss, TryParse, separator);

        public static bool TryParse(string[] ss, out LongVector3DRange range, char separator = ',') =>
            TryParseRange(ss, LongVector3D.TryParse, FromArray, out range, separator);

        private static LongVector3DRange FromArray(LongVector3D[] values) =>
            new(values[0], values[1]);

        public readonly bool IsMatch(LongVector3D vector) =>
            vector.x >= Min.x && vector.x <= Max.x &&
            vector.y >= Min.y && vector.y <= Max.y &&
            vector.z >= Min.z && vector.z <= Max.z;

        public readonly bool IsMatch(LongVector3DRange other) =>
            other.Min.x <= Max.x && other.Max.x >= Min.x &&
            other.Min.y <= Max.y && other.Max.y >= Min.y &&
            other.Min.z <= Max.z && other.Max.z >= Min.z;

        public readonly bool Contains(LongVector3D vector) =>
            IsMatch(vector);

        public readonly bool Contains(LongVector3DRange other) =>
            other.Min.x >= Min.x && other.Max.x <= Max.x &&
            other.Min.y >= Min.y && other.Max.y <= Max.y &&
            other.Min.z >= Min.z && other.Max.z <= Max.z;

        public static implicit operator (LongVector3D min, LongVector3D max)(LongVector3DRange value) =>
            (value.Min, value.Max);

        public static implicit operator LongVector3DRange((LongVector3D min, LongVector3D max) value) =>
            new(value.min, value.max);

        public static explicit operator LongVectorRange(LongVector3DRange range) =>
            new((LongVector)range.Min, (LongVector)range.Max);

        public static bool operator ==(LongVector3DRange left, LongVector3DRange right) =>
            left.Equals(right);

        public static bool operator !=(LongVector3DRange left, LongVector3DRange right) =>
            !left.Equals(right);
    }
}
