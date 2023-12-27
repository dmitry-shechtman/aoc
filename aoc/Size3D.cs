using System;

namespace aoc
{
    public struct Size3D : ISize3D<Size3D, Vector3D, int>
    {
        public readonly int x;
        public readonly int y;
        public readonly int z;

        public Size3D(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Size3D(Size size)
            : this(size.x, size.y, 0)
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
            x == other.x &&
            y == other.y &&
            z == other.z;

        public readonly override int GetHashCode() =>
            HashCode.Combine(x, y, z);

        public readonly int Width  => x;
        public readonly int Height => y;
        public readonly int Depth  => z;

        public readonly int Length =>
            x * y * z;

        public readonly long LongLength =>
            (long)x * y * z;

        public readonly bool Contains(Vector3D vector) =>
            vector.x >= 0 && vector.x < x &&
            vector.y >= 0 && vector.y < y &&
            vector.z >= 0 && vector.z < z;

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
            vector.x + x * (vector.y + y * vector.z);

        public static explicit operator Size3D(Size size) =>
            new(size);

        public static explicit operator Size(Size3D size) =>
            new(size.x, size.y);

        public static explicit operator Vector3D(Size3D size) =>
            new(size.x, size.y, size.z);

        public static bool operator ==(Size3D left, Size3D right) =>
            left.Equals(right);

        public static bool operator !=(Size3D left, Size3D right) =>
            !left.Equals(right);
    }
}
