namespace System.Text.RegularExpressions
{
    internal static class StringExtensions
    {
        public static T ConvertTo<T>(this string value, IFormatProvider? provider)
            where T : IConvertible =>
                (T)Convert.ChangeType(value, typeof(T), provider);
    }
}
