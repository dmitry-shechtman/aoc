using System;
using System.Collections.Generic;

namespace aoc
{
    internal abstract class Helper<T, TValue, TTryParse>
        where T : IReadOnlyList<TValue>
        where TValue : IFormattable
    {
        protected Helper(Func<TValue[], T> fromArray, TTryParse tryParse)
        {
            FromArray = fromArray;
            TryParseValue = tryParse;
            DefaultFormat = GetDefaultFormat();
            FormatKeys = GetFormatKeys();
        }

        protected Func<TValue[], T> FromArray { get; }
        protected TTryParse TryParseValue { get; }
        protected string    DefaultFormat { get; }
        private   string[]  FormatKeys    { get; }

        public string ToString(T value, IFormatProvider provider = null) =>
            ToStringInner(value, DefaultFormat, provider);

        public string ToString(T value, string format, IFormatProvider provider = null)
        {
            if (string.IsNullOrEmpty(format))
                format = DefaultFormat;
            return ToStringOuter(value, format, provider);
        }

        protected virtual string ToStringOuter(T value, string format, IFormatProvider provider) =>
            ToStringInner(value, format, provider);

        protected string ToStringInner(T value, string format, IFormatProvider provider)
        {
            for (int i = 0; i < FormatKeys.Length; i++)
                format = format.Replace(FormatKeys[i], value[i].ToString(null, provider));
            return format;
        }

        protected abstract string   GetDefaultFormat();
        protected abstract string[] GetFormatKeys();
    }

    internal delegate bool TryParseValue1<T>(string s, out T value);

    internal abstract class Helper1<T, TValue> : Helper<T, TValue, TryParseValue1<TValue>>
        where T : IReadOnlyList<TValue>
        where TValue : IFormattable
    {
        protected Helper1(Func<TValue[], T> fromArray, TryParseValue1<TValue> tryParse, int count)
            : base(fromArray, tryParse)
        {
            Count = count;
        }

        private int Count { get; }

        public T Parse(string s, char separator) =>
            TryParse(s, out T value, separator)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(string s, out T value, char separator) =>
            TryParse(s.Trim().Split(separator, StringSplitOptions.TrimEntries), out value);

        public T Parse(string[] ss) =>
            TryParse(ss, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

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

    internal delegate bool TryParseValue2<T>(string s, out T value, char separator);

    internal abstract class Helper2<T, TValue> : Helper<T, TValue, TryParseValue2<TValue>>
        where T : IReadOnlyList<TValue>
        where TValue : IFormattable
    {
        protected Helper2(Func<TValue[], T> fromArray, TryParseValue2<TValue> tryParse, int minCount, int maxCount)
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
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(string s, out T value, char separator, char separator2) =>
            TryParse(s.Trim().Split(separator, StringSplitOptions.TrimEntries), out value, separator2);

        public T Parse(string[] ss, char separator) =>
            TryParse(ss, out T value, separator)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

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

    internal sealed class RangeHelper<TRange, TValue> : Helper1<TRange, TValue>
        where TRange : struct, IRange<TRange, TValue>
        where TValue : struct, IFormattable
    {
        private const int Count = 2;

        public RangeHelper(Func<TValue[], TRange> fromArray, TryParseValue1<TValue> tryParse)
            : base(fromArray, tryParse, Count)
        {
        }

        protected override string   GetDefaultFormat() => "min ~ max";
        protected override string[] GetFormatKeys()    => new[] { "min", "max" };
    }

    internal sealed class VectorRangeHelper<TRange, TVector> : Helper2<TRange, TVector>
        where TRange : struct, IRange<TRange, TVector>
        where TVector : struct, IFormattable
    {
        private const int Count = 2;

        public VectorRangeHelper(Func<TVector[], TRange> fromArray, TryParseValue2<TVector> tryParse)
            : base(fromArray, tryParse, Count, Count)
        {
        }

        protected override string   GetDefaultFormat() => "min ~ max";
        protected override string[] GetFormatKeys()    => new[] { "min", "max" };
    }

    internal sealed class ParticleHelper<TParticle, TVector> : Helper2<TParticle, TVector>
        where TParticle : struct, IParticle<TParticle, TVector>
        where TVector : struct, IFormattable
    {
        private const int MinCount = 2;
        private const int MaxCount = 3;

        public ParticleHelper(Func<TVector[], TParticle> fromArray, TryParseValue2<TVector> tryParse)
            : base(fromArray, tryParse, MinCount, MaxCount)
        {
        }

        protected override string   GetDefaultFormat() => "p @ v @ a";
        protected override string[] GetFormatKeys()    => new[] { "p", "v", "a" };
    }
}
