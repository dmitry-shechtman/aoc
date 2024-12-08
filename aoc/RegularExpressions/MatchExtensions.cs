using System.Collections.Generic;

namespace System.Text.RegularExpressions
{
    public static class MatchExtensions
    {
        public static string[] GetStrings(this Match match) =>
            match.Groups.GetStrings();

        public static int[] GetInts(this Match match) =>
            match.Groups.GetInts();

        public static long[] GetLongs(this Match match) =>
            match.Groups.GetLongs();

        public static Dictionary<string, string[]> GetAllStrings(this Match match) =>
            match.Groups.GetAllStrings();

        public static Dictionary<string, int[]> GetAllInts(this Match match) =>
            match.Groups.GetAllInts();

        public static Dictionary<string, long[]> GetAllLongs(this Match match) =>
            match.Groups.GetAllLongs();
    }
}
