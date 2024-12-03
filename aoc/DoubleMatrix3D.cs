using System;
using System.Text.RegularExpressions;

namespace aoc
{
    using Helper = Internal.Matrix3DHelper<DoubleMatrix3D, DoubleVector3D, double>;

    public readonly struct DoubleMatrix3D : IMatrix3D<DoubleMatrix3D, DoubleVector3D, double>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromRowArray, DoubleVector3D.Helper));

        private static Helper Helper => _helper.Value;

        public static readonly DoubleMatrix3D Zero = default;
        public static readonly DoubleMatrix3D Identity = Helper.Identity;

        public readonly double m11;
        public readonly double m12;
        public readonly double m13;
        public readonly double m14;
        public readonly double m21;
        public readonly double m22;
        public readonly double m23;
        public readonly double m24;
        public readonly double m31;
        public readonly double m32;
        public readonly double m33;
        public readonly double m34;
        public readonly double m41;
        public readonly double m42;
        public readonly double m43;
        public readonly double m44;

        public DoubleMatrix3D(double m11, double m12, double m13, double m14, double m21, double m22, double m23, double m24, double m31, double m32, double m33, double m34, double m41, double m42, double m43, double m44)
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

        public DoubleMatrix3D(double m11, double m12, double m13, double m21, double m22, double m23, double m31, double m32, double m33, double m41, double m42, double m43)
            : this(m11, m12, m13, 0, m21, m22, m23, 0, m31, m32, m33, 0, m41, m42, m43, 1)
        {
        }

        public DoubleMatrix3D(double m11, double m12, double m13, double m21, double m22, double m23, double m31, double m32, double m33)
            : this(m11, m12, m13, m21, m22, m23, m31, m32, m33, 0, 0, 0)
        {
        }

        public DoubleMatrix3D(Matrix3D m)
            : this(m.m11, m.m12, m.m13, m.m14, m.m21, m.m22, m.m23, m.m24, m.m31, m.m32, m.m33, m.m34, m.m41, m.m42, m.m43, m.m44)
        {
        }

        public DoubleMatrix3D(LongMatrix3D m)
            : this(m.m11, m.m12, m.m13, m.m14, m.m21, m.m22, m.m23, m.m24, m.m31, m.m32, m.m33, m.m34, m.m41, m.m42, m.m43, m.m44)
        {
        }

        public readonly override bool Equals(object obj) =>
            obj is DoubleMatrix3D other && Equals(other);

        public readonly bool Equals(DoubleMatrix3D other) =>
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

        public readonly void Deconstruct(out DoubleVector3D r1, out DoubleVector3D r2)
        {
            r1 = R1;
            r2 = R2;
        }

        public readonly void Deconstruct(out DoubleVector3D r1, out DoubleVector3D r2, out DoubleVector3D r3)
        {
            r1 = R1;
            r2 = R2;
            r3 = R3;
        }

        public readonly void Deconstruct(out DoubleVector3D r1, out DoubleVector3D r2, out DoubleVector3D r3, out DoubleVector3D r4)
        {
            r1 = R1;
            r2 = R2;
            r3 = R3;
            r4 = R4;
        }

        public readonly DoubleVector3D R1 => new(m11, m12, m13);
        public readonly DoubleVector3D R2 => new(m21, m22, m23);
        public readonly DoubleVector3D R3 => new(m31, m32, m33);
        public readonly DoubleVector3D R4 => new(m41, m42, m43);

        public readonly DoubleVector3D C1 => new(m11, m21, m31);
        public readonly DoubleVector3D C2 => new(m12, m22, m32);
        public readonly DoubleVector3D C3 => new(m13, m23, m33);
        public readonly DoubleVector3D C4 => new(m14, m24, m34);

        public static DoubleMatrix3D Parse(string s) =>
            Helper.Parse(s);

        public static bool TryParse(string s, out DoubleMatrix3D matrix) =>
            Helper.TryParse(s, out matrix);

        public static DoubleMatrix3D Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out DoubleMatrix3D matrix) =>
            Helper.TryParse(s, separator, out matrix);

        public static DoubleMatrix3D Parse(string s, string separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, string separator, out DoubleMatrix3D matrix) =>
            Helper.TryParse(s, separator, out matrix);

        public static DoubleMatrix3D Parse(string s, Regex separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, Regex separator, out DoubleMatrix3D matrix) =>
            Helper.TryParse(s, separator, out matrix);

        public static DoubleMatrix3D Parse(string s, char separator, char separator2) =>
            Helper.Parse(s, separator, separator2);

        public static bool TryParse(string s, char separator, char separator2, out DoubleMatrix3D matrix) =>
            Helper.TryParse(s, separator, separator2, out matrix);

        public static DoubleMatrix3D Parse(string[] ss) =>
            Helper.Parse(ss);

        public static bool TryParse(string[] ss, out DoubleMatrix3D matrix) =>
            Helper.TryParse(ss, out matrix);

        public static DoubleMatrix3D Parse(string[] ss, char separator) =>
            Helper.Parse(ss, separator);

        public static bool TryParse(string[] ss, char separator, out DoubleMatrix3D matrix) =>
            Helper.TryParse(ss, separator, out matrix);

        public static DoubleMatrix3D FromRowArray(DoubleVector3D[] rows) =>
            FromRows(rows[0], rows[1], rows[2], rows.Length > 3 ? rows[3] : default);

        public static DoubleMatrix3D FromColumnArray(DoubleVector3D[] columns) =>
            FromColumns(columns[0], columns[1], columns[2], columns.Length > 3 ? columns[3] : default);

        public static DoubleMatrix3D FromRows(DoubleVector3D r1, DoubleVector3D r2, DoubleVector3D r3 = default, DoubleVector3D r4 = default) =>
            new(r1.x, r1.y, r1.z,
                r2.x, r2.y, r2.z,
                r3.x, r3.y, r3.z,
                r4.x, r4.y, r4.z);

        public static DoubleMatrix3D FromColumns(DoubleVector3D c1, DoubleVector3D c2, DoubleVector3D c3, DoubleVector3D c4 = default) =>
            new(c1.x, c2.x, c3.x, c4.x,
                c1.y, c2.y, c3.y, c4.y,
                c1.z, c2.z, c3.z, c4.z,
                0, 0, 0, 1);

        public static DoubleMatrix3D Translate(double x, double y, double z) =>
            Helper.Translate(x, y, z);

        public static DoubleMatrix3D Translate(DoubleVector3D v) =>
            Helper.Translate(v);

        public readonly DoubleVector3D Mul(DoubleVector3D v) =>
            new(m11 * v.x + m12 * v.y + m13 * v.z + m14,
                m21 * v.x + m22 * v.y + m23 * v.z + m24,
                m31 * v.x + m32 * v.y + m33 * v.z + m34);

        public static DoubleVector3D Mul(DoubleMatrix3D matrix, DoubleVector3D vector) =>
            matrix.Mul(vector);

        public static DoubleVector3D operator *(DoubleMatrix3D matrix, DoubleVector3D vector) =>
            matrix.Mul(vector);

        public static implicit operator DoubleMatrix3D((double m11, double m12, double m13, double m21, double m22, double m23, double m31, double m32, double m33) m) =>
            new(m.m11, m.m12, m.m13,
                m.m21, m.m22, m.m23,
                m.m31, m.m32, m.m33);

        public static implicit operator DoubleMatrix3D((double m11, double m12, double m13, double m21, double m22, double m23, double m31, double m32, double m33, double m41, double m42, double m43) m) =>
            new(m.m11, m.m12, m.m13,
                m.m21, m.m22, m.m23,
                m.m31, m.m32, m.m33,
                m.m41, m.m42, m.m43);

        public static implicit operator DoubleMatrix3D((double m11, double m12, double m13, double m14, double m21, double m22, double m23, double m24, double m31, double m32, double m33, double m34, double m41, double m42, double m43, double m44) m) =>
            new(m.m11, m.m12, m.m13, m.m14,
                m.m21, m.m22, m.m23, m.m24,
                m.m31, m.m32, m.m33, m.m34,
                m.m41, m.m42, m.m43, m.m44);

        public static explicit operator DoubleMatrix3D(Matrix3D matrix) =>
            new(matrix);

        public static explicit operator DoubleMatrix3D(LongMatrix3D matrix) =>
            new(matrix);

        public static explicit operator Matrix3D(DoubleMatrix3D m) =>
            new((int)m.m11, (int)m.m12, (int)m.m13, (int)m.m14,
                (int)m.m21, (int)m.m22, (int)m.m23, (int)m.m24,
                (int)m.m31, (int)m.m32, (int)m.m33, (int)m.m34,
                (int)m.m41, (int)m.m42, (int)m.m43, (int)m.m44);

        public static explicit operator LongMatrix3D(DoubleMatrix3D m) =>
            new((long)m.m11, (long)m.m12, (long)m.m13, (long)m.m14,
                (long)m.m21, (long)m.m22, (long)m.m23, (long)m.m24,
                (long)m.m31, (long)m.m32, (long)m.m33, (long)m.m34,
                (long)m.m41, (long)m.m42, (long)m.m43, (long)m.m44);

        public static implicit operator DoubleMatrix3D((DoubleVector3D r1, DoubleVector3D r2, DoubleVector3D r3, DoubleVector3D r4) r) =>
            FromRows(r.r1, r.r2, r.r3, r.r4);

        public static implicit operator DoubleMatrix3D((DoubleVector3D r1, DoubleVector3D r2, DoubleVector3D r3) r) =>
            FromRows(r.r1, r.r2, r.r3);

        public static implicit operator DoubleMatrix3D((DoubleVector3D r1, DoubleVector3D r2) r) =>
            FromRows(r.r1, r.r2);

        public static explicit operator DoubleMatrix3D(DoubleVector3D vector) =>
            Translate(vector);

        public static bool operator ==(DoubleMatrix3D left, DoubleMatrix3D right) =>
            left.Equals(right);

        public static bool operator !=(DoubleMatrix3D left, DoubleMatrix3D right) =>
            !left.Equals(right);
    }
}
