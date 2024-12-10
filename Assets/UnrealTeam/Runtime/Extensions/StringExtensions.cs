using System.Text.RegularExpressions;

namespace UnrealTeam.Common.Extensions
{
    public static class StringExtensions
    {
        public static string SplitByUpperCase(this string str, string separator = " ")
        {
            var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

            return r.Replace(str, separator);
        }
    }
}