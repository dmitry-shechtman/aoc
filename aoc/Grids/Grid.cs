using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace aoc.Grids
{
    using Helper = Internal.GridHelper;

    public abstract class Grid<TSelf, TVector, TSize, TRange, T> : IReadOnlyCollection<TVector>
        where TSelf : Grid<TSelf, TVector, TSize, TRange, T>
        where TVector : struct, IVector<TVector, T>
        where TSize : struct, ISize<TSize, TVector>
        where TRange : struct, IRange<TRange, TVector, T>
        where T : struct
    {
        protected Grid(TVector[] points) =>
            Points = new(points);

        protected Grid(IEnumerable<TVector> points) =>
            Points = new(points);

        protected Grid(HashSet<TVector> points) =>
            Points = points;

        protected HashSet<TVector> Points { get; private set; }

        public IEnumerator<TVector> GetEnumerator() =>
            Points.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public int Count =>
            Points.Count;

        public bool Add(TVector point) =>
            Points.Add(point);

        public bool Remove(TVector point) =>
            Points.Remove(point);

        public bool Contains(TVector point) =>
            Points.Contains(point);

        public int FloodFill(TVector from)
        {
            Queue<TVector> queue = new();
            if (Points.Add(from))
                queue.Enqueue(from);
            int count = 0;
            for (; queue.TryDequeue(out from); ++count)
                foreach (var p in GetNeighbors(from).Where(Points.Add))
                    queue.Enqueue(p);
            return count;
        }

        public TSelf MoveNext() =>
            MoveNext(DefaultFilterInclusive);

        public TSelf MoveNext(TSize size) =>
            MoveNext(GetAllNeighborsAndSelf(size), DefaultFilterInclusive);

        public TSelf MoveNext(TRange range) =>
            MoveNext(GetAllNeighborsAndSelf(range), DefaultFilterInclusive);

        public TSelf MoveNext(Func<TVector, bool> predicate, bool inclusive = false) =>
            MoveNext(GetAllNeighbors(inclusive), predicate);

        public TSelf MoveNext(Func<TVector, bool> predicate, TSize size, bool inclusive = false) =>
            MoveNext(GetAllNeighbors(size, inclusive), predicate);

        public TSelf MoveNext(Func<TVector, bool> predicate, TRange range, bool inclusive = false) =>
            MoveNext(GetAllNeighbors(range, inclusive), predicate);

        public TSelf MoveNext(Func<TVector, int, bool> filterInclusive) =>
            MoveNext(GetAllNeighborsAndSelf(), filterInclusive);

        public TSelf MoveNext(Func<TVector, int, bool> filterInclusive, TSize size) =>
            MoveNext(GetAllNeighborsAndSelf(size), filterInclusive);

        public TSelf MoveNext(Func<TVector, int, bool> filterInclusive, TRange range) =>
            MoveNext(GetAllNeighborsAndSelf(range), filterInclusive);

        private TSelf MoveNext(ParallelQuery<TVector> pp, Func<TVector, bool> predicate)
        {
            Points = pp.Where(predicate).ToHashSet();
            return (TSelf)this;
        }

        private TSelf MoveNext(ParallelQuery<TVector> pp, Func<TVector, int, bool> filterInclusive)
        {
            Points = pp.Where(p => filterInclusive(p, CountNeighborsAndSelf(p))).ToHashSet();
            return (TSelf)this;
        }

        private ParallelQuery<TVector> GetAllNeighborsAndSelf(TSize size) =>
            GetAllNeighbors(size, true);

        private ParallelQuery<TVector> GetAllNeighborsAndSelf(TRange range) =>
            GetAllNeighbors(range, true);

        private ParallelQuery<TVector> GetAllNeighborsAndSelf() =>
            GetAllNeighbors(true);

        private ParallelQuery<TVector> GetAllNeighbors(TSize size, bool inclusive) =>
            GetAllNeighbors(inclusive).Where(size.Contains);

        private ParallelQuery<TVector> GetAllNeighbors(TRange range, bool inclusive) =>
            GetAllNeighbors(inclusive).Where(range.Contains);

        private ParallelQuery<TVector> GetAllNeighbors(bool inclusive)
        {
            var query = inclusive
                ? Points.SelectMany(GetNeighborsAndSelf)
                : Points.SelectMany(GetNeighbors);
            return query.Distinct().AsParallel();
        }

        private bool DefaultFilterInclusive(TVector p, int count) =>
            count == 3 || count == 4 && Points.Contains(p);

        public virtual int CountNeighbors(TVector p) =>
            GetNeighbors(p).Count(Points.Contains);

        public virtual int CountNeighborsAndSelf(TVector p) =>
            GetNeighborsAndSelf(p).Count(Points.Contains);

        public abstract IEnumerable<TVector> GetNeighbors(TVector p);
        public abstract IEnumerable<TVector> GetNeighborsAndSelf(TVector p);

        public static implicit operator HashSet<TVector>(Grid<TSelf, TVector, TSize, TRange, T> grid) =>
            grid.Points;
    }

    public abstract class Grid<TSelf> : Grid<TSelf, Vector, Size, VectorRange, int>, IFormattableEx
        where TSelf : Grid<TSelf>
    {
        protected Grid(Vector[] points)
            : base(points)
        {
        }

        protected Grid(IEnumerable<Vector> points)
            : base(points)
        {
        }

        protected Grid(HashSet<Vector> points)
            : base(points)
        {
        }

        public bool Add(int x, int y) =>
            Points.Add(new(x, y));

        public bool Remove(int x, int y) =>
            Points.Remove(new(x, y));

        public bool AddRange(Size size) =>
            new VectorRange(size).All(Points.Add);

        public bool AddRange(IEnumerable<Vector> range) =>
            range.All(Points.Add);

        public bool RemoveRange(Size size) =>
            new VectorRange(size).All(Points.Remove);

        public bool RemoveRange(IEnumerable<Vector> range) =>
            range.All(Points.Remove);

        public abstract string ToString(IFormatProvider provider);
        public abstract string ToString(string format, IFormatProvider formatProvider);
    }

    public sealed class Grid : Grid<Grid>
    {
        static Helper Helper { get; } = Helper.Instance;

        public Grid(params Vector[] points)
            : base(points)
        {
        }

        public Grid(IEnumerable<Vector> points)
            : base(points)
        {
        }

        internal Grid(HashSet<Vector> points)
            : base(points)
        {
        }

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

        public override string ToString() =>
            Helper.ToString(this);

        public override string ToString(IFormatProvider provider) =>
            Helper.ToString(this, provider);

        public override string ToString(string format, IFormatProvider provider = null) =>
            Helper.ToString(this, format, provider);

        public string ToString(Size size, ReadOnlySpan<char> format = default, IFormatProvider provider = null) =>
            Helper.ToString(this, size, format, provider);

        public string ToString(VectorRange range, ReadOnlySpan<char> format = default, IFormatProvider provider = null) =>
            Helper.ToString(this, range, format, provider);

        public static Vector[] Headings =>
            Helper.Headings;

        public static int GetHeading(ReadOnlySpan<char> input) =>
            Helper.GetHeading(input);

        public static bool TryGetHeading(ReadOnlySpan<char> input, out int heading) =>
            Helper.TryGetHeading(input, out heading);

        public static string ToString(Vector vector, char format) =>
            Helper.ToString(vector, format);

        public static Grid Parse(ReadOnlySpan<char> input) =>
            Helper.Parse(input);

        public static Grid Parse(string input) =>
            Helper.Parse(input);

        public static Grid Parse(ReadOnlySpan<char> input, out VectorRange range) =>
            Helper.Parse(input, out range);

        public static Grid Parse(ReadOnlySpan<char> input, out Size size) =>
            Helper.Parse(input, out size);

        public static Grid Parse(ReadOnlySpan<char> input, char separator) =>
            Helper.Parse(input, separator);

        public static Grid Parse(ReadOnlySpan<char> input, char separator, out VectorRange range) =>
            Helper.Parse(input, separator, out range);

        public static Grid Parse(ReadOnlySpan<char> input, char separator, out Size size) =>
            Helper.Parse(input, separator, out size);

        public static Grid Parse(ReadOnlySpan<char> input, char separator, char point) =>
            Helper.Parse(input, separator, point);

        public static Grid Parse(ReadOnlySpan<char> input, char separator, char point, out VectorRange range) =>
            Helper.Parse(input, separator, point, out range);

        public static Grid Parse(ReadOnlySpan<char> input, char separator, char point, out Size size) =>
            Helper.Parse(input, separator, point, out size);

        public static Vector ParseVector(ReadOnlySpan<char> input) =>
            Helper.ParseVector(input);

        public static bool TryParseVector(ReadOnlySpan<char> input, out Vector vector) =>
            Helper.TryParseVector(input, out vector);

        public static IEnumerable<Vector> ParseVectors(ReadOnlySpan<char> input, params char[] skip) =>
            Helper.ParseVectors(input, skip);

        public static bool TryParseVectors(ReadOnlySpan<char> input, ReadOnlySpan<char> skip, out IEnumerable<Vector> vectors) =>
            Helper.TryParseVectors(input, skip, out vectors);

        public static IEnumerable<PathSegment<Vector>> ParsePath(ReadOnlySpan<char> input) =>
            Helper.ParsePath(input);

        public static IEnumerable<PathSegment<Vector>> ParsePath(string input, char separator) =>
            Helper.ParsePath(input, separator);

        public static IEnumerable<PathSegment<Vector>> ParsePath(string input, string separator) =>
            Helper.ParsePath(input, separator);

        public static IEnumerable<PathSegment<Vector>> ParsePath(string[] ss) =>
            Helper.ParsePath(ss);

        public static IEnumerable<PathSegment<Vector>> ParsePath(string[] ss, char separator) =>
            Helper.ParsePath(ss, separator);

        public static IEnumerable<PathSegment<Vector>> ParsePath(string[] ss, string separator) =>
            Helper.ParsePath(ss, separator);

        public static IEnumerable<(Matrix, int)> ParseTurns(string input) =>
            Helper.ParseTurns(input);
    }
}
