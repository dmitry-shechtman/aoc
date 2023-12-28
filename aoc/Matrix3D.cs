using System;

namespace aoc
{
    public struct Matrix3D : IMatrix<Matrix3D, Vector3D, int>
    {
        public static readonly Matrix3D Zero             = default;
        public static readonly Matrix3D Identity         = new(1, 0, 0, 0, 1, 0, 0, 0, 1);

        public readonly int m11;
        public readonly int m12;
        public readonly int m13;
        public readonly int m14;
        public readonly int m21;
        public readonly int m22;
        public readonly int m23;
        public readonly int m24;
        public readonly int m31;
        public readonly int m32;
        public readonly int m33;
        public readonly int m34;
        public readonly int m41;
        public readonly int m42;
        public readonly int m43;
        public readonly int m44;

        public Matrix3D(int m11, int m12, int m13, int m14, int m21, int m22, int m23, int m24, int m31, int m32, int m33, int m34, int m41, int m42, int m43, int m44)
        {
            this.m11 = m11;
            this.m12 = m12;
            this.m13 = m13;
            this.m14 = m14;
            this.m21 = m21;
            this.m22 = m22;
            this.m23 = m23;
            this.m24 = m24;
            this.m31 = m31;
            this.m32 = m32;
            this.m33 = m33;
            this.m34 = m34;
            this.m41 = m41;
            this.m42 = m42;
            this.m43 = m43;
            this.m44 = m44;
        }

        public Matrix3D(int m11, int m12, int m13, int m21, int m22, int m23, int m31, int m32, int m33, int m41, int m42, int m43)
            : this(m11, m12, m13, 0, m21, m22, m23, 0, m31, m32, m33, 0, m41, m42, m43, 0)
        {
        }

        public Matrix3D(int m11, int m12, int m13, int m21, int m22, int m23, int m31, int m32, int m33)
            : this(m11, m12, m13, m21, m22, m23, m31, m32, m33, 0, 0, 0)
        {
        }

        public Matrix3D(Matrix m)
            : this(m.m11, m.m12, m.m13, m.m21, m.m22, m.m23, m.m31, m.m32, m.m33)
        {
        }

        public readonly override bool Equals(object obj) =>
            obj is Matrix3D other && Equals(other);

        public readonly bool Equals(Matrix3D other) =>
            m11 == other.m11 &&
            m12 == other.m12 &&
            m13 == other.m13 &&
            m14 == other.m14 &&
            m21 == other.m21 &&
            m22 == other.m22 &&
            m23 == other.m23 &&
            m24 == other.m24 &&
            m31 == other.m31 &&
            m32 == other.m32 &&
            m33 == other.m33 &&
            m34 == other.m34 &&
            m41 == other.m41 &&
            m42 == other.m42 &&
            m43 == other.m43 &&
            m44 == other.m44;

        public readonly override int GetHashCode()
        {
            HashCode hash = new();
            hash.Add(m11);
            hash.Add(m12);
            hash.Add(m13);
            hash.Add(m14);
            hash.Add(m21);
            hash.Add(m22);
            hash.Add(m23);
            hash.Add(m24);
            hash.Add(m31);
            hash.Add(m32);
            hash.Add(m33);
            hash.Add(m34);
            hash.Add(m41);
            hash.Add(m42);
            hash.Add(m43);
            hash.Add(m44);
            return hash.ToHashCode();
        }

        public static Matrix3D Translate(int x, int y, int z) =>
            Translate(new(x, y, z));

        public static Matrix3D Translate(Vector3D v) =>
            new(1, 0, 0,
                0, 1, 0,
                0, 0, 1,
                v.x, v.y, v.z);

        public readonly Vector3D Mul(Vector3D v) =>
            new(m11 * v.x + m12 * v.y + m13 * v.z + m14,
                m21 * v.x + m22 * v.y + m23 * v.z + m24,
                m31 * v.x + m32 * v.y + m33 * v.z + m34);

        public static Vector3D Mul(Matrix3D matrix, Vector3D vector) =>
            matrix.Mul(vector);

        public static Vector3D operator *(Matrix3D matrix, Vector3D vector) =>
            matrix.Mul(vector);

        public static implicit operator Matrix3D((int m11, int m12, int m13, int m21, int m22, int m23, int m31, int m32, int m33) m) =>
            new(m.m11, m.m12, m.m13,
                m.m21, m.m22, m.m23,
                m.m31, m.m32, m.m33);

        public static implicit operator Matrix3D((int m11, int m12, int m13, int m21, int m22, int m23, int m31, int m32, int m33, int m41, int m42, int m43) m) =>
            new(m.m11, m.m12, m.m13,
                m.m21, m.m22, m.m23,
                m.m31, m.m32, m.m33,
                m.m41, m.m42, m.m43);

        public static implicit operator Matrix3D((int m11, int m12, int m13, int m14, int m21, int m22, int m23, int m24, int m31, int m32, int m33, int m34, int m41, int m42, int m43, int m44) m) =>
            new(m.m11, m.m12, m.m13, m.m14,
                m.m21, m.m22, m.m23, m.m24,
                m.m31, m.m32, m.m33, m.m34,
                m.m41, m.m42, m.m43, m.m44);

        public static explicit operator Matrix3D(Matrix matrix) =>
            new(matrix);

        public static explicit operator Matrix3D(Vector3D vector) =>
            Translate(vector);

        public static bool operator ==(Matrix3D left, Matrix3D right) =>
            left.Equals(right);

        public static bool operator !=(Matrix3D left, Matrix3D right) =>
            !left.Equals(right);
    }
}
