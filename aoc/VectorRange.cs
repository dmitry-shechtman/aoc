using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace aoc
{
    using Helper = Internal.VectorRangeHelper<VectorRange, Vector>;

    public struct VectorRange : IIntegerRange<VectorRange, Vector>, IRange2D<VectorRange, Vector, int>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromArray, Vector.Helper));

        private static Helper Helper => _helper.Value;

        public VectorRange(Vector min, Vector max)
        {
            Min = min;
            Max = max;
        }

        public VectorRange(Vector max)
            : this(Vector.Zero, max)
        {
        }

        public VectorRange(Size size)
            : this((Vector)size - (1, 1))
        {
        }

        public VectorRange(int min, int max)
            : this((min, min), (max, max))
        {
        }

        public Vector Min { get; }
        public Vector Max { get; }

        public readonly int Width  => Max.x - Min.x + 1;
        public readonly int Height => Max.y - Min.y + 1;
        public readonly int Length  => Width * Height;

        public readonly long LongLength =>
            (long)Width * Height;

        public readonly override bool Equals(object obj) =>
            obj is VectorRange other && Equals(other);

        public readonly bool Equals(VectorRange other) =>
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

        public readonly void Deconstruct(out Vector min, out Vector max)
        {
            min = Min;
            max = Max;
        }

        public readonly IEnumerator<Vector> GetEnumerator()
        {
            for (var y = Min.y; y <= Max.y; y++)
                for (var x = Min.x; x <= Max.x; x++)
                    yield return new(x, y);
        }

        readonly int IReadOnlyCollection<Vector>.Count =>
            Length;

        public readonly Vector this[int index] =>
            new(Min.x + index % Width, Min.y + index / Width);

        public static VectorRange Parse(string s) =>
            Helper.Parse(s);

        public static bool TryParse(string s, out VectorRange range) =>
            Helper.TryParse(s, out range);

        public static VectorRange Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out VectorRange range) =>
            Helper.TryParse(s, separator, out range);

        public static VectorRange Parse(string s, string separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, string separator, out VectorRange range) =>
            Helper.TryParse(s, separator, out range);

        public static VectorRange Parse(string s, Regex separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, Regex separator, out VectorRange range) =>
            Helper.TryParse(s, separator, out range);

        public static VectorRange Parse(string s, char separator, char separator2) =>
            Helper.Parse(s, separator, separator2);

        public static bool TryParse(string s, char separator, char separator2, out VectorRange range) =>
            Helper.TryParse(s, separator, separator2, out range);

        public static VectorRange Parse(string[] ss) =>
            Helper.Parse(ss);

        public static bool TryParse(string[] ss, out VectorRange range) =>
            Helper.TryParse(ss, out range);

        public static VectorRange Parse(string[] ss, char separator) =>
            Helper.Parse(ss, separator);

        public static bool TryParse(string[] ss, char separator, out VectorRange range) =>
            Helper.TryParse(ss, separator, out range);

        private static VectorRange FromArray(Vector[] values) =>
            new(values[0], values[1]);

        public readonly bool Contains(Vector vector) =>
            vector.x >= Min.x && vector.x <= Max.x &&
            vector.y >= Min.y && vector.y <= Max.y;

        public readonly bool Contains(VectorRange other) =>
            other.Min.x >= Min.x && other.Max.x <= Max.x &&
            other.Min.y >= Min.y && other.Max.y <= Max.y;

        public readonly bool Overlaps(VectorRange other) =>
            other.Min.x <= Max.x && other.Max.x >= Min.x &&
            other.Min.y <= Max.y && other.Max.y >= Min.y;

        public readonly bool OverlapsOrAdjacentTo(VectorRange other) =>
            other.Min.x - 1 <= Max.x && other.Max.x + 1 >= Min.x &&
            other.Min.y - 1 <= Max.y && other.Max.y + 1 >= Min.y;

        public readonly VectorRange Unify(VectorRange other) =>
            new(min: Vector.Min(Min, other.Min), max: Vector.Max(Max, other.Max));

        public readonly IEnumerable<VectorRange> Union(VectorRange other)
        {
            if (OverlapsOrAdjacentTo(other))
                throw new NotImplementedException();
            else
            {
                yield return (Vector.Min(Min, other.Min), Vector.Min(Max, other.Max));
                yield return (Vector.Max(Min, other.Min), Vector.Max(Max, other.Max));
            }
        }

        public static IEnumerable<VectorRange> Union(VectorRange left, VectorRange right) =>
            left.Union(right);

        public static IEnumerable<VectorRange> operator |(VectorRange left, VectorRange right) =>
            left.Union(right);

        public readonly bool Intersect(VectorRange other, out VectorRange result)
        {
            result = new(Vector.Max(Min, other.Min), Vector.Min(Max, other.Max));
            return Overlaps(other);
        }

        public readonly IEnumerable<VectorRange> Intersect(VectorRange other)
        {
            if (Intersect(other, out VectorRange result))
                yield return result;
        }

        public static IEnumerable<VectorRange> Intersect(VectorRange left, VectorRange right) =>
            left.Intersect(right);

        public static IEnumerable<VectorRange> operator &(VectorRange left, VectorRange right) =>
            left.Intersect(right);

        public readonly VectorRange Add(Vector vector) =>
            new(Min + vector, Max + vector);

        public static VectorRange Add(VectorRange range, Vector vector) =>
            range.Add(vector);

        public static VectorRange operator +(VectorRange range, Vector vector) =>
            range.Add(vector);

        public readonly VectorRange Sub(Vector vector) =>
            new(Min - vector, Max - vector);

        public static VectorRange Sub(VectorRange range, Vector vector) =>
            range.Sub(vector);

        public static VectorRange operator -(VectorRange range, Vector vector) =>
            Sub(range, vector);

        public readonly VectorRange Grow(Size size) =>
            new(Min - size, Max + size);

        public static VectorRange Grow(VectorRange range, Size size) =>
            range.Grow(size);

        public readonly VectorRange Grow(int size) =>
            Grow((size, size));

        public static VectorRange Grow(VectorRange range, int size) =>
            range.Grow(size);

        public readonly VectorRange Shrink(Size size) =>
            new(Min + size, Max - size);

        public static VectorRange Shrink(VectorRange range, Size size) =>
            range.Shrink(size);

        public readonly VectorRange Shrink(int size) =>
            Shrink((size, size));

        public static VectorRange Shrink(VectorRange range, int size) =>
            range.Shrink(size);

        public readonly IEnumerable<Vector> Border(int size = 1) =>
            System.Linq.Enumerable.Except(this, Shrink(size));

        public static IEnumerable<Vector> Border(VectorRange range, int size = 1) =>
            range.Border(size);

        public readonly VectorRange Mul(Matrix matrix) =>
            new(Min * matrix, Max * matrix);

        public static VectorRange Mul(VectorRange range, Matrix matrix) =>
            range.Mul(matrix);

        public static VectorRange operator *(VectorRange range, Matrix matrix) =>
            range.Mul(matrix);

        public readonly VectorRange SplitWest(int width) =>
            new(Min, new(Min.x + width, Max.y));

        public readonly VectorRange SplitEast(int width) =>
            new(new(Min.x + width, Min.y), Max);

        public readonly VectorRange SplitNorth(int height) =>
            new(Min, new(Max.x, Min.y + height));

        public readonly VectorRange SplitSouth(int height) =>
            new(new(Min.x, Min.y + height), Max);

        public readonly T GetValue<T>(T[] array, Vector vector) =>
            array[GetIndex(vector)];

        public readonly bool TryGetValue<T>(T[] array, Vector vector, out T value)
        {
            if (!Contains(vector))
            {
                value = default;
                return false;
            }
            value = GetValue(array, vector);
            return true;
        }

        public readonly T SetValue<T>(T[] array, Vector vector, T value) =>
            array[GetIndex(vector)] = value;

        public readonly int GetIndex(Vector vector) =>
            vector.x + vector.y * Width;

        public static VectorRange FromField(string s) =>
            new(Size.FromField(s));

        public static implicit operator (Vector min, Vector max)(VectorRange value) =>
            (value.Min, value.Max);

        public static implicit operator VectorRange((Vector min, Vector max) value) =>
            new(value.min, value.max);

        public static implicit operator VectorRange((int min, int max) value) =>
            new(value.min, value.max);

        public static bool operator ==(VectorRange left, VectorRange right) =>
            left.Equals(right);

        public static bool operator !=(VectorRange left, VectorRange right) =>
            !left.Equals(right);
    }
}
