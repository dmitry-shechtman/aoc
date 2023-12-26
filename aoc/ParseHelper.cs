using System;

namespace aoc
{
    internal class ParseHelper
    {
        public delegate bool TryParse1<T>(string[] ss, out T value);

        public delegate bool TryParseValue2<T>(string s, out T value, char separator);

        public static T Parse<T>(string s, TryParseValue2<T> tryParse, char separator) =>
            tryParse(s, out T value, separator)
                ? value
                : throw new InvalidOperationException($"Input string was not in a correct format.");

        public static bool TryParse<T>(string s, TryParse1<T> tryParse, char separator, out T value) =>
            tryParse(s.Trim().Split(separator, StringSplitOptions.TrimEntries), out value);

        public static T Parse<T>(string[] ss, TryParse1<T> tryParse) =>
            tryParse(ss, out T value)
                ? value
                : throw new InvalidOperationException($"Input string was not in a correct format.");
    }

    internal class ParseHelper<T, TValue> : ParseHelper
    {
        public delegate bool TryParseValue1(string s, out TValue value);

        protected static bool TryParse(string[] ss, TryParseValue1 tryParse, Func<TValue[], T> factory, out T value, int count)
        {
            value = default;
            if (ss.Length < count ||
                !TryParse(ss, tryParse, count, out TValue[] values))
                return false;
            value = factory(values);
            return true;
        }

        private static bool TryParse(string[] ss, TryParseValue1 tryParse, int count, out TValue[] values)
        {
            values = new TValue[count];
            for (int i = 0; i < count; i++)
                if (!tryParse(ss[i], out values[i]))
                    return false;
            return true;
        }
    }

    internal class Vector2DParseHelper<TVector, TValue> : ParseHelper<TVector, TValue>
        where TVector : struct, IVector2D<TVector, TValue>
        where TValue : struct
    {
        public static bool TryParseVector(string[] ss, TryParseValue1 tryParse, Func<TValue[], TVector> factory, out TVector value) =>
            TryParse(ss, tryParse, factory, out value, IVector2D<TVector, TValue>.Cardinality);
    }

    internal class Vector3DParseHelper<TVector, TValue> : ParseHelper<TVector, TValue>
        where TVector : struct, IVector3D<TVector, TValue>
        where TValue : struct
    {
        public static bool TryParseVector(string[] ss, TryParseValue1 tryParse, Func<TValue[], TVector> factory, out TVector value) =>
            TryParse(ss, tryParse, factory, out value, IVector3D<TVector, TValue>.Cardinality);
    }
}
