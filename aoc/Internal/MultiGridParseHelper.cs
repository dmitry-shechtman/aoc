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

        public bool TryParse(ReadOnlySpan<char> input, out TMulti multi) =>
            TryParse(input, out _, out multi);

        public TMulti Parse(ReadOnlySpan<char> input, out VectorRange range) =>
            TryParse(input, out range, out TMulti multi)
                ? multi
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(ReadOnlySpan<char> input, out VectorRange range, out TMulti multi) =>
            TryParse(input, GetFormat(input, stackalloc char[256]), out range, out multi);

        public TMulti Parse(ReadOnlySpan<char> input, Func<char, bool> predicate) =>
            Parse(input, predicate, out _);

        public bool TryParse(ReadOnlySpan<char> input, Func<char, bool> predicate, out TMulti multi) =>
            TryParse(input, predicate, out _, out multi);

        public TMulti Parse(ReadOnlySpan<char> input, Func<char, bool> predicate, out VectorRange range) =>
            TryParse(input, predicate, out range, out TMulti multi)
                ? multi
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(ReadOnlySpan<char> input, Func<char, bool> predicate, out VectorRange range, out TMulti multi) =>
            TryParse(input, GetFormat(input, stackalloc char[256], predicate), out range, out multi);

        public TMulti Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format) =>
            Parse(input, format, out _);

        public bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out TMulti multi) =>
            TryParse(input, format, out _, out multi);

        public TMulti Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out VectorRange range) =>
            Parse(input, format, DefaultSeparator, out range);

        public bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out VectorRange range, out TMulti multi) =>
            TryParse(input, format, DefaultSeparator, out range, out multi);

        public TMulti Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, ReadOnlySpan<char> separator) =>
            Parse(input, format, separator, out _);

        public bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, ReadOnlySpan<char> separator, out TMulti multi) =>
            TryParse(input, format, separator, out _, out multi);

        public TMulti Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, ReadOnlySpan<char> separator, out VectorRange range) =>
            TryParse(input, format, separator, out range, out TMulti multi)
                ? multi
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, ReadOnlySpan<char> separator, out VectorRange range, out TMulti multi)
        {
            int width = 0, height = 1, x = 0, y = 0, i, j;
            char c, empty = DefaultEmptyChar;
            var points = new HashSet<Vector>[format.Length + 1];
            for (i = 0; i < points.Length; i++)
                points[i] = new();
            for (i = 0; i < input.Length; ++i, ++x)
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
                    input[i..(i + separator.Length)].SequenceEqual(separator))
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
            var grids = new TGrid[points.Length];
            for (i = 0; i < points.Length; i++)
                grids[i] = CreateGrid(points[i]);
            multi = CreateMulti(grids);
            return true;
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
