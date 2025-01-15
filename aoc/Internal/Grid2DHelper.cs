using aoc.Grids;
using aoc.Grids.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace aoc.Internal
{
    abstract class Grid2DHelper<TSelf, TGrid> : GridHelper<TSelf, TGrid, Vector>, IGridBuilder<TGrid>
        where TSelf : Grid2DHelper<TSelf, TGrid>
        where TGrid : Grid2D<TGrid>
    {
        private const char   DefaultEmptyChar = '.';
        private const char   DefaultPointChar = '#';
        private const string DefaultSeparator = "\n";

        public string ToString(TGrid grid, IFormatProvider? provider = null) =>
            ToString(grid, null, provider);

        public string ToString(TGrid grid, ReadOnlySpan<char> format, IFormatProvider? _) =>
            ToString(grid, grid.Range(), format);

        public string ToString<TSize>(TGrid grid, TSize size, ReadOnlySpan<char> format)
            where TSize : struct, ISize2D<TSize, int>
        {
            GetSpecials(format, out var point, out var empty, out var separator);
            var chars = new char[(size.Width + separator.Length) * size.Height];
            for (int y = 0, i = 0, k; y < size.Height; y++)
            {
                for (int x = 0; x < size.Width; x++)
                    chars[i++] = grid.Contains((x, y))
                        ? point
                        : empty;
                for (k = 0; k < separator.Length; k++)
                    chars[i++] = separator[k];
            }
            return new(chars);
        }

        protected static void GetSpecials(ReadOnlySpan<char> format, out char point, out char empty, out ReadOnlySpan<char> separator, int index = 0)
        {
            point = format.Length > index
                ? format[index]
                : DefaultPointChar;
            empty = format.Length > index + 1
                ? format[index + 1]
                : DefaultEmptyChar;
            separator = format.Length > index + 2
                ? format[(index + 2)..]
                : DefaultSeparator;
        }

        public TGrid Parse(ReadOnlySpan<char> input) =>
            Parse(input, out _);

        public bool TryParse(ReadOnlySpan<char> input,
            [MaybeNullWhen(false)] out TGrid grid) =>
                TryParse(input, out _, out grid);

        public TGrid Parse(ReadOnlySpan<char> input, out VectorRange range) =>
            TryParse(input, out range, out TGrid? grid)
                ? grid
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(ReadOnlySpan<char> input, out VectorRange range,
            [MaybeNullWhen(false)] out TGrid grid) =>
                TryParse(input, string.Empty, out range, out grid);

        public TGrid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format) =>
            Parse(input, format, out _);

        public bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format,
            [MaybeNullWhen(false)] out TGrid grid) =>
                TryParse(input, format, out _, out grid);

        public TGrid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out VectorRange range) =>
            TryParse(input, format, out range, out TGrid? grid)
                ? grid
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out VectorRange range,
            [MaybeNullWhen(false)] out TGrid grid) =>
                TryParse(input, format, Span<Vector>.Empty, out range, out grid);

        public TGrid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, Span<Vector> output) =>
            Parse(input, format, output, out _);

        public bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, Span<Vector> output,
            [MaybeNullWhen(false)] out TGrid grid) =>
                TryParse(input, format, output, out _, out grid);

        public TGrid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, Span<Vector> output, out VectorRange range) =>
            TryParse(input, format, output, out range, out TGrid? grid)
                ? grid
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, Span<Vector> output, out VectorRange range,
            [MaybeNullWhen(false)] out TGrid grid)
        {
            int width = 0, height = 1, x = 0, y = 0;
            char c;
            HashSet<Vector> points = new();
            GetSpecials(format, out var point, out var empty, out var separator, output.Length);
            for (int i = 0, j; i < input.Length; ++i, ++x)
            {
                if ((c = input[i]) == empty)
                {
                }
                else if (c == point)
                {
                    points.Add((x, y));
                }
                else if ((j = format.IndexOf(c)) >= 0 && j < output.Length)
                {
                    output[j] = (x, y);
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
                    grid = null;
                    return false;
                }
            }
            width = x > width ? x : width;
            range = new((Size)(width, height));
            grid = CreateGrid(points);
            return true;
        }

        public TGrid FromArray<TSize>(int[] array, TSize size)
            where TSize : struct, ISize2D<TSize, int>
        {
            HashSet<Vector> points = new();
            for (int y = 0, i = 0; y < size.Height; y++)
                for (int x = 0; x < size.Width; x++, i++)
                    if (array[i] > 0)
                        points.Add((x, y));
            return CreateGrid(points);
        }

        public TGrid FromArray(int[,] array)
        {
            HashSet<Vector> points = new();
            for (int x = 0; x < array.GetLength(0); x++)
                for (int y = 0; y < array.GetLength(1); y++)
                    if (array[x, y] > 0)
                        points.Add((x, y));
            return CreateGrid(points);
        }
    }

    abstract class Grid2DHelper2<TSelf, TGrid> : Grid2DHelper<TSelf, TGrid>
        where TSelf : Grid2DHelper2<TSelf, TGrid>
        where TGrid : Grid2D<TGrid>
    {
        protected override bool TryGetHeading(ReadOnlySpan<char> input, ref int i, out int heading) =>
            i < input.Length + 2 && TryGetHeading(input[i..(i + 2)], out heading) ||
            base.TryGetHeading(input, ref i, out heading);
    }
}
