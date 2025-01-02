using System.Collections.Generic;
using System.Linq;

namespace aoc
{
    public static class MathEx
    {
        public static int Median(this int[] array)
        {
            array.Sort();
            return (array[array.Length / 2] + array[(array.Length + 1) / 2]) / 2;
        }

        public static int Median(this IEnumerable<int> input) =>
            Median(input.ToArray());

        public static int Gcd(int a, int b) =>
            b == 0 ? a : Gcd(b, a % b);

        public static long Gcd(long a, long b) =>
            b == 0 ? a : Gcd(b, a % b);

        public static int Lcm(int a, int b) =>
            a / Gcd(a, b) * b;

        public static long Lcm(long a, long b) =>
            a / Gcd(a, b) * b;
    }
}
