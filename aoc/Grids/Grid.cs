using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace aoc.Grids
{
    using Helper = Grid.GridHelper;

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

    public abstract class Grid2D<TSelf> : Grid<TSelf, Vector, Size, VectorRange, int>, IFormattableEx
        where TSelf : Grid2D<TSelf>
    {
        internal sealed class GridHelper : Internal.Grid2DHelper<GridHelper, Grid>
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

            public static IEnumerable<(Matrix, int)> ParseTurns(ReadOnlySpan<char> input)
            {
                Matrix t = Matrix.One;
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

            protected override Grid CreateGrid(HashSet<Vector> points) => new(points);
        }

        protected Grid2D(IEnumerable<Vector> points)
            : base(points)
        {
        }

        protected Grid2D(HashSet<Vector> points)
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

        public abstract string ToString(IFormatProvider? provider);
        public abstract string ToString(string? format, IFormatProvider? provider = null);
    }

    public sealed class Grid : Grid2D<Grid>, IGrid2D<Grid, Vector>
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

        public static IEnumerable<Vector> GetNeighbors(Vector p) =>
            Helper.GetNeighbors(p);

        public static IEnumerable<Vector> GetNeighborsAndSelf(Vector p) =>
            Helper.GetNeighborsAndSelf(p);

        public int CountNeighbors(Vector p) =>
            Helper.CountNeighbors(this, p);

        public int CountNeighborsAndSelf(Vector p) =>
            Helper.CountNeighborsAndSelf(this, p);

        public override string ToString() =>
            Helper.ToString(this);

        public override string ToString(IFormatProvider? provider) =>
            Helper.ToString(this, provider);

        public override string ToString(string? format, IFormatProvider? provider = null) =>
            Helper.ToString(this, format, provider);

        public static Vector[] Headings =>
            Helper.Headings;

        public static Builders.IHeadingBuilder        Heading => Helper;
        public static Builders.IGridBuilder<Grid>     Builder => Helper;
        public static Builders.IVectorBuilder<Vector> Vector  => Helper;
        public static Builders.IPathBuilder<Vector>   Path    => Helper;

        public static Grid Parse(string input) =>
            Helper.Parse(input);

        public static bool TryParse(
            [NotNullWhen(true)] string? input,
            [MaybeNullWhen(false)] out Grid grid) =>
                Helper.TryParse(input, out grid);

        public static Grid Parse(ReadOnlySpan<char> input) =>
            Helper.Parse(input);

        public static bool TryParse(
            ReadOnlySpan<char> input,
            [MaybeNullWhen(false)] out Grid grid) =>
                Helper.TryParse(input, out grid);

        public static IEnumerable<(Matrix, int)> ParseTurns(ReadOnlySpan<char> input) =>
            Helper.ParseTurns(input);
    }
}
