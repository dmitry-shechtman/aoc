using System;

namespace aoc
{
    public static class ArrayExtensions
    {
        public static int IndexOf<T>(this T[] array, T value) =>
            Array.IndexOf(array, value);

        public static int LastIndexOf<T>(this T[] array, T value) =>
            Array.LastIndexOf(array, value);
    }
}
