using System;
using System.Collections.Generic;
using System.Globalization;
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

        bool TryGetItem(T value, string format, IFormatProvider provider, ref int i, out IFormattable item);
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

        public abstract bool TryGetItem(T value, string format, IFormatProvider provider, ref int i, out IFormattable item);
    }

    abstract class ItemHelperStrategy<TSelf, T, TItem> : HelperStrategy<TSelf, T, TItem>
        where TSelf : ItemHelperStrategy<TSelf, T, TItem>
        where T : IReadOnlyCollection<TItem>
        where TItem : IFormattable
    {
        protected ItemHelperStrategy(params string[] formatKeys)
            : base(formatKeys)
        {
        }

        public sealed override bool TryGetItem(T value, string format, IFormatProvider provider, ref int i, out IFormattable item)
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
    }

    abstract class ListHelperStrategy<TSelf, T, TItem> : ItemHelperStrategy<TSelf, T, TItem>
        where TSelf : ListHelperStrategy<TSelf, T, TItem>
        where T : IReadOnlyList<TItem>
        where TItem : IFormattable
    {
        public ListHelperStrategy(params string[] formatKeys)
            : base(formatKeys)
        {
        }

        protected sealed override TItem GetItem(T value, int i) =>
            value[i];
    }

    delegate T FromSpan<T, TItem>(ReadOnlySpan<TItem> values);

    interface IItemHelper<T>
        where T : struct, IFormattable
    {
        bool TryParse(ReadOnlySpan<char> input, NumberStyles styles, IFormatProvider provider, out T value);
        IEnumerable<Match> GetMatches(string input, out int count);
    }

    interface IParseHelper<T, TSeparator>
        where T : struct, IFormattable
    {
        bool TryParse(ReadOnlySpan<char> input, TSeparator separator, IFormatProvider provider, out T value);
    }

    abstract class Helper<T, TItem, TStrategy, TItemHelper>
        where T : unmanaged, IReadOnlyCollection<TItem>
        where TItem : unmanaged, IFormattable
        where TStrategy : IHelperStrategy<T, TItem>
        where TItemHelper : IItemHelper<TItem>
    {
        protected Helper(FromSpan<T, TItem> fromSpan, TItemHelper item, int itemSize = 0)
        {
            FromSpan = fromSpan;
            Item = item;
            DefaultFormat = Strategy.DefaultFormat;
            DefaultSeparator = Strategy.DefaultSeparator;
            MinCount = Strategy.MinCount;
            MaxCount = Strategy.MaxCount;
            ItemSize = itemSize > 0 ? itemSize : MinCount;
        }

        public    FromSpan<T, TItem>  FromSpan         { get; }
        protected TItemHelper         Item             { get; }
        private   string              DefaultFormat    { get; }
        public    char                DefaultSeparator { get; }
        public    int                 MinCount         { get; }
        public    int                 MaxCount         { get; }
        private   int                 ItemSize         { get; }

        public T FromArray(params TItem[] values) =>
            FromSpan(values);

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
                if (Strategy.TryGetItem(value, format, provider, ref i, out var item))
                    sb.Append(item.ToString(null, provider));
                else
                    sb.Append(format[i]);
            return sb.ToString();
        }

        public T Parse(ReadOnlySpan<char> input, IFormatProvider provider) =>
            TryParse(input, provider, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(ReadOnlySpan<char> input, out T value) =>
            TryParse(input, provider: null, out value);

        public bool TryParse(ReadOnlySpan<char> input, IFormatProvider provider, out T value) =>
            TryParse(input, DefaultSeparator, provider, out value);

        public T Parse(ReadOnlySpan<char> input, NumberStyles styles, IFormatProvider provider) =>
            TryParse(input, styles, provider, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(ReadOnlySpan<char> input, NumberStyles styles, out T value) =>
            TryParse(input, styles, null, out value);

        public bool TryParse(ReadOnlySpan<char> input, NumberStyles styles, IFormatProvider provider, out T value) =>
            TryParse(input, DefaultSeparator, styles, provider, out value);

        public T Parse(ReadOnlySpan<char> input, char separator, IFormatProvider provider) =>
            TryParse(input, separator, provider, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(ReadOnlySpan<char> input, char separator, out T value) =>
            TryParse(input, separator, null, out value);

        public bool TryParse(ReadOnlySpan<char> input, char separator, IFormatProvider provider, out T value) =>
            TryParse(input, separator, 0, provider, out value);

        private bool TryParse(ReadOnlySpan<char> input, char separator, NumberStyles styles, IFormatProvider provider, out T value)
        {
            Span<System.Range> split = stackalloc System.Range[MaxCount];
            int count = input.Split(split, separator, StringSplitOptions.TrimEntries);
            return TryParse(input, split[..count], styles, provider, out value);
        }

        public T Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, IFormatProvider provider) =>
            TryParse(input, separator, provider, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, out T value) =>
            TryParse(input, separator, null, out value);

        public bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, IFormatProvider provider, out T value) =>
            TryParse(input, separator, 0, provider, out value);

        private bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, NumberStyles styles, IFormatProvider provider, out T value)
        {
            Span<System.Range> split = stackalloc System.Range[MaxCount];
            int count = input.Split(split, separator, StringSplitOptions.TrimEntries);
            return TryParse(input, split[..count], styles, provider, out value);
        }

        public T Parse(string input, Regex separator, IFormatProvider provider) =>
            TryParse(input, separator, provider, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(string input, Regex separator, out T value) =>
            TryParse(input, separator, null, out value);

        public bool TryParse(string input, Regex separator, IFormatProvider provider, out T value) =>
            TryParse(separator.Split(input)[1..^1], provider, out value);

        public T ParseAny(string input, IFormatProvider provider) =>
            ParseAny(input, 0, provider);

        public T ParseAny(string input, NumberStyles styles, IFormatProvider provider) =>
            TryParseAny(input, styles, provider, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParseAny(string input, out T value) =>
            TryParseAny(input, null, out value);

        public bool TryParseAny(string input, NumberStyles styles, out T value) =>
            TryParseAny(input, styles, null, out value);

        public bool TryParseAny(string input, IFormatProvider provider, out T value) =>
            TryParseAny(input, 0, provider, out value);

        public bool TryParseAny(string input, NumberStyles styles, IFormatProvider provider, out T value)
        {
            value = default;
            return TryGetMatches(input, out var matches, out var matchCount, out _)
                && TryParse(matches.GetEnumerator(), styles, provider, matchCount, out value);
        }

        public T[] ParseAll(string input, IFormatProvider provider) =>
            ParseAll(input, 0, provider);

        public T[] ParseAll(string input, NumberStyles styles, IFormatProvider provider) =>
            TryParseAll(input, styles, provider, out var value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParseAll(string input, out T[] values) =>
            TryParseAll(input, null, out values);

        public bool TryParseAll(string input, NumberStyles styles, out T[] values) =>
            TryParseAll(input, styles, null, out values);

        public bool TryParseAll(string input, IFormatProvider provider, out T[] values) =>
            TryParseAll(input, 0, provider, out values);

        public bool TryParseAll(string input, NumberStyles styles, IFormatProvider provider, out T[] values) =>
            TryParseAll(input, styles, provider, ItemSize, out values);

        public T[] ParseAll(string input, NumberStyles styles, IFormatProvider provider, int itemSize) =>
            TryParseAll(input, styles, provider, itemSize, out var values)
                ? values
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public virtual bool TryParseAll(string input, NumberStyles styles, IFormatProvider provider, int itemSize, out T[] values)
        {
            values = default;
            var matches = GetMatches(input, out var matchCount);

            if (matchCount == 0 || matchCount % itemSize != 0)
                return false;
            values = new T[matchCount / itemSize];
            var enumerator = matches.GetEnumerator();
            for (int i = 0; i < values.Length; i++)
                if (!TryParse(enumerator, styles, provider, itemSize, out values[i]))
                    return false;
            return true;
        }

        private bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<System.Range> split, NumberStyles styles, IFormatProvider provider, out T value)
        {
            value = default;
            if (split.Length < MinCount || split.Length > MaxCount)
                return false;
            Span<TItem> values = stackalloc TItem[split.Length];
            if (!TryParse(input, split, styles, provider, values))
                return false;
            value = FromSpan(values);
            return true;
        }

        private bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<System.Range> split, NumberStyles styles, IFormatProvider provider, Span<TItem> values)
        {
            for (int i = 0; i < split.Length; i++)
                if (!Item.TryParse(input[split[i]], styles, provider, out values[i]))
                    return false;
            return true;
        }

        private bool TryParse(string[] ss, IFormatProvider provider, out T value)
        {
            value = default;
            if (ss.Length < MinCount || ss.Length > MaxCount)
                return false;
            Span<TItem> values = stackalloc TItem[ss.Length];
            if (!TryParse(ss, provider, values))
                return false;
            value = FromSpan(values);
            return true;
        }

        private bool TryParse(string[] ss, IFormatProvider provider, Span<TItem> values)
        {
            for (int i = 0; i < ss.Length; i++)
                if (!Item.TryParse(ss[i], 0, provider, out values[i]))
                    return false;
            return true;
        }

        protected bool TryGetMatches(string input, out IEnumerable<Match> matches, out int count, out int chunkSize)
        {
            matches = GetMatches(input, out count);
            return (chunkSize = GetChunkSize(count)) > 0;
        }

        public IEnumerable<Match> GetMatches(string input, out int count) =>
            Item.GetMatches(input, out count);

        protected virtual int GetChunkSize(int count) =>
            count >= MinCount && count <= MaxCount
                ? 1
                : 0;

        private bool TryParse(IEnumerator<Match> matches, NumberStyles styles, IFormatProvider provider, int itemSize, out T value)
        {
            value = default;
            Span<TItem> items = stackalloc TItem[itemSize];
            if (!TryParse(matches, styles, provider, items))
                return false;
            value = FromSpan(items);
            return true;
        }

        public bool TryParse(IEnumerator<Match> matches, NumberStyles styles, IFormatProvider provider, Span<TItem> values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                matches.MoveNext();
                if (!Item.TryParse(matches.Current.Value, styles, provider, out values[i]))
                    return false;
            }
            return true;
        }

        protected abstract TStrategy Strategy { get; }
    }

    abstract class Helper<T, TItem, TStrategy> : Helper<T, TItem, TStrategy, INumberHelper<TItem>>
        where T : unmanaged, IReadOnlyCollection<TItem>
        where TItem : unmanaged, IFormattable
        where TStrategy : IHelperStrategy<T, TItem>
    {
        protected Helper(FromSpan<T, TItem> fromSpan, INumberHelper<TItem> number, int itemSize = 0)
            : base(fromSpan, number, itemSize)
        {
        }
    }
}
