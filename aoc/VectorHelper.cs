using System;

namespace aoc
{
    internal abstract class VectorHelper<TVector, T> : ParseHelper<TVector, T>
        where TVector : struct, IVector<TVector, T>
        where T : struct, IFormattable
    {
        public VectorHelper(FromArray1 fromArray, TryParseValue1 tryParse, int cardinality, T minusOne, T zero, T one)
        {
            FromArray = fromArray;
            TryParseValue = tryParse;
            Cardinality = cardinality;

            Headings = GetHeadings(minusOne, zero, one);
            DefaultFormat = GetDefaultFormat();
            FormatKeys = GetFormatKeys();
            FormatStrings = GetFormatStrings();
        }

        public TVector[] Headings { get; }

        protected FromArray1     FromArray     { get; }
        private   TryParseValue1 TryParseValue { get; }
        private   int            Cardinality   { get; }

        private string   DefaultFormat { get; }
        private string[] FormatKeys    { get; }
        private string[] FormatStrings { get; }

        public TVector Parse(string s, char separator) =>
            Parse<TVector>(s, TryParse, separator);

        public bool TryParse(string s, out TVector vector, char separator) =>
            TryParse(s, TryParse, separator, out vector);

        public TVector Parse(string[] ss) =>
            Parse<TVector>(ss, TryParse);

        public bool TryParse(string[] ss, out TVector vector) =>
            TryParse(ss, TryParseValue, FromArray, out vector, Cardinality);

        public string ToString(TVector vector, IFormatProvider provider = null) =>
            ToStringInner(vector, DefaultFormat, provider);

        public string ToString(TVector vector, string format, IFormatProvider provider = null)
        {
            if (string.IsNullOrEmpty(format))
                format = DefaultFormat;
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

        private string ToStringInner(TVector vector, string format, IFormatProvider provider)
        {
            for (int i = 0; i < FormatKeys.Length; i++)
                format = format.Replace(FormatKeys[i], vector[i].ToString(null, provider));
            return format;
        }

        protected abstract TVector[] GetHeadings(T minusOne, T zero, T one);
        protected abstract string    GetDefaultFormat();
        protected abstract string[]  GetFormatKeys();
        protected abstract string[]  GetFormatStrings();
    }

    internal sealed class Vector2DHelper<TVector, T> : VectorHelper<TVector, T>
        where TVector : struct, IVector2D<TVector, T>
        where T : struct, IFormattable
    {
        private const int Cardinality = 2;

        public Vector2DHelper(FromArray1 fromArray, TryParseValue1 tryParse, T minusOne, T zero, T one)
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

        public Vector3DHelper(FromArray1 fromArray, TryParseValue1 tryParse, T minusOne, T zero, T one)
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
