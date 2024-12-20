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
        private const char DefaultEmptyChar     = '.';
        private const char DefaultSeparatorChar = '\n';

        public static string ToString(TMulti multi, IFormatProvider provider = null) =>
            ToString(multi, format: null, provider);

        public static string ToString(TMulti multi, ReadOnlySpan<char> format, IFormatProvider provider) =>
            ToString(multi, multi[^1].Range(), format, provider);

        public static string ToString(TMulti multi, Size size, ReadOnlySpan<char> format, IFormatProvider provider) =>
            ToString(multi, range: new(size), format, provider);

        public static string ToString(TMulti multi, VectorRange range, ReadOnlySpan<char> format, IFormatProvider _)
        {
            var empty = format.Length >= multi.Count
                ? format[multi.Count - 1]
                : DefaultEmptyChar;
            var separator = format.Length > multi.Count
                ? format[multi.Count..]
                : new[] { DefaultSeparatorChar };
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
            Parse(input, size: out _);

        public TMulti Parse(string input) =>
            Parse(input, size: out _);

        public TMulti Parse(ReadOnlySpan<char> input, out VectorRange range) =>
            Parse(input.ToString(), out range);

        public TMulti Parse(string input, out VectorRange range) =>
            Parse(input, input.Distinct().ToArray(), out range);

        public TMulti Parse(ReadOnlySpan<char> input, out Size size) =>
            Parse(input.ToString(), out size);

        public TMulti Parse(string input, out Size size) =>
            Parse(input, input.Distinct().ToArray(), out size);

        public TMulti Parse(ReadOnlySpan<char> input, Func<char, bool> predicate) =>
            Parse(input, predicate, size: out _);

        public TMulti Parse(string input, Func<char, bool> predicate) =>
            Parse(input, predicate, size: out _);

        public TMulti Parse(ReadOnlySpan<char> input, Func<char, bool> predicate, out VectorRange range) =>
            Parse(input.ToString(), predicate, out range);

        public TMulti Parse(string input, Func<char, bool> predicate, out VectorRange range) =>
            Parse(input, input.Where(predicate).Distinct().ToArray(), out range);

        public TMulti Parse(ReadOnlySpan<char> input, Func<char, bool> predicate, out Size size) =>
            Parse(input.ToString(), predicate, out size);

        public TMulti Parse(string input, Func<char, bool> predicate, out Size size) =>
            Parse(input, input.Where(predicate).Distinct().ToArray(), out size);

        public TMulti Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format) =>
            Parse(input, format, size: out _);

        public TMulti Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out VectorRange range) =>
            Parse(input, DefaultSeparatorChar, format, out range);

        public TMulti Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out Size size) =>
            Parse(input, DefaultSeparatorChar, format, out size);

        public TMulti Parse(ReadOnlySpan<char> input, char separator, ReadOnlySpan<char> format) =>
            Parse(input, separator, format, size: out _);

        public TMulti Parse(ReadOnlySpan<char> input, char separator, ReadOnlySpan<char> format, out VectorRange range)
        {
            TMulti multi = Parse(input, separator, format, out Size size);
            range = new(size);
            return multi;
        }

        public TMulti Parse(ReadOnlySpan<char> input, char separator, ReadOnlySpan<char> format, out Size size)
        {
            int width = 0, height = 1, x = 0, y = 0, i;
            var points = new HashSet<Vector>[format.Length + 1];
            for (i = 0; i < points.Length; i++)
                points[i] = new();
            for (int j = 0; j < input.Length; ++j, ++x)
            {
                if (input[j] == separator)
                {
                    width = x > width ? x : width;
                    if (x > 0)
                        ++height;
                    (x, y) = (-1, ++y);
                }
                else if ((i = format.IndexOf(input[j])) >= 0)
                {
                    points[i].Add((x, y));
                    points[^1].Add((x, y));
                }
            }
            width = x > width ? x : width;
            size = new(width, height);
            var grids = new TGrid[points.Length];
            for (i = 0; i < points.Length; i++)
                grids[i] = CreateGrid(points[i]);
            return CreateMulti(grids);
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
