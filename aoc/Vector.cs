using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc
{
    public enum GridType
    {
        Square = 0,
        HexNS  = 2,
        HexWE  = 3
    }

    public enum NeighborhoodType
    {
        JVN   = 0,
        Moore = 1,
        HexNS = 2,
        HexWE = 3,
        Self  = 4
    }

    public struct Vector : IEquatable<Vector>
    {
        public static readonly Vector Zero      = default;
                                                
        public static readonly Vector North     = ( 0, -1);
        public static readonly Vector East      = ( 1,  0);
        public static readonly Vector South     = ( 0,  1);
        public static readonly Vector West      = (-1,  0);

        public static readonly Vector North2    = ( 0, -2);
        public static readonly Vector East2     = ( 2,  0);
        public static readonly Vector South2    = ( 0,  2);
        public static readonly Vector West2     = (-2,  0);

        public static readonly Vector NorthWest = (-1, -1);
        public static readonly Vector NorthEast = ( 1, -1);
        public static readonly Vector SouthWest = (-1,  1);
        public static readonly Vector SouthEast = ( 1,  1);

        public static readonly Vector[] Headings = { North, East, South, West };

        public readonly int x;
        public readonly int y;

        public Vector(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public readonly override bool Equals(object obj) =>
            obj is Vector other && Equals(other);

        public readonly bool Equals(Vector other) =>
            x == other.x &&
            y == other.y;

        public readonly override int GetHashCode() =>
            HashCode.Combine(x, y);

        public readonly override string ToString() =>
            $"{x},{y}";

        public readonly void Deconstruct(out int x, out int y)
        {
            x = this.x;
            y = this.y;
        }

        public readonly int Abs(GridType grid) => grid switch
        {
            GridType.Square => Abs(),
            GridType.HexNS => AbsNS(),
            GridType.HexWE => AbsWE(),
            _ => throw new InvalidOperationException("Invalid grid type"),
        };

        public readonly int Abs() =>
            Math.Abs(x) + Math.Abs(y);

        public readonly Vector Abs2() =>
            new(Math.Abs(x), Math.Abs(y));

        public readonly Vector Sign() =>
            new(Math.Sign(x), Math.Sign(y));

        public readonly int AbsNS() =>
            Math.Abs(x) + Math.Abs(Math.Abs(x) - Math.Abs(y)) / 2;

        public readonly int AbsWE() =>
            Math.Abs(y) + Math.Abs(Math.Abs(y) - Math.Abs(x)) / 2;

        public static IEnumerable<Vector> Range(Vector toExclusive) =>
            Range(Zero, toExclusive);

        public static IEnumerable<Vector> Range(Vector fromInclusive, Vector toExclusive)
        {
            for (var y = fromInclusive.y; y < toExclusive.y; y++)
                for (var x = fromInclusive.x; x < toExclusive.x; x++)
                    yield return new(x, y);
        }

        public static int CountNeighbors(Vector p, HashSet<Vector> points)
        {
            int count = 0;
            for (var y = p.y - 1; y <= p.y + 1; y++)
                for (var x = p.x - 1; x <= p.x + 1; x++)
                    count += p != (x, y) && points.Contains((x, y)) ? 1 : 0;
            return count;
        }

        public static int CountNeighbors(Vector p, HashSet<Vector> points, NeighborhoodType neighborhood) =>
            GetNeighbors(p, neighborhood).Count(points.Contains);

        public static int CountNeighborsAndSelf(Vector p, HashSet<Vector> points)
        {
            int count = 0;
            for (var y = p.y - 1; y <= p.y + 1; y++)
                for (var x = p.x - 1; x <= p.x + 1; x++)
                    count += points.Contains((x, y)) ? 1 : 0;
            return count;
        }

        public static IEnumerable<Vector> GetNeighbors(Vector p, NeighborhoodType neighborhood) => neighborhood switch
        {
            NeighborhoodType.Moore => GetNeighbors(p),
            NeighborhoodType.Moore | NeighborhoodType.Self => GetNeighborsAndSelf(p),
            NeighborhoodType.HexNS => GetNeighborsNS(p),
            NeighborhoodType.HexWE => GetNeighborsWE(p),
            NeighborhoodType.JVN   => GetNeighborsJVN(p),
            NeighborhoodType.JVN   | NeighborhoodType.Self => GetNeighborsJVNAndSelf(p),
            _ => throw new InvalidOperationException("Invalid grid type"),
        };

        public static IEnumerable<Vector> GetNeighbors(Vector p)
        {
            for (var y = p.y - 1; y <= p.y + 1; y++)
                for (var x = p.x - 1; x <= p.x + 1; x++)
                    if (p != (x, y))
                        yield return new(x, y);
        }

        public static IEnumerable<Vector> GetNeighbors(Vector p, Vector fromInclusive, Vector toExclusive)
        {
            for (var y = Math.Max(fromInclusive.y, p.y - 1); y <= Math.Min(toExclusive.y - 1, p.y + 1); y++)
                for (var x = Math.Max(fromInclusive.x, p.x - 1); x <= Math.Min(toExclusive.x - 1, p.x + 1); x++)
                    if (p != (x, y))
                        yield return new(x, y);
        }

        public static IEnumerable<Vector> GetNeighbors(Vector p, Vector toExclusive) =>
            GetNeighbors(p, Zero, toExclusive);

        public static IEnumerable<Vector> GetNeighborsAndSelf(Vector p)
        {
            for (var y = p.y - 1; y <= p.y + 1; y++)
                for (var x = p.x - 1; x <= p.x + 1; x++)
                    yield return new(x, y);
        }

        public static Vector[] GetNeighborsJVN(Vector p) => new Vector[]
        {
            new(p.x, p.y - 1),
            new(p.x + 1, p.y),
            new(p.x, p.y + 1),
            new(p.x - 1, p.y)
        };

        public static Vector[] GetNeighborsJVNAndSelf(Vector p) => new Vector[]
        {
            new(p.x, p.y),
            new(p.x, p.y - 1),
            new(p.x + 1, p.y),
            new(p.x, p.y + 1),
            new(p.x - 1, p.y)
        };

        public static Vector[] GetNeighborsNS(Vector p) => new Vector[]
        {
            new(p.x, p.y + 2),
            new(p.x + 1, p.y + 1),
            new(p.x - 1, p.y + 1),
            new(p.x, p.y - 2),
            new(p.x - 1, p.y - 1),
            new(p.x + 1, p.y - 1)
        };

        public static Vector[] GetNeighborsWE(Vector p) => new Vector[]
        {
            new(p.x - 2, p.y),
            new(p.x - 1, p.y - 1),
            new(p.x + 1, p.y - 1),
            new(p.x + 2, p.y),
            new(p.x + 1, p.y + 1),
            new(p.x - 1, p.y + 1),
        };

        public static HashSet<Vector> GetNext(HashSet<Vector> pp) =>
            GetNext(pp, GetNeighborsAndSelf, FilterInclusive);

        public static HashSet<Vector> GetNext(HashSet<Vector> pp, Func<Vector, IEnumerable<Vector>> getNeighbors, Func<Vector, int, HashSet<Vector>, bool> filter) =>
            pp.SelectMany(getNeighbors).Distinct().AsParallel()
                .Select(p => (p, c: getNeighbors(p).Count(pp.Contains)))
                .Where(t => filter(t.p, t.c, pp))
                .Select(t => t.p)
                .ToHashSet();

        public static HashSet<Vector> GetNext(HashSet<Vector> pp, Vector fromInclusive, Vector toExclusive) =>
            GetNext(pp, fromInclusive, toExclusive, GetNeighborsAndSelf, FilterInclusive);

        public static HashSet<Vector> GetNext(HashSet<Vector> pp, Vector toExclusive) =>
            GetNext(pp, toExclusive, GetNeighborsAndSelf, FilterInclusive);

        public static HashSet<Vector> GetNext(HashSet<Vector> pp, Vector fromInclusive, Vector toExclusive, Func<Vector, IEnumerable<Vector>> getNeighbors, Func<Vector, int, HashSet<Vector>, bool> filter) =>
            pp.SelectMany(getNeighbors).Distinct().AsParallel()
                .Where(p => p.x >= fromInclusive.x && p.x < toExclusive.x && p.y >= fromInclusive.y && p.y < toExclusive.y)
                .Select(p => (p, c: getNeighbors(p).Count(pp.Contains)))
                .Where(t => filter(t.p, t.c, pp))
                .Select(t => t.p)
                .ToHashSet();

        private static HashSet<Vector> GetNext(HashSet<Vector> pp, Vector toExclusive, Func<Vector, IEnumerable<Vector>> getNeighbors, Func<Vector, int, HashSet<Vector>, bool> filter) =>
            GetNext(pp, Zero, toExclusive, getNeighbors, filter);

        public static bool Filter(Vector p, int count, HashSet<Vector> pp) =>
            count == 3 || count == 2 && pp.Contains(p);

        public static bool FilterInclusive(Vector p, int count, HashSet<Vector> pp) =>
            count == 3 || count == 4 && pp.Contains(p);

        public static IEnumerable<Vector> ParseMany(string s, NeighborhoodType neighborhood = NeighborhoodType.JVN)
        {
            for (int i = 0; i < s.Length;)
                yield return Parse(s, ref i, neighborhood);
        }

        public static Vector Parse(string s, char separator = ',') =>
            TryParse(s, out Vector vector, separator)
                ? vector
                : throw new InvalidOperationException($"Incorrect string format: {s}");

        public static bool TryParse(string s, out Vector vector, char separator = ',')
        {
            var ss = s.Trim().Split(separator);
            vector = default;
            if (ss.Length != 2 ||
                !int.TryParse(ss[0], out int x) ||
                !int.TryParse(ss[1], out int y))
                return false;
            vector = new(x, y);
            return true;
        }

        public static Vector Parse(string s, NeighborhoodType neighborhood)
        {
            int i = 0;
            return Parse(s, ref i, neighborhood);
        }

        public static bool TryParse(string s, out Vector vector, NeighborhoodType neighborhood)
        {
            int i = 0;
            return TryParse(s, ref i, out vector, neighborhood);
        }

        public static Vector Parse(string s, ref int i, NeighborhoodType grid) =>
            TryParse(s, ref i, out Vector vector, grid)
                ? vector
                : throw new InvalidOperationException($"Incorrect string format: {s}");

        public static bool TryParse(string s, ref int i, out Vector vector, NeighborhoodType neighborhood) => neighborhood switch
        {
            NeighborhoodType.JVN =>
                TryParse(s, ref i, out vector),
            NeighborhoodType.Moore =>
                TryParse2(s, ref i, out vector) || TryParse(s, ref i, out vector),
            NeighborhoodType.HexNS =>
                TryParse2(s, ref i, out vector) || TryParseHexNS(s, ref i, out vector),
            NeighborhoodType.HexWE =>
                TryParse2(s, ref i, out vector) || TryParseHexWE(s, ref i, out vector),
            _ => throw new(),
        };

        private static bool TryParse(string s, ref int i, out Vector vector)
        {
            vector = default;
            if (i < s.Length && TryParse(s[i], out vector))
                ++i;
            return vector != default;
        }

        public static Vector Parse(char c) =>
            TryParse(c, out Vector vector)
                ? vector
                : throw new InvalidOperationException($"Unexpected character: {c}");

        public static bool TryParse(char c, out Vector vector) => (vector = char.ToLower(c) switch
        {
            'n' or 'u' or '^' => North,
            'e' or 'r' or '>' => East,
            's' or 'd' or 'v' => South,
            'w' or 'l' or '<' => West,
            _ => default
        }) != default;

        private static bool TryParse2(string s, ref int i, out Vector vector)
        {
            vector = default;
            if (i > s.Length - 2)
                return false;
            vector = s[i..(i + 2)].ToLower() switch
            {
                "nw" => NorthWest,
                "ne" => NorthEast,
                "sw" => SouthWest,
                "se" => SouthEast,
                _ => default
            };
            if (vector != default)
                i += 2;
            return vector != default;
        }

        private static bool TryParseHexNS(string s, ref int i, out Vector vector)
        {
            vector = char.ToLower(s[i]) switch
            {
                'n' => North2,
                's' => South2,
                _ => default
            };
            if (vector != default)
                ++i;
            return vector != default;
        }

        private static bool TryParseHexWE(string s, ref int i, out Vector vector)
        {
            vector = char.ToLower(s[i]) switch
            {
                'e' => East2,
                'w' => West2,
                _ => default
            };
            if (vector != default)
                ++i;
            return vector != default;
        }

        public static HashSet<Vector> ParseField(string s) =>
            ParseField(s, '\n');

        public static HashSet<Vector> ParseField(string s, char separator, char c = '#') =>
            ParseField(s.Split(separator), c);

        public static HashSet<Vector> ParseField(string[] ss) =>
            ParseField(ss, '#');

        public static HashSet<Vector> ParseField(string[] ss, char c)
        {
            HashSet<Vector> pp = new();
            for (int y = 0; y < ss.Length; y++)
                for (int x = 0; x < ss[y].Length; x++)
                    if (ss[y][x] == c)
                        pp.Add((x, y));
            return pp;
        }

        public static HashSet<Vector>[] ParseField(string[] ss, string cc)
        {
            var pp = new HashSet<Vector>[cc.Length + 1];
            int i;
            for (i = 0; i <= cc.Length; i++)
                pp[i] = new();
            for (int y = 0; y < ss.Length; y++)
                for (int x = 0; x < ss[y].Length; x++)
                    if ((i = cc.IndexOf(ss[y][x])) >= 0)
                    {
                        pp[i].Add((x, y));
                        pp[^1].Add((x, y));
                    }
            return pp;
        }

        public static IEnumerable<(Vector, int)> ParsePath(string s, GridType grid = GridType.Square)
        {
            for (int i = 0; i < s.Length;)
            {
                Vector v = Parse(s, ref i, (NeighborhoodType)grid);
                yield return (v, ParseInt32(s, ref i));
            }
        }

        public static IEnumerable<(Vector, int)> ParsePath(string s, char separator, GridType grid = GridType.Square) =>
            ParsePath(s.Split(separator), grid);

        public static IEnumerable<(Vector, int)> ParsePath(string s, string separator, GridType grid = GridType.Square) =>
            ParsePath(s.Split(separator), grid);

        public static IEnumerable<(Vector, int)> ParsePath(string[] ss, GridType grid = GridType.Square) =>
            ss.Select(s => ParsePathSegment(s, grid));

        public static IEnumerable<(Vector, int)> ParsePath(string[] ss, string separator, GridType grid = GridType.Square) =>
            ss.Select(s => s.Split(separator))
                .Select(tt => ParsePathSegment(tt, grid));

        public static IEnumerable<(Vector, int)> ParsePath(string[] ss, char separator, GridType grid = GridType.Square) =>
            ss.Select(s => s.Split(separator))
                .Select(tt => ParsePathSegment(tt, grid));

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

        private static (Vector, int) ParsePathSegment(string s, GridType grid)
        {
            int i = 0;
            return (Parse(s, ref i, (NeighborhoodType)grid), int.Parse(s[i..]));
        }

        private static (Vector, int) ParsePathSegment(string[] tt, GridType grid)
        {
            return (Parse(tt[0], (NeighborhoodType)grid), int.Parse(tt[1]));
        }

        private static int ParseInt32(string s, ref int i)
        {
            int j;
            for (j = i + 1; j < s.Length; j++)
                if (!char.IsDigit(s[j]))
                    break;
            var p = int.Parse(s[i..j]);
            i = j;
            return p;
        }

        public static int FloodFill(Vector from, HashSet<Vector> pp, NeighborhoodType neighborhood = NeighborhoodType.JVN) =>
             pp.Add(from)
                ? 1 + GetNeighbors(from, neighborhood).Sum(q => FloodFill(q, pp, neighborhood))
                : 0;

        public static Vector FindChar(string s, char c, VectorRange range) =>
            new(s.IndexOf(c) % (range.Width + 1), s.IndexOf(c) / (range.Width + 1));

        public static Vector FindChar(string s, char c) =>
            FindChar(s, c, VectorRange.FromField(s));

        public char GetChar(string s, VectorRange range) =>
            s[y * (range.Width + 1) + x];

        public static char GetChar(Vector p, string s, VectorRange range) =>
            p.GetChar(s, range);

        public char GetChar(string s) =>
            GetChar(s, VectorRange.FromField(s));

        public static char GetChar(Vector p, string s) =>
            p.GetChar(s);

        public readonly Vector Add(Vector other) =>
            new(x + other.x, y + other.y);

        public static Vector Add(Vector left, Vector right) =>
            left.Add(right);

        public static Vector operator +(Vector left, Vector right) =>
            left.Add(right);

        public readonly Vector Sub(Vector other) =>
            new(x - other.x, y - other.y);

        public static Vector Sub(Vector left, Vector right) =>
            left.Sub(right);

        public static Vector operator -(Vector left, Vector right) =>
            left.Sub(right);

        public readonly Vector Negate() =>
            new(-x, -y);

        public static Vector Negate(Vector vector) =>
            vector.Negate();

        public static Vector operator -(Vector vector) =>
            vector.Negate();

        public readonly Vector Dot(Vector other) =>
            new(x * other.x, y * other.y);

        public static Vector Dot(Vector left, Vector right) =>
            left.Dot(right);

        public readonly Vector Mul(int scalar) =>
            new(x * scalar, y * scalar);

        public static Vector Mul(Vector vector, int scalar) =>
            vector.Mul(scalar);

        public static Vector operator *(Vector vector, int scalar) =>
            vector.Mul(scalar);

        public static Vector operator *(int scalar, Vector vector) =>
            vector.Mul(scalar);

        public readonly Vector Mul(Matrix m) =>
            new(x * m.m11 + y * m.m21 + m.m31, x * m.m12 + y * m.m22 + m.m32);

        public static Vector Mul(Vector vector, Matrix matrix) =>
            vector.Mul(matrix);

        public static Vector operator *(Vector vector, Matrix matrix) =>
            vector.Mul(matrix);

        public readonly Vector Div(int scalar) =>
            new(x / scalar, y / scalar);

        public static Vector Div(Vector vector, int scalar) =>
            vector.Div(scalar);

        public static Vector operator /(Vector vector, int scalar) =>
            vector.Div(scalar);

        public static Vector Min(Vector left, Vector right) =>
            new(Math.Min(left.x, right.x), Math.Min(left.y, right.y));

        public static Vector Max(Vector left, Vector right) =>
            new(Math.Max(left.x, right.x), Math.Max(left.y, right.y));

        public static implicit operator (int x, int y)(Vector value) =>
            (value.x, value.y);

        public static implicit operator Vector((int x, int y) value) =>
            new(value.x, value.y);

        public static bool operator ==(Vector left, Vector right) =>
            left.Equals(right);

        public static bool operator !=(Vector left, Vector right) =>
            !left.Equals(right);

    }
}
