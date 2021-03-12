namespace CvOnlineCrawler
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    public class RegexFunctions
    {
        private static readonly RegexOptions RegexOptionsCompiled = RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase;

        public static string RegexToString(string content, string pattern)
        {
            return ReformRegexString(pattern, out int valueNumber).Match(content).Groups["Value0"].ToString();
        }

        public static string[] RegexToStringArray(string content, string pattern)
        {
            return ReformRegexString(pattern, out int valueNumber).Matches(content).Cast<Match>()
            .Select(m => m.Groups["Value0"].ToString())
            .ToArray();
        }

        public static string[][] RegexToMultiStringArray(string content, string pattern)
        {
            Regex regex = ReformRegexString(pattern, out int valueNumber);

            string[][] contentMultiArray = new string[regex.Matches(content).Count][];

            int matchNumber = 0;
            foreach (Match match in regex.Matches(content))
            {
                contentMultiArray[matchNumber] = new string[valueNumber];

                for (int i = 0; i < valueNumber; i++)
                {
                    contentMultiArray[matchNumber][i] = match.Groups[$"Value{i}"].ToString();
                }
                matchNumber += 1;
            }

            return contentMultiArray;
        }

        private static Regex ReformRegexString(string pattern, out int valueNumber)
        {
            List<int> openingBracketIndexes = new List<int>();

            openingBracketIndexes = Regex.Matches(pattern, @"\(").Cast<Match>().Select(match => match.Index).ToList();

            int searchIndex = 1;
            valueNumber = 0;

            foreach (int openingBracketIndex in openingBracketIndexes)
            {
                if (pattern[openingBracketIndex + searchIndex] != '\u003F')
                {
                    pattern = pattern.Insert(openingBracketIndex + searchIndex, $"?<Value{valueNumber}>");

                    searchIndex += 8 + valueNumber.ToString().Length;
                    valueNumber += 1;
                }
            }
            Regex regex = new Regex(pattern, RegexOptionsCompiled);

            return regex;
        }
    }
}
