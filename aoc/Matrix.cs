using System;

namespace aoc
{
    public struct Matrix : IEquatable<Matrix>
    {
        public static readonly Matrix Zero             = default;
        public static readonly Matrix Identity         = new( 1,  0,  0,  1);
        public static readonly Matrix RotateRight      = new( 0,  1, -1,  0);
        public static readonly Matrix RotateLeft       = new( 0, -1,  1,  0);
        public static readonly Matrix MirrorHorizontal = new( 1,  0,  0, -1);
        public static readonly Matrix MirrorVertical   = new(-1,  0,  0,  1);
        public static readonly Matrix Flip             = new(-1,  0,  0, -1);

        public readonly int m11;
        public readonly int m12;
        public readonly int m21;
        public readonly int m22;
        public readonly int m31;
        public readonly int m32;

        public Matrix(int m11, int m12, int m21, int m22, int m31 = 0, int m32 = 0)
        {
            this.m11 = m11;
            this.m12 = m12;
            this.m21 = m21;
            this.m22 = m22;
            this.m31 = m31;
            this.m32 = m32;
        }

        public Matrix(int[] m)
            : this(m[0], m[1], m[2], m[3])
        {
        }

        public Matrix(int m)
        {
            m11 = (m & 3) - 1;
            m12 = ((m >> 2) & 3) - 1;
            m21 = ((m >> 4) & 3) - 1;
            m22 = ((m >> 6) & 3) - 1;
            m31 = m32 = 0;
        }

        public readonly override bool Equals(object obj) =>
            obj is Matrix matrix && Equals(matrix);

        public readonly bool Equals(Matrix other) =>
            m11 == other.m11 &&
            m12 == other.m12 &&
            m21 == other.m21 &&
            m22 == other.m22 &&
            m31 == other.m31 &&
            m32 == other.m32;

        public readonly override int GetHashCode() =>
            HashCode.Combine(m11, m12, m21, m22, m31, m32);

        public static Matrix Translate(int x, int y) =>
            new(1, 0, 0, 1, x, y);

        public static Matrix Translate(Vector v) =>
            new(1, 0, 0, 1, v.x, v.y);

        public static Matrix Rotate(int degrees) => degrees switch
        {
            0           => Identity,
            90  or -270 => RotateRight,
            180 or -180 => Flip,
            270 or  -90 => RotateLeft,
            _ => throw new(),
        };

        public static implicit operator Matrix((int m11, int m12, int m21, int m22) m) =>
            new(m.m11, m.m12, m.m21, m.m22);

        public static implicit operator Matrix((int m11, int m12, int m21, int m22, int m31, int m32) m) =>
            new(m.m11, m.m12, m.m21, m.m22, m.m31, m.m32);

        public static explicit operator Matrix(int m) =>
            new(m);

        public static bool operator ==(Matrix left, Matrix right) =>
            left.Equals(right);

        public static bool operator !=(Matrix left, Matrix right) =>
            !left.Equals(right);
    }
}
