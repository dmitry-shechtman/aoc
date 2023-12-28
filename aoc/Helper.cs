using System;
using System.Collections.Generic;

namespace aoc
{
    interface IHelperStrategy
    {
        char      DefaultSeparator { get; }
        string    DefaultFormat    { get; }
        string[]  FormatKeys       { get; }
    }

    abstract class HelperStrategy<TSelf> : IHelperStrategy
        where TSelf : HelperStrategy<TSelf>
    {
        private static readonly Lazy<TSelf> _instance = new(CreateInstance);

        private static TSelf CreateInstance() =>
            (TSelf)Activator.CreateInstance(typeof(TSelf), true);

        public static TSelf Instance => _instance.Value;

        public HelperStrategy(string[] formatKeys)
        {
            FormatKeys = formatKeys;
        }

        public abstract char DefaultSeparator { get; }
        public string[] FormatKeys { get; }
        public string   DefaultFormat =>
            string.Join(SeparatorString, FormatKeys);

        protected abstract string SeparatorString { get; }
    }

    internal abstract class Helper<T, TValue, TTryParse, TStrategy>
        where T : IReadOnlyList<TValue>
        where TValue : IFormattable
        where TStrategy : IHelperStrategy
    {
        protected Helper(Func<TValue[], T> fromArray, TTryParse tryParse)
        {
            FromArray = fromArray;
            TryParseValue = tryParse;
            DefaultFormat = Strategy.DefaultFormat;
            FormatKeys = Strategy.FormatKeys;
        }

        protected Func<TValue[], T> FromArray     { get; }
        protected TTryParse         TryParseValue { get; }
        protected string            DefaultFormat { get; }
        private   string[]          FormatKeys    { get; }

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

        protected abstract TStrategy Strategy { get; }
    }

    interface IHelper1Strategy : IHelperStrategy
    {
        int Count { get; }
    }

    abstract class Helper1Strategy<TSelf> : HelperStrategy<TSelf>, IHelper1Strategy
        where TSelf : Helper1Strategy<TSelf>
    {
        protected Helper1Strategy(string[] formatKeys)
            : base(formatKeys)
        {
        }

        public int Count => FormatKeys.Length;

        protected override string SeparatorString =>
            $"{DefaultSeparator}";
    }

    internal delegate bool TryParseValue1<T>(string s, out T value);

    internal abstract class Helper1<T, TValue, TStrategy> : Helper<T, TValue, TryParseValue1<TValue>, TStrategy>
        where T : IReadOnlyList<TValue>
        where TValue : IFormattable
        where TStrategy : IHelper1Strategy
    {
        protected Helper1(Func<TValue[], T> fromArray, TryParseValue1<TValue> tryParse)
            : base(fromArray, tryParse)
        {
            Count = Strategy.Count;
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

    internal interface IHelper2Strategy : IHelperStrategy
    {
        int MinCount { get; }
        int MaxCount { get; }
    }

    abstract class Helper2Strategy<TSelf> : HelperStrategy<TSelf>, IHelper2Strategy
        where TSelf : Helper2Strategy<TSelf>
    {
        protected Helper2Strategy(params string[] formatKeys)
            : base(formatKeys)
        {
        }

        public abstract int MinCount { get; }
        public abstract int MaxCount { get; }

        protected override string SeparatorString =>
            $" {DefaultSeparator} ";
    }

    internal delegate bool TryParseValue2<T>(string s, out T value, char separator);

    internal abstract class Helper2<T, TValue, TStrategy> : Helper<T, TValue, TryParseValue2<TValue>, TStrategy>
        where T : IReadOnlyList<TValue>
        where TValue : IFormattable
        where TStrategy : IHelper2Strategy
    {
        protected Helper2(Func<TValue[], T> fromArray, TryParseValue2<TValue> tryParse)
            : base(fromArray, tryParse)
        {
            MinCount = Strategy.MinCount;
            MaxCount = Strategy.MaxCount;
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

    sealed class RangeHelperStrategy : Helper2Strategy<RangeHelperStrategy>, IHelper1Strategy
    {
        private RangeHelperStrategy()
            : base("min", "max")
        {
        }

        public int Count => 2;
        public override int MinCount => 2;
        public override int MaxCount => 2;

        public override char DefaultSeparator => '~';
    }

    internal sealed class RangeHelper<TRange, TValue> : Helper1<TRange, TValue, RangeHelperStrategy>
        where TRange : struct, IRange<TRange, TValue>
        where TValue : struct, IFormattable
    {
        public RangeHelper(Func<TValue[], TRange> fromArray, TryParseValue1<TValue> tryParse)
            : base(fromArray, tryParse)
        {
        }

        protected override RangeHelperStrategy Strategy =>
            RangeHelperStrategy.Instance;
    }

    internal sealed class VectorRangeHelper<TRange, TVector> : Helper2<TRange, TVector, RangeHelperStrategy>
        where TRange : struct, IRange<TRange, TVector>
        where TVector : struct, IFormattable
    {
        public VectorRangeHelper(Func<TVector[], TRange> fromArray, TryParseValue2<TVector> tryParse)
            : base(fromArray, tryParse)
        {
        }

        protected override RangeHelperStrategy Strategy =>
            RangeHelperStrategy.Instance;
    }

    sealed class ParticleHelperStrategy : Helper2Strategy<ParticleHelperStrategy>
    {
        private ParticleHelperStrategy()
            : base("p", "v", "a")
        {
        }

        public override int MinCount => 2;
        public override int MaxCount => 3;

        public override char DefaultSeparator => '@';
    }

    internal sealed class ParticleHelper<TParticle, TVector> : Helper2<TParticle, TVector, ParticleHelperStrategy>
        where TParticle : struct, IParticle<TParticle, TVector>
        where TVector : struct, IFormattable
    {
        public ParticleHelper(Func<TVector[], TParticle> fromArray, TryParseValue2<TVector> tryParse)
            : base(fromArray, tryParse)
        {
        }

        protected override ParticleHelperStrategy Strategy =>
            ParticleHelperStrategy.Instance;
    }
}
