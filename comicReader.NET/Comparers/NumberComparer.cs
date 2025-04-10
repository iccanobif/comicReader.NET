using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace comicReader.NET
{
    public class NumberComparer: IComparer<string>
    {
        private bool IsNumber(string s)
        {
            return Regex.IsMatch(s, @"(\d|\.)*");
        }

        private double ExtractChapterNumber(string filename)
        {
            string[] splitted1 = (from s in Regex.Split(filename, @"([0-9]+|\.)") //split by groups of numeric characters and periods
                                  where !string.IsNullOrWhiteSpace(s) && s != "." //the idea here is to ignore spaces and periods, so that, for example, xxx15xx < xx15.5xx where x's are non numeric characters
                                  && IsNumber(s)
                                  select Regex.Replace(s.ToUpper(), @"[\[\]\(\)\-_ ]", "") //clean characters i'd rather ignore in the comparison (whitespace, dashes, underscores...)
                                  ).ToArray<string>();

               

            return 0;
        }


        public int Compare(string s1, string s2)
        {
            double n1 = ExtractChapterNumber(s1);
            double n2 = ExtractChapterNumber(s2);
            if (n1 > n2) return 1;
            if (n1 < n2) return -1;
            return 0;
        }
    }
}
