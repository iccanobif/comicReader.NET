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

    public class ArchiveReader
    {
        static Regex allowedExtensions = new Regex(@"\.(jpg|jpeg|png)$", RegexOptions.IgnoreCase);

        string path;
        public List<string> FileNames;
        public List<string> ParentCollections;
        public List<string> SiblingCollections;
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
            {
                using (SevenZipExtractor ex = new SevenZipExtractor(path))
                    FileNames = ex.ArchiveFileNames.ToList<string>();
                SiblingCollections = new List<string>();
            }
            else
            {
                FileNames = Directory.GetFiles(path).ToList<string>();
                SiblingCollections = Directory.GetDirectories(path).ToList<string>();
                SiblingCollections.AddRange(Directory.GetFiles(path, "*.zip"));
            }

            FileNames = (from names in FileNames
                         where allowedExtensions.IsMatch(names.ToString())
                         orderby names
                         select names).ToList<string>();

            FileNames.Sort(new NaturalComparer());

            // Load parent names
            if (isArchive)
                ParentCollections = Directory.GetFiles(Path.GetDirectoryName(path), "*.zip").ToList<string>();
            else
                if (Directory.GetDirectoryRoot(path) == path)
                    ParentCollections = new List<string>();
                else
                    ParentCollections = Directory.GetDirectories(Path.GetDirectoryName(path)).ToList<string>();

            ParentCollections.Sort(new NaturalComparer());
        }

        public string GetCurrentFileName()
        {
            return FileNames[CurrentPosition];
        }

        public byte[] GetCurrentFile()
        {
            if (!isArchive)
                return File.ReadAllBytes(FileNames[CurrentPosition]);

            using (SevenZipExtractor ex = new SevenZipExtractor(path))
            {
                MemoryStream str = new MemoryStream();
                ex.ExtractFile(FileNames[CurrentPosition], str);
                return str.ToArray();
            }
        }

        public byte[] GetNextFile()
        {
            CurrentPosition++;
            if (CurrentPosition >= FileNames.Count)
                CurrentPosition = 0;
            return GetCurrentFile();
        }

        public byte[] GetPreviousFile()
        {
            CurrentPosition--;
            if (CurrentPosition < 0)
                CurrentPosition = FileNames.Count - 1;
            return GetCurrentFile();
        }

        public void MoveToNextCollection()
        {
            for (int i = 0; i < ParentCollections.Count; i++)
                if (ParentCollections[i] == path)
                {
                    if (i >= ParentCollections.Count - 1)
                        path = ParentCollections[0];
                    else
                        path = ParentCollections[i + 1];
                    
                    break;
                }

            LoadFileList();
        }
    }
}
