using System;

namespace aoc
{
    public struct Size : ISize2D<Size, Vector, int>
    {
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

        public readonly int Width  => width;
        public readonly int Height => height;

        public readonly int Length =>
            width * height;

        public readonly long LongLength =>
            (long)width * height;

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
