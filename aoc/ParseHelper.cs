using System;

namespace aoc
{
    internal class ParseHelper
    {
        public delegate bool TryParse1<T>(string[] ss, out T value);
        public delegate bool TryParse2<T>(string[] ss, out T value, char separator);

        public delegate bool TryParseValue2<T>(string s, out T value, char separator);
        public delegate bool TryParseValue3<T>(string s, out T value, char separator, char separator2);

        public static T Parse<T>(string s, TryParseValue2<T> tryParse, char separator) =>
            tryParse(s, out T value, separator)
                ? value
                : throw new InvalidOperationException($"Input string was not in a correct format.");

        public static T Parse<T>(string s, TryParseValue3<T> tryParse, char separator, char separator2) =>
            tryParse(s, out T value, separator, separator2)
                ? value
                : throw new InvalidOperationException($"Input string was not in a correct format.");

        public static bool TryParse<T>(string s, TryParse1<T> tryParse, char separator, out T value) =>
            tryParse(s.Trim().Split(separator, StringSplitOptions.TrimEntries), out value);

        public static T Parse<T>(string[] ss, TryParse1<T> tryParse) =>
            tryParse(ss, out T value)
                ? value
                : throw new InvalidOperationException($"Input string was not in a correct format.");

        public static bool TryParse<T>(string s, TryParse2<T> tryParse, char separator, char separator2, out T value) =>
            tryParse(s.Trim().Split(separator, StringSplitOptions.TrimEntries), out value, separator2);

        public static T Parse<T>(string[] ss, TryParse2<T> tryParse, char separator) =>
            tryParse(ss, out T value, separator)
                ? value
                : throw new InvalidOperationException($"Input string was not in a correct format.");
    }

    internal class ParseHelper<T, TValue> : ParseHelper
    {
        public delegate bool TryParseValue1(string s, out TValue value);
        public delegate bool TryParseValue2(string s, out TValue value, char separator);

        protected static bool TryParse(string[] ss, TryParseValue1 tryParse, Func<TValue[], T> factory, out T value, int count)
        {
            value = default;
            if (ss.Length < count ||
                !TryParse(ss, tryParse, count, out TValue[] values))
                    return false;
            value = factory(values);
            return true;
        }

        protected static bool TryParse(string[] ss, TryParseValue2 tryParse, Func<TValue[], T> factory, out T value, char separator, int minCount, int maxCount)
        {
            value = default;
            if (ss.Length < minCount ||
                !TryParse(ss, tryParse, separator, maxCount, out TValue[] values))
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

        private static bool TryParse(string[] ss, TryParseValue2 tryParse, char separator, int count, out TValue[] values)
        {
            values = new TValue[count];
            for (int i = 0; i < count; i++)
                if (i < ss.Length &&
                    !tryParse(ss[i], out values[i], separator))
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

    internal class RangeParseHelper<TRange, TValue> : ParseHelper<TRange, TValue>
        where TRange : struct, IRange<TRange, TValue>
        where TValue : struct
    {
        public static bool TryParseRange(string[] ss, TryParseValue1 tryParse, Func<TValue[], TRange> factory, out TRange value) =>
            TryParse(ss, tryParse, factory, out value, IRange<TValue>.Cardinality);

        public static bool TryParseRange(string[] ss, TryParseValue2 tryParse, Func<TValue[], TRange> factory, out TRange value, char separator) =>
            TryParse(ss, tryParse, factory, out value, separator, IRange<TValue>.Cardinality, IRange<TValue>.Cardinality);
    }

    internal class ParticleParseHelper<TParticle, TVector, T> : ParseHelper<TParticle, TVector>
        where TParticle : struct, IParticle<TParticle, TVector, T>
        where TVector : struct, IVector<T>
        where T : struct
    {
        public static bool TryParseParticle(string[] ss, TryParseValue2 tryParse, Func<TVector[], TParticle> factory, out TParticle value, char separator) =>
            TryParse(ss, tryParse, factory, out value, separator, IParticle<TVector>.MinCardinality, IParticle<TVector>.MaxCardinality);
    }
}
