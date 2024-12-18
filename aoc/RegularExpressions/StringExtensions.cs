namespace System.Text.RegularExpressions
{
    internal static class StringExtensions
    {
        public static T ConvertTo<T>(this string value)
            where T : IConvertible =>
                (T)Convert.ChangeType(value, typeof(T));
    }
}
