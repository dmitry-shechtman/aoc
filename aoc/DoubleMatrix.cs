using System;

namespace aoc
{
    public struct DoubleMatrix : IEquatable<DoubleMatrix>
    {
        public static readonly DoubleMatrix Zero             = default;
        public static readonly DoubleMatrix Identity         = new( 1,  0,  0,  1);
        public static readonly DoubleMatrix RotateRight      = new( 0,  1, -1,  0);
        public static readonly DoubleMatrix RotateLeft       = new( 0, -1,  1,  0);
        public static readonly DoubleMatrix MirrorHorizontal = new( 1,  0,  0, -1);
        public static readonly DoubleMatrix MirrorVertical   = new(-1,  0,  0,  1);
        public static readonly DoubleMatrix Flip             = new(-1,  0,  0, -1);

        public readonly double m11;
        public readonly double m12;
        public readonly double m21;
        public readonly double m22;
        public readonly double m31;
        public readonly double m32;

        public DoubleMatrix(double m11, double m12, double m21, double m22, double m31, double m32)
        {
            this.m11 = m11;
            this.m12 = m12;
            this.m21 = m21;
            this.m22 = m22;
            this.m31 = m31;
            this.m32 = m32;
        }

        public DoubleMatrix(double m11, double m12, double m21, double m22)
            : this(m11, m12, m21, m22, 0, 0)
        {
        }

        public readonly override bool Equals(object obj) =>
            obj is DoubleMatrix other && Equals(other);

        public readonly bool Equals(DoubleMatrix other) =>
            m11 == other.m11 &&
            m12 == other.m12 &&
            m21 == other.m21 &&
            m22 == other.m22 &&
            m31 == other.m31 &&
            m32 == other.m32;

        public readonly override int GetHashCode() =>
            HashCode.Combine(m11, m12, m21, m22, m31, m32);

        public double GetDeterminant() =>
            m11 * m22 - m12 * m21;

        public static double GetDeterminant(DoubleMatrix matrix) =>
            matrix.GetDeterminant();

        public bool Invert(out DoubleMatrix inv)
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

        public static DoubleMatrix Translate(int x, int y) =>
            Translate(new(x, y));

        public static DoubleMatrix Translate(DoubleVector v) =>
            new(1, 0, 0, 1, v.x, v.y);

        public static DoubleMatrix Rotate(int degrees) => degrees switch
        {
            0           => Identity,
            90  or -270 => RotateRight,
            180 or -180 => Flip,
            270 or  -90 => RotateLeft,
            _ => throw new(),
        };

        public readonly DoubleVector Mul(DoubleVector vector) =>
            new(m11 * vector.x + m12 * vector.y, m21 * vector.x + m22 * vector.y);

        public static DoubleVector Mul(DoubleMatrix matrix, DoubleVector vector) =>
            matrix.Mul(vector);

        public static DoubleVector operator *(DoubleMatrix matrix, DoubleVector vector) =>
            matrix.Mul(vector);

        public static implicit operator DoubleMatrix((double m11, double m12, double m21, double m22) m) =>
            new(m.m11, m.m12, m.m21, m.m22);

        public static implicit operator DoubleMatrix((double m11, double m12, double m21, double m22, double m31, double m32) m) =>
            new(m.m11, m.m12, m.m21, m.m22, m.m31, m.m32);

        public static explicit operator DoubleMatrix(DoubleVector vector) =>
            Translate(vector);

        public static bool operator ==(DoubleMatrix left, DoubleMatrix right) =>
            left.Equals(right);

        public static bool operator !=(DoubleMatrix left, DoubleMatrix right) =>
            !left.Equals(right);
    }
}
