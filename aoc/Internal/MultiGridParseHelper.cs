using aoc.Grids;
using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc.Internal
{
    abstract class MultiGridParseHelper<TSelf, TMulti, TGrid> : Singleton<TSelf>
        where TSelf : MultiGridParseHelper<TSelf, TMulti, TGrid>
        where TMulti : MultiGrid<TMulti, TGrid>
        where TGrid : Grid<TGrid>
    {
        private const char   DefaultEmptyChar = '.';
        private const string DefaultSeparator = "\n";

        public static string ToString(TMulti multi, IFormatProvider provider = null) =>
            ToString(multi, format: null, provider);

        public static string ToString(TMulti multi, ReadOnlySpan<char> format, IFormatProvider provider) =>
            ToString(multi, multi[^1].Range(), format, provider);

        public static string ToString(TMulti multi, Size size, ReadOnlySpan<char> format, IFormatProvider provider) =>
            ToString(multi, range: new(size), format, provider);

        public static string ToString(TMulti multi, VectorRange range, ReadOnlySpan<char> format, IFormatProvider _)
        {
            GetSpecials(format, out var empty, out var separator, multi.Count - 1);
            var chars = new char[(range.Width + separator.Length) * range.Height];
            for (int y = range.Min.Y, i = 0, j = 0, k; y <= range.Max.Y; y++)
            {
                for (int x = range.Min.X; x <= range.Max.X; x++, i++)
                {
                    var index = multi[..^1].FindIndex(g => g.Contains((x, y)));
                    chars[j++] = index >= 0
                        ? format[index]
                        : empty;
                }
                for (k = 0; k < separator.Length; k++)
                    chars[j++] = separator[k];
            }
            return new(chars);
        }

        public TMulti Parse(ReadOnlySpan<char> input) =>
            Parse(input, out _);

        public TMulti Parse(ReadOnlySpan<char> input, out VectorRange range) =>
            Parse(input, GetFormat(input, stackalloc char[256]), out range);

        public TMulti Parse(ReadOnlySpan<char> input, Func<char, bool> predicate) =>
            Parse(input, predicate, out _);

        public TMulti Parse(ReadOnlySpan<char> input, Func<char, bool> predicate, out VectorRange range) =>
            Parse(input, GetFormat(input, stackalloc char[256], predicate), out range);

        public TMulti Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format) =>
            Parse(input, format, out _);

        public TMulti Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out VectorRange range) =>
            Parse(input, format, DefaultSeparator, out range);

        public TMulti Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, ReadOnlySpan<char> separator) =>
            Parse(input, format, separator, out _);

        public TMulti Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, ReadOnlySpan<char> separator, out VectorRange range)
        {
            int width = 0, height = 1, x = 0, y = 0, i;
            var points = new HashSet<Vector>[format.Length + 1];
            for (i = 0; i < points.Length; i++)
                points[i] = new();
            for (int j = 0; j < input.Length; ++j, ++x)
            {
                if ((i = format.IndexOf(input[j])) >= 0)
                {
                    points[i].Add((x, y));
                    points[^1].Add((x, y));
                }
                else if (j <= input.Length - separator.Length &&
                    input[j..(j + separator.Length)].SequenceEqual(separator))
                {
                    width = x > width ? x : width;
                    if (x > 0)
                        ++height;
                    (x, y, j) = (-1, ++y, j + separator.Length - 1);
                }
            }
            width = x > width ? x : width;
            range = new((Size)(width, height));
            var grids = new TGrid[points.Length];
            for (i = 0; i < points.Length; i++)
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
