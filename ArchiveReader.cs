using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SevenZip;
using System.Text.RegularExpressions;
//using SevenZip.Sdk;

namespace comicReader.NET
{
    class NaturalComparer : IComparer<string>
    {
        public int Compare(string s1, string s2)
        {
            return s1.CompareTo(s2);
        }
    }

    class ArchiveReader
    {
        static Regex allowedExtensions = new Regex(@"\.(jpg|jpeg|png)$", RegexOptions.IgnoreCase);

        string path;
        List<string> fileNames;
        public int CurrentPosition { get; set; }
        private bool isArchive;

        public ArchiveReader(string path)
        {
            this.path = path;
            LoadFileList();
        }

        private void LoadFileList()
        {
            CurrentPosition = 0;

            isArchive = !System.IO.File.GetAttributes(path).HasFlag(FileAttributes.Directory);

            if (isArchive)
                using (SevenZipExtractor ex = new SevenZipExtractor(path))
                    fileNames = ex.ArchiveFileNames.ToList<string>();
            else
                fileNames = Directory.GetFiles(path).ToList<string>();

            fileNames = (from names in fileNames
                         where allowedExtensions.IsMatch(names.ToString())
                         orderby names
                         select names).ToList<string>();

            fileNames.Sort(new NaturalComparer());
        }

        public string GetCurrentFileName()
        {
            return fileNames[CurrentPosition];
        }

        public byte[] GetCurrentFile()
        {
            if (!isArchive)
                return File.ReadAllBytes(fileNames[CurrentPosition]);

            using (SevenZipExtractor ex = new SevenZipExtractor(path))
            {
                MemoryStream str = new MemoryStream();
                ex.ExtractFile(fileNames[CurrentPosition], str);
                return str.ToArray();
            }
        }

        public byte[] GetNextFile()
        {
            CurrentPosition++;
            if (CurrentPosition >= fileNames.Count)
                CurrentPosition = 0;
            return GetCurrentFile();
        }

        public byte[] GetPreviousFile()
        {
            CurrentPosition--;
            if (CurrentPosition < 0)
                CurrentPosition = fileNames.Count - 1;
            return GetCurrentFile();
        }

        public void MoveToNextCollection()
        {
            Path.GetDirectoryName(path);
            string[] entries;

            if (isArchive)
                entries = Directory.GetFiles(Path.GetDirectoryName(path), "*.zip");
            else
                entries = Directory.GetDirectories(Path.GetDirectoryName(path));

            for (int i = 0; i < entries.Length; i++)
                if (entries[i] == path)
                {
                    if (i >= entries.Length - 1)
                        path = entries[0];
                    else
                        path = entries[i + 1];

                    break;
                }

            LoadFileList();

        }
    }
}
