using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace comicReader.NET
{
    public class NaturalComparer : IComparer<string>
    {
        public enum NaturalComparerMode
        {
            DirectoryNames,
            FileNames
        }

        NaturalComparerMode currentComparerMode;

        public NaturalComparer(NaturalComparerMode mode)
        {
            currentComparerMode = mode;
        }

        private bool IsNumber(string s)
        {
            return Regex.IsMatch(s, @"\d");
        }

        private bool IsNumber(char c)
        {
            return IsNumber(c.ToString());
        }

        public int Compare(string s1, string s2)
        {
            //string[] splitted1 = (from s in Regex.Split(Regex.Replace(s1.ToUpper(), @"[\.\-_ ]", ""), @"([0-9]+)") //split by groups of numeric characters and periods
            //                      where !string.IsNullOrWhiteSpace(s) && s != "." //the idea here is to ignore spaces and periods, so that, for example, xxx15xx < xx15.5xx where x's are non numeric characters
            //                      select s).ToArray<string>();

            //string[] splitted2 = (from s in Regex.Split(Regex.Replace(s2.ToUpper(), @"[\.\-_ ]", ""), @"([0-9]+)")
            //                      where !string.IsNullOrWhiteSpace(s) && s != "."
            //                      select s).ToArray<string>();

            string cleanedS1 = Regex.Replace(s1.ToUpper(), @"[\-_ ]", "");
            string cleanedS2 = Regex.Replace(s2.ToUpper(), @"[\-_ ]", "");

            string[] splitted1 = (from s in Regex.Split(cleanedS1, @"([0-9]+|\.)") //split by groups of numeric characters and periods
                                  where !string.IsNullOrWhiteSpace(s) && s != "." //the idea here is to ignore spaces and periods, so that, for example, xxx15xx < xx15.5xx where x's are non numeric characters
                                  select s).ToArray<string>();

            string[] splitted2 = (from s in Regex.Split(cleanedS2, @"([0-9]+|\.)")
                                  where !string.IsNullOrWhiteSpace(s) && s != "."
                                  select s).ToArray<string>();

            if (currentComparerMode == NaturalComparerMode.FileNames)
            {
                //If the strings I'm comparing are filenames (eg. "b\altan_Pagina_018.jpg" and "b\altan_Pagina_018b.jpg") I remove the extension, so that
                //in the above example 018 comes before 018b (longer names should come after shorter ones)
                if (splitted1.Length > 1)
                    splitted1[splitted1.Length - 1] = string.Empty;

                if (splitted2.Length > 1)
                    splitted2[splitted2.Length - 1] = string.Empty;
            }

            int i = 0;
            while (i < (splitted1.Length < splitted2.Length ? splitted1.Length : splitted2.Length))
            {
                if (IsNumber(splitted1[i]) && IsNumber(splitted2[i]))
                {
                    int n1 = int.Parse(splitted1[i]);
                    int n2 = int.Parse(splitted2[i]);
                    if (n1 != n2)
                        return n1 - n2;
                }
                else
                {
                    //this is to handle non integer chapter numbers (ex. xxx15xx < xx15.5xx), numeric blocks are considered greater than non-numeric ones
                    if (IsNumber(splitted1[i]))
                        return 1;
                    if (IsNumber(splitted2[i]))
                        return -1;

                    int compareResult = splitted1[i].CompareTo(splitted2[i]);
                    if (compareResult != 0)
                        return compareResult;
                }

                i++;
            }

            return splitted1.Length - splitted2.Length;
        }
    }

}
