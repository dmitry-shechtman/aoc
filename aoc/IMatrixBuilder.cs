using System;
using System.Globalization;

namespace aoc
{
    public interface IMatrixBuilder<TMatrix, TVector, T> : IBuilder<TMatrix>
        where TMatrix : struct, IMatrix<TMatrix, TVector, T>
        where TVector : struct, IVector<TVector, TMatrix, T>
        where T : struct
    {
        TMatrix FromVectors(params TVector[] vectors);
        TMatrix FromVectors(ReadOnlySpan<TVector> vectors);
        TMatrix FromVectors(params T[][] values);

        TMatrix FromElements(params T[] values);
        TMatrix FromElements(ReadOnlySpan<T> values);
        TMatrix FromElements(ReadOnlySpan<T> values, int chunkSize);

        TMatrix Parse(ReadOnlySpan<char> input, char separator, char separator2, IFormatProvider provider = null);
        bool TryParse(ReadOnlySpan<char> input, char separator, char separator2, out TMatrix matrix);
        bool TryParse(ReadOnlySpan<char> input, char separator, char separator2, IFormatProvider provider, out TMatrix matrix);

        TMatrix[] ParseAll(string input, IFormatProvider provider, int count, int size);
        bool TryParseAll(string input, int count, int size, out TMatrix[] matrices);
        bool TryParseAll(string input, IFormatProvider provider, int count, int size, out TMatrix[] matrices);

        TMatrix[] ParseAll(string input, NumberStyles styles, IFormatProvider provider, int count, int size);
        bool TryParseAll(string input, NumberStyles styles, int count, int size, out TMatrix[] matrices);
        bool TryParseAll(string input, NumberStyles styles, IFormatProvider provider, int count, int size, out TMatrix[] matrices);
    }
}
