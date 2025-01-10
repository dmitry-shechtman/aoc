using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace aoc.Internal
{
    delegate bool TryParseNumber<T>(ReadOnlySpan<char> input, NumberStyles styles, IFormatProvider? provider, out T value);

    interface INumberHelper<T> : IItemHelper<T>
        where T : struct, IFormattable
    {
        T NegativeOne { get; }
        T Zero { get; }
        T One { get; }
    }

    abstract class NumberHelper<TSelf, T> : Singleton<TSelf>, INumberHelper<T>
        where TSelf : NumberHelper<TSelf, T>
        where T : struct, IFormattable
    {
        private readonly Lazy<Regex> _regex;
        private Regex Regex => _regex.Value;

        protected NumberHelper(TryParseNumber<T> tryParse, NumberStyles defaultStyles, T negativeOne, T zero, T one, string pattern)
        {
            TryParseNumber = tryParse;
            DefaultStyles = defaultStyles;
            NegativeOne = negativeOne;
            Zero = zero;
            One = one;
            _regex = new(() => new(pattern));
        }

        public bool TryParse(ReadOnlySpan<char> input, NumberStyles styles, IFormatProvider? provider, out T value) =>
            TryParseNumber(input, styles != 0 ? styles : DefaultStyles, provider, out value);

        public IEnumerable<Match> GetMatches(string? input, out int count)
        {
            IReadOnlyCollection<Match> result = input is not null
                ? Regex.Matches(input)
                : Array.Empty<Match>();
            count = result.Count;
            return result;
        }

        private TryParseNumber<T> TryParseNumber { get; }
        private NumberStyles DefaultStyles { get; }

        public T NegativeOne { get; }
        public T Zero { get; }
        public T One { get; }
    }

    abstract class IntegerHelper<TSelf, T> : NumberHelper<TSelf, T>
        where TSelf : IntegerHelper<TSelf, T>
        where T : struct, IFormattable
    {
        protected IntegerHelper(TryParseNumber<T> tryParse, T negativeOne, T zero, T one)
            : base(tryParse, NumberStyles.Integer, negativeOne, zero, one, @"[-+]?\d+")
        {
        }
    }

    sealed class Int32Helper : IntegerHelper<Int32Helper, int>
    {
        private Int32Helper()
            : base(int.TryParse, -1, 0, 1)
        {
        }
    }

    sealed class Int64Helper : IntegerHelper<Int64Helper, long>
    {
        private Int64Helper()
            : base(long.TryParse, -1, 0, 1)
        {
        }
    }

    abstract class FloatHelper<TSelf, T> : NumberHelper<TSelf, T>
        where TSelf : FloatHelper<TSelf, T>
        where T : struct, IFormattable
    {
        protected FloatHelper(TryParseNumber<T> tryParse, T negativeOne, T zero, T one)
            : base(tryParse, NumberStyles.Float, negativeOne, zero, one, @"[-+]?\d+(.\d+)?")
        {
        }
    }

    sealed class DoubleHelper : FloatHelper<DoubleHelper, double>
    {
        private DoubleHelper()
            : base(double.TryParse, -1, 0, 1)
        {
        }
    }
}
