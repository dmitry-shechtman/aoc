using System;
using System.Collections.Generic;

namespace aoc
{
    interface IHelper2Strategy : IHelperStrategy
    {
        int  MinCount          { get; }
        int  MaxCount          { get; }
        char DefaultSeparator2 { get; }
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

        public char DefaultSeparator2 => ',';

        protected override string SeparatorString =>
            $" {DefaultSeparator} ";
    }

    delegate bool TryParseValue2<T>(string s, char separator, out T value);

    abstract class Helper2<T, TValue, TStrategy> : Helper<T, TValue, TryParseValue2<TValue>, TStrategy>
        where T : IReadOnlyList<TValue>
        where TValue : IFormattable
        where TStrategy : IHelper2Strategy
    {
        protected Helper2(Func<TValue[], T> fromArray, TryParseValue2<TValue> tryParse)
            : base(fromArray, tryParse)
        {
            MinCount = Strategy.MinCount;
            MaxCount = Strategy.MaxCount;
            DefaultSeparator2 = Strategy.DefaultSeparator2;
        }

        private int  MinCount          { get; }
        private int  MaxCount          { get; }
        private char DefaultSeparator2 { get; }

        public override T Parse(string s, char separator) =>
            Parse(s, separator, DefaultSeparator2);

        public override bool TryParse(string s, char separator, out T value) =>
            TryParse(s, separator, DefaultSeparator2, out value);

        public override T Parse(string[] ss) =>
            Parse(ss, DefaultSeparator2);

        public override bool TryParse(string[] ss, out T value) =>
            TryParse(ss, DefaultSeparator2, out value);

        public T Parse(string s, char separator, char separator2) =>
            TryParse(s, separator, separator2, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(string s, char separator, char separator2, out T value) =>
            TryParse(s.Trim().Split(separator, StringSplitOptions.TrimEntries), separator2, out value);

        public T Parse(string[] ss, char separator) =>
            TryParse(ss, separator, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(string[] ss, char separator, out T value)
        {
            value = default;
            if (ss.Length < MinCount ||
                !TryParse(ss, separator, out TValue[] values))
                    return false;
            value = FromArray(values);
            return true;
        }

        private bool TryParse(string[] ss, char separator, out TValue[] values)
        {
            values = new TValue[MaxCount];
            for (int i = 0; i < MaxCount; i++)
                if (i < ss.Length &&
                    !TryParseValue(ss[i], separator, out values[i]))
                        return false;
            return true;
        }
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

    sealed class ParticleHelper<TParticle, TVector> : Helper2<TParticle, TVector, ParticleHelperStrategy>
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
