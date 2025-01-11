﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions
{
    public static class GroupExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[] GetValues(this Group group) =>
            SelectValues(group).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>(this Group group,
            IFormatProvider? provider = null)
                where T : IConvertible =>
                    SelectValues<T>(group, provider).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>(this Group group,
            ParseSpan<T> selector) =>
                SelectValues(group, selector).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetValues<T>(this Group group,
            ParseSpan<T, IFormatProvider> selector,
            IFormatProvider? provider = null) =>
                SelectValues(group, selector, provider).ToArray();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<string> SelectValues(this Group group) =>
            group.Captures.SelectValues();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> SelectValues<T>(this Group group,
            IFormatProvider? provider = null)
                where T : IConvertible =>
                    group.Captures.SelectValues<T>(provider);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> SelectValues<T>(this Group group,
            ParseSpan<T> selector) =>
                group.Captures.SelectValues(selector);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> SelectValues<T>(this Group group,
            ParseSpan<T, IFormatProvider> selector,
            IFormatProvider? provider = null) =>
                group.Captures.SelectValues(selector, provider);
    }
}
