using System;
using System.Text.RegularExpressions;

namespace aoc
{
    using Helper = Internal.Matrix3DHelper<Matrix3D, Vector3D, int>;

    public readonly struct Matrix3D : IMatrix3D<Matrix3D, Vector3D, int>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromRows, Vector3D.Helper));

        private static Helper Helper => _helper.Value;

        public static readonly Matrix3D Zero             = default;
        public static readonly Matrix3D Identity         = Helper.Identity;

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
            : this(m11, m12, m13, 0, m21, m22, m23, 0, m31, m32, m33, 0, m41, m42, m43, 1)
        {
        }

        public Matrix3D(int m11, int m12, int m13, int m21, int m22, int m23, int m31, int m32, int m33)
            : this(m11, m12, m13, m21, m22, m23, m31, m32, m33, 0, 0, 0)
        {
        }

        public Matrix3D(Vector3D r1, Vector3D r2, Vector3D r3 = default, Vector3D r4 = default)
            : this(r1.x, r1.y, r1.z, r2.x, r2.y, r2.z, r3.x, r3.y, r3.z, r4.x, r4.y, r4.z)
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

        public readonly override string ToString() =>
            Helper.ToString(this);

        public readonly string ToString(IFormatProvider provider) =>
            Helper.ToString(this, provider);

        public readonly string ToString(string format, IFormatProvider provider = null) =>
            Helper.ToString(this, format, provider);

        public readonly void Deconstruct(out Vector3D r1, out Vector3D r2)
        {
            r1 = R1;
            r2 = R2;
        }

        public readonly void Deconstruct(out Vector3D r1, out Vector3D r2, out Vector3D r3)
        {
            r1 = R1;
            r2 = R2;
            r3 = R3;
        }

        public readonly void Deconstruct(out Vector3D r1, out Vector3D r2, out Vector3D r3, out Vector3D r4)
        {
            r1 = R1;
            r2 = R2;
            r3 = R3;
            r4 = R4;
        }

        public readonly Vector3D R1 => new(m11, m12, m13);
        public readonly Vector3D R2 => new(m21, m22, m23);
        public readonly Vector3D R3 => new(m31, m32, m33);
        public readonly Vector3D R4 => new(m41, m42, m43);

        public readonly Vector3D C1 => new(m11, m21, m31);
        public readonly Vector3D C2 => new(m12, m22, m32);
        public readonly Vector3D C3 => new(m13, m23, m33);
        public readonly Vector3D C4 => new(m14, m24, m34);

        public static Matrix3D Parse(string s) =>
            Helper.Parse(s);

        public static bool TryParse(string s, out Matrix3D matrix) =>
            Helper.TryParse(s, out matrix);

        public static Matrix3D Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out Matrix3D matrix) =>
            Helper.TryParse(s, separator, out matrix);

        public static Matrix3D Parse(string s, string separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, string separator, out Matrix3D matrix) =>
            Helper.TryParse(s, separator, out matrix);

        public static Matrix3D Parse(string s, Regex separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, Regex separator, out Matrix3D matrix) =>
            Helper.TryParse(s, separator, out matrix);

        public static Matrix3D Parse(string s, char separator, char separator2) =>
            Helper.Parse(s, separator, separator2);

        public static bool TryParse(string s, char separator, char separator2, out Matrix3D matrix) =>
            Helper.TryParse(s, separator, separator2, out matrix);

        public static Matrix3D Parse(string[] ss) =>
            Helper.Parse(ss);

        public static bool TryParse(string[] ss, out Matrix3D matrix) =>
            Helper.TryParse(ss, out matrix);

        public static Matrix3D Parse(string[] ss, char separator) =>
            Helper.Parse(ss, separator);

        public static bool TryParse(string[] ss, char separator, out Matrix3D matrix) =>
            Helper.TryParse(ss, separator, out matrix);

        public static Matrix3D FromRows(params Vector3D[] rows) =>
            new(rows[0], rows[1], rows[2], rows.Length > 3 ? rows[3] : default);

        public static Matrix3D FromColumns(Vector3D[] columns) =>
            FromColumns(columns[0], columns[1], columns[2], columns.Length > 3 ? columns[3] : default);

        public static Matrix3D FromColumns(Vector3D c1, Vector3D c2, Vector3D c3, Vector3D c4 = default) =>
            new(c1.x, c2.x, c3.x, c4.x,
                c1.y, c2.y, c3.y, c4.y,
                c1.z, c2.z, c3.z, c4.z,
                0,    0,    0,    1);

        public static Matrix3D Translate(int x, int y, int z) =>
            Helper.Translate(x, y, z);

        public static Matrix3D Translate(Vector3D v) =>
            Helper.Translate(v);

        private readonly Matrix3D Add(Matrix3D right) =>
            new(m11 + right.m11, m12 + right.m12, m13 + right.m13, m14 + right.m14,
                m21 + right.m21, m22 + right.m22, m23 + right.m23, m24 + right.m24,
                m31 + right.m31, m32 + right.m32, m33 + right.m33, m34 + right.m34,
                m41 + right.m41, m42 + right.m42, m43 + right.m43, m44 + right.m44);

        public static Matrix3D Add(Matrix3D left, Matrix3D right) =>
            left.Add(right);

        public static Matrix3D operator +(Matrix3D left, Matrix3D right) =>
            left.Add(right);

        private readonly Matrix3D Sub(Matrix3D right) =>
            new(m11 - right.m11, m12 - right.m12, m13 - right.m13, m14 - right.m14,
                m21 - right.m21, m22 - right.m22, m23 - right.m23, m24 - right.m24,
                m31 - right.m31, m32 - right.m32, m33 - right.m33, m34 - right.m34,
                m41 - right.m41, m42 - right.m42, m43 - right.m43, m44 - right.m44);

        public static Matrix3D Sub(Matrix3D left, Matrix3D right) =>
            left.Sub(right);

        public static Matrix3D operator -(Matrix3D left, Matrix3D right) =>
            left.Sub(right);

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

        public static implicit operator Matrix3D((Vector3D r1, Vector3D r2, Vector3D r3, Vector3D r4) r) =>
            new(r.r1, r.r2, r.r3, r.r4);

        public static implicit operator Matrix3D((Vector3D r1, Vector3D r2, Vector3D r3) r) =>
            new(r.r1, r.r2, r.r3);

        public static implicit operator Matrix3D((Vector3D r1, Vector3D r2) r) =>
            new(r.r1, r.r2);

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
