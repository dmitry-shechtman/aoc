using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace aoc.Grids
{
    using Helper = Internal.GridHelper;

    public interface IGrid<TVector>
        where TVector : struct, IEquatable<TVector>
    {
        HashSet<TVector> Points { get; set; }
    }

    public abstract class Grid<TSelf, TVector, TSize, TRange, T> : IGrid<TVector>, IReadOnlyCollection<TVector>
        where TSelf : Grid<TSelf, TVector, TSize, TRange, T>
        where TVector : struct, IVector<TVector, T>
        where TSize : struct, ISize<TSize, TVector>
        where TRange : struct, IRange<TRange, TVector, T>
        where T : struct
    {
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

        HashSet<TVector> IGrid<TVector>.Points
        {
            get => Points;
            set => Points = value;
        }

        public static implicit operator HashSet<TVector>(Grid<TSelf, TVector, TSize, TRange, T> grid) =>
            grid.Points;
    }

    public abstract class Grid<TSelf> : Grid<TSelf, Vector, Size, VectorRange, int>, IFormattableEx
        where TSelf : Grid<TSelf>
    {
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

        public int FloodFill(Vector from) =>
            Helper.FloodFill(this, from);

        public Grid MoveNext() =>
            Helper.MoveNext(this);

        public Grid MoveNext(Size size) =>
            Helper.MoveNext(this, size);

        public Grid MoveNext(VectorRange range) =>
            Helper.MoveNext(this, range);

        public Grid MoveNext(Func<Vector, bool> predicate, bool inclusive = false) =>
            Helper.MoveNext(this, predicate, inclusive);

        public Grid MoveNext(Func<Vector, bool> predicate, Size size, bool inclusive = false) =>
            Helper.MoveNext(this, predicate, size, inclusive);

        public Grid MoveNext(Func<Vector, bool> predicate, VectorRange range, bool inclusive = false) =>
            Helper.MoveNext(this, predicate, range, inclusive);

        public Grid MoveNext(Func<Vector, int, bool> filterInclusive) =>
            Helper.MoveNext(this, filterInclusive);

        public Grid MoveNext(Func<Vector, int, bool> filterInclusive, Size size) =>
            Helper.MoveNext(this, filterInclusive, size);

        public Grid MoveNext(Func<Vector, int, bool> filterInclusive, VectorRange range) =>
            Helper.MoveNext(this, filterInclusive, range);

        public IEnumerable<Vector> GetNeighbors(Vector p) =>
            Helper.GetNeighbors(p);

        public IEnumerable<Vector> GetNeighborsAndSelf(Vector p) =>
            Helper.GetNeighborsAndSelf(p);

        public int CountNeighbors(Vector p) =>
            Helper.CountNeighbors(this, p);

        public int CountNeighborsAndSelf(Vector p) =>
            Helper.CountNeighborsAndSelf(this, p);

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

        public static Grid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format) =>
            Helper.Parse(input, format);

        public static Grid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, out VectorRange range) =>
            Helper.Parse(input, format, out range);

        public static Grid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, Span<Vector> output) =>
            Helper.Parse(input, format, output);

        public static Grid Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> format, Span<Vector> output, out VectorRange range) =>
            Helper.Parse(input, format, output, out range);

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
