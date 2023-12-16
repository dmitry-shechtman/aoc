using System;

namespace aoc
{
    public static class ArrayExtensions
    {
        public static void Clear<T>(this T[] array) =>
            Array.Clear(array, 0, array.Length);

        public static int IndexOf<T>(this T[] array, T value) =>
            Array.IndexOf(array, value);

        public static int LastIndexOf<T>(this T[] array, T value) =>
            Array.LastIndexOf(array, value);

        public static void Sort<T>(this T[] array, Comparison<T> comparison) =>
            Array.Sort(array, comparison);
    }
}
