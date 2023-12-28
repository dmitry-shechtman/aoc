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
        public readonly int m13;
        public readonly int m21;
        public readonly int m22;
        public readonly int m23;
        public readonly int m31;
        public readonly int m32;
        public readonly int m33;

        public Matrix(int m11, int m12, int m13, int m21, int m22, int m23, int m31, int m32, int m33)
        {
            this.m11 = m11;
            this.m12 = m12;
            this.m13 = m13;
            this.m21 = m21;
            this.m22 = m22;
            this.m23 = m23;
            this.m31 = m31;
            this.m32 = m32;
            this.m33 = m33;
        }

        public Matrix(int m11, int m12, int m21, int m22, int m31, int m32)
            : this(m11, m12, 0, m21, m22, 0, m31, m32, 0)
        {
        }

        public Matrix(int m11, int m12, int m21, int m22)
            : this(m11, m12, m21, m22, 0, 0)
        {
        }

        public Matrix(int m)
        {
            m11 = (m & 3) - 1;
            m12 = ((m >> 2) & 3) - 1;
            m21 = ((m >> 4) & 3) - 1;
            m22 = ((m >> 6) & 3) - 1;
            m13 = m23 = m31 = m32 = m33 = 0;
        }

        public readonly override bool Equals(object obj) =>
            obj is Matrix matrix && Equals(matrix);

        public readonly bool Equals(Matrix other) =>
            m11 == other.m11 &&
            m12 == other.m12 &&
            m13 == other.m13 &&
            m21 == other.m21 &&
            m22 == other.m22 &&
            m23 == other.m23 &&
            m31 == other.m31 &&
            m32 == other.m32 &&
            m33 == other.m33;

        public readonly override int GetHashCode()
        {
            HashCode hash = new();
            hash.Add(m11);
            hash.Add(m12);
            hash.Add(m13);
            hash.Add(m21);
            hash.Add(m22);
            hash.Add(m23);
            hash.Add(m31);
            hash.Add(m32);
            hash.Add(m33);
            return hash.ToHashCode();
        }

        public static Matrix Translate(int x, int y) =>
            Translate(new(x, y));

        public static Matrix Translate(Vector v) =>
            new(1, 0,
                0, 1,
                v.x, v.y);

        public static Matrix Rotate(int degrees) => degrees switch
        {
            0           => Identity,
            90  or -270 => RotateRight,
            180 or -180 => Flip,
            270 or  -90 => RotateLeft,
            _ => throw new(),
        };

        public readonly Vector Mul(Vector v) =>
            new(m11 * v.x + m12 * v.y + m13,
                m21 * v.x + m22 * v.y + m23);

        public static Vector Mul(Matrix matrix, Vector vector) =>
            matrix.Mul(vector);

        public static Vector operator *(Matrix matrix, Vector vector) =>
            matrix.Mul(vector);

        public static implicit operator Matrix((int m11, int m12, int m21, int m22) m) =>
            new(m.m11, m.m12,
                m.m21, m.m22);

        public static implicit operator Matrix((int m11, int m12, int m21, int m22, int m31, int m32) m) =>
            new(m.m11, m.m12,
                m.m21, m.m22,
                m.m31, m.m32);

        public static implicit operator Matrix((int m11, int m12, int m13, int m21, int m22, int m23, int m31, int m32, int m33) m) =>
            new(m.m11, m.m12, m.m13,
                m.m21, m.m22, m.m23,
                m.m31, m.m32, m.m33);

        public static explicit operator Matrix(Vector vector) =>
            Translate(vector);

        public static explicit operator Matrix(int m) =>
            new(m);

        public static bool operator ==(Matrix left, Matrix right) =>
            left.Equals(right);

        public static bool operator !=(Matrix left, Matrix right) =>
            !left.Equals(right);
    }
}
