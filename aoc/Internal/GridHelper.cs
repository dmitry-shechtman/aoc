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

        public int GetHeading(ReadOnlySpan<char> s) =>
            TryGetHeading(s, out int heading)
                ? heading
                : throw new ArgumentOutOfRangeException(nameof(s));

        public bool TryGetHeading(ReadOnlySpan<char> s, out int heading)
        {
            var t = s.ToString().ToLowerInvariant();
            foreach (var ss in FormatStrings)
                if ((heading = ss.IndexOf(t)) >= 0)
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
                ? ss[index].ToUpperInvariant().ToString()
                : ss[index].ToString();
        }

        public IEnumerable<TVector> ParseVectors(ReadOnlySpan<char> s, ReadOnlySpan<char> skip)
        {
            var vectors = Enumerable.Empty<TVector>();
            for (int i = 0; i < s.Length;)
                if (skip.Contains(s[i]))
                    ++i;
                else
                    vectors = vectors.Append(ParseVector(s, ref i));
            return vectors;
        }

        public bool TryParseVectors(ReadOnlySpan<char> s, ReadOnlySpan<char> skip, out IEnumerable<TVector> vectors)
        {
            vectors = Enumerable.Empty<TVector>();
            for (int i = 0; i < s.Length;)
                if (skip.Contains(s[i]))
                    ++i;
                else if (TryParse(s, ref i, out TVector vector))
                    vectors = vectors.Append(vector);
                else
                    return false;
            return true;
        }

        public TVector ParseVector(ReadOnlySpan<char> s)
        {
            int i = 0;
            return ParseVector(s, ref i);
        }

        public bool TryParseVector(ReadOnlySpan<char> s, out TVector vector)
        {
            int i = 0;
            return TryParse(s, ref i, out vector);
        }

        private TVector ParseVector(ReadOnlySpan<char> s, ref int i) =>
            TryParse(s, ref i, out TVector vector)
                ? vector
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public IEnumerable<PathSegment<TVector>> ParsePath(ReadOnlySpan<char> s)
        {
            var path = Enumerable.Empty<PathSegment<TVector>>();
            for (int i = 0; i < s.Length;)
                path = path.Append((ParseVector(s, ref i), ParseInt32(s, ref i)));
            return path;
        }

        public IEnumerable<PathSegment<TVector>> ParsePath(string s, char separator) =>
            ParsePath(s.Split(separator));

        public IEnumerable<PathSegment<TVector>> ParsePath(string s, string separator) =>
            ParsePath(s.Split(separator));

        public IEnumerable<PathSegment<TVector>> ParsePath(string[] ss) =>
            ss.Select(ParsePathSegment);

        public IEnumerable<PathSegment<TVector>> ParsePath(string[] ss, char separator) =>
            ss.Select(s => s.Split(separator))
                .Select(ParsePathSegment);

        public IEnumerable<PathSegment<TVector>> ParsePath(string[] ss, string separator) =>
            ss.Select(s => s.Split(separator))
                .Select(ParsePathSegment);

        private PathSegment<TVector> ParsePathSegment(string s)
        {
            int i = 0;
            return new(ParseVector(s, ref i), int.Parse(s[i..]));
        }

        private PathSegment<TVector> ParsePathSegment(string[] tt) =>
            new(ParseVector(tt[0]), int.Parse(tt[1]));

        public virtual bool TryParse(ReadOnlySpan<char> s, ref int i, out TVector vector)
        {
            if (!TryGetHeading(s, ref i, out int heading))
            {
                vector = default;
                return false;
            }
            vector = Headings[heading];
            return true;
        }

        protected abstract string[][] FormatStrings { get; }

        protected virtual bool TryGetHeading(ReadOnlySpan<char> s, ref int i, out int heading) =>
            TryGetHeading(s[i..++i], out heading);

        private string[] GetFormatStrings(char format)
        {
            string s = $"{format}";
            foreach (var ss in FormatStrings)
                if (ss.Contains(s, StringComparer.OrdinalIgnoreCase))
                    return ss;
            return null;
        }

        protected static int ParseInt32(ReadOnlySpan<char> s, ref int i)
        {
            int j;
            for (j = i + 1; j < s.Length; j++)
                if (!char.IsDigit(s[j]))
                    break;
            var p = int.Parse(s[i..j]);
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

        public static string ToString(TGrid grid) =>
            ToString(grid, grid.Range());

        public static string ToString(TGrid grid, Size size) =>
            ToString(grid, range: new(size));

        public static string ToString(TGrid grid, VectorRange range)
        {
            var chars = new char[(range.Width + 1) * range.Height];
            for (int y = range.Min.X, i = 0; y <= range.Max.Y; y++, chars[i++] = DefaultSeparatorChar)
                for (int x = range.Min.Y; x <= range.Max.Y; x++)
                    chars[i++] = grid.Contains((x, y))
                        ? DefaultPointChar
                        : DefaultEmptyChar;
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

        public static Grid Parse(ReadOnlySpan<char> s) =>
            Parse(s, out _);

        public static Grid Parse(ReadOnlySpan<char> s, out Size size) =>
            Parse(s, DefaultSeparatorChar, out size);

        public static Grid Parse(ReadOnlySpan<char> s, char separator) =>
            Parse(s, separator, out _);

        public static Grid Parse(ReadOnlySpan<char> s, char separator, out Size size) =>
            Parse(s, separator, DefaultPointChar, out size);

        public static Grid Parse(ReadOnlySpan<char> s, char separator, char c) =>
            Parse(s, separator, c, out _);

        public static Grid Parse(ReadOnlySpan<char> s, char separator, char c, out Size size)
        {
            int width = 0, height = 1, x = 0, y = 0;
            HashSet<Vector> points = new();
            for (int i = 0; i < s.Length; ++i, ++x)
            {
                if (s[i] == separator)
                {
                    width = x > width ? x : width;
                    if (x > 0)
                        ++height;
                    (x, y) = (-1, ++y);
                }
                else if (s[i] == c)
                {
                    points.Add((x, y));
                }
            }
            width = x > width ? x : width;
            size = new(width, height);
            return new(points);
        }

        public static IEnumerable<(Matrix, int)> ParseTurns(ReadOnlySpan<char> s)
        {
            Matrix t = Matrix.Identity;
            var turns = Enumerable.Empty<(Matrix, int)>();
            for (int i = 0; i < s.Length; i++)
            {
                turns = turns.Append((t, ParseInt32(s, ref i)));
                if (i == s.Length)
                    break;
                t = ParseTurn(s[i]);
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
        protected override bool TryGetHeading(ReadOnlySpan<char> s, ref int i, out int heading) =>
            i < s.Length + 2 && TryGetHeading(s[i..(i + 2)], out heading) ||
            base.TryGetHeading(s, ref i, out heading);
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
