using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace aoc
{
    using Helper = Internal.RangeHelper<Vector4DRange, Vector4D, int>;

    public readonly struct Vector4DRange : IIntegerRange<Vector4DRange, Vector4D>, IRange4D<Vector4DRange, Vector4D, int>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromSpan, Vector4D.Helper));

        private static Helper Helper => _helper.Value;

        public Vector4DRange(Vector4D min, Vector4D max)
        {
            Min = min;
            Max = max;
        }

        public Vector4DRange(Vector4D max)
            : this(Vector4D.Zero, max)
        {
        }

        public Vector4DRange(Size4D size)
            : this((Vector4D)size - (1, 1, 1, 1))
        {
        }

        public Vector4DRange(int min, int max)
            : this((min, min, min, min), (max, max, max, max))
        {
        }

        public Vector4D Min { get; }
        public Vector4D Max { get; }

        public readonly int Width   => Max.x - Min.x + 1;
        public readonly int Height  => Max.y - Min.y + 1;
        public readonly int Depth   => Max.z - Min.z + 1;
        public readonly int Anakata => Max.w - Min.w + 1;

        public readonly int Length =>
            Width * Height * Depth * Anakata;

        public readonly long LongLength =>
            (long)Width * Height * Depth * Anakata;

        public readonly override bool Equals(object obj) =>
            obj is Vector4DRange other && Equals(other);

        public readonly bool Equals(Vector4DRange other) =>
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

        public readonly void Deconstruct(out Vector4D min, out Vector4D max)
        {
            min = Min;
            max = Max;
        }

        public readonly IEnumerator<Vector4D> GetEnumerator()
        {
            for (var w = Min.w; w <= Max.w; w++)
                for (var z = Min.z; z <= Max.z; z++)
                    for (var y = Min.y; y <= Max.y; y++)
                        for (var x = Min.x; x <= Max.x; x++)
                            yield return new(x, y, z, w);
        }

        readonly int IReadOnlyCollection<Vector4D>.Count =>
            Length;

        public readonly Vector4D this[int index] =>
            new(Min.x + index % Width,
                Min.y + index / Width % Height,
                Min.z + index / (Width * Height) % Depth,
                Min.w + index / (Width * Height * Depth));

        public static Vector4DRange Parse(string s) =>
            Helper.Parse(s);

        public static bool TryParse(string s, out Vector4DRange range) =>
            Helper.TryParse(s, out range);

        public static Vector4DRange Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out Vector4DRange range) =>
            Helper.TryParse(s, separator, out range);

        public static Vector4DRange Parse(string s, string separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, string separator, out Vector4DRange range) =>
            Helper.TryParse(s, separator, out range);

        public static Vector4DRange Parse(string s, Regex separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, Regex separator, out Vector4DRange range) =>
            Helper.TryParse(s, separator, out range);

        public static Vector4DRange Parse(string s, char separator, char separator2) =>
            Helper.Parse(s, separator, separator2);

        public static bool TryParse(string s, char separator, char separator2, out Vector4DRange range) =>
            Helper.TryParse(s, separator, separator2, out range);

        private static Vector4DRange FromSpan(ReadOnlySpan<Vector4D> values) =>
            new(values[0], values[1]);

        public readonly bool Contains(Vector4D vector) =>
            vector.x >= Min.x && vector.x <= Max.x &&
            vector.y >= Min.y && vector.y <= Max.y &&
            vector.z >= Min.z && vector.z <= Max.z &&
            vector.w >= Min.w && vector.w <= Max.w;

        public readonly bool Contains(Vector4DRange other) =>
            other.Min.x >= Min.x && other.Max.x <= Max.x &&
            other.Min.y >= Min.y && other.Max.y <= Max.y &&
            other.Min.z >= Min.z && other.Max.z <= Max.z &&
            other.Min.w >= Min.w && other.Max.w <= Max.w;

        public readonly bool Overlaps(Vector4DRange other) =>
            other.Min.x <= Max.x && other.Max.x >= Min.x &&
            other.Min.y <= Max.y && other.Max.y >= Min.y &&
            other.Min.z <= Max.z && other.Max.z >= Min.z &&
            other.Min.w <= Max.w && other.Max.w >= Min.w;

        public readonly bool OverlapsOrAdjacentTo(Vector4DRange other) =>
            other.Min.x - 1 <= Max.x && other.Max.x + 1 >= Min.x &&
            other.Min.y - 1 <= Max.y && other.Max.y + 1 >= Min.y &&
            other.Min.z - 1 <= Max.z && other.Max.z + 1 >= Min.z &&
            other.Min.w - 1 <= Max.w && other.Max.w + 1 >= Min.w;

        public readonly Vector4DRange Unify(Vector4DRange other) =>
            new(min: Vector4D.Min(Min, other.Min), max: Vector4D.Max(Max, other.Max));

        public readonly IEnumerable<Vector4DRange> Union(Vector4DRange other)
        {
            if (OverlapsOrAdjacentTo(other))
                throw new NotImplementedException();
            yield return (Vector4D.Min(Min, other.Min), Vector4D.Min(Max, other.Max));
            yield return (Vector4D.Max(Min, other.Min), Vector4D.Max(Max, other.Max));
        }

        public static IEnumerable<Vector4DRange> Union(Vector4DRange left, Vector4DRange right) =>
            left.Union(right);

        public static IEnumerable<Vector4DRange> operator |(Vector4DRange left, Vector4DRange right) =>
            left.Union(right);

        public readonly bool Intersect(Vector4DRange other, out Vector4DRange result)
        {
            result = new(Vector4D.Max(Min, other.Min), Vector4D.Min(Max, other.Max));
            return Overlaps(other);
        }

        public readonly IEnumerable<Vector4DRange> Intersect(Vector4DRange other)
        {
            if (Intersect(other, out Vector4DRange result))
                yield return result;
        }

        public static IEnumerable<Vector4DRange> Intersect(Vector4DRange left, Vector4DRange right) =>
            left.Intersect(right);

        public static IEnumerable<Vector4DRange> operator &(Vector4DRange left, Vector4DRange right) =>
            left.Intersect(right);

        public readonly Vector4DRange Add(Vector4D vector) =>
            (Min + vector, Max + vector);

        public static Vector4DRange Add(Vector4DRange range, Vector4D vector) =>
            range.Add(vector);

        public static Vector4DRange operator +(Vector4DRange range, Vector4D vector) =>
            range.Add(vector);

        public readonly Vector4DRange Sub(Vector4D vector) =>
            (Min - vector, Max - vector);

        public static Vector4DRange Sub(Vector4DRange range, Vector4D vector) =>
            range.Sub(vector);

        public static Vector4DRange operator -(Vector4DRange range, Vector4D vector) =>
            Sub(range, vector);

        public readonly Vector4DRange Grow(Size4D size) =>
            new(Min - size, Max + size);

        public static Vector4DRange Grow(Vector4DRange range, Size4D size) =>
            range.Grow(size);

        public readonly Vector4DRange Grow(int size) =>
            Grow((size, size, size, size));

        public static Vector4DRange Grow(Vector4DRange range, int size) =>
            range.Grow(size);

        public readonly Vector4DRange Shrink(Size4D size) =>
            new(Min + size, Max - size);

        public static Vector4DRange Shrink(Vector4DRange range, Size4D size) =>
            range.Shrink(size);

        public readonly Vector4DRange Shrink(int size) =>
            Shrink((size, size, size, size));

        public static Vector4DRange Shrink(Vector4DRange range, int size) =>
            range.Shrink(size);

        public readonly IEnumerable<Vector4D> Border(int size = 1) =>
            size > 0
                ? this.Except(Shrink(size))
                : Shrink(size).Except(this);

        public static IEnumerable<Vector4D> Border(Vector4DRange range, int size = 1) =>
            range.Border(size);

        public readonly Vector4DRange SplitWest(int width) =>
            new(Min, new(Min.x + width, Max.y, Max.z, Max.w));

        public readonly Vector4DRange SplitEast(int width) =>
            new(new(Min.x + width, Min.y, Min.z, Min.w), Max);

        public readonly Vector4DRange SplitNorth(int height) =>
            new(Min, new(Max.x, Min.y + height, Max.z, Max.w));

        public readonly Vector4DRange SplitSouth(int height) =>
            new(new(Min.x, Min.y + height, Min.z, Min.w), Max);

        public readonly Vector4DRange SplitUp(int depth) =>
            new(Min, new(Max.x, Max.y, Min.z + depth, Max.w));

        public readonly Vector4DRange SplitDown(int depth) =>
            new(new(Min.x, Min.y, Min.z + depth, Min.w), Max);

        public readonly Vector4DRange SplitAna(int anakata) =>
            new(Min, new(Max.x, Max.y, Max.z, Min.w + anakata));

        public readonly Vector4DRange SplitKata(int anakata) =>
            new(new(Min.x, Min.y, Min.z, Min.w + anakata), Max);

        public readonly int GetIndex(Vector4D vector) =>
            vector.x - Min.x + Width * (vector.y - Min.y + Height * (vector.z - Min.z + Depth * (vector.w - Min.w)));

        public readonly long GetLongIndex(Vector4D vector) =>
            vector.x - Min.x + (long)Width * (vector.y - Min.y + Height * (vector.z - Min.z + Depth * (vector.w - Min.w)));

        public static implicit operator (Vector4D min, Vector4D max)(Vector4DRange value) =>
            (value.Min, value.Max);

        public static implicit operator Vector4DRange((Vector4D min, Vector4D max) value) =>
            new(value.min, value.max);

        public static implicit operator Vector4DRange((int min, int max) value) =>
            new(value.min, value.max);

        public static explicit operator VectorRange(Vector4DRange range) =>
            new((Vector)range.Min, (Vector)range.Max);

        public static bool operator ==(Vector4DRange left, Vector4DRange right) =>
            left.Equals(right);

        public static bool operator !=(Vector4DRange left, Vector4DRange right) =>
            !left.Equals(right);
    }
}
