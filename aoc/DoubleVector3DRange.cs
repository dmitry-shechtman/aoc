using System;

namespace aoc
{
    public struct DoubleVector3DRange : IRange3D<DoubleVector3DRange, DoubleVector3D, double>
    {
        private static readonly Lazy<VectorRangeHelper<DoubleVector3DRange, DoubleVector3D>> _helper =
            new(() => new(FromArray, DoubleVector3D.TryParse));

        private static VectorRangeHelper<DoubleVector3DRange, DoubleVector3D> Helper => _helper.Value;

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

        public readonly double Width  => Max.x - Min.x;
        public readonly double Height => Max.y - Min.y;
        public readonly double Depth  => Max.z - Min.z;
        public readonly double Count  => Width * Height * Depth;

        public readonly override bool Equals(object obj) =>
            obj is DoubleVector3DRange other && Equals(other);

        public readonly bool Equals(DoubleVector3DRange other) =>
            Min.Equals(other.Min) &&
            Max.Equals(other.Max);

        public readonly override int GetHashCode() =>
            HashCode.Combine(Min, Max);

        public readonly override string ToString() =>
            $"{Min}~{Max}";

        public readonly void Deconstruct(out DoubleVector3D min, out DoubleVector3D max)
        {
            min = Min;
            max = Max;
        }

        readonly double ISize3D<DoubleVector3DRange, DoubleVector3D, double>.Length =>
            Count;

        public static DoubleVector3DRange Parse(string s) =>
            Parse(s, '~');

        public static DoubleVector3DRange Parse(string s, char separator, char separator2 = ',') =>
            Helper.Parse(s, separator, separator2);

        public static bool TryParse(string s, out DoubleVector3DRange range, char separator = '~', char separator2 = ',') =>
            Helper.TryParse(s, out range, separator, separator2);

        public static DoubleVector3DRange Parse(string[] ss) =>
            Parse(ss, ',');

        public static DoubleVector3DRange Parse(string[] ss, char separator) =>
            Helper.Parse(ss, separator);

        public static bool TryParse(string[] ss, out DoubleVector3DRange range, char separator = ',') =>
            Helper.TryParse(ss, out range, separator);

        private static DoubleVector3DRange FromArray(DoubleVector3D[] values) =>
            new(values[0], values[1]);

        public readonly bool IsMatch(DoubleVector3D vector) =>
            vector.x >= Min.x && vector.x <= Max.x &&
            vector.y >= Min.y && vector.y <= Max.y &&
            vector.z >= Min.z && vector.z <= Max.z;

        public readonly bool IsMatch(DoubleVector3DRange other) =>
            other.Min.x <= Max.x && other.Max.x >= Min.x &&
            other.Min.y <= Max.y && other.Max.y >= Min.y &&
            other.Min.z <= Max.z && other.Max.z >= Min.z;

        public readonly bool Contains(DoubleVector3D vector) =>
            IsMatch(vector);

        public readonly bool Contains(DoubleVector3DRange other) =>
            other.Min.x >= Min.x && other.Max.x <= Max.x &&
            other.Min.y >= Min.y && other.Max.y <= Max.y &&
            other.Min.z >= Min.z && other.Max.z <= Max.z;

        public static implicit operator (DoubleVector3D min, DoubleVector3D max)(DoubleVector3DRange value) =>
            (value.Min, value.Max);

        public static implicit operator DoubleVector3DRange((DoubleVector3D min, DoubleVector3D max) value) =>
            new(value.min, value.max);

        public static explicit operator DoubleVectorRange(DoubleVector3DRange range) =>
            new((DoubleVector)range.Min, (DoubleVector)range.Max);

        public static bool operator ==(DoubleVector3DRange left, DoubleVector3DRange right) =>
            left.Equals(right);

        public static bool operator !=(DoubleVector3DRange left, DoubleVector3DRange right) =>
            !left.Equals(right);
    }
}
