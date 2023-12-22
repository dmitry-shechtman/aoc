using System;

namespace aoc
{
    public struct Matrix3D : IEquatable<Matrix3D>
    {
        public static readonly Matrix3D Zero             = default;
        public static readonly Matrix3D Identity         = new(1, 0, 0, 0, 1, 0, 0, 0, 1);

        public readonly int m11;
        public readonly int m12;
        public readonly int m13;
        public readonly int m21;
        public readonly int m22;
        public readonly int m23;
        public readonly int m31;
        public readonly int m32;
        public readonly int m33;
        public readonly int m41;
        public readonly int m42;
        public readonly int m43;

        public Matrix3D(int m11, int m12, int m13, int m21, int m22, int m23, int m31, int m32, int m33, int m41, int m42, int m43)
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
            this.m41 = m41;
            this.m42 = m42;
            this.m43 = m43;
        }

        public Matrix3D(int m11, int m12, int m13, int m21, int m22, int m23, int m31, int m32, int m33)
            : this(m11, m12, m13, m21, m22, m23, m31, m32, m33, 0, 0, 0)
        {
        }

        public Matrix3D(Matrix m)
            : this(m.m11, m.m12, 0, m.m21, m.m22, 0, m.m31, m.m32, 0)
        {
        }

        public readonly override bool Equals(object obj) =>
            obj is Matrix3D other && Equals(other);

        public readonly bool Equals(Matrix3D other) =>
            m11 == other.m11 &&
            m12 == other.m12 &&
            m13 == other.m13 &&
            m21 == other.m21 &&
            m22 == other.m22 &&
            m23 == other.m23 &&
            m31 == other.m31 &&
            m32 == other.m32 &&
            m33 == other.m33 &&
            m41 == other.m41 &&
            m42 == other.m42 &&
            m43 == other.m43;

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
            hash.Add(m41);
            hash.Add(m42);
            hash.Add(m43);
            return hash.ToHashCode();
        }

        public static Matrix3D Translate(int x, int y, int z) =>
            Translate(new(x, y, z));

        public static Matrix3D Translate(Vector3D v) =>
            new(1, 0, 0, 0, 1, 0, 0, 0, 1, v.x, v.y, v.z);

        public static implicit operator Matrix3D((int m11, int m12, int m13, int m21, int m22, int m23, int m31, int m32, int m33) m) =>
            new(m.m11, m.m12, m.m13, m.m21, m.m22, m.m23, m.m31, m.m32, m.m33);

        public static implicit operator Matrix3D((int m11, int m12, int m13, int m21, int m22, int m23, int m31, int m32, int m33, int m41, int m42, int m43) m) =>
            new(m.m11, m.m12, m.m13, m.m21, m.m22, m.m23, m.m31, m.m32, m.m33, m.m41, m.m42, m.m43);

        public static explicit operator Matrix3D(Matrix matrix) =>
            new(matrix);

        public static explicit operator Matrix3D(Vector3D vector) =>
            Translate(vector);

        public static bool operator ==(Matrix3D left, Matrix3D right) =>
            left.Equals((object)right);

        public static bool operator !=(Matrix3D left, Matrix3D right) =>
            !left.Equals((object)right);
    }
}
