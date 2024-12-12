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

    public abstract class Grid<TSelf> : Grid<TSelf, Vector, Size, VectorRange, int>
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

        public static Vector[] Headings =>
            Helper.Headings;

        public static int GetHeading(ReadOnlySpan<char> s) =>
            Helper.GetHeading(s);

        public static bool TryGetHeading(ReadOnlySpan<char> s, out int heading) =>
            Helper.TryGetHeading(s, out heading);

        public static string ToString(Vector vector, char format) =>
            Helper.ToString(vector, format);

        public static Grid Parse(ReadOnlySpan<char> s) =>
            Helper.Parse(s);

        public static Grid Parse(string s) =>
            Helper.Parse(s);

        public static Grid Parse(ReadOnlySpan<char> s, out Size size) =>
            Helper.Parse(s, out size);

        public static Grid Parse(ReadOnlySpan<char> s, char separator) =>
            Helper.Parse(s, separator);

        public static Grid Parse(ReadOnlySpan<char> s, char separator, out Size size) =>
            Helper.Parse(s, separator, out size);

        public static Grid Parse(ReadOnlySpan<char> s, char separator, char c) =>
            Helper.Parse(s, separator, c);

        public static Grid Parse(ReadOnlySpan<char> s, char separator, char c, out Size size) =>
            Helper.Parse(s, separator, c, out size);

        public static Vector ParseVector(ReadOnlySpan<char> s) =>
            Helper.ParseVector(s);

        public static bool TryParseVector(ReadOnlySpan<char> s, out Vector vector) =>
            Helper.TryParseVector(s, out vector);

        public static IEnumerable<Vector> ParseVectors(ReadOnlySpan<char> s) =>
            Helper.ParseVectors(s);

        public static bool TryParseVectors(ReadOnlySpan<char> s, out IEnumerable<Vector> vectors) =>
            Helper.TryParseVectors(s, out vectors);

        public static IEnumerable<PathSegment<Vector>> ParsePath(ReadOnlySpan<char> s) =>
            Helper.ParsePath(s);

        public static IEnumerable<PathSegment<Vector>> ParsePath(string s, char separator) =>
            Helper.ParsePath(s, separator);

        public static IEnumerable<PathSegment<Vector>> ParsePath(string s, string separator) =>
            Helper.ParsePath(s, separator);

        public static IEnumerable<PathSegment<Vector>> ParsePath(string[] ss) =>
            Helper.ParsePath(ss);

        public static IEnumerable<PathSegment<Vector>> ParsePath(string[] ss, char separator) =>
            Helper.ParsePath(ss, separator);

        public static IEnumerable<PathSegment<Vector>> ParsePath(string[] ss, string separator) =>
            Helper.ParsePath(ss, separator);

        public static IEnumerable<(Matrix, int)> ParseTurns(string s) =>
            Helper.ParseTurns(s);
    }
}
