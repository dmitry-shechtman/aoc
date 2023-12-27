using System;

namespace aoc
{
    internal class ParseHelper<T, TValue, TTryParse>
    {
        protected ParseHelper(Func<TValue[], T> fromArray, TTryParse tryParse)
        {
            FromArray = fromArray;
            TryParseValue = tryParse;
        }

        protected Func<TValue[], T> FromArray { get; }
        protected TTryParse TryParseValue { get; }
    }

    public delegate bool TryParseValue1<T>(string s, out T value);

    internal class ParseHelper1<T, TValue> : ParseHelper<T, TValue, TryParseValue1<TValue>>
    {
        protected ParseHelper1(Func<TValue[], T> fromArray, TryParseValue1<TValue> tryParse, int count)
            : base(fromArray, tryParse)
        {
            Count = count;
        }

        private int Count { get; }

        public T Parse(string s, char separator) =>
            TryParse(s, out T value, separator)
                ? value
                : throw new InvalidOperationException($"Input string was not in a correct format.");

        public bool TryParse(string s, out T value, char separator) =>
            TryParse(s.Trim().Split(separator, StringSplitOptions.TrimEntries), out value);

        public T Parse(string[] ss) =>
            TryParse(ss, out T value)
                ? value
                : throw new InvalidOperationException($"Input string was not in a correct format.");

        public bool TryParse(string[] ss, out T value)
        {
            value = default;
            if (ss.Length < Count ||
                !TryParse(ss, out TValue[] values))
                    return false;
            value = FromArray(values);
            return true;
        }

        private bool TryParse(string[] ss, out TValue[] values)
        {
            values = new TValue[Count];
            for (int i = 0; i < Count; i++)
                if (!TryParseValue(ss[i], out values[i]))
                    return false;
            return true;
        }
    }

    public delegate bool TryParseValue2<T>(string s, out T value, char separator);

    internal class ParseHelper2<T, TValue> : ParseHelper<T, TValue, TryParseValue2<TValue>>
    {
        protected ParseHelper2(Func<TValue[], T> fromArray, TryParseValue2<TValue> tryParse, int minCount, int maxCount)
            : base(fromArray, tryParse)
        {
            MinCount = minCount;
            MaxCount = maxCount;
        }

        private int MinCount { get; }
        private int MaxCount { get; }

        public T Parse(string s, char separator, char separator2) =>
            TryParse(s, out T value, separator, separator2)
                ? value
                : throw new InvalidOperationException($"Input string was not in a correct format.");

        public bool TryParse(string s, out T value, char separator, char separator2) =>
            TryParse(s.Trim().Split(separator, StringSplitOptions.TrimEntries), out value, separator2);

        public T Parse(string[] ss, char separator) =>
            TryParse(ss, out T value, separator)
                ? value
                : throw new InvalidOperationException($"Input string was not in a correct format.");

        public bool TryParse(string[] ss, out T value, char separator)
        {
            value = default;
            if (ss.Length < MinCount ||
                !TryParse(ss, out TValue[] values, separator))
                    return false;
            value = FromArray(values);
            return true;
        }

        private bool TryParse(string[] ss, out TValue[] values, char separator)
        {
            values = new TValue[MaxCount];
            for (int i = 0; i < MaxCount; i++)
                if (i < ss.Length &&
                    !TryParseValue(ss[i], out values[i], separator))
                        return false;
            return true;
        }
    }

    internal sealed class RangeHelper<TRange, TValue> : ParseHelper1<TRange, TValue>
        where TRange : struct, IRange<TRange, TValue>
        where TValue : struct
    {
        private const int Count = 2;

        public RangeHelper(Func<TValue[], TRange> fromArray, TryParseValue1<TValue> tryParse)
            : base(fromArray, tryParse, Count)
        {
        }
    }

    internal sealed class VectorRangeHelper<TRange, TVector> : ParseHelper2<TRange, TVector>
        where TRange : struct, IRange<TRange, TVector>
        where TVector : struct
    {
        private const int Count = 2;

        public VectorRangeHelper(Func<TVector[], TRange> fromArray, TryParseValue2<TVector> tryParse)
            : base(fromArray, tryParse, Count, Count)
        {
        }
    }

    internal sealed class ParticleHelper<TParticle, TVector> : ParseHelper2<TParticle, TVector>
        where TParticle : struct, IParticle<TParticle, TVector>
        where TVector : struct
    {
        private const int MinCount = 2;
        private const int MaxCount = 3;

        public ParticleHelper(Func<TVector[], TParticle> fromArray, TryParseValue2<TVector> tryParse)
            : base(fromArray, tryParse, MinCount, MaxCount)
        {
        }
    }
}
