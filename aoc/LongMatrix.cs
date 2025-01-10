using System;

namespace aoc
{
    using Helper = Internal.Matrix2DHelper<LongMatrix, LongVector, long>;

    public readonly struct LongMatrix : IIntegerMatrix<LongMatrix, LongVector, long>, IMatrix2D<LongMatrix, LongVector, long>
    {
        private static readonly Lazy<Helper> _helper =
            new(() => new(FromRows, FromColumns, LongVector.Helper));

        private static Helper Helper => _helper.Value;

        public static LongMatrix NegativeOne => Helper.NegativeOne;
        public static LongMatrix Zero        => default;
        public static LongMatrix One         => Helper.One;

        public static LongMatrix RotateRight      => Helper.RotateRight;
        public static LongMatrix RotateLeft       => Helper.RotateLeft;
        public static LongMatrix MirrorHorizontal => Helper.MirrorHorizontal;
        public static LongMatrix MirrorVertical   => Helper.MirrorVertical;
        public static LongMatrix Flip             => Helper.Flip;

        public static LongMatrix AdditiveIdentity       => Zero;
        public static LongMatrix MultiplicativeIdentity => One;

        public readonly long m11;
        public readonly long m12;
        public readonly long m13;
        public readonly long m21;
        public readonly long m22;
        public readonly long m23;
        public readonly long m31;
        public readonly long m32;
        public readonly long m33;

        public LongMatrix(long m11, long m12, long m13, long m21, long m22, long m23, long m31, long m32, long m33)
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

        public LongMatrix(long m11, long m12, long m21, long m22, long m31, long m32)
            : this(m11, m12, 0, m21, m22, 0, m31, m32, 1)
        {
        }

        public LongMatrix(long m11, long m12, long m21, long m22)
            : this(m11, m12, m21, m22, 0, 0)
        {
        }

        public LongMatrix(LongVector r1, LongVector r2, LongVector r3 = default)
            : this(r1.x, r1.y, r2.x, r2.y, r3.x, r3.y)
        {
        }

        public LongMatrix(Matrix m)
            : this(m.m11, m.m12, m.m13, m.m21, m.m22, m.m23, m.m31, m.m32, m.m33)
        {
        }

        public readonly override bool Equals(object? obj) =>
            obj is LongMatrix other && Equals(other);

        public readonly bool Equals(LongMatrix other) =>
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

        public readonly string ToString(IFormatProvider? provider) =>
            Helper.ToString(this, provider);

        public readonly string ToString(string? format, IFormatProvider? provider = null) =>
            Helper.ToString(this, format, provider);

        public readonly void Deconstruct(out LongVector r1, out LongVector r2)
        {
            r1 = R1;
            r2 = R2;
        }

        public readonly void Deconstruct(out LongVector r1, out LongVector r2, out LongVector r3)
        {
            r1 = R1;
            r2 = R2;
            r3 = R3;
        }

        public readonly LongVector R1 => new(m11, m12);
        public readonly LongVector R2 => new(m21, m22);
        public readonly LongVector R3 => new(m31, m32);

        public readonly LongVector C1 => new(m11, m21);
        public readonly LongVector C2 => new(m12, m22);
        public readonly LongVector C3 => new(m13, m23);

        public static IMatrixBuilder<LongMatrix, LongVector, long> Rows =>
            Helper.Rows;

        public static IMatrixBuilder<LongMatrix, LongVector, long> Columns =>
            Helper.Columns;

        public static LongMatrix Parse(string? s) =>
            Helper.Parse(s);

        public static bool TryParse(string? s, out LongMatrix matrix) =>
            Helper.TryParse(s, out matrix);

        public static LongMatrix Parse(string? s, IFormatProvider? provider) =>
            Helper.Parse(s, provider);

        public static bool TryParse(string? s, IFormatProvider? provider, out LongMatrix matrix) =>
            Helper.TryParse(s, provider, out matrix);

        public static LongMatrix Parse(ReadOnlySpan<char> s) =>
            Helper.Parse(s);

        public static bool TryParse(ReadOnlySpan<char> s, out LongMatrix matrix) =>
            Helper.TryParse(s, out matrix);

        public static LongMatrix Parse(ReadOnlySpan<char> s, IFormatProvider? provider) =>
            Helper.Parse(s, provider);

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out LongMatrix matrix) =>
            Helper.TryParse(s, provider, out matrix);

        public static LongMatrix FromRows(params LongVector[] rows) =>
            FromRows(rows.AsSpan());

        private static LongMatrix FromRows(ReadOnlySpan<LongVector> rows) =>
            new(rows[0], rows[1], rows.Length > 2 ? rows[2] : default);

        private static LongMatrix FromColumns(ReadOnlySpan<LongVector> columns) =>
            FromColumns(columns[0], columns[1], columns.Length > 2 ? columns[2] : default);

        public static LongMatrix FromColumns(LongVector c1, LongVector c2, LongVector c3 = default) =>
            new(c1.x, c2.x, c3.x,
                c1.y, c2.y, c3.y,
                0,    0,    1);

        public static LongMatrix Translate(long x, long y) =>
            Helper.Translate(x, y);

        public static LongMatrix Translate(LongVector v) =>
            Helper.Translate(v);

        public static LongMatrix Rotate(int degrees) =>
            Helper.Rotate(degrees);

        private readonly LongMatrix Add(LongMatrix right) =>
            new(m11 + right.m11, m12 + right.m12, m13 + right.m13,
                m21 + right.m21, m22 + right.m22, m23 + right.m23,
                m31 + right.m31, m32 + right.m32, m33 + right.m33);

        public static LongMatrix Add(LongMatrix left, LongMatrix right) =>
            left.Add(right);

        public static LongMatrix operator +(LongMatrix left, LongMatrix right) =>
            left.Add(right);

        private readonly LongMatrix Sub(LongMatrix right) =>
            new(m11 - right.m11, m12 - right.m12, m13 - right.m13,
                m21 - right.m21, m22 - right.m22, m23 - right.m23,
                m31 - right.m31, m32 - right.m32, m33 - right.m33);

        public static LongMatrix Sub(LongMatrix left, LongMatrix right) =>
            left.Sub(right);

        public static LongMatrix operator -(LongMatrix left, LongMatrix right) =>
            left.Sub(right);

        public readonly LongVector Mul(LongVector v) =>
            new(m11 * v.x + m12 * v.y + m13,
                m21 * v.x + m22 * v.y + m23);

        public static LongVector Mul(LongMatrix matrix, LongVector vector) =>
            matrix.Mul(vector);

        public static LongVector operator *(LongMatrix matrix, LongVector vector) =>
            matrix.Mul(vector);

        public static implicit operator LongMatrix((long m11, long m12, long m21, long m22) m) =>
            new(m.m11, m.m12,
                m.m21, m.m22);

        public static implicit operator LongMatrix((long m11, long m12, long m21, long m22, long m31, long m32) m) =>
            new(m.m11, m.m12,
                m.m21, m.m22,
                m.m31, m.m32);

        public static implicit operator LongMatrix((long m11, long m12, long m13, long m21, long m22, long m23, long m31, long m32, long m33) m) =>
            new(m.m11, m.m12, m.m13,
                m.m21, m.m22, m.m23,
                m.m31, m.m32, m.m33);

        public static implicit operator LongMatrix((LongVector r1, LongVector r2, LongVector r3) r) =>
            new(r.r1, r.r2, r.r3);

        public static implicit operator LongMatrix((LongVector r1, LongVector r2) r) =>
            new(r.r1, r.r2);

        public static explicit operator LongMatrix(LongVector vector) =>
            Translate(vector);

        public static bool operator ==(LongMatrix left, LongMatrix right) =>
            left.Equals(right);

        public static bool operator !=(LongMatrix left, LongMatrix right) =>
            !left.Equals(right);
    }
}
