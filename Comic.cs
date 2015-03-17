using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace comicReader.NET
{
    public class Comic
    {
        public string Title { get; set; }
        public string Path { get; set; }
        public int Position { get; set; }
        public string Id { get; set; }
        public DateTime CreationDate { get; set; }
        public double Zoom { get; set; }

        public Comic()
        {
            Id = Guid.NewGuid().ToString();
            Zoom = 1;
        }

        public ArchiveReader CreateArchiveReader()
        {
            ArchiveReader output = new ArchiveReader(Path, this);
            output.CurrentPosition = Position;

            return output;
        }
    }
}
