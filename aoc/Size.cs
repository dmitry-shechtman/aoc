using System;
using System.Text.RegularExpressions;

namespace aoc
{
    using Helper = Internal.Size2DHelper<Size, Vector, int>;

    public readonly struct Size : ISize2D<Size, Vector, int>, IIntegerSize<Size, Vector>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromSpan, int.TryParse));

        private static Helper Helper => _helper.Value;

        public readonly int width;
        public readonly int height;

        public Size(int width, int height)
        {
            this.width   = width;
            this.height  = height;
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
            width   == other.width &&
            height  == other.height;

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

        public readonly int Width   => width;
        public readonly int Height  => height;

        public readonly int Length =>
            width * height;

        public readonly long LongLength =>
            (long)width * height;

        public static Size Parse(string s) =>
            Helper.Parse(s);

        public static bool TryParse(string s, out Size size) =>
            Helper.TryParse(s, out size);

        public static Size Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out Size size) =>
            Helper.TryParse(s, separator, out size);

        public static Size Parse(string s, string separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, string separator, out Size size) =>
            Helper.TryParse(s, separator, out size);

        public static Size Parse(string s, Regex separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, Regex separator, out Size size) =>
            Helper.TryParse(s, separator, out size);

        private static Size FromSpan(ReadOnlySpan<int> values) =>
            new(values[0], values[1]);

        public readonly bool Contains(Vector vector) =>
            vector.x >= 0 && vector.x < width &&
            vector.y >= 0 && vector.y < height;

        public static bool Contains(Size size, Vector vector) =>
            size.Contains(vector);

        public readonly Vector FindChar(ReadOnlySpan<char> s, char c) =>
            FromFieldIndex(s.IndexOf(c), width);

        public readonly char GetChar(ReadOnlySpan<char> s, Vector vector) =>
            s[GetFieldIndex(vector)];

        public readonly int GetIndex(Vector vector) =>
            vector.x + width * vector.y;

        public readonly long GetLongIndex(Vector vector) =>
            vector.x + (long)width * vector.y;

        private readonly int GetFieldIndex(Vector vector) =>
            GetFieldIndex(vector, width);

        public static Size FromField(ReadOnlySpan<char> s) =>
            FromField(s, GetFieldWidth(s));

        private static Size FromField(ReadOnlySpan<char> s, int width) =>
            new(width, GetFieldHeight(s, width));

        internal static Vector FromFieldIndex(int index, int width) =>
            new(index % (width + 1), index / (width + 1));

        private static int GetFieldWidth(ReadOnlySpan<char> s) =>
            s.IndexOf('\n');

        private static int GetFieldHeight(ReadOnlySpan<char> s, int width) =>
            (s.Length + 1) / (width + 1);

        internal static int GetFieldIndex(Vector vector, int width) =>
            vector.x + (width + 1) * vector.y;

        public static Vector operator +(Vector vector, Size size) =>
            new(vector.x + size.width, vector.y + size.height);

        public static Vector operator -(Vector vector, Size size) =>
            new(vector.x - size.width, vector.y - size.height);

        public static implicit operator Size((int x, int y) value) =>
            new(value.x, value.y);

        public static explicit operator Vector(Size size) =>
            new(size.width, size.height);

        public static bool operator ==(Size left, Size right) =>
            left.Equals(right);

        public static bool operator !=(Size left, Size right) =>
            !left.Equals(right);
    }
}
