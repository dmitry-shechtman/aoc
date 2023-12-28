using System;

namespace aoc.Internal
{
    abstract class VectorHelperStrategy<TSelf> : Helper1Strategy<TSelf>
        where TSelf : VectorHelperStrategy<TSelf>
    {
        protected VectorHelperStrategy(params string[] formatKeys)
            : base(formatKeys)
        {
        }

        public override char DefaultSeparator => ',';

        public abstract string[] FormatStrings { get; }
    }

    abstract class VectorHelper<TVector, T, TStrategy> : Helper1<TVector, T, TStrategy>
        where TVector : struct, IVector<TVector, T>
        where T : struct, IFormattable
        where TStrategy : VectorHelperStrategy<TStrategy>
    {
        protected VectorHelper(Func<T[], TVector> fromArray, TryParse1<T> tryParse, T minusOne, T zero, T one)
            : base(fromArray, tryParse)
        {
            Headings = GetHeadings(minusOne, zero, one);
            FormatStrings = Strategy.FormatStrings;
        }

        public TVector[] Headings { get; }

        private string[] FormatStrings { get; }

        protected override string ToStringOuter(TVector vector, string format, IFormatProvider provider)
        {
            if (format.Length > 1)
                return ToStringInner(vector, format, provider);
            int index = Headings.IndexOf(vector);
            if (index < 0)
                return ToStringInner(vector, DefaultFormat, provider);
            char c = char.ToLowerInvariant(format[0]);
            string s = FormatStrings.Find(s => s.Contains(c));
            if (s.Length == 0)
                return ToStringInner(vector, DefaultFormat, provider);
            return char.IsUpper(format[0])
                ? char.ToUpperInvariant(s[index]).ToString()
                : s[index].ToString();
        }

        public int GetHeading(char c) =>
            TryGetHeading(c, out int heading)
                ? heading
                : throw new InvalidOperationException($"Unexpected character: {c}");

        public bool TryGetHeading(char c, out int heading)
        {
            c = char.ToLower(c);
            foreach (var s in FormatStrings)
                if ((heading = s.IndexOf(c)) >= 0)
                    return true;
            heading = -1;
            return false;
        }

        public TVector Parse(char c) =>
            TryParse(c, out TVector vector)
                ? vector
                : throw new InvalidOperationException($"Unexpected character: {c}");

        public bool TryParse(char c, out TVector vector)
        {
            if (!TryGetHeading(c, out int heading))
            {
                vector = default;
                return false;
            }
            vector = Headings[heading];
            return true;
        }

        protected new TVector FromArray(params T[] values) =>
            base.FromArray(values);

        protected abstract TVector[] GetHeadings(T minusOne, T zero, T one);
    }

    sealed class Vector2DHelperStrategy : VectorHelperStrategy<Vector2DHelperStrategy>
    {
        private Vector2DHelperStrategy()
            : base("x", "y")
        {
        }

        public override string[] FormatStrings => new[] { "nesw", "urdl", "^>v<" };
    }

    sealed class Vector2DHelper<TVector, T> : VectorHelper<TVector, T, Vector2DHelperStrategy>
        where TVector : struct, IVector2D<TVector, T>
        where T : struct, IFormattable
    {
        public Vector2DHelper(Func<T[], TVector> fromArray, TryParse1<T> tryParse, T minusOne, T zero, T one)
            : base(fromArray, tryParse, minusOne, zero, one)
        {
        }

        protected override TVector[] GetHeadings(T minusOne, T zero, T one)
        {
            North = FromArray(zero,     minusOne, zero);
            East  = FromArray(one,      zero,     zero);
            South = FromArray(zero,     one,      zero);
            West  = FromArray(minusOne, zero,     zero);
            return new[] { North, East, South, West };
        }

        protected override Vector2DHelperStrategy Strategy => Vector2DHelperStrategy.Instance;

        public TVector North { get; private set; }
        public TVector East  { get; private set; }
        public TVector South { get; private set; }
        public TVector West  { get; private set; }
    }

    sealed class Vector3DHelperStrategy : VectorHelperStrategy<Vector3DHelperStrategy>
    {
        private Vector3DHelperStrategy()
            : base("x", "y", "z")
        {
        }

        public override string[] FormatStrings => new[] { "neswud" };
    }

    sealed class Vector3DHelper<TVector, T> : VectorHelper<TVector, T, Vector3DHelperStrategy>
        where TVector : struct, IVector3D<TVector, T>
        where T : struct, IFormattable
    {
        public Vector3DHelper(Func<T[], TVector> fromArray, TryParse1<T> tryParse, T minusOne, T zero, T one)
            : base(fromArray, tryParse, minusOne, zero, one)
        {
        }

        protected override TVector[] GetHeadings(T minusOne, T zero, T one)
        {
            North = FromArray(zero,     minusOne, zero);
            East  = FromArray(one,      zero,     zero);
            South = FromArray(zero,     one,      zero);
            West  = FromArray(minusOne, zero,     zero);
            Up    = FromArray(zero,     zero,     minusOne);
            Down  = FromArray(zero,     zero,     one);
            return new[] { North, East, South, West, Up, Down };
        }

        protected override Vector3DHelperStrategy Strategy => Vector3DHelperStrategy.Instance;

        public TVector North { get; private set; }
        public TVector East  { get; private set; }
        public TVector South { get; private set; }
        public TVector West  { get; private set; }
        public TVector Up    { get; private set; }
        public TVector Down  { get; private set; }
    }
}
