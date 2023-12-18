using System;

namespace aoc
{
    public static class ArrayExtensions
    {
        public static void Clear(this Array array) =>
            Array.Clear(array, 0, array.Length);

        public static T Find<T>(this T[] array, Predicate<T> match) =>
            Array.Find(array, match);

        public static int FindIndex<T>(this T[] array, Predicate<T> match) =>
            Array.FindIndex(array, match);

        public static T FindLast<T>(this T[] array, Predicate<T> match) =>
            Array.FindLast(array, match);

        public static int FindLastIndex<T>(this T[] array, Predicate<T> match) =>
            Array.FindLastIndex(array, match);

        public static int IndexOf<T>(this T[] array, T value) =>
            Array.IndexOf(array, value);

        public static int LastIndexOf<T>(this T[] array, T value) =>
            Array.LastIndexOf(array, value);

        public static void Sort<T>(this T[] array, Comparison<T> comparison) =>
            Array.Sort(array, comparison);
    }
}
