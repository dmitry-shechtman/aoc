using aoc.Grids;
using aoc.Grids.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace aoc.Internal
{
    abstract class GridHelper<TSelf, TGrid, TVector> : Singleton<TSelf>,
            IHeadingBuilder, IVectorBuilder<TVector>, IPathBuilder<TVector>,
            INextBuilder<TGrid, TVector>
        where TSelf : GridHelper<TSelf, TGrid, TVector>
        where TGrid : IGrid<TVector>, IReadOnlyCollection<TVector>
        where TVector : struct, IVector<TVector>
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

        public TGrid Move(TGrid grid) =>
            Move(grid, (p, c) => DefaultFilterInclusive(grid, p, c));

        public TGrid Move<TSize>(TGrid grid, TSize size)
            where TSize : struct, ISize<TSize, TVector> =>
                Move(grid, GetAllNeighborsAndSelf(grid, size), (p, c) => DefaultFilterInclusive(grid, p, c));

        public TGrid Move(TGrid grid, Func<TVector, bool> predicate, bool inclusive) =>
            Move(GetAllNeighbors(grid, inclusive), predicate);

        public TGrid Move<TSize>(TGrid grid, Func<TVector, bool> predicate, TSize size, bool inclusive)
            where TSize : struct, ISize<TSize, TVector> =>
                Move(GetAllNeighbors(grid, size, inclusive), predicate);

        public TGrid Move(TGrid grid, Func<TVector, int, bool> filterInclusive) =>
            Move(grid, GetAllNeighborsAndSelf(grid), filterInclusive);

        public TGrid Move<TSize>(TGrid grid, Func<TVector, int, bool> filterInclusive, TSize size)
            where TSize : struct, ISize<TSize, TVector> =>
                Move(grid, GetAllNeighborsAndSelf(grid, size), filterInclusive);

        private TGrid Move(ParallelQuery<TVector> pp, Func<TVector, bool> predicate) =>
            CreateGrid(pp.Where(predicate).ToHashSet());

        private TGrid Move(TGrid grid, ParallelQuery<TVector> pp, Func<TVector, int, bool> filterInclusive) =>
            CreateGrid(pp.Where(p => filterInclusive(p, CountNeighborsAndSelf(grid, p))).ToHashSet());

        private static bool DefaultFilterInclusive(TGrid grid, TVector p, int count) =>
            count == 3 || count == 4 && grid.Points.Contains(p);

        private ParallelQuery<TVector> GetAllNeighborsAndSelf<TSize>(TGrid grid, TSize size)
            where TSize : struct, ISize<TSize, TVector> =>
                GetAllNeighbors(grid, size, true);

        private ParallelQuery<TVector> GetAllNeighborsAndSelf(TGrid grid) =>
            GetAllNeighbors(grid, true);

        private ParallelQuery<TVector> GetAllNeighbors<TSize>(TGrid grid, TSize size, bool inclusive)
            where TSize : struct, ISize<TSize, TVector> =>
                GetAllNeighbors(grid, inclusive).Where(size.Contains);

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

        protected abstract TGrid CreateGrid(HashSet<TVector> points);
    }
}
