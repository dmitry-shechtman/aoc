﻿using aoc.Grids;
using aoc.Grids.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace aoc.Internal
{
    abstract class MultiGridParseHelper<TSelf, TMulti, TGrid> : Singleton<TSelf>, IMultiGridBuilder<TMulti, TGrid>
        where TSelf : MultiGridParseHelper<TSelf, TMulti, TGrid>
        where TMulti : MultiGrid<TMulti, TGrid>
        where TGrid : Grid2D<TGrid>
    {
        private const char   DefaultEmptyChar = '.';
        private const string DefaultSeparator = "\n";

        public string ToString(TMulti multi, IFormatProvider? provider = null) =>
            ToString(multi, null, provider);

        public string ToString(TMulti multi, ReadOnlySpan<char> format, IFormatProvider? _) =>
            ToString(multi, multi[^1].Range(), format);

        public string ToString<TSize>(TMulti multi, TSize size, ReadOnlySpan<char> format)
            where TSize : struct, ISize2D<TSize, int>
        {
            GetSpecials(format, out var empty, out var separator, multi.Count - 1);
            var chars = new char[(size.Width + separator.Length) * size.Height];
            for (int y = 0, i = 0, k; y < size.Height; y++)
            {
                for (int x = 0; x < size.Width; x++)
                    chars[i++] = multi[^1].Contains((x, y))
                        ? format[multi[..^1].FindIndex(g => g.Contains((x, y)))]
                        : empty;
                for (k = 0; k < separator.Length; k++)
                    chars[i++] = separator[k];
            }
            return new(chars);
        }

        public TMulti Parse(ReadOnlySpan<char> input) =>
            Parse(input, out _);

        public bool TryParse(ReadOnlySpan<char> input,
            [MaybeNullWhen(false)] out TMulti multi) =>
                TryParse(input, out _, out multi);

        public TMulti Parse(ReadOnlySpan<char> input, out VectorRange range) =>
            TryParse(input, out range, out TMulti? multi)
                ? multi
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(ReadOnlySpan<char> input, out VectorRange range,
            [MaybeNullWhen(false)] out TMulti multi) =>
                TryParse(input, GetFormat(input, stackalloc char[256]), out range, out multi);

        public TMulti Parse(ReadOnlySpan<char> input, Func<char, bool> predicate) =>
            Parse(input, predicate, out _);

        public bool TryParse(ReadOnlySpan<char> input, Func<char, bool> predicate,
            [MaybeNullWhen(false)] out TMulti multi) =>
                TryParse(input, predicate, out _, out multi);

        public TMulti Parse(ReadOnlySpan<char> input, Func<char, bool> predicate, out VectorRange range) =>
            TryParse(input, predicate, out range, out TMulti? multi)
                ? multi
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(ReadOnlySpan<char> input, Func<char, bool> predicate, out VectorRange range,
            [MaybeNullWhen(false)] out TMulti multi) =>
                TryParse(input, GetFormat(input, stackalloc char[256], predicate), out range, out multi);

        public TMulti Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format) =>
            Parse(input, format, out _);

        public bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format,
            [MaybeNullWhen(false)] out TMulti multi) =>
                TryParse(input, format, out _, out multi);

        public TMulti Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out VectorRange range) =>
            Parse(input, format, DefaultSeparator, out range);

        public bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out VectorRange range,
            [MaybeNullWhen(false)] out TMulti multi) =>
                TryParse(input, format, DefaultSeparator, out range, out multi);

        public TMulti Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, ReadOnlySpan<char> separator) =>
            Parse(input, format, separator, out _);

        public bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, ReadOnlySpan<char> separator,
            [MaybeNullWhen(false)] out TMulti multi) =>
                TryParse(input, format, separator, out _, out multi);

        public TMulti Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, ReadOnlySpan<char> separator, out VectorRange range) =>
            TryParse(input, format, separator, out range, out TMulti? multi)
                ? multi
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, ReadOnlySpan<char> separator, out VectorRange range,
            [MaybeNullWhen(false)] out TMulti multi)
        {
            int width = 0, height = 1, x = 0, y = 0;
            char c, empty = DefaultEmptyChar;
            var points = CreatePoints(format.Length);
            for (int i = 0, j; i < input.Length; ++i, ++x)
            {
                if ((c = input[i]) == empty)
                {
                }
                else if ((j = format.IndexOf(c)) >= 0)
                {
                    points[j].Add((x, y));
                    points[^1].Add((x, y));
                }
                else if (i <= input.Length - separator.Length &&
                    input[i..].StartsWith(separator))
                {
                    width = x > width ? x : width;
                    if (x > 0)
                        ++height;
                    (x, y, i) = (-1, ++y, i + separator.Length - 1);
                }
                else
                {
                    range = default;
                    multi = null;
                    return false;
                }
            }
            width = x > width ? x : width;
            range = new((Size)(width, height));
            multi = CreateMulti(points);
            return true;
        }

        public TMulti FromArray<TSize>(int[] array, TSize size)
            where TSize : struct, ISize2D<TSize, int>
        {
            int min = int.MaxValue, max = 0;
            for (int i = 0; i < array.Length; i++)
                if (array[i] > 0)
                    (min, max) = (Math.Min(min, array[i]), Math.Max(max, array[i]));
            return FromArray(array, size, (min, max));
        }

        public TMulti FromArray<TSize>(int[] array, TSize size, Range range)
            where TSize : struct, ISize2D<TSize, int>
        {
            var points = CreatePoints(range.Count);
            for (int y = 0, i = 0; y < size.Height; y++)
            {
                for (int x = 0; x < size.Width; x++, i++)
                {
                    if (array[i] > 0)
                    {
                        points[array[i] - range.Min].Add((x, y));
                        points[^1].Add((x, y));
                    }
                }
            }
            return CreateMulti(points);
        }

        public TMulti FromArray(int[,] array)
        {
            int min = int.MaxValue, max = 0;
            for (int x = 0; x < array.GetLength(0); x++)
                for (int y = 0; y < array.GetLength(1); y++)
                    if (array[x, y] > 0)
                        (min, max) = (Math.Min(min, array[x, y]), Math.Max(max, array[x, y]));
            return FromArray(array, (min, max));
        }

        public TMulti FromArray(int[,] array, Range range)
        {
            var points = CreatePoints(range.Count);
            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    if (array[x, y] > 0)
                    {
                        points[array[x, y] - range.Min].Add((x, y));
                        points[^1].Add((x, y));
                    }
                }
            }
            return CreateMulti(points);
        }

        private static HashSet<Vector>[] CreatePoints(int count)
        {
            var points = new HashSet<Vector>[count + 1];
            for (int i = 0; i < points.Length; i++)
                points[i] = new();
            return points;
        }

        private TMulti CreateMulti(HashSet<Vector>[] points)
        {
            var grids = new TGrid[points.Length];
            for (int i = 0; i < points.Length; i++)
                grids[i] = CreateGrid(points[i]);
            return CreateMulti(grids);
        }

        private static Span<char> GetFormat(ReadOnlySpan<char> input, Span<char> format) =>
            GetFormat(input, format, c => true);

        private static Span<char> GetFormat(ReadOnlySpan<char> input, Span<char> format, Func<char, bool> predicate)
        {
            int count = 0;
            for (int i = 0; i < input.Length; i++)
                if (input[i] != '\n' && predicate(input[i])
                    && format[..count].IndexOf(input[i]) < 0)
                    format[count++] = input[i];
            format[..count].Sort();
            return format[..count];
        }

        private static void GetSpecials(ReadOnlySpan<char> format, out char empty, out ReadOnlySpan<char> separator, int index)
        {
            empty = format.Length > index
                ? format[index]
                : DefaultEmptyChar;
            separator = format.Length > index + 1
                ? format[(index + 1)..]
                : DefaultSeparator;
        }

        protected abstract TGrid CreateGrid(HashSet<Vector> points);
        protected abstract TMulti CreateMulti(TGrid[] grids);
    }

    sealed class MultiGridParseHelper : MultiGridParseHelper<MultiGridParseHelper, MultiGrid, Grid>
    {
        private MultiGridParseHelper()
        {
        }

        protected override Grid CreateGrid(HashSet<Vector> points) =>
            new(points);

        protected override MultiGrid CreateMulti(Grid[] grids) =>
            new(grids);
    }
}
