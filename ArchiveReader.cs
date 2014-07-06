using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SevenZip;
using System.Text.RegularExpressions;
using System.Diagnostics;
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

        Comic parentComic = null; //Cam be null
        string currentPath;
        public string CurrentPath
        {
            get { return currentPath; }
        }
        public List<string> FileNames;
        public List<string> ParentCollections;
        public List<string> SiblingCollections;
        public int CurrentPosition { get; set; }
        private bool isArchive;

        public ArchiveReader(string path, Comic parentComic)
        {
            this.currentPath = path;
            this.parentComic = parentComic;
            LoadFileList();

            if (parentComic != null)
                CurrentPosition = parentComic.Position;
        }

        private void LoadFileList()
        {
            CurrentPosition = 0;

            isArchive = !System.IO.File.GetAttributes(currentPath).HasFlag(FileAttributes.Directory);

            if (isArchive)
            {
                using (SevenZipExtractor ex = new SevenZipExtractor(currentPath))
                    FileNames = ex.ArchiveFileNames.ToList<string>();
                SiblingCollections = new List<string>();
            }
            else
            {
                FileNames = (from p in Directory.GetFiles(currentPath)
                             select Path.GetFileName(p)).ToList<string>();

                SiblingCollections = Directory.GetDirectories(currentPath).ToList<string>();
                SiblingCollections.AddRange(Directory.GetFiles(currentPath, "*.zip"));
            }

            FileNames = (from names in FileNames
                         where allowedExtensions.IsMatch(names)
                               && !names.StartsWith("__MACOSX")
                         orderby names
                         select names).ToList<string>();

            FileNames.Sort(new NaturalComparer());

            PopulateParentCollections();
        }

        private void PopulateParentCollections()
        {
            if (isArchive)
                ParentCollections = Directory.GetFiles(Path.GetDirectoryName(currentPath), "*.zip").ToList<string>();
            else
                if (Directory.GetDirectoryRoot(currentPath) == currentPath)
                    ParentCollections = new List<string>();
                else
                    ParentCollections = Directory.GetDirectories(Path.GetDirectoryName(currentPath)).ToList<string>();

            ParentCollections.Sort(new NaturalComparer());
        }

        public string GetCurrentFileName()
        {
            return FileNames[CurrentPosition];
        }

        public byte[] GetCurrentFile()
        {
            DateTime start = DateTime.Now;
            byte[] output;

            if (!isArchive)
                output = File.ReadAllBytes(Path.Combine(currentPath, FileNames[CurrentPosition]));
            else
                using (SevenZipExtractor ex = new SevenZipExtractor(currentPath))
                {
                    MemoryStream str = new MemoryStream();
                    ex.ExtractFile(FileNames[CurrentPosition], str);
                    output = str.ToArray();
                }

            Debug.Print("File extraction time: " + DateTime.Now.Subtract(start).ToString());
            return output;
        }

        public byte[] GetNextFile()
        {
            CurrentPosition++;
            if (CurrentPosition >= FileNames.Count)
                CurrentPosition = 0;

            if (parentComic != null) 
                parentComic.Position = CurrentPosition;

            return GetCurrentFile();
        }

        public byte[] GetPreviousFile()
        {
            CurrentPosition--;
            if (CurrentPosition < 0)
                CurrentPosition = FileNames.Count - 1;

            if (parentComic != null) 
                parentComic.Position = CurrentPosition;

            return GetCurrentFile();
        }

        public void MoveToNextCollection()
        {
            PopulateParentCollections();

            for (int i = 0; i < ParentCollections.Count; i++)
                if (ParentCollections[i] == currentPath)
                {
                    if (i >= ParentCollections.Count - 1)
                        currentPath = ParentCollections[0];
                    else
                        currentPath = ParentCollections[i + 1];

                    break;
                }

            LoadFileList();

            if (parentComic != null)
            {
                parentComic.Path = currentPath;
                parentComic.Position = CurrentPosition;
            }
        }
    }
}
