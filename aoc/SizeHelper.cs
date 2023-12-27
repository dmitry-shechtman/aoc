using System;

namespace aoc
{
    internal abstract class SizeHelper<TSize, TVector, T> : Helper1<TSize, T>
        where TSize : struct, ISize<TSize, TVector, T>
        where TVector : struct, IVector<TVector, T>
        where T : struct, IFormattable
    {
        protected SizeHelper(Func<T[], TSize> fromArray, TryParseValue1<T> tryParse, int cardinality)
            : base(fromArray, tryParse, cardinality)
        {
        }
    }

    internal sealed class Size2DHelper<TSize, TVector, T> : SizeHelper<TSize, TVector, T>
        where TSize : struct, ISize2D<TSize, TVector, T>
        where TVector : struct, IVector2D<TVector, T>
        where T : struct, IFormattable
    {
        private const int Cardinality = 2;

        public Size2DHelper(Func<T[], TSize> fromArray, TryParseValue1<T> tryParse)
            : base(fromArray, tryParse, Cardinality)
        {
        }

        protected override string   GetDefaultFormat() => "w:h";
        protected override string[] GetFormatKeys()    => new[] { "w", "h" };
    }

    internal sealed class Size3DHelper<TSize, TVector, T> : SizeHelper<TSize, TVector, T>
        where TSize : struct, ISize3D<TSize, TVector, T>
        where TVector : struct, IVector3D<TVector, T>
        where T : struct, IFormattable
    {
        private const int Cardinality = 3;

        public Size3DHelper(Func<T[], TSize> fromArray, TryParseValue1<T> tryParse)
            : base(fromArray, tryParse, Cardinality)
        {
        }

        protected override string   GetDefaultFormat() => "w:h:d";
        protected override string[] GetFormatKeys()    => new[] { "w", "h", "d" };
    }
}
