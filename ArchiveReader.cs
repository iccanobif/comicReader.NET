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

    public class ArchiveReader
    {
        public enum CollectionMovementDirection
        {
            Forward,
            Backwards
        }

        static Regex allowedImageExtensions = new Regex(@"\.(jpg|jpeg|png|gif|bmp)$", RegexOptions.IgnoreCase);
        static Regex allowedArchiveExtensions = new Regex(@"\.(zip|rar|cbr|cbz|cbt|cba|cb7|7z)$", RegexOptions.IgnoreCase);

        Comic parentComic = null; //Can be null
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
            if (!Directory.Exists(path) && !File.Exists(path))
                throw new FileNotFoundException();

            this.currentPath = path;
            this.parentComic = parentComic;
            LoadFileList();

            if (parentComic != null)
                CurrentPosition = parentComic.Position;
        }

        public void SetParentComic(Comic parentComic) 
        {
            //This sucks, maybe Comic and ArchiveReader should have been a single class. ArchiveReader is too dependent on Comic, whenever i change something in an ArchiveReader,
            //i have to change it in the Comic too.
            //TODO: I could remove Comic.Path and Comic.Position and only use a new Comic.Archive of type ArchiveReader. In the GUI, I'll have only a currentComic object and when I open the
            //navigation window I'll just do a Comic.Archive = newArchiveReader;

            this.parentComic = parentComic;
            parentComic.Path = currentPath;
            parentComic.Position = CurrentPosition;
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
                SiblingCollections.AddRange((from names in Directory.GetFiles(currentPath)
                                             where allowedArchiveExtensions.IsMatch(names)
                                             select names).OrderBy(x => x, new NaturalComparer(NaturalComparer.NaturalComparerMode.FileNames)).ToList<string>());
            }

            FileNames = (from names in FileNames
                         where allowedImageExtensions.IsMatch(names)
                               && !names.StartsWith("__MACOSX")
                         //orderby names
                         select names).ToList<string>();
            FileNames.Sort(new NaturalComparer(NaturalComparer.NaturalComparerMode.FileNames));

            PopulateParentCollections();
        }

        private void PopulateParentCollections()
        {
            if (isArchive)
                //ParentCollections = Directory.GetFiles(Path.GetDirectoryName(currentPath), allowedArchiveExtensions).ToList<string>();
                ParentCollections = (from names in Directory.GetFiles(Path.GetDirectoryName(currentPath))
                                     where allowedArchiveExtensions.IsMatch(names)
                                     select names).ToList<string>();
            else
                if (Directory.GetDirectoryRoot(currentPath) == currentPath)
                    ParentCollections = new List<string>();
                else
                    ParentCollections = Directory.GetDirectories(Path.GetDirectoryName(currentPath)).ToList<string>();

            ParentCollections.Sort(new NaturalComparer(NaturalComparer.NaturalComparerMode.DirectoryNames));
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

        public void MoveToNextCollection(CollectionMovementDirection direction)
        {
            PopulateParentCollections();

            for (int i = 0; i < ParentCollections.Count; i++)
                if (ParentCollections[i] == currentPath)
                { //Current collection found, moving to the next one

                    if (direction == CollectionMovementDirection.Forward)
                    {
                        if (i >= ParentCollections.Count - 1)
                            currentPath = ParentCollections[0];
                        else
                            currentPath = ParentCollections[i + 1];
                    }
                    else
                    {
                        if (i == 0)
                            currentPath = ParentCollections[ParentCollections.Count - 1];
                        else
                            currentPath = ParentCollections[i - 1];
                    }

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
