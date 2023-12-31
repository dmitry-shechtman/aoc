using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace aoc
{
    using Helper = Internal.VectorRangeHelper<LongVector3DRange, LongVector3D>;

    public struct LongVector3DRange : IIntegerRange<LongVector3DRange, LongVector3D>, IRange3D<LongVector3DRange, LongVector3D, long>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromArray, LongVector3D.Helper));

        private static Helper Helper => _helper.Value;

        public LongVector3DRange(LongVector3D min, LongVector3D max)
        {
            Min = min;
            Max = max;
        }

        public LongVector3DRange(LongVector3D max)
            : this(LongVector3D.Zero, max)
        {
        }

        public LongVector3DRange(long min, long max)
            : this((min, min, min), (max, max, max))
        {
        }

        public LongVector3DRange(LongVectorRange range)
            : this(new LongVector3D(range.Min), new LongVector3D(range.Max))
        {
        }

        public LongVector3D Min { get; }
        public LongVector3D Max { get; }

        public readonly long Width  => Max.x - Min.x + 1;
        public readonly long Height => Max.y - Min.y + 1;
        public readonly long Depth  => Max.z - Min.z + 1;
        public readonly long Length => Width * Height * Depth;

        public readonly override bool Equals(object obj) =>
            obj is LongVector3DRange other && Equals(other);

        public readonly bool Equals(LongVector3DRange other) =>
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
            (int)Length;

        public static LongVector3DRange Parse(string s) =>
            Helper.Parse(s);

        public static bool TryParse(string s, out LongVector3DRange range) =>
            Helper.TryParse(s, out range);

        public static LongVector3DRange Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out LongVector3DRange range) =>
            Helper.TryParse(s, separator, out range);

        public static LongVector3DRange Parse(string s, string separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, string separator, out LongVector3DRange range) =>
            Helper.TryParse(s, separator, out range);

        public static LongVector3DRange Parse(string s, Regex separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, Regex separator, out LongVector3DRange range) =>
            Helper.TryParse(s, separator, out range);

        public static LongVector3DRange Parse(string s, char separator, char separator2) =>
            Helper.Parse(s, separator, separator2);

        public static bool TryParse(string s, char separator, char separator2, out LongVector3DRange range) =>
            Helper.TryParse(s, separator, separator2, out range);

        public static LongVector3DRange Parse(string[] ss) =>
            Helper.Parse(ss);

        public static bool TryParse(string[] ss, out LongVector3DRange range) =>
            Helper.TryParse(ss, out range);

        public static LongVector3DRange Parse(string[] ss, char separator) =>
            Helper.Parse(ss, separator);

        public static bool TryParse(string[] ss, char separator, out LongVector3DRange range) =>
            Helper.TryParse(ss, separator, out range);

        private static LongVector3DRange FromArray(LongVector3D[] values) =>
            new(values[0], values[1]);

        public readonly bool Contains(LongVector3D vector) =>
            vector.x >= Min.x && vector.x <= Max.x &&
            vector.y >= Min.y && vector.y <= Max.y &&
            vector.z >= Min.z && vector.z <= Max.z;

        public readonly bool Contains(LongVector3DRange other) =>
            other.Min.x >= Min.x && other.Max.x <= Max.x &&
            other.Min.y >= Min.y && other.Max.y <= Max.y &&
            other.Min.z >= Min.z && other.Max.z <= Max.z;

        public readonly bool Overlaps(LongVector3DRange other) =>
            other.Min.x <= Max.x && other.Max.x >= Min.x &&
            other.Min.y <= Max.y && other.Max.y >= Min.y &&
            other.Min.z <= Max.z && other.Max.z >= Min.z;

        public readonly bool OverlapsOrAdjacentTo(LongVector3DRange other) =>
            other.Min.x - 1 <= Max.x && other.Max.x + 1 >= Min.x &&
            other.Min.y - 1 <= Max.y && other.Max.y + 1 >= Min.y &&
            other.Min.z - 1 <= Max.z && other.Max.z + 1 >= Min.z;

        public readonly LongVector3DRange Unify(LongVector3DRange other) =>
            new(min: LongVector3D.Min(Min, other.Min), max: LongVector3D.Max(Max, other.Max));

        public readonly IEnumerable<LongVector3DRange> Union(LongVector3DRange other)
        {
            if (OverlapsOrAdjacentTo(other))
                throw new NotImplementedException();
            else
            {
                yield return (LongVector3D.Min(Min, other.Min), LongVector3D.Min(Max, other.Max));
                yield return (LongVector3D.Max(Min, other.Min), LongVector3D.Max(Max, other.Max));
            }
        }

        public static IEnumerable<LongVector3DRange> Union(LongVector3DRange left, LongVector3DRange right) =>
            left.Union(right);

        public static IEnumerable<LongVector3DRange> operator |(LongVector3DRange left, LongVector3DRange right) =>
            left.Union(right);

        public readonly bool Intersect(LongVector3DRange other, out LongVector3DRange result)
        {
            result = new(LongVector3D.Max(Min, other.Min), LongVector3D.Min(Max, other.Max));
            return Overlaps(other);
        }

        public readonly IEnumerable<LongVector3DRange> Intersect(LongVector3DRange other)
        {
            if (Intersect(other, out LongVector3DRange result))
                yield return result;
        }

        public static IEnumerable<LongVector3DRange> Intersect(LongVector3DRange left, LongVector3DRange right) =>
            left.Intersect(right);

        public static IEnumerable<LongVector3DRange> operator &(LongVector3DRange left, LongVector3DRange right) =>
            left.Intersect(right);

        public static implicit operator (LongVector3D min, LongVector3D max)(LongVector3DRange value) =>
            (value.Min, value.Max);

        public static implicit operator LongVector3DRange((LongVector3D min, LongVector3D max) value) =>
            new(value.min, value.max);

        public static implicit operator LongVector3DRange((long min, long max) value) =>
            new(value.min, value.max);

        public static explicit operator LongVectorRange(LongVector3DRange range) =>
            new((LongVector)range.Min, (LongVector)range.Max);

        public static bool operator ==(LongVector3DRange left, LongVector3DRange right) =>
            left.Equals(right);

        public static bool operator !=(LongVector3DRange left, LongVector3DRange right) =>
            !left.Equals(right);
    }
}
