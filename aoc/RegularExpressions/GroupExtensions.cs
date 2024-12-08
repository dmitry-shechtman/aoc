namespace System.Text.RegularExpressions
{
    public static class GroupExtensions
    {
        public static string[] GetStrings(this Group group) =>
            group.Captures.GetStrings();

        public static int[] GetInts(this Group group) =>
            group.Captures.GetInts();

        public static long[] GetLongs(this Group group) =>
            group.Captures.GetLongs();
    }
}
