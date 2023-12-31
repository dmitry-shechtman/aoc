using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace aoc
{
    using Helper = Internal.Size3DHelper<Size3D, Vector3D, int>;

    public struct Size3D : ISize3D<Size3D, Vector3D, int>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromArray, int.TryParse));

        private static Helper Helper => _helper.Value;

        public readonly int width;
        public readonly int height;
        public readonly int depth;

        public Size3D(int width, int height, int depth)
        {
            this.width  = width;
            this.height = height;
            this.depth  = depth;
        }

        public Size3D(Size size)
            : this(size.width, size.height, 0)
        {
        }

        public Size3D(Vector3D vector)
            : this(vector.x, vector.y, vector.z)
        {
        }

        public Size3D(Vector3DRange range)
            : this(range.Max + (1, 1, 1))
        {
        }

        public readonly override bool Equals(object obj) =>
            obj is Size3D other && Equals(other);

        public readonly bool Equals(Size3D other) =>
            width  == other.width &&
            height == other.height &&
            depth  == other.depth;

        public readonly override int GetHashCode() =>
            HashCode.Combine(width, height, depth);

        public readonly override string ToString() =>
            Helper.ToString(this);

        public readonly string ToString(IFormatProvider provider) =>
            Helper.ToString(this, provider);

        public readonly string ToString(string format, IFormatProvider provider = null) =>
            Helper.ToString(this, format, provider);

        public readonly int Width  => width;
        public readonly int Height => height;
        public readonly int Depth  => depth;

        public readonly int Length =>
            width * height * depth;

        public readonly long LongLength =>
            (long)width * height * depth;

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public static Size3D Parse(string s) =>
            Helper.Parse(s);

        public static bool TryParse(string s, out Size3D size) =>
            Helper.TryParse(s, out size);

        public static Size3D Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out Size3D size) =>
            Helper.TryParse(s, separator, out size);

        public static Size3D Parse(string s, string separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, string separator, out Size3D size) =>
            Helper.TryParse(s, separator, out size);

        public static Size3D Parse(string s, Regex separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, Regex separator, out Size3D size) =>
            Helper.TryParse(s, separator, out size);

        public static Size3D Parse(string[] ss) =>
            Helper.Parse(ss);

        public static bool TryParse(string[] ss, out Size3D size) =>
            Helper.TryParse(ss, out size);

        private static Size3D FromArray(int[] values) =>
            new(values[0], values[1], values[2]);

        public readonly bool Contains(Vector3D vector) =>
            vector.x >= 0 && vector.x < width &&
            vector.y >= 0 && vector.y < height &&
            vector.z >= 0 && vector.z < depth;

        public static bool Contains(Size3D size, Vector3D vector) =>
            size.Contains(vector);

        public readonly T GetValue<T>(T[] array, Vector3D vector) =>
            array[GetIndex(vector)];

        public readonly bool TryGetValue<T>(T[] array, Vector3D vector, out T value)
        {
            if (!Contains(vector))
            {
                value = default;
                return false;
            }
            value = GetValue(array, vector);
            return true;
        }

        public readonly T SetValue<T>(T[] array, Vector3D vector, T value) =>
            array[GetIndex(vector)] = value;

        public readonly int GetIndex(Vector3D vector) =>
            vector.x + width * (vector.y + height * vector.z);

        public static Vector3D operator +(Vector3D vector, Size3D size) =>
            new(vector.x + size.width, vector.y + size.height, vector.z + size.depth);

        public static Vector3D operator -(Vector3D vector, Size3D size) =>
            new(vector.x - size.width, vector.y - size.height, vector.z - size.depth);

        public static implicit operator Size3D((int x, int y, int z) value) =>
            new(value.x, value.y, value.z);

        public static explicit operator Size3D(Size size) =>
            new(size);

        public static explicit operator Size(Size3D size) =>
            new(size.width, size.height);

        public static explicit operator Vector3D(Size3D size) =>
            new(size.width, size.height, size.depth);

        public static bool operator ==(Size3D left, Size3D right) =>
            left.Equals(right);

        public static bool operator !=(Size3D left, Size3D right) =>
            !left.Equals(right);
    }
}
