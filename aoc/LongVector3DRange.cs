using System;
using System.Collections;
using System.Collections.Generic;

namespace aoc
{
    public struct LongVector3DRange : IEquatable<LongVector3DRange>, IEnumerable<LongVector3D>
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

        public long Width  => Max.x - Min.x + 1;
        public long Height => Max.y - Min.y + 1;
        public long Depth  => Max.z - Min.z + 1;
        public long Count  => Width * Height * Depth;

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

        readonly IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public static LongVector3DRange Parse(string s) =>
            Parse(s, '~');

        public static LongVector3DRange Parse(string s, char separator, char separator2 = ',') =>
            TryParse(s, out LongVector3DRange range, separator, separator2)
                ? range
                : throw new InvalidOperationException($"Incorrect string format: {s}");

        public static bool TryParse(string s, out LongVector3DRange range, char separator = '~', char separator2 = ',') =>
            TryParse(s.Trim().Split(separator), out range, separator2);

        public static LongVector3DRange Parse(string[] ss) =>
            Parse(ss, ',');

        public static LongVector3DRange Parse(string[] ss, char separator) =>
            TryParse(ss, out LongVector3DRange range, separator)
                ? range
                : throw new InvalidOperationException($"Input string was not in a correct format.");

        public static bool TryParse(string[] ss, out LongVector3DRange range, char separator = ',')
        {
            range = default;
            if (ss.Length < 2 ||
                !LongVector3D.TryParse(ss[0], out LongVector3D min, separator) ||
                !LongVector3D.TryParse(ss[1], out LongVector3D max, separator))
                return false;
            range = new(min, max);
            return true;
        }

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
