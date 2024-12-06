using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace aoc.Internal
{
    interface IHelperStrategy<T, TItem>
        where T : IReadOnlyCollection<TItem>
        where TItem : IFormattable
    {
        char      DefaultSeparator { get; }
        string    DefaultFormat    { get; }
        string[]  FormatKeys       { get; }
        int       MinCount         { get; }
        int       MaxCount         { get; }
    }

    abstract class HelperStrategy<TSelf, T, TItem> : Singleton<TSelf>, IHelperStrategy<T, TItem>
        where TSelf : HelperStrategy<TSelf, T, TItem>
        where T : IReadOnlyCollection<TItem>
        where TItem : IFormattable
    {
        protected HelperStrategy(params string[] formatKeys)
        {
            FormatKeys = formatKeys;
        }

        public abstract char DefaultSeparator { get; }
        public string[] FormatKeys { get; }

        public string   DefaultFormat =>
            string.Join(SeparatorString, FormatKeys);

        public virtual int MinCount => FormatKeys.Length;
        public virtual int MaxCount => FormatKeys.Length;

        protected virtual string SeparatorString =>
            $"{DefaultSeparator}";
    }

    delegate bool TryParse<T>(string s, out T value);

    abstract class Helper<T, TItem, TStrategy>
        where T : IReadOnlyCollection<TItem>
        where TItem : IFormattable
        where TStrategy : IHelperStrategy<T, TItem>
    {
        protected Helper(Func<TItem[], T> fromArray, TryParse<TItem> tryParse)
        {
            FromArray = fromArray;
            TryParseItem = tryParse;
            DefaultFormat = Strategy.DefaultFormat;
            FormatKeys = Strategy.FormatKeys;
            DefaultSeparator = Strategy.DefaultSeparator;
            MinCount = Strategy.MinCount;
            MaxCount = Strategy.MaxCount;
        }

        protected Func<TItem[], T> FromArray        { get; }
        protected TryParse<TItem>  TryParseItem     { get; }
        protected string           DefaultFormat    { get; }
        private   string[]         FormatKeys       { get; }
        public    char             DefaultSeparator { get; }
        protected int              MinCount         { get; }
        protected int              MaxCount         { get; }

        public string ToString(T value, IFormatProvider provider = null) =>
            ToStringInner(value, DefaultFormat, provider);

        public string ToString(T value, string format, IFormatProvider provider = null)
        {
            if (string.IsNullOrEmpty(format))
                format = DefaultFormat;
            return ToStringInner(value, format, provider);
        }

        protected string ToStringInner(T value, string format, IFormatProvider provider)
        {
            StringBuilder sb = new();
            for (int i = 0; i < format.Length; i++)
                if (TryGetItem(value, format, provider, ref i, out var item))
                    sb.Append(item.ToString(null, provider));
                else
                    sb.Append(format[i]);
            return sb.ToString();
        }

        protected virtual bool TryGetItem(T value, string format, IFormatProvider provider, ref int i, out IFormattable item)
        {
            for (int j = 0; j < FormatKeys.Length; j++)
            {
                string key = FormatKeys[j];
                if (i + key.Length <= format.Length && format[i..(i + key.Length)] == key)
                {
                    item = GetItem(value, j);
                    i += key.Length - 1;
                    return true;
                }
            }
            item = null;
            return false;
        }

        protected abstract TItem GetItem(T value, int i);

        public T Parse(string s) =>
            Parse(s, DefaultSeparator);

        public bool TryParse(string s, out T value) =>
            TryParse(s, DefaultSeparator, out value);

        public T Parse(string s, char separator) =>
            TryParse(s, separator, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(string s, char separator, out T value) =>
            TryParse(s.Trim().Split(separator, StringSplitOptions.TrimEntries), out value);

        public T Parse(string s, string separator) =>
            TryParse(s, separator, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(string s, string separator, out T value) =>
            TryParse(s.Trim().Split(separator), out value);

        public T Parse(string s, Regex separator) =>
            TryParse(s, separator, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(string s, Regex separator, out T value) =>
            TryParse(separator.Split(s)[1..], out value);

        public T Parse(string[] ss) =>
            TryParse(ss, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(string[] ss, out T value)
        {
            value = default;
            if (ss.Length < MinCount ||
                !TryParse(ss, out TItem[] values))
                    return false;
            value = FromArray(values);
            return true;
        }

        private bool TryParse(string[] ss, out TItem[] values)
        {
            values = new TItem[MaxCount];
            for (int i = 0; i < MaxCount; i++)
                if (i < ss.Length &&
                    !TryParseItem(ss[i], out values[i]))
                        return false;
            return true;
        }

        protected abstract TStrategy Strategy { get; }
    }
}
