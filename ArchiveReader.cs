using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace comicReader.NET
{
    class ArchiveReader
    {
        string path;
        
        string[] fileNames;

        public int CurrentPosition { get; set; }

        public ArchiveReader(string path)
        {
            this.path = path;
            CurrentPosition = 0;
            fileNames = Directory.GetFiles(path);
        }

        public byte[] GetCurrentFile() {
            return File.ReadAllBytes(fileNames[CurrentPosition]);
        }

        public byte[] GetNextFile()
        {
            CurrentPosition++;
            if (CurrentPosition >= fileNames.Length)
                CurrentPosition = 0;
            return GetCurrentFile();
        }

        public byte[] GetPreviousFile()
        {
            CurrentPosition--;
            if (CurrentPosition < 0)
                CurrentPosition = fileNames.Length - 1;
            return GetCurrentFile();
        }
    }
}
