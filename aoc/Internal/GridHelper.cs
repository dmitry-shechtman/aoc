using aoc.Grids;
using aoc.Grids.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace aoc.Internal
{
    abstract class GridHelper<TSelf, TGrid, TVector, TSize, TRange, T> : Singleton<TSelf>, IHeadingBuilder, IVectorBuilder<TVector>, IPathBuilder<TVector>
        where TSelf : GridHelper<TSelf, TGrid, TVector, TSize, TRange, T>
        where TGrid : IGrid<TVector>, IReadOnlyCollection<TVector>
        where TVector : struct, IVector<TVector, T>
        where TSize : struct, ISize<TSize, TVector, T>
        where TRange : struct, IRange<TRange, TVector, T>
        where T : struct
    {
        public abstract TVector[] Headings { get; }

        public virtual int CountNeighbors(TGrid grid, TVector p) =>
            GetNeighbors(p).Count(grid.Points.Contains);

        public virtual int CountNeighborsAndSelf(TGrid grid, TVector p) =>
            GetNeighborsAndSelf(p).Count(grid.Points.Contains);

        public int FloodFill(TGrid grid, TVector from)
        {
            Queue<TVector> queue = new();
            if (grid.Points.Add(from))
                queue.Enqueue(from);
            int count = 0;
            for (; queue.TryDequeue(out from); ++count)
                foreach (var p in GetNeighbors(from).Where(grid.Points.Add))
                    queue.Enqueue(p);
            return count;
        }

        public TGrid MoveNext(TGrid grid) =>
            MoveNext(grid, (p, c) => DefaultFilterInclusive(grid, p, c));

        public TGrid MoveNext(TGrid grid, TSize size) =>
            MoveNext(grid, GetAllNeighborsAndSelf(grid, size), (p, c) => DefaultFilterInclusive(grid, p, c));

        public TGrid MoveNext(TGrid grid, TRange range) =>
            MoveNext(grid, GetAllNeighborsAndSelf(grid, range), (p, c) => DefaultFilterInclusive(grid, p, c));

        public TGrid MoveNext(TGrid grid, Func<TVector, bool> predicate, bool inclusive) =>
            MoveNext(grid, GetAllNeighbors(grid, inclusive), predicate);

        public TGrid MoveNext(TGrid grid, Func<TVector, bool> predicate, TSize size, bool inclusive) =>
            MoveNext(grid, GetAllNeighbors(grid, size, inclusive), predicate);

        public TGrid MoveNext(TGrid grid, Func<TVector, bool> predicate, TRange range, bool inclusive) =>
            MoveNext(grid, GetAllNeighbors(grid, range, inclusive), predicate);

        public TGrid MoveNext(TGrid grid, Func<TVector, int, bool> filterInclusive) =>
            MoveNext(grid, GetAllNeighborsAndSelf(grid), filterInclusive);

        public TGrid MoveNext(TGrid grid, Func<TVector, int, bool> filterInclusive, TSize size) =>
            MoveNext(grid, GetAllNeighborsAndSelf(grid, size), filterInclusive);

        public TGrid MoveNext(TGrid grid, Func<TVector, int, bool> filterInclusive, TRange range) =>
            MoveNext(grid, GetAllNeighborsAndSelf(grid, range), filterInclusive);

        private static TGrid MoveNext(TGrid grid, ParallelQuery<TVector> pp, Func<TVector, bool> predicate)
        {
            grid.Points = pp.Where(predicate).ToHashSet();
            return grid;
        }

        private TGrid MoveNext(TGrid grid, ParallelQuery<TVector> pp, Func<TVector, int, bool> filterInclusive)
        {
            grid.Points = pp.Where(p => filterInclusive(p, CountNeighborsAndSelf(grid, p))).ToHashSet();
            return grid;
        }

        private static bool DefaultFilterInclusive(TGrid grid, TVector p, int count) =>
            count == 3 || count == 4 && grid.Points.Contains(p);

        private ParallelQuery<TVector> GetAllNeighborsAndSelf(TGrid grid, TSize size) =>
            GetAllNeighbors(grid, size, true);

        private ParallelQuery<TVector> GetAllNeighborsAndSelf(TGrid grid, TRange range) =>
            GetAllNeighbors(grid, range, true);

        private ParallelQuery<TVector> GetAllNeighborsAndSelf(TGrid grid) =>
            GetAllNeighbors(grid, true);

        private ParallelQuery<TVector> GetAllNeighbors(TGrid grid, TSize size, bool inclusive) =>
            GetAllNeighbors(grid, inclusive).Where(size.Contains);

        private ParallelQuery<TVector> GetAllNeighbors(TGrid grid, TRange range, bool inclusive) =>
            GetAllNeighbors(grid, inclusive).Where(range.Contains);

        private ParallelQuery<TVector> GetAllNeighbors(TGrid grid, bool inclusive)
        {
            var query = inclusive
                ? grid.SelectMany(GetNeighborsAndSelf)
                : grid.SelectMany(GetNeighbors);
            return query.Distinct().AsParallel();
        }

        public abstract IEnumerable<TVector> GetNeighbors(TVector p);
        public abstract IEnumerable<TVector> GetNeighborsAndSelf(TVector p);

        int IHeadingBuilder.Parse(ReadOnlySpan<char> input) =>
            TryGetHeading(input, out int heading)
                ? heading
                : throw new ArgumentOutOfRangeException(nameof(input));

        bool IHeadingBuilder.TryParse(ReadOnlySpan<char> input, out int heading) =>
            TryGetHeading(input, out heading);

        protected bool TryGetHeading(ReadOnlySpan<char> input, out int heading)
        {
            for (int i = 0; i < FormatStrings.Length; i++)
                for (heading = 0; heading < FormatStrings[i].Length; heading++)
                    if (input.Equals(FormatStrings[i][heading], StringComparison.OrdinalIgnoreCase))
                        return true;
            heading = -1;
            return false;
        }

        string IVectorBuilder<TVector>.ToString(TVector vector, char format)
        {
            int index = Headings.IndexOf(vector);
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(vector));
            if (!TryGetFormatStrings(format, out var ss))
                throw new ArgumentOutOfRangeException(nameof(format));
            return char.IsUpper(format)
                ? ss[index].ToUpperInvariant()
                : ss[index];
        }

        IEnumerable<TVector> IVectorBuilder<TVector>.ParseAll(ReadOnlySpan<char> input, ReadOnlySpan<char> skip)
        {
            var vectors = Enumerable.Empty<TVector>();
            for (int i = 0; i < input.Length;)
                if (skip.Contains(input[i]))
                    ++i;
                else
                    vectors = vectors.Append(ParseVector(input, ref i));
            return vectors;
        }

        bool IVectorBuilder<TVector>.TryParseAll(ReadOnlySpan<char> input, ReadOnlySpan<char> skip, out IEnumerable<TVector> vectors)
        {
            vectors = Enumerable.Empty<TVector>();
            for (int i = 0; i < input.Length;)
                if (skip.Contains(input[i]))
                    ++i;
                else if (TryParse(input, ref i, out TVector vector))
                    vectors = vectors.Append(vector);
                else
                    return false;
            return true;
        }

        TVector IVectorBuilder<TVector>.Parse(ReadOnlySpan<char> input) =>
            ParseVector(input);

        TVector ParseVector(ReadOnlySpan<char> input)
        {
            int i = 0;
            return ParseVector(input, ref i);
        }

        bool IVectorBuilder<TVector>.TryParse(ReadOnlySpan<char> input, out TVector vector)
        {
            int i = 0;
            return TryParse(input, ref i, out vector);
        }

        private TVector ParseVector(ReadOnlySpan<char> input, ref int i) =>
            TryParse(input, ref i, out TVector vector)
                ? vector
                : throw new InvalidOperationException("Input string was not in a correct format.");

        IEnumerable<PathSegment<TVector>> IPathBuilder<TVector>.Parse(ReadOnlySpan<char> input)
        {
            var path = Enumerable.Empty<PathSegment<TVector>>();
            for (int i = 0; i < input.Length;)
                path = path.Append((ParseVector(input, ref i), ParseInt32(input, ref i)));
            return path;
        }

        IEnumerable<PathSegment<TVector>> IPathBuilder<TVector>.Parse(string? input, char separator) =>
            ParsePath(input?.Split(separator));

        IEnumerable<PathSegment<TVector>> IPathBuilder<TVector>.Parse(string? input, string separator) =>
            ParsePath(input?.Split(separator));

        IEnumerable<PathSegment<TVector>> IPathBuilder<TVector>.Parse(string[]? ss) =>
            ParsePath(ss);

        IEnumerable<PathSegment<TVector>> ParsePath(string[]? ss) =>
            ss?.Select(ParsePathSegment)
                ?? Enumerable.Empty<PathSegment<TVector>>();

        IEnumerable<PathSegment<TVector>> IPathBuilder<TVector>.Parse(string[]? ss, char separator) =>
            ParsePath(ss?.Select(input => input.Split(separator)));

        IEnumerable<PathSegment<TVector>> IPathBuilder<TVector>.Parse(string[]? ss, string separator) =>
            ParsePath(ss?.Select(input => input.Split(separator)));

        IEnumerable<PathSegment<TVector>> ParsePath(IEnumerable<string[]>? tt) =>
            tt?.Select(ParsePathSegment)
                ?? Enumerable.Empty<PathSegment<TVector>>();

        private PathSegment<TVector> ParsePathSegment(string input)
        {
            int i = 0;
            return new(ParseVector(input, ref i), int.Parse(input[i..]));
        }

        private PathSegment<TVector> ParsePathSegment(string[] tt) =>
            new(ParseVector(tt[0]), int.Parse(tt[1]));

        private bool TryParse(ReadOnlySpan<char> input, ref int i, out TVector vector)
        {
            if (!TryGetHeading(input, ref i, out int heading))
            {
                vector = default;
                return false;
            }
            vector = Headings[heading];
            return true;
        }

        protected abstract string[][] FormatStrings { get; }

        protected virtual bool TryGetHeading(ReadOnlySpan<char> input, ref int i, out int heading) =>
            TryGetHeading(input[i..++i], out heading);

        private bool TryGetFormatStrings(char format, [MaybeNullWhen(false)] out string[] ss)
        {
            string input = $"{format}";
            for (int i = 0; i < FormatStrings.Length; i++)
                if ((ss = FormatStrings[i]).Contains(input, StringComparer.OrdinalIgnoreCase))
                    return true;
            ss = null;
            return false;
        }

        protected static int ParseInt32(ReadOnlySpan<char> input, ref int i)
        {
            int j;
            for (j = i + 1; j < input.Length; j++)
                if (!char.IsDigit(input[j]))
                    break;
            var p = int.Parse(input[i..j]);
            i = j;
            return p;
        }
    }

    abstract class GridHelper<TSelf, TGrid> : GridHelper<TSelf, TGrid, Vector, Size, VectorRange, int>, IGridBuilder<TGrid>
        where TSelf : GridHelper<TSelf, TGrid>
        where TGrid : Grid<TGrid>
    {
        private const char   DefaultEmptyChar = '.';
        private const char   DefaultPointChar = '#';
        private const string DefaultSeparator = "\n";

        public string ToString(TGrid grid, IFormatProvider? provider = null) =>
            ToString(grid, null, provider);

        public string ToString(TGrid grid, ReadOnlySpan<char> format, IFormatProvider? _) =>
            ToString(grid, grid.Range(), format);

        public string ToString(TGrid grid, Size size, ReadOnlySpan<char> format) =>
            ToString(grid, range: new(size), format);

        public string ToString(TGrid grid, VectorRange range, ReadOnlySpan<char> format)
        {
            GetSpecials(format, out var point, out var empty, out var separator);
            var chars = new char[(range.Width + separator.Length) * range.Height];
            for (int y = range.Min.X, i = 0, k; y <= range.Max.Y; y++)
            {
                for (int x = range.Min.Y; x <= range.Max.Y; x++)
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
                    grid = null;
                    return false;
                }
            }
            width = x > width ? x : width;
            range = new((Size)(width, height));
            grid = CreateGrid(points);
            return true;
        }

        protected abstract TGrid CreateGrid(HashSet<Vector> points);
    }

    abstract class GridHelper2<TSelf, TGrid> : GridHelper<TSelf, TGrid>
        where TSelf : GridHelper2<TSelf, TGrid>
        where TGrid : Grid<TGrid>
    {
        protected override bool TryGetHeading(ReadOnlySpan<char> input, ref int i, out int heading) =>
            i < input.Length + 2 && TryGetHeading(input[i..(i + 2)], out heading) ||
            base.TryGetHeading(input, ref i, out heading);
    }
}
