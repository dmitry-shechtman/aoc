﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace aoc.Grids.Builders
{
    public interface IBaseGridBuilder<TGrid>
    {
        string ToString<TSize>(TGrid grid, TSize size, ReadOnlySpan<char> format)
            where TSize : struct, ISize2D<TSize, int>;

        TGrid Parse(ReadOnlySpan<char> input);
        bool TryParse(ReadOnlySpan<char> input,
            [MaybeNullWhen(false)] out TGrid grid);

        TGrid Parse(ReadOnlySpan<char> input, out VectorRange range);
        bool TryParse(ReadOnlySpan<char> input, out VectorRange range,
            [MaybeNullWhen(false)] out TGrid grid);

        TGrid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format,
            [MaybeNullWhen(false)] out TGrid grid);

        TGrid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out VectorRange range);
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out VectorRange range,
            [MaybeNullWhen(false)] out TGrid grid);

        TGrid FromArray<TSize>(int[] array, TSize size)
            where TSize : struct, ISize2D<TSize, int>;

        TGrid FromArray(int[,] array);
    }
}
