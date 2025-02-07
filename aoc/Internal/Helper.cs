﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace aoc.Internal
{
    abstract class HelperStrategy<TSelf, T, TItem> : Singleton<TSelf>
        where TSelf : HelperStrategy<TSelf, T, TItem>
        where T : IReadOnlyCollection<TItem>
        where TItem : IFormattable
    {
        protected HelperStrategy(int minCount, int maxCount, char separatorChar, string separatorString, string[] formatKeys)
        {
            MinCount = minCount;
            MaxCount = maxCount;
            DefaultSeparator = separatorChar;
            SeparatorString = separatorString;
            FormatKeys = formatKeys;
        }

        public    int      MinCount         { get; }
        public    int      MaxCount         { get; }
        public    char     DefaultSeparator { get; }
        public    string   SeparatorString  { get; }
        protected string[] FormatKeys       { get; }

        public string GetDefaultFormat() =>
            string.Join(SeparatorString, FormatKeys);

        public abstract bool TryGetItem(T value, string format, IFormatProvider? provider, ref int i,
            [MaybeNullWhen(false)] out IFormattable item);
    }

    abstract class ItemHelperStrategy<TSelf, T, TItem> : HelperStrategy<TSelf, T, TItem>
        where TSelf : ItemHelperStrategy<TSelf, T, TItem>
        where T : IReadOnlyCollection<TItem>
        where TItem : IFormattable
    {
        protected ItemHelperStrategy(char separatorChar, string separatorString, params string[] formatKeys)
            : base(formatKeys.Length, formatKeys.Length, separatorChar, separatorString, formatKeys)
        {
        }

        public sealed override bool TryGetItem(T value, string format, IFormatProvider? provider, ref int i,
            [MaybeNullWhen(false)] out IFormattable item)
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
        public ListHelperStrategy(char separator, string[] formatKeys)
            : base(separator, $"{separator}", formatKeys)
        {
        }

        protected sealed override TItem GetItem(T value, int i) =>
            value[i];
    }

    delegate T FromSpan<T, TItem>(ReadOnlySpan<TItem> values);

    interface IItemHelper<T>
        where T : struct, IFormattable
    {
        bool TryParse(ReadOnlySpan<char> input, NumberStyles styles, IFormatProvider? provider, out T value);
        IEnumerable<Match> GetMatches(string? input, out int count);
    }

    interface IParseHelper<T, TSeparator>
        where T : struct, IFormattable
    {
        bool TryParse(ReadOnlySpan<char> input, TSeparator separator, NumberStyles styles, IFormatProvider? provider, out T value);
    }

    interface ISpanParseHelper<T> : IItemHelper<T>
        where T : unmanaged, IFormattable
    {
        bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, NumberStyles styles, IFormatProvider? provider, out T value);
    }

    abstract class Helper<T, TItem, TStrategy, TItemHelper> : Builders.IBuilder<T>
        where T : unmanaged, IReadOnlyCollection<TItem>
        where TItem : unmanaged, IFormattable
        where TStrategy : HelperStrategy<TStrategy, T, TItem>
        where TItemHelper : IItemHelper<TItem>
    {
        protected Helper(TStrategy strategy, FromSpan<T, TItem> fromSpan, TItemHelper item, int chunkCount = 1, int chunkSize = 0)
        {
            Strategy = strategy;
            FromSpan = fromSpan;
            Item = item;
            _defaultFormat = new(Strategy.GetDefaultFormat);
            DefaultSeparator = Strategy.DefaultSeparator;
            MinCount = Strategy.MinCount;
            MaxCount = Strategy.MaxCount;
            ChunkCount = chunkCount;
            ChunkSize = chunkSize > 0 ? chunkSize : MinCount;
        }

        private   TStrategy           Strategy         { get; }
        public    FromSpan<T, TItem>  FromSpan         { get; }
        protected TItemHelper         Item             { get; }
        private   char                DefaultSeparator { get; }
        public    int                 MinCount         { get; }
        public    int                 MaxCount         { get; }
        private   int                 ChunkCount       { get; }
        protected int                 ChunkSize        { get; }

        private readonly Lazy<string> _defaultFormat;
        private string DefaultFormat => _defaultFormat.Value;

        public T FromArray(params TItem[] values) =>
            FromSpan(values);

        public string ToString(T value, IFormatProvider? provider = null) =>
            ToStringInner(value, DefaultFormat, provider);

        public string ToString(T value, string? format, IFormatProvider? provider = null)
        {
            if (string.IsNullOrEmpty(format))
                format = DefaultFormat;
            return ToStringInner(value, format, provider);
        }

        private string ToStringInner(T value, string format, IFormatProvider? provider)
        {
            StringBuilder sb = new();
            for (int i = 0; i < format.Length; i++)
                if (Strategy.TryGetItem(value, format, provider, ref i, out var item))
                    sb.Append(item.ToString(null, provider));
                else
                    sb.Append(format[i]);
            return sb.ToString();
        }

        public T Parse(ReadOnlySpan<char> input, IFormatProvider? provider = null) =>
            TryParse(input, provider, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(ReadOnlySpan<char> input, out T value) =>
            TryParse(input, provider: null, out value);

        public bool TryParse(ReadOnlySpan<char> input, IFormatProvider? provider, out T value) =>
            TryParse(input, DefaultSeparator, provider, out value);

        public T Parse(ReadOnlySpan<char> input, NumberStyles styles, IFormatProvider? provider) =>
            TryParse(input, styles, provider, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(ReadOnlySpan<char> input, NumberStyles styles, out T value) =>
            TryParse(input, styles, null, out value);

        public bool TryParse(ReadOnlySpan<char> input, NumberStyles styles, IFormatProvider? provider, out T value) =>
            TryParse(input, DefaultSeparator, styles, provider, out value);

        public T Parse(ReadOnlySpan<char> input, char separator, IFormatProvider? provider) =>
            Parse(input, separator, 0, provider);

        public T Parse(ReadOnlySpan<char> input, char separator, NumberStyles styles, IFormatProvider? provider) =>
            TryParse(input, separator, styles, provider, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(ReadOnlySpan<char> input, char separator, out T value) =>
            TryParse(input, separator, null, out value);

        public bool TryParse(ReadOnlySpan<char> input, char separator, NumberStyles styles, out T value) =>
            TryParse(input, separator, styles, null, out value);

        public bool TryParse(ReadOnlySpan<char> input, char separator, IFormatProvider? provider, out T value) =>
            TryParse(input, separator, 0, provider, out value);

        public bool TryParse(ReadOnlySpan<char> input, char separator, NumberStyles styles, IFormatProvider? provider, out T value)
        {
            Span<System.Range> split = stackalloc System.Range[MaxCount];
            int count = input.Split(split, separator, StringSplitOptions.TrimEntries);
            return TryParse(input, split[..count], styles, provider, out value);
        }

        public T Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, IFormatProvider? provider) =>
            Parse(input, separator, 0, provider);

        public T Parse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, NumberStyles styles, IFormatProvider? provider) =>
            TryParse(input, separator, styles, provider, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, out T value) =>
            TryParse(input, separator, null, out value);

        public bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, NumberStyles styles, out T value) =>
            TryParse(input, separator, styles, null, out value);

        public bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, IFormatProvider? provider, out T value) =>
            TryParse(input, separator, 0, provider, out value);

        public bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, NumberStyles styles, IFormatProvider? provider, out T value)
        {
            Span<System.Range> split = stackalloc System.Range[MaxCount];
            int count = input.Split(split, separator, StringSplitOptions.TrimEntries);
            return TryParse(input, split[..count], styles, provider, out value);
        }

        public T ParseAny(string? input, IFormatProvider? provider) =>
            ParseAny(input, 0, provider);

        public T ParseAny(string? input, NumberStyles styles, IFormatProvider? provider) =>
            TryParseAny(input, styles, provider, out T value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParseAny(string? input, out T value) =>
            TryParseAny(input, null, out value);

        public bool TryParseAny(string? input, NumberStyles styles, out T value) =>
            TryParseAny(input, styles, null, out value);

        public bool TryParseAny(string? input, IFormatProvider? provider, out T value) =>
            TryParseAny(input, 0, provider, out value);

        public bool TryParseAny(string? input, NumberStyles styles, IFormatProvider? provider, out T value)
        {
            value = default;
            return TryGetMatches(input, out var matches, out var matchCount, out var chunkSize)
                && TryParse(matches.GetEnumerator(), styles, provider, matchCount, chunkSize, out value);
        }

        public T[] ParseAll(string? input, IFormatProvider? provider) =>
            ParseAll(input, 0, provider);

        public T[] ParseAll(string? input, NumberStyles styles, IFormatProvider? provider) =>
            TryParseAll(input, styles, provider, out var value)
                ? value
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParseAll(string? input,
            [MaybeNullWhen(false)] out T[] values) =>
                TryParseAll(input, null, out values);

        public bool TryParseAll(string? input, NumberStyles styles,
            [MaybeNullWhen(false)] out T[] values) =>
                TryParseAll(input, styles, null, out values);

        public bool TryParseAll(string? input, IFormatProvider? provider,
            [MaybeNullWhen(false)] out T[] values) =>
                TryParseAll(input, 0, provider, out values);

        public bool TryParseAll(string? input, NumberStyles styles, IFormatProvider? provider,
            [MaybeNullWhen(false)] out T[] values) =>
                TryParseAll(input, styles, provider, ChunkSize, out values);

        public virtual T[] ParseAll(string? input, IFormatProvider? provider, int itemSize) =>
            ParseAll(input, 0, provider, itemSize);

        public T[] ParseAll(string? input, NumberStyles styles, IFormatProvider? provider, int itemSize) =>
            TryParseAll(input, styles, provider, itemSize, out var values)
                ? values
                : throw new InvalidOperationException("Input string was not in a correct format.");

        public bool TryParseAll(string? input, int itemSize,
            [MaybeNullWhen(false)] out T[] values) =>
                TryParseAll(input, null, itemSize, out values);

        public bool TryParseAll(string? input, NumberStyles styles, int itemSize,
            [MaybeNullWhen(false)] out T[] values) =>
                TryParseAll(input, styles, null, itemSize, out values);

        public bool TryParseAll(string? input, IFormatProvider? provider, int itemSize,
            [MaybeNullWhen(false)] out T[] values) =>
                TryParseAll(input, 0, provider, itemSize, out values);

        public bool TryParseAll(string? input, NumberStyles styles, IFormatProvider? provider, int chunkSize,
            [MaybeNullWhen(false)] out T[] values) =>
                TryParseAll(input, styles, provider, ChunkCount, chunkSize, out values);

        public bool TryParseAll(string? input, NumberStyles styles, IFormatProvider? provider, int chunkCount, int chunkSize,
            [MaybeNullWhen(false)] out T[] values)
        {
            var matches = GetMatches(input, out var matchCount);
            var itemSize = chunkCount * chunkSize;
            if (matchCount == 0 || matchCount % itemSize != 0)
            {
                values = null;
                return false;
            }
            values = new T[matchCount / itemSize];
            var enumerator = matches.GetEnumerator();
            for (int i = 0; i < values.Length; i++)
                if (!TryParse(enumerator, styles, provider, itemSize, chunkSize, out values[i]))
                    return false;
            return true;
        }

        private bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<System.Range> split, NumberStyles styles, IFormatProvider? provider, out T value)
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

        private bool TryParse(ReadOnlySpan<char> input, ReadOnlySpan<System.Range> split, NumberStyles styles, IFormatProvider? provider, Span<TItem> values)
        {
            for (int i = 0; i < split.Length; i++)
                if (!Item.TryParse(input[split[i]], styles, provider, out values[i]))
                    return false;
            return true;
        }

        protected bool TryGetMatches(string? input, out IEnumerable<Match> matches, out int count, out int chunkSize)
        {
            matches = GetMatches(input, out count);
            return (chunkSize = GetChunkSize(count)) > 0;
        }

        public IEnumerable<Match> GetMatches(string? input, out int count) =>
            Item.GetMatches(input, out count);

        protected virtual int GetChunkSize(int count) =>
            count >= MinCount && count <= MaxCount
                ? 1
                : 0;

        protected virtual bool TryParse(IEnumerator<Match> matches, NumberStyles styles, IFormatProvider? provider, int itemSize, int _, out T value)
        {
            value = default;
            Span<TItem> items = stackalloc TItem[itemSize];
            if (!TryParse(matches, styles, provider, items))
                return false;
            value = FromSpan(items);
            return true;
        }

        public bool TryParse(IEnumerator<Match> matches, NumberStyles styles, IFormatProvider? provider, Span<TItem> values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                matches.MoveNext();
                if (!Item.TryParse(matches.Current.ValueSpan, styles, provider, out values[i]))
                    return false;
            }
            return true;
        }
    }

    abstract class Helper<T, TItem, TStrategy> : Helper<T, TItem, TStrategy, INumberHelper<TItem>>
        where T : unmanaged, IReadOnlyCollection<TItem>
        where TItem : unmanaged, IFormattable
        where TStrategy : HelperStrategy<TStrategy, T, TItem>
    {
        protected Helper(TStrategy strategy, FromSpan<T, TItem> fromSpan, INumberHelper<TItem> number)
            : base(strategy, fromSpan, number)
        {
        }
    }
}
