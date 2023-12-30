using System;

namespace aoc
{
    using Helper = Internal.Matrix2DHelper<Matrix, Vector, int>;

    public struct Matrix : IMatrix2D<Matrix, Vector, int>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromRowArray, Vector.Helper));

        private static Helper Helper => _helper.Value;

        public static readonly Matrix Zero             = default;
        public static readonly Matrix Identity         = Helper.Identity;
        public static readonly Matrix RotateRight      = Helper.RotateRight;
        public static readonly Matrix RotateLeft       = Helper.RotateLeft;
        public static readonly Matrix MirrorHorizontal = Helper.MirrorHorizontal;
        public static readonly Matrix MirrorVertical   = Helper.MirrorVertical;
        public static readonly Matrix Flip             = Helper.Flip;

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
            : this(m11, m12, 0, m21, m22, 0, m31, m32, 1)
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

        public readonly override string ToString() =>
            Helper.ToString(this);

        public readonly string ToString(IFormatProvider provider) =>
            Helper.ToString(this, provider);

        public readonly string ToString(string format, IFormatProvider provider = null) =>
            Helper.ToString(this, format, provider);

        public readonly void Deconstruct(out Vector r1, out Vector r2)
        {
            r1 = R1;
            r2 = R2;
        }

        public readonly void Deconstruct(out Vector r1, out Vector r2, out Vector r3)
        {
            r1 = R1;
            r2 = R2;
            r3 = R3;
        }

        public readonly Vector R1 => new(m11, m12);
        public readonly Vector R2 => new(m21, m22);
        public readonly Vector R3 => new(m31, m32);

        public readonly Vector C1 => new(m11, m21);
        public readonly Vector C2 => new(m12, m22);
        public readonly Vector C3 => new(m13, m23);

        public static Matrix Parse(string s) =>
            Helper.Parse(s);

        public static bool TryParse(string s, out Matrix matrix) =>
            Helper.TryParse(s, out matrix);

        public static Matrix Parse(string s, char separator) =>
            Helper.Parse(s, separator);

        public static bool TryParse(string s, char separator, out Matrix matrix) =>
            Helper.TryParse(s, separator, out matrix);

        public static Matrix Parse(string s, char separator, char separator2) =>
            Helper.Parse(s, separator, separator2);

        public static bool TryParse(string s, char separator, char separator2, out Matrix matrix) =>
            Helper.TryParse(s, separator, separator2, out matrix);

        public static Matrix Parse(string[] ss) =>
            Helper.Parse(ss);

        public static bool TryParse(string[] ss, out Matrix matrix) =>
            Helper.TryParse(ss, out matrix);

        public static Matrix Parse(string[] ss, char separator) =>
            Helper.Parse(ss, separator);

        public static bool TryParse(string[] ss, char separator, out Matrix matrix) =>
            Helper.TryParse(ss, separator, out matrix);

        public static Matrix FromRowArray(Vector[] rows) =>
            FromRows(rows[0], rows[1], rows.Length > 2 ? rows[2] : default);

        public static Matrix FromColumnArray(Vector[] columns) =>
            FromColumns(columns[0], columns[1], columns.Length > 2 ? columns[2] : default);

        public static Matrix FromRows(Vector r1, Vector r2, Vector r3 = default) =>
            new(r1.x, r1.y,
                r2.x, r2.y,
                r3.x, r3.y);

        public static Matrix FromColumns(Vector c1, Vector c2, Vector c3 = default) =>
            new(c1.x, c2.x, c3.x,
                c1.y, c2.y, c3.y,
                0,    0,    1);

        public static Matrix Translate(int x, int y) =>
            Helper.Translate(x, y);

        public static Matrix Translate(Vector v) =>
            Helper.Translate(v);

        public static Matrix Rotate(int degrees) =>
            Helper.Rotate(degrees);

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

        public static implicit operator Matrix((Vector r1, Vector r2, Vector r3) r) =>
            FromRows(r.r1, r.r2, r.r3);

        public static implicit operator Matrix((Vector r1, Vector r2) r) =>
            FromRows(r.r1, r.r2);

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
