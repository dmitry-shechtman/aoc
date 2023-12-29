using aoc.Grids;
using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc.Internal
{
    abstract class GridParseHelper<TSelf, TGrid, TVector> : Singleton<TSelf>
        where TSelf : GridParseHelper<TSelf, TGrid, TVector>
        where TGrid : IReadOnlyCollection<TVector>
        where TVector : struct
    {
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
            {
                TVector v = ParseVector(s, ref i);
                yield return (v, ParseInt32(s, ref i));
            }
        }

        public IEnumerable<(TVector v, int d)> ParsePath(string s, char separator) =>
            ParsePath(s.Split(separator));

        public IEnumerable<(TVector v, int d)> ParsePath(string s, string separator) =>
            ParsePath(s.Split(separator));

        public IEnumerable<(TVector v, int d)> ParsePath(string[] ss) =>
            ss.Select(s => ParsePathSegment(s));

        public IEnumerable<(TVector v, int d)> ParsePath(string[] ss, string separator) =>
            ss.Select(s => s.Split(separator))
                .Select(tt => ParsePathSegment(tt));

        public IEnumerable<(TVector v, int d)> ParsePath(string[] ss, char separator) =>
            ss.Select(s => s.Split(separator))
                .Select(tt => ParsePathSegment(tt));

        private (TVector, int) ParsePathSegment(string s)
        {
            int i = 0;
            return (ParseVector(s, ref i), int.Parse(s[i..]));
        }

        private (TVector, int) ParsePathSegment(string[] tt) =>
            (ParseVector(tt[0]), int.Parse(tt[1]));

        public virtual bool TryParse(string s, ref int i, out TVector vector) =>
            TryParse1(s, ref i, out vector);

        protected bool TryParse1(string s, ref int i, out TVector vector)
        {
            vector = default;
            if (i < s.Length && TryParse(s[i], out vector))
                ++i;
            return !Equals(vector, default(TVector));
        }

        protected abstract bool TryParse(char c, out TVector vector);

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

    abstract class GridParseHelper<TSelf, TGrid> : GridParseHelper<TSelf, TGrid, Vector>
        where TSelf : GridParseHelper<TSelf, TGrid>
        where TGrid : Grid<TGrid>
    {
        protected override bool TryParse(char c, out Vector vector) =>
            Vector.TryParse(c, out vector);
    }

    sealed class GridParseHelper : GridParseHelper<GridParseHelper, Grid>
    {
        private const char DefaultSeparatorChar = '\n';
        private const char DefaultPointChar     = '#';

        private GridParseHelper()
        {
        }

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

    abstract class GridParseHelper2<TSelf, TGrid> : GridParseHelper<TSelf, TGrid>
        where TSelf : GridParseHelper2<TSelf, TGrid>
        where TGrid : Grid<TGrid>
    {
        public sealed override bool TryParse(string s, ref int i, out Vector vector) =>
            TryParse2(s, ref i, out vector) || TryParseInner(s, ref i, out vector);

        protected static bool TryParse2(string s, ref int i, out Vector vector)
        {
            vector = default;
            if (i > s.Length - 2)
                return false;
            vector = s[i..(i + 2)].ToLower() switch
            {
                "nw" => Vector.NorthWest,
                "ne" => Vector.NorthEast,
                "sw" => Vector.SouthWest,
                "se" => Vector.SouthEast,
                _ => default
            };
            if (vector != default)
                i += 2;
            return vector != default;
        }

        protected abstract bool TryParseInner(string s, ref int i, out Vector vector);
    }

    sealed class MooreGridParseHelper : GridParseHelper2<MooreGridParseHelper, MooreGrid>
    {
        private MooreGridParseHelper()
        {
        }

        protected override bool TryParseInner(string s, ref int i, out Vector vector) =>
            TryParse1(s, ref i, out vector);
    }

    sealed class HexNSGridParseHelper : GridParseHelper2<HexNSGridParseHelper, HexNSGrid>
    {
        private HexNSGridParseHelper()
        {
        }

        protected override bool TryParseInner(string s, ref int i, out Vector vector)
        {
            vector = char.ToLower(s[i]) switch
            {
                'n' => Vector.North2,
                's' => Vector.South2,
                _ => default
            };
            if (vector != default)
                ++i;
            return vector != default;
        }
    }

    sealed class HexWEGridParseHelper : GridParseHelper2<HexWEGridParseHelper, HexWEGrid>
    {
        private HexWEGridParseHelper()
        {
        }

        protected override bool TryParseInner(string s, ref int i, out Vector vector)
        {
            vector = char.ToLower(s[i]) switch
            {
                'e' => Vector.East2,
                'w' => Vector.West2,
                _ => default
            };
            if (vector != default)
                ++i;
            return vector != default;
        }
    }

    sealed class Grid3DParseHelper : GridParseHelper<Grid3DParseHelper, Grid3D, Vector3D>
    {
        private Grid3DParseHelper()
        {
        }

        protected override bool TryParse(char c, out Vector3D vector) =>
            Vector3D.TryParse(c, out vector);
    }

    sealed class Grid4DParseHelper : GridParseHelper<Grid4DParseHelper, Grid4D, Vector4D>
    {
        private Grid4DParseHelper()
        {
        }

        protected override bool TryParse(char c, out Vector4D vector) =>
            Vector4D.TryParse(c, out vector);
    }
}
