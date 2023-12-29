using System;
using System.Collections.Generic;

namespace aoc.Internal
{
    interface IHelperStrategy
    {
        char      DefaultSeparator { get; }
        string    DefaultFormat    { get; }
        string[]  FormatKeys       { get; }
    }

    abstract class HelperStrategy<TSelf> : Singleton<TSelf>, IHelperStrategy
        where TSelf : HelperStrategy<TSelf>
    {
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

    abstract class Helper<T, TItem, TTryParse, TStrategy>
        where T : IReadOnlyList<TItem>
        where TItem : IFormattable
        where TStrategy : IHelperStrategy
    {
        protected Helper(Func<TItem[], T> fromArray, TTryParse tryParse)
        {
            FromArray = fromArray;
            TryParseItem = tryParse;
            DefaultFormat = Strategy.DefaultFormat;
            FormatKeys = Strategy.FormatKeys;
            DefaultSeparatorChar = Strategy.DefaultSeparator;
        }

        protected Func<TItem[], T> FromArray            { get; }
        protected TTryParse        TryParseItem         { get; }
        protected string           DefaultFormat        { get; }
        private   string[]         FormatKeys           { get; }
        private   char             DefaultSeparatorChar { get; }

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

        public T Parse(string s) =>
            Parse(s, DefaultSeparatorChar);

        public bool TryParse(string s, out T value) =>
            TryParse(s, DefaultSeparatorChar, out value);

        public abstract T    Parse(string s, char separator);
        public abstract bool TryParse(string s, char separator, out T value);
        public abstract T    Parse(string[] ss);
        public abstract bool TryParse(string[] ss, out T value);

        protected abstract TStrategy Strategy { get; }
    }
}
