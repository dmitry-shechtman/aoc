using System;
using System.Collections;

namespace aoc
{
    public struct Size : ISize2D<Size, Vector, int>
    {
        private static readonly Lazy<Size2DHelper<Size, Vector, int>> _helper =
            new(() => new(FromArray, int.TryParse));

        private static Size2DHelper<Size, Vector, int> Helper => _helper.Value;

        public readonly int width;
        public readonly int height;

        public Size(int width, int height)
        {
            this.width  = width;
            this.height = height;
        }

        public Size(Vector vector)
            : this(vector.x, vector.y)
        {
        }

        public Size(VectorRange range)
            : this(range.Max + (1, 1))
        {
        }

        public readonly bool Equals(Size other) =>
            width  == other.width &&
            height == other.height;

        public readonly override bool Equals(object obj) =>
            obj is Size other && Equals(other);

        public readonly override int GetHashCode() =>
            HashCode.Combine(width, height);

        public readonly override string ToString() =>
            Helper.ToString(this);

        public readonly string ToString(IFormatProvider provider) =>
            Helper.ToString(this, provider);

        public readonly string ToString(string format, IFormatProvider provider = null) =>
            Helper.ToString(this, format, provider);

        public readonly int Width  => width;
        public readonly int Height => height;

        public readonly int Length =>
            width * height;

        public readonly long LongLength =>
            (long)width * height;

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public static Size Parse(string s) =>
            Parse(s, ':');

        public static Size Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, out Size size, char separator = ':') =>
            Helper.TryParse(s, out size, separator);

        public static Size Parse(string[] ss) =>
            Helper.Parse(ss);

        public static bool TryParse(string[] ss, out Size size) =>
            Helper.TryParse(ss, out size);

        private static Size FromArray(int[] values) =>
            new(values[0], values[1]);

        public readonly bool Contains(Vector vector) =>
            vector.x >= 0 && vector.x < width &&
            vector.y >= 0 && vector.y < height;

        public static bool Contains(Size size, Vector vector) =>
            size.Contains(vector);

        public readonly Vector FindChar(string s, char c) =>
            FromFieldIndex(s.IndexOf(c), width);

        public readonly char GetChar(string s, Vector vector) =>
            s[GetFieldIndex(vector)];

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
            vector.x + vector.y * width;

        private readonly int GetFieldIndex(Vector vector) =>
            vector.x + vector.y * (width + 1);

        public static Size FromField(string s) =>
            FromField(s, GetFieldWidth(s));

        private static Size FromField(string s, int width) =>
            new(width, GetFieldHeight(s, width));

        private static Vector FromFieldIndex(int index, int width) =>
            new(index % (width + 1), index / (width + 1));

        private static int GetFieldWidth(string s) =>
            s.IndexOf('\n');

        private static int GetFieldHeight(string s, int width) =>
            (s.Length + 1) / (width + 1);

        public static explicit operator Vector(Size size) =>
            new(size.width, size.height);

        public static bool operator ==(Size left, Size right) =>
            left.Equals(right);

        public static bool operator !=(Size left, Size right) =>
            !left.Equals(right);
    }
}
