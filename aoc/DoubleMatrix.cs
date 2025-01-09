using System;

namespace aoc
{
    using Helper = Internal.Matrix2DHelper<DoubleMatrix, DoubleVector, double>;

    public readonly struct DoubleMatrix : IMatrix2D<DoubleMatrix, DoubleVector, double>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromRows, FromColumns, DoubleVector.Helper));

        private static Helper Helper => _helper.Value;

        public static DoubleMatrix NegativeOne => Helper.NegativeOne;
        public static DoubleMatrix Zero        => default;
        public static DoubleMatrix One         => Helper.One;

        public static DoubleMatrix RotateRight      => Helper.RotateRight;
        public static DoubleMatrix RotateLeft       => Helper.RotateLeft;
        public static DoubleMatrix MirrorHorizontal => Helper.MirrorHorizontal;
        public static DoubleMatrix MirrorVertical   => Helper.MirrorVertical;
        public static DoubleMatrix Flip             => Helper.Flip;

        public static DoubleMatrix AdditiveIdentity       => Zero;
        public static DoubleMatrix MultiplicativeIdentity => One;

        public readonly double m11;
        public readonly double m12;
        public readonly double m13;
        public readonly double m21;
        public readonly double m22;
        public readonly double m23;
        public readonly double m31;
        public readonly double m32;
        public readonly double m33;

        public DoubleMatrix(double m11, double m12, double m13, double m21, double m22, double m23, double m31, double m32, double m33)
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

        public DoubleMatrix(double m11, double m12, double m21, double m22, double m31, double m32)
            : this(m11, m12, 0, m21, m22, 0, m31, m32, 1)
        {
        }

        public DoubleMatrix(double m11, double m12, double m21, double m22)
            : this(m11, m12, m21, m22, 0, 0)
        {
        }

        public DoubleMatrix(DoubleVector r1, DoubleVector r2, DoubleVector r3 = default)
            : this(r1.x, r1.y, r2.x, r2.y, r3.x, r3.y)
        {
        }

        public DoubleMatrix(Matrix m)
            : this(m.m11, m.m12, m.m13, m.m21, m.m22, m.m23, m.m31, m.m32, m.m33)
        {
        }

        public DoubleMatrix(LongMatrix m)
            : this(m.m11, m.m12, m.m13, m.m21, m.m22, m.m23, m.m31, m.m32, m.m33)
        {
        }

        public readonly override bool Equals(object obj) =>
            obj is DoubleMatrix other && Equals(other);

        public readonly bool Equals(DoubleMatrix other) =>
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

        public readonly void Deconstruct(out DoubleVector r1, out DoubleVector r2)
        {
            r1 = R1;
            r2 = R2;
        }

        public readonly void Deconstruct(out DoubleVector r1, out DoubleVector r2, out DoubleVector r3)
        {
            r1 = R1;
            r2 = R2;
            r3 = R3;
        }

        public readonly DoubleVector R1 => new(m11, m12);
        public readonly DoubleVector R2 => new(m21, m22);
        public readonly DoubleVector R3 => new(m31, m32);

        public readonly DoubleVector C1 => new(m11, m21);
        public readonly DoubleVector C2 => new(m12, m22);
        public readonly DoubleVector C3 => new(m13, m23);

        public static IMatrixBuilder<DoubleMatrix, DoubleVector, double> Rows =>
            Helper.Rows;

        public static IMatrixBuilder<DoubleMatrix, DoubleVector, double> Columns =>
            Helper.Columns;

        public static DoubleMatrix Parse(string s, IFormatProvider provider = null) =>
            Helper.Parse(s, provider);

        public static bool TryParse(string s, out DoubleMatrix matrix) =>
            Helper.TryParse(s, out matrix);

        public static bool TryParse(string s, IFormatProvider provider, out DoubleMatrix matrix) =>
            Helper.TryParse(s, provider, out matrix);

        public static DoubleMatrix Parse(ReadOnlySpan<char> s, IFormatProvider provider = null) =>
            Helper.Parse(s, provider);

        public static bool TryParse(ReadOnlySpan<char> s, out DoubleMatrix matrix) =>
            Helper.TryParse(s, out matrix);

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out DoubleMatrix matrix) =>
            Helper.TryParse(s, provider, out matrix);

        public static DoubleMatrix FromRows(params DoubleVector[] rows) =>
            FromRows(rows.AsSpan());

        private static DoubleMatrix FromRows(ReadOnlySpan<DoubleVector> rows) =>
            new(rows[0], rows[1], rows.Length > 2 ? rows[2] : default);

        private static DoubleMatrix FromColumns(ReadOnlySpan<DoubleVector> columns) =>
            FromColumns(columns[0], columns[1], columns.Length > 2 ? columns[2] : default);

        public static DoubleMatrix FromColumns(DoubleVector c1, DoubleVector c2, DoubleVector c3 = default) =>
            new(c1.x, c2.x, c3.x,
                c1.y, c2.y, c3.y,
                0,    0,    1);

        public readonly double GetDeterminant() =>
            m11 * m22 - m12 * m21;

        public static double GetDeterminant(DoubleMatrix matrix) =>
            matrix.GetDeterminant();

        public readonly bool Invert(out DoubleMatrix inv)
        {
            double det = GetDeterminant();
            if (det == 0)
            {
                inv = default;
                return false;
            }
            inv = new(m22 / det, -m12 / det, -m21 / det, m11 / det, 0, 0);
            return true;
        }

        public static bool Invert(DoubleMatrix matrix, out DoubleMatrix inv) =>
            matrix.Invert(out inv);

        public readonly bool Solve(out DoubleVector x) =>
            Solve(C3, out x);

        public static bool Solve(DoubleMatrix m, out DoubleVector x) =>
            m.Solve(out x);

        public readonly bool Solve(DoubleVector b, out DoubleVector x)
        {
            if (!Invert(out DoubleMatrix inv))
            {
                x = default;
                return false;
            }
            x = inv * b;
            return true;
        }

        public static bool Solve(DoubleMatrix a, DoubleVector b, out DoubleVector x) =>
            a.Solve(b, out x);

        public static DoubleMatrix Translate(double x, double y) =>
            Helper.Translate(x, y);

        public static DoubleMatrix Translate(DoubleVector v) =>
            Helper.Translate(v);

        public static DoubleMatrix Rotate(int degrees) =>
            Helper.Rotate(degrees);

        private readonly DoubleMatrix Add(DoubleMatrix right) =>
            new(m11 + right.m11, m12 + right.m12, m13 + right.m13,
                m21 + right.m21, m22 + right.m22, m23 + right.m23,
                m31 + right.m31, m32 + right.m32, m33 + right.m33);

        public static DoubleMatrix Add(DoubleMatrix left, DoubleMatrix right) =>
            left.Add(right);

        public static DoubleMatrix operator +(DoubleMatrix left, DoubleMatrix right) =>
            left.Add(right);

        private readonly DoubleMatrix Sub(DoubleMatrix right) =>
            new(m11 - right.m11, m12 - right.m12, m13 - right.m13,
                m21 - right.m21, m22 - right.m22, m23 - right.m23,
                m31 - right.m31, m32 - right.m32, m33 - right.m33);

        public static DoubleMatrix Sub(DoubleMatrix left, DoubleMatrix right) =>
            left.Sub(right);

        public static DoubleMatrix operator -(DoubleMatrix left, DoubleMatrix right) =>
            left.Sub(right);

        public readonly DoubleVector Mul(DoubleVector v) =>
            new(m11 * v.x + m12 * v.y + m13,
                m21 * v.x + m22 * v.y + m23);

        public static DoubleVector Mul(DoubleMatrix matrix, DoubleVector vector) =>
            matrix.Mul(vector);

        public static DoubleVector operator *(DoubleMatrix matrix, DoubleVector vector) =>
            matrix.Mul(vector);

        public static implicit operator DoubleMatrix((double m11, double m12, double m21, double m22) m) =>
            new(m.m11, m.m12,
                m.m21, m.m22);

        public static implicit operator DoubleMatrix((double m11, double m12, double m21, double m22, double m31, double m32) m) =>
            new(m.m11, m.m12,
                m.m21, m.m22,
                m.m31, m.m32);

        public static implicit operator DoubleMatrix((double m11, double m12, double m13, double m21, double m22, double m23, double m31, double m32, double m33) m) =>
            new(m.m11, m.m12, m.m13,
                m.m21, m.m22, m.m23,
                m.m31, m.m32, m.m33);

        public static implicit operator DoubleMatrix(Matrix matrix) =>
            new(matrix);

        public static explicit operator DoubleMatrix(LongMatrix matrix) =>
            new(matrix);

        public static explicit operator Matrix(DoubleMatrix m) =>
            new((int)m.m11, (int)m.m12, (int)m.m13,
                (int)m.m21, (int)m.m22, (int)m.m23,
                (int)m.m31, (int)m.m32, (int)m.m33);

        public static explicit operator LongMatrix(DoubleMatrix m) =>
            new((long)m.m11, (long)m.m12, (long)m.m13,
                (long)m.m21, (long)m.m22, (long)m.m23,
                (long)m.m31, (long)m.m32, (long)m.m33);

        public static implicit operator DoubleMatrix((DoubleVector r1, DoubleVector r2, DoubleVector r3) r) =>
            new(r.r1, r.r2, r.r3);

        public static implicit operator DoubleMatrix((DoubleVector r1, DoubleVector r2) r) =>
            new(r.r1, r.r2);

        public static explicit operator DoubleMatrix(DoubleVector vector) =>
            Translate(vector);

        public static bool operator ==(DoubleMatrix left, DoubleMatrix right) =>
            left.Equals(right);

        public static bool operator !=(DoubleMatrix left, DoubleMatrix right) =>
            !left.Equals(right);
    }
}
