using aoc.Grids;
using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc.Internal
{
    abstract class GridHelper<TSelf, TGrid, TVector> : Singleton<TSelf>
        where TSelf : GridHelper<TSelf, TGrid, TVector>
        where TGrid : IReadOnlyCollection<TVector>
        where TVector : struct, IEquatable<TVector>
    {
        public abstract TVector[] Headings { get; }

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

    abstract class GridHelper<TSelf, TGrid> : GridHelper<TSelf, TGrid, Vector>
        where TSelf : GridHelper<TSelf, TGrid>
        where TGrid : Grid<TGrid>
    {
        protected const char DefaultEmptyChar     = '.';
        protected const char DefaultPointChar     = '#';
        protected const char DefaultSeparatorChar = '\n';

        public static string ToString(TGrid grid, IFormatProvider provider = null) =>
            ToString(grid, null, provider);

        public static string ToString(TGrid grid, ReadOnlySpan<char> format, IFormatProvider provider) =>
            ToString(grid, grid.Range(), format, provider);

        public static string ToString(TGrid grid, Size size, ReadOnlySpan<char> format, IFormatProvider provider) =>
            ToString(grid, range: new(size), format, provider);

        public static string ToString(TGrid grid, VectorRange range, ReadOnlySpan<char> format, IFormatProvider _)
        {
            var point = format.Length > 0
                ? format[0]
                : DefaultPointChar;
            var empty = format.Length > 1
                ? format[1]
                : DefaultEmptyChar;
            var separator = format.Length > 2
                ? format[2..]
                : new[] { DefaultSeparatorChar };
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

        public static Grid Parse(ReadOnlySpan<char> input) =>
            Parse(input, size: out _);

        public static Grid Parse(ReadOnlySpan<char> input, out VectorRange range) =>
            Parse(input, DefaultSeparatorChar, out range);

        public static Grid Parse(ReadOnlySpan<char> input, out Size size) =>
            Parse(input, DefaultSeparatorChar, out size);

        public static Grid Parse(ReadOnlySpan<char> input, char separator) =>
            Parse(input, separator, size: out _);

        public static Grid Parse(ReadOnlySpan<char> input, char separator, out VectorRange range) =>
            Parse(input, separator, DefaultPointChar, out range);

        public static Grid Parse(ReadOnlySpan<char> input, char separator, out Size size) =>
            Parse(input, separator, DefaultPointChar, out size);

        public static Grid Parse(ReadOnlySpan<char> input, char separator, char point) =>
            Parse(input, separator, point, size: out _);

        public static Grid Parse(ReadOnlySpan<char> input, char separator, char point, out VectorRange range)
        {
            Grid grid = Parse(input, separator, point, out Size size);
            range = new(size);
            return grid;
        }

        public static Grid Parse(ReadOnlySpan<char> input, char separator, char point, out Size size)
        {
            int width = 0, height = 1, x = 0, y = 0;
            HashSet<Vector> points = new();
            for (int i = 0; i < input.Length; ++i, ++x)
            {
                if (input[i] == separator)
                {
                    width = x > width ? x : width;
                    if (x > 0)
                        ++height;
                    (x, y) = (-1, ++y);
                }
                else if (input[i] == point)
                {
                    points.Add((x, y));
                }
            }
            width = x > width ? x : width;
            size = new(width, height);
            return new(points);
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

    sealed class DiagGridHelper : GridHelper2<DiagGridHelper, DiagGrid>
    {
        public override Vector[] Headings => new[]
        {
            Vector.NorthEast, Vector.SouthEast, Vector.SouthWest, Vector.NorthWest
        };

        protected override string[][] FormatStrings => new[]
        {
            new[] { "ne", "se", "sw", "nw" }
        };
    }

    sealed class MooreGridHelper : GridHelper2<MooreGridHelper, MooreGrid>
    {
        private MooreGridHelper()
        {
        }

        public override Vector[] Headings => new[]
        {
            Vector.North, Vector.NorthEast, Vector.East, Vector.SouthEast,
            Vector.South, Vector.SouthWest, Vector.West, Vector.NorthWest,
        };

        protected override string[][] FormatStrings => new[]
        {
            new[] { "n", "ne", "e", "se", "s", "sw", "w", "nw" }
        };
    }

    sealed class HexNSGridHelper : GridHelper2<HexNSGridHelper, HexNSGrid>
    {
        private HexNSGridHelper()
        {
        }

        public override Vector[] Headings => new[]
        {
            Vector.North, Vector.NorthEast, Vector.SouthEast,
            Vector.South, Vector.SouthWest, Vector.NorthWest
        };

        protected override string[][] FormatStrings => new[]
        {
            new[] { "n", "ne", "se", "s", "sw", "nw" }
        };
    }

    sealed class HexWEGridHelper : GridHelper2<HexWEGridHelper, HexWEGrid>
    {
        private HexWEGridHelper()
        {
        }

        public override Vector[] Headings => new[]
        {
            Vector.East, Vector.SouthEast, Vector.SouthWest,
            Vector.West, Vector.NorthWest, Vector.NorthEast
        };

        protected override string[][] FormatStrings => new[]
        {
            new[] { "e", "se", "sw", "w", "nw", "ne" }
        };
    }

    sealed class Grid3DHelper : GridHelper<Grid3DHelper, Grid3D, Vector3D>
    {
        private Grid3DHelper()
        {
        }

        public override Vector3D[] Headings => new[]
        {
            Vector3D.North, Vector3D.East, Vector3D.South, Vector3D.West,
            Vector3D.Up,    Vector3D.Down
        };

        protected override string[][] FormatStrings => new[]
        {
            new[] { "n", "e", "s", "w", "u", "d" }
        };
    }

    sealed class Grid4DHelper : GridHelper<Grid4DHelper, Grid4D, Vector4D>
    {
        private Grid4DHelper()
        {
        }

        public override Vector4D[] Headings => new[]
        {
            Vector4D.North, Vector4D.East, Vector4D.South, Vector4D.West,
            Vector4D.Up,    Vector4D.Down, Vector4D.Ana,   Vector4D.Kata
        };

        protected override string[][] FormatStrings => new[]
        {
            new[] { "n", "e", "s", "w", "u", "d", "a", "k" }
        };
    }
}
