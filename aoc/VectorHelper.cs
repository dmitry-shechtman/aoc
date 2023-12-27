using System;

namespace aoc
{
    internal abstract class VectorHelper<TVector, T> : Helper1<TVector, T>
        where TVector : struct, IVector<TVector, T>
        where T : struct, IFormattable
    {
        public VectorHelper(Func<T[], TVector> fromArray, TryParseValue1<T> tryParse, int cardinality, T minusOne, T zero, T one)
            : base(fromArray, tryParse, cardinality)
        {
            Headings = GetHeadings(minusOne, zero, one);
            FormatStrings = GetFormatStrings();
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
        protected abstract string[]  GetFormatStrings();
    }

    internal sealed class Vector2DHelper<TVector, T> : VectorHelper<TVector, T>
        where TVector : struct, IVector2D<TVector, T>
        where T : struct, IFormattable
    {
        private const int Cardinality = 2;

        public Vector2DHelper(Func<T[], TVector> fromArray, TryParseValue1<T> tryParse, T minusOne, T zero, T one)
            : base(fromArray, tryParse, Cardinality, minusOne, zero, one)
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

        protected override string   GetDefaultFormat() => "x,y";
        protected override string[] GetFormatKeys()    => new[] { "x", "y" };
        protected override string[] GetFormatStrings() => new[] { "nesw", "urdl", "^>v<" };

        public TVector North { get; private set; }
        public TVector East  { get; private set; }
        public TVector South { get; private set; }
        public TVector West  { get; private set; }
    }

    internal sealed class Vector3DHelper<TVector, T> : VectorHelper<TVector, T>
        where TVector : struct, IVector3D<TVector, T>
        where T : struct, IFormattable
    {
        private const int Cardinality = 3;

        public Vector3DHelper(Func<T[], TVector> fromArray, TryParseValue1<T> tryParse, T minusOne, T zero, T one)
            : base(fromArray, tryParse, Cardinality, minusOne, zero, one)
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

        protected override string   GetDefaultFormat() => "x,y,z";
        protected override string[] GetFormatKeys()    => new[] { "x", "y", "z" };
        protected override string[] GetFormatStrings() => new[] { "neswud" };

        public TVector North { get; private set; }
        public TVector East  { get; private set; }
        public TVector South { get; private set; }
        public TVector West  { get; private set; }
        public TVector Up    { get; private set; }
        public TVector Down  { get; private set; }
    }
}
