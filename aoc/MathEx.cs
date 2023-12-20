namespace aoc
{
    public static class MathEx
    {
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
