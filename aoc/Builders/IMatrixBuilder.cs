﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace aoc.Builders
{
    public interface IMatrixBuilder<TMatrix, TVector, T> : IBuilder2<TMatrix>
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

        TMatrix[] ParseAll(string? input, int count, int size) =>
            ParseAll(input, null, count, size);
        bool TryParseAll(
            [NotNullWhen(true)] string? input,
            int count, int size,
            [MaybeNullWhen(false)] out TMatrix[] matrices);

        TMatrix[] ParseAll(string? input, IFormatProvider? provider, int count, int size);
        bool TryParseAll(
            [NotNullWhen(true)] string? input,
            IFormatProvider? provider, int count, int size,
            [MaybeNullWhen(false)] out TMatrix[] matrices);

        TMatrix[] ParseAll(string? input, NumberStyles styles, int count, int size) =>
            ParseAll(input, styles, null, count, size);
        bool TryParseAll(
            [NotNullWhen(true)] string? input,
            NumberStyles styles, int count, int size,
            [MaybeNullWhen(false)] out TMatrix[] matrices);

        TMatrix[] ParseAll(string? input, NumberStyles styles, IFormatProvider? provider, int count, int size);
        bool TryParseAll(
            [NotNullWhen(true)] string? input,
            NumberStyles styles, IFormatProvider? provider, int count, int size,
            [MaybeNullWhen(false)] out TMatrix[] matrices);
    }
}
