using System;

namespace aoc
{
    public struct Size : ISize2D<Size, Vector, int>
    {
        public readonly int x;
        public readonly int y;

        public Size(int x, int y)
        {
            this.x = x;
            this.y = y;
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
            x == other.x &&
            y == other.y;

        public readonly override bool Equals(object obj) =>
            obj is Size other && Equals(other);

        public readonly override int GetHashCode() =>
            HashCode.Combine(x, y);

        public readonly int Width  => x;
        public readonly int Height => y;

        public readonly int Length =>
            x * y;

        public readonly long LongLength =>
            (long)x * y;

        public readonly bool Contains(Vector vector) =>
            vector.x >= 0 && vector.x < x &&
            vector.y >= 0 && vector.y < y;

        public static bool Contains(Size size, Vector vector) =>
            size.Contains(vector);

        public readonly Vector FindChar(string s, char c) =>
            FromFieldIndex(s.IndexOf(c), x);

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
            vector.x + vector.y * x;

        private readonly int GetFieldIndex(Vector vector) =>
            vector.x + vector.y * (x + 1);

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
            new(size.x, size.y);

        public static bool operator ==(Size left, Size right) =>
            left.Equals(right);

        public static bool operator !=(Size left, Size right) =>
            !left.Equals(right);
    }
}
