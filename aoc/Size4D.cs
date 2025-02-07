﻿using System;

namespace aoc
{
    using Helper = Internal.Size4DHelper<Size4D, Vector4D, int>;

    public readonly struct Size4D : ISize4D<Size4D, Vector4D, int>, IIntegerSize<Size4D, Vector4D>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromSpan, Internal.Int32Helper.Instance));

        private static Helper Helper => _helper.Value;

        public readonly int width;
        public readonly int height;
        public readonly int depth;
        public readonly int anakata;

        public Size4D(int width, int height, int depth, int anakata)
        {
            this.width   = width;
            this.height  = height;
            this.depth   = depth;
            this.anakata = anakata;
        }

        public Size4D(Size size)
            : this(size.width, size.height, 0, 0)
        {
        }

        public Size4D(Size3D size)
            : this(size.width, size.height, size.depth, 0)
        {
        }

        public Size4D(Vector4D vector)
            : this(vector.x, vector.y, vector.z, vector.w)
        {
        }

        public Size4D(Vector4DRange range)
            : this(range.Max + Vector4D.One)
        {
        }

        public readonly bool Equals(Size4D other) =>
            width   == other.width &&
            height  == other.height &&
            depth   == other.depth &&
            anakata == other.anakata;

        public readonly override bool Equals(object? obj) =>
            obj is Size4D other && Equals(other);

        public readonly override int GetHashCode() =>
            HashCode.Combine(width, height, depth, anakata);

        public readonly override string ToString() =>
            Helper.ToString(this);

        public readonly string ToString(IFormatProvider? provider) =>
            Helper.ToString(this, provider);

        public readonly string ToString(string? format, IFormatProvider? provider = null) =>
            Helper.ToString(this, format, provider);

        public readonly int Width   => width;
        public readonly int Height  => height;
        public readonly int Depth   => depth;
        public readonly int Anakata => anakata;

        public readonly int Length =>
            width * height * depth * anakata;

        public readonly long LongLength =>
            (long)width * height * depth * anakata;

        public static Builders.IBuilder<Size4D> Builder => Helper;

        public static Size4D Parse(string? s) =>
            Helper.Parse(s);

        public static bool TryParse(string? s, out Size4D size) =>
            Helper.TryParse(s, out size);

        public static Size4D Parse(string? s, IFormatProvider? provider) =>
            Helper.Parse(s, provider);

        public static bool TryParse(string? s, IFormatProvider? provider, out Size4D size) =>
            Helper.TryParse(s, provider, out size);

        public static Size4D Parse(ReadOnlySpan<char> s) =>
            Helper.Parse(s);

        public static bool TryParse(ReadOnlySpan<char> s, out Size4D size) =>
            Helper.TryParse(s, out size);

        public static Size4D Parse(ReadOnlySpan<char> s, IFormatProvider? provider) =>
            Helper.Parse(s, provider);

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Size4D size) =>
            Helper.TryParse(s, provider, out size);

        private static Size4D FromSpan(ReadOnlySpan<int> values) =>
            new(values[0], values[1], values[2], values[3]);

        public readonly bool Contains(Vector4D vector) =>
            vector.x >= 0 && vector.x < width &&
            vector.y >= 0 && vector.y < height &&
            vector.z >= 0 && vector.z < depth &&
            vector.w >= 0 && vector.w < anakata;

        public static bool Contains(Size4D size, Vector4D vector) =>
            size.Contains(vector);

        public readonly int GetIndex(Vector4D vector) =>
            vector.x + width * (vector.y + height * (vector.z + depth * vector.w));

        public readonly long GetLongIndex(Vector4D vector) =>
            vector.x + (long)width * (vector.y + height * (vector.z + depth * vector.w));

        public static Vector4D operator +(Vector4D vector, Size4D size) =>
            new(vector.x + size.width, vector.y + size.height, vector.z + size.depth, vector.w + size.anakata);

        public static Vector4D operator -(Vector4D vector, Size4D size) =>
            new(vector.x - size.width, vector.y - size.height, vector.z - size.depth, vector.w - size.anakata);

        public static implicit operator Size4D((int x, int y, int z, int w) value) =>
            new(value.x, value.y, value.z, value.w);

        public static explicit operator Size4D(Size size) =>
            new(size);

        public static explicit operator Size4D(Size3D size) =>
            new(size);

        public static explicit operator Size4D(Vector4D vector) =>
            new(vector);

        public static explicit operator Size(Size4D size) =>
            new(size.width, size.height);

        public static explicit operator Size3D(Size4D size) =>
            new(size.width, size.height, size.depth);

        public static explicit operator Vector4D(Size4D size) =>
            new(size.width, size.height, size.depth, size.anakata);

        public static bool operator ==(Size4D left, Size4D right) =>
            left.Equals(right);

        public static bool operator !=(Size4D left, Size4D right) =>
            !left.Equals(right);
    }
}
