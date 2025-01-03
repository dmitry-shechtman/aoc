﻿using aoc.Grids;
using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc.Internal
{
    abstract class GridHelper<TSelf, TGrid, TVector, TSize, TRange, T> : Singleton<TSelf>
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

        private TGrid MoveNext(TGrid grid, ParallelQuery<TVector> pp, Func<TVector, bool> predicate)
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

        public int GetHeading(ReadOnlySpan<char> input) =>
            TryGetHeading(input, out int heading)
                ? heading
                : throw new ArgumentOutOfRangeException(nameof(input));

        public bool TryGetHeading(ReadOnlySpan<char> input, out int heading)
        {
            for (int i = 0; i < FormatStrings.Length; i++)
                for (heading = 0; heading < FormatStrings[i].Length; heading++)
                    if (input.Equals(FormatStrings[i][heading], StringComparison.OrdinalIgnoreCase))
                        return true;
            heading = -1;
            return false;
        }

        public string ToString(TVector vector, char format)
        {
            int index = Headings.IndexOf(vector);
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(vector));
            string[] ss = GetFormatStrings(format);
            if (ss is null)
                throw new ArgumentOutOfRangeException(nameof(format));
            return char.IsUpper(format)
                ? ss[index].ToUpperInvariant()
                : ss[index];
        }

        public IEnumerable<TVector> ParseVectors(ReadOnlySpan<char> input, ReadOnlySpan<char> skip)
        {
            var vectors = Enumerable.Empty<TVector>();
            for (int i = 0; i < input.Length;)
                if (skip.Contains(input[i]))
                    ++i;
                else
                    vectors = vectors.Append(ParseVector(input, ref i));
            return vectors;
        }

        public bool TryParseVectors(ReadOnlySpan<char> input, ReadOnlySpan<char> skip, out IEnumerable<TVector> vectors)
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

        public TVector ParseVector(ReadOnlySpan<char> input)
        {
            int i = 0;
            return ParseVector(input, ref i);
        }

        public bool TryParseVector(ReadOnlySpan<char> input, out TVector vector)
        {
            int i = 0;
            return TryParse(input, ref i, out vector);
        }

        private TVector ParseVector(ReadOnlySpan<char> input, ref int i) =>
            TryParse(input, ref i, out TVector vector)
                ? vector
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public IEnumerable<PathSegment<TVector>> ParsePath(ReadOnlySpan<char> input)
        {
            var path = Enumerable.Empty<PathSegment<TVector>>();
            for (int i = 0; i < input.Length;)
                path = path.Append((ParseVector(input, ref i), ParseInt32(input, ref i)));
            return path;
        }

        public IEnumerable<PathSegment<TVector>> ParsePath(string input, char separator) =>
            ParsePath(input.Split(separator));

        public IEnumerable<PathSegment<TVector>> ParsePath(string input, string separator) =>
            ParsePath(input.Split(separator));

        public IEnumerable<PathSegment<TVector>> ParsePath(string[] ss) =>
            ss.Select(ParsePathSegment);

        public IEnumerable<PathSegment<TVector>> ParsePath(string[] ss, char separator) =>
            ss.Select(input => input.Split(separator))
                .Select(ParsePathSegment);

        public IEnumerable<PathSegment<TVector>> ParsePath(string[] ss, string separator) =>
            ss.Select(input => input.Split(separator))
                .Select(ParsePathSegment);

        private PathSegment<TVector> ParsePathSegment(string input)
        {
            int i = 0;
            return new(ParseVector(input, ref i), int.Parse(input[i..]));
        }

        private PathSegment<TVector> ParsePathSegment(string[] tt) =>
            new(ParseVector(tt[0]), int.Parse(tt[1]));

        public virtual bool TryParse(ReadOnlySpan<char> input, ref int i, out TVector vector)
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

        private string[] GetFormatStrings(char format)
        {
            string input = $"{format}";
            foreach (var ss in FormatStrings)
                if (ss.Contains(input, StringComparer.OrdinalIgnoreCase))
                    return ss;
            return null;
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

    abstract class GridHelper<TSelf, TGrid> : GridHelper<TSelf, TGrid, Vector, Size, VectorRange, int>
        where TSelf : GridHelper<TSelf, TGrid>
        where TGrid : Grid<TGrid>
    {
        private const char   DefaultEmptyChar = '.';
        private const char   DefaultPointChar = '#';
        private const string DefaultSeparator = "\n";

        public static string ToString(TGrid grid, IFormatProvider provider = null) =>
            ToString(grid, null, provider);

        public static string ToString(TGrid grid, ReadOnlySpan<char> format, IFormatProvider provider) =>
            ToString(grid, grid.Range(), format, provider);

        public static string ToString(TGrid grid, Size size, ReadOnlySpan<char> format, IFormatProvider provider) =>
            ToString(grid, range: new(size), format, provider);

        public static string ToString(TGrid grid, VectorRange range, ReadOnlySpan<char> format, IFormatProvider _)
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
    }

    sealed class GridHelper : GridHelper<GridHelper, Grid>
    {
        private GridHelper()
        {
        }

        public override Vector[] Headings => new[]
        {
            Vector.North, Vector.East, Vector.South, Vector.West
        };

        protected override string[][] FormatStrings => new[]
        {
            new[] { "n", "e", "s", "w" },
            new[] { "u", "r", "d", "l" },
            new[] { "^", ">", "v", "<" }
        };

        public override Vector[] GetNeighbors(Vector p) => new Vector[]
        {
            new(p.x, p.y - 1),
            new(p.x + 1, p.y),
            new(p.x, p.y + 1),
            new(p.x - 1, p.y)
        };

        public override Vector[] GetNeighborsAndSelf(Vector p) => new Vector[]
        {
            new(p.x, p.y),
            new(p.x, p.y - 1),
            new(p.x + 1, p.y),
            new(p.x, p.y + 1),
            new(p.x - 1, p.y)
        };

        public static Grid Parse(ReadOnlySpan<char> input) =>
            Parse(input, out _);

        public static Grid Parse(ReadOnlySpan<char> input, out VectorRange range) =>
            Parse(input, string.Empty, out range);

        public static Grid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format) =>
            Parse(input, format, out _);

        public static Grid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out VectorRange range) =>
            Parse(input, format, Span<Vector>.Empty, out range);

        public static Grid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, Span<Vector> output) =>
            Parse(input, format, output, out _);

        public static Grid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, Span<Vector> output, out VectorRange range) =>
            TryParse(input, format, output, out range, out Grid value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public static bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, Span<Vector> output, out VectorRange range, out Grid grid)
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
            grid = new(points);
            return true;
        }

        public static IEnumerable<(Matrix, int)> ParseTurns(ReadOnlySpan<char> input)
        {
            Matrix t = Matrix.Identity;
            var turns = Enumerable.Empty<(Matrix, int)>();
            for (int i = 0; i < input.Length; i++)
            {
                turns = turns.Append((t, ParseInt32(input, ref i)));
                if (i == input.Length)
                    break;
                t = ParseTurn(input[i]);
            }
            return turns;
        }

        private static Matrix ParseTurn(char c) => char.ToLower(c) switch
        {
            'r' => Matrix.RotateRight,
            'l' => Matrix.RotateLeft,
            _ => throw new InvalidOperationException($"Unexpected character: {c}"),
        };
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
