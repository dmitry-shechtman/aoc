using System;

namespace aoc
{
    public struct LongMatrix : IMatrix<LongMatrix, LongVector, long>
    {
        public static readonly LongMatrix Zero             = default;
        public static readonly LongMatrix Identity         = new( 1,  0,  0,  1);
        public static readonly LongMatrix RotateRight      = new( 0,  1, -1,  0);
        public static readonly LongMatrix RotateLeft       = new( 0, -1,  1,  0);
        public static readonly LongMatrix MirrorHorizontal = new( 1,  0,  0, -1);
        public static readonly LongMatrix MirrorVertical   = new(-1,  0,  0,  1);
        public static readonly LongMatrix Flip             = new(-1,  0,  0, -1);

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
            : this(m11, m12, 0, m21, m22, 0, m31, m32, 0)
        {
        }

        public LongMatrix(long m11, long m12, long m21, long m22)
            : this(m11, m12, m21, m22, 0, 0)
        {
        }

        public readonly override bool Equals(object obj) =>
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

        public static LongMatrix Translate(long x, long y) =>
            Translate(new(x, y));

        public static LongMatrix Translate(LongVector v) =>
            new(1, 0,
                0, 1,
                v.x, v.y);

        public static LongMatrix Rotate(int degrees) => degrees switch
        {
            0           => Identity,
            90  or -270 => RotateRight,
            180 or -180 => Flip,
            270 or  -90 => RotateLeft,
            _ => throw new(),
        };

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

        public static explicit operator LongMatrix(LongVector vector) =>
            Translate(vector);

        public static bool operator ==(LongMatrix left, LongMatrix right) =>
            left.Equals(right);

        public static bool operator !=(LongMatrix left, LongMatrix right) =>
            !left.Equals(right);
    }
}
