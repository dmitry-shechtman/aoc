using aoc.Grids;
using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc.Internal
{
    abstract class GridHelper<TSelf, TGrid, TVector> : Singleton<TSelf>
        where TSelf : GridHelper<TSelf, TGrid, TVector>
        where TGrid : IReadOnlyCollection<TVector>
        where TVector : struct
    {
        public abstract TVector[] Headings { get; }

        public int GetHeading(string s) =>
            TryGetHeading(s, out int heading)
                ? heading
                : throw new ArgumentOutOfRangeException(nameof(s));

        public bool TryGetHeading(string s, out int heading)
        {
            s = s.ToLowerInvariant();
            foreach (var ss in FormatStrings)
                if ((heading = ss.IndexOf(s)) >= 0)
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

        public IEnumerable<TVector> ParseVectors(string s)
        {
            for (int i = 0; i < s.Length;)
                yield return ParseVector(s, ref i);
        }

        public bool TryParseVectors(string s, out IEnumerable<TVector> value)
        {
            value = Enumerable.Empty<TVector>();
            for (int i = 0; i < s.Length;)
                if (TryParse(s, ref i, out TVector vector))
                    value = value.Append(vector);
                else
                    return false;
            return true;
        }

        public TVector ParseVector(string s)
        {
            int i = 0;
            return ParseVector(s, ref i);
        }

        public bool TryParseVector(string s, out TVector vector)
        {
            int i = 0;
            return TryParse(s, ref i, out vector);
        }

        private TVector ParseVector(string s, ref int i) =>
            TryParse(s, ref i, out TVector vector)
                ? vector
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public IEnumerable<(TVector v, int d)> ParsePath(string s)
        {
            for (int i = 0; i < s.Length;)
                yield return (ParseVector(s, ref i), ParseInt32(s, ref i));
        }

        public IEnumerable<(TVector v, int d)> ParsePath(string s, char separator) =>
            ParsePath(s.Split(separator));

        public IEnumerable<(TVector v, int d)> ParsePath(string s, string separator) =>
            ParsePath(s.Split(separator));

        public IEnumerable<(TVector v, int d)> ParsePath(string[] ss) =>
            ss.Select(s => ParsePathSegment(s));

        public IEnumerable<(TVector v, int d)> ParsePath(string[] ss, char separator) =>
            ss.Select(s => s.Split(separator))
                .Select(tt => ParsePathSegment(tt));

        public IEnumerable<(TVector v, int d)> ParsePath(string[] ss, string separator) =>
            ss.Select(s => s.Split(separator))
                .Select(tt => ParsePathSegment(tt));

        private (TVector, int) ParsePathSegment(string s)
        {
            int i = 0;
            return (ParseVector(s, ref i), int.Parse(s[i..]));
        }

        private (TVector, int) ParsePathSegment(string[] tt) =>
            (ParseVector(tt[0]), int.Parse(tt[1]));

        public virtual bool TryParse(string s, ref int i, out TVector vector)
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

        protected virtual bool TryGetHeading(string s, ref int i, out int heading) =>
            TryGetHeading(s[i..(i + 1)], out heading);

        private string[] GetFormatStrings(char format)
        {
            string s = $"{format}";
            foreach (var ss in FormatStrings)
                if (ss.Contains(s, StringComparer.OrdinalIgnoreCase))
                    return ss;
            return null;
        }

        protected static int ParseInt32(string s, ref int i)
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
    }

    sealed class GridHelper : GridHelper<GridHelper, Grid>
    {
        private const char DefaultSeparatorChar = '\n';
        private const char DefaultPointChar     = '#';

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

        public static Grid Parse(string s) =>
            Parse(s, DefaultSeparatorChar);

        public static Grid Parse(string s, char separator) =>
            Parse(s, separator, DefaultPointChar);

        public static Grid Parse(string s, char separator, char c) =>
            Parse(s.Split(separator), c);

        public static Grid Parse(string[] ss) =>
            Parse(ss, DefaultPointChar);

        public static Grid Parse(string[] ss, char c) =>
            new(ParsePoints(ss, c));

        private static IEnumerable<Vector> ParsePoints(string[] ss, char c)
        {
            for (int y = 0; y < ss.Length; y++)
                for (int x = 0; x < ss[y].Length; x++)
                    if (ss[y][x] == c)
                        yield return (x, y);
        }

        public static IEnumerable<(Matrix, int)> ParseTurns(string s)
        {
            Matrix t = Matrix.Identity;
            for (int i = 0; i < s.Length; i++)
            {
                yield return (t, ParseInt32(s, ref i));
                if (i == s.Length)
                    break;
                t = ParseTurn(s[i]);
            }
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
        protected override bool TryGetHeading(string s, ref int i, out int heading) =>
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
