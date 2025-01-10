using System;
using System.Collections.Generic;

namespace aoc
{
    using Helper = Internal.RangeHelper<DoubleVector3DRange, DoubleVector3D, double>;

    public readonly struct DoubleVector3DRange : IRange3D<DoubleVector3DRange, DoubleVector3D, double>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromSpan, DoubleVector3D.Helper));

        private static Helper Helper => _helper.Value;

        public DoubleVector3DRange(DoubleVector3D min, DoubleVector3D max)
        {
            Min = min;
            Max = max;
        }

        public DoubleVector3DRange(DoubleVector3D max)
            : this(DoubleVector3D.Zero, max)
        {
        }

        public DoubleVector3DRange(double min, double max)
            : this((min, min, min), (max, max, max))
        {
        }

        public DoubleVector3DRange(DoubleVectorRange range)
            : this(new DoubleVector3D(range.Min), new DoubleVector3D(range.Max))
        {
        }

        public DoubleVector3D Min { get; }
        public DoubleVector3D Max { get; }

        public readonly double Width   => Max.x - Min.x;
        public readonly double Height  => Max.y - Min.y;
        public readonly double Depth   => Max.z - Min.z;

        public readonly double Length =>
            Width * Height * Depth;

        public readonly override bool Equals(object? obj) =>
            obj is DoubleVector3DRange other && Equals(other);

        public readonly bool Equals(DoubleVector3DRange other) =>
            Min.Equals(other.Min) &&
            Max.Equals(other.Max);

        public readonly override int GetHashCode() =>
            HashCode.Combine(Min, Max);

        public readonly override string ToString() =>
            Helper.ToString(this);

        public readonly string ToString(IFormatProvider? provider) =>
            Helper.ToString(this, provider);

        public readonly string ToString(string? format, IFormatProvider? provider = null) =>
            Helper.ToString(this, format, provider);

        public readonly void Deconstruct(out DoubleVector3D min, out DoubleVector3D max)
        {
            min = Min;
            max = Max;
        }

        public static Builders.IRangeBuilder<DoubleVector3DRange, DoubleVector3D> Builder =>
            Helper;

        public static DoubleVector3DRange Parse(string? s) =>
            Helper.Parse(s);

        public static bool TryParse(string? s, out DoubleVector3DRange range) =>
            Helper.TryParse(s, out range);

        public static DoubleVector3DRange Parse(string? s, IFormatProvider? provider) =>
            Helper.Parse(s, provider);

        public static bool TryParse(string? s, IFormatProvider? provider, out DoubleVector3DRange range) =>
            Helper.TryParse(s, provider, out range);

        public static DoubleVector3DRange Parse(ReadOnlySpan<char> s) =>
            Helper.Parse(s);

        public static bool TryParse(ReadOnlySpan<char> s, out DoubleVector3DRange range) =>
            Helper.TryParse(s, out range);

        public static DoubleVector3DRange Parse(ReadOnlySpan<char> s, IFormatProvider? provider) =>
            Helper.Parse(s, provider);

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out DoubleVector3DRange range) =>
            Helper.TryParse(s, provider, out range);

        private static DoubleVector3DRange FromSpan(ReadOnlySpan<DoubleVector3D> values) =>
            new(values[0], values[1]);

        public readonly bool Contains(DoubleVector3D vector) =>
            vector.x >= Min.x && vector.x <= Max.x &&
            vector.y >= Min.y && vector.y <= Max.y &&
            vector.z >= Min.z && vector.z <= Max.z;

        public readonly bool Contains(DoubleVector3DRange other) =>
            other.Min.x >= Min.x && other.Max.x <= Max.x &&
            other.Min.y >= Min.y && other.Max.y <= Max.y &&
            other.Min.z >= Min.z && other.Max.z <= Max.z;

        public readonly bool Overlaps(DoubleVector3DRange other) =>
            other.Min.x <= Max.x && other.Max.x >= Min.x &&
            other.Min.y <= Max.y && other.Max.y >= Min.y &&
            other.Min.z <= Max.z && other.Max.z >= Min.z;

        public readonly DoubleVector3DRange Unify(DoubleVector3DRange other) =>
            new(min: DoubleVector3D.Min(Min, other.Min), max: DoubleVector3D.Max(Max, other.Max));

        public readonly IEnumerable<DoubleVector3DRange> Union(DoubleVector3DRange other)
        {
            if (Overlaps(other))
                throw new NotImplementedException();
            yield return (DoubleVector3D.Min(Min, other.Min), DoubleVector3D.Min(Max, other.Max));
            yield return (DoubleVector3D.Max(Min, other.Min), DoubleVector3D.Max(Max, other.Max));
        }

        public static IEnumerable<DoubleVector3DRange> Union(DoubleVector3DRange left, DoubleVector3DRange right) =>
            left.Union(right);

        public static IEnumerable<DoubleVector3DRange> operator |(DoubleVector3DRange left, DoubleVector3DRange right) =>
            left.Union(right);

        public readonly bool Intersect(DoubleVector3DRange other, out DoubleVector3DRange result)
        {
            result = new(DoubleVector3D.Max(Min, other.Min), DoubleVector3D.Min(Max, other.Max));
            return Overlaps(other);
        }

        public readonly IEnumerable<DoubleVector3DRange> Intersect(DoubleVector3DRange other)
        {
            if (Intersect(other, out DoubleVector3DRange result))
                yield return result;
        }

        public static IEnumerable<DoubleVector3DRange> Intersect(DoubleVector3DRange left, DoubleVector3DRange right) =>
            left.Intersect(right);

        public static IEnumerable<DoubleVector3DRange> operator &(DoubleVector3DRange left, DoubleVector3DRange right) =>
            left.Intersect(right);

        public static implicit operator (DoubleVector3D min, DoubleVector3D max)(DoubleVector3DRange value) =>
            (value.Min, value.Max);

        public static implicit operator DoubleVector3DRange((DoubleVector3D min, DoubleVector3D max) value) =>
            new(value.min, value.max);

        public static implicit operator DoubleVector3DRange((double min, double max) value) =>
            new(value.min, value.max);

        public static explicit operator DoubleVectorRange(DoubleVector3DRange range) =>
            new((DoubleVector)range.Min, (DoubleVector)range.Max);

        public static bool operator ==(DoubleVector3DRange left, DoubleVector3DRange right) =>
            left.Equals(right);

        public static bool operator !=(DoubleVector3DRange left, DoubleVector3DRange right) =>
            !left.Equals(right);
    }
}
