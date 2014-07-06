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

        private DateTime _creationTime;
        public DateTime creationTime
        {
            get { return _creationTime; }
        }

        public Comic()
        {
            _creationTime = DateTime.Now;
            Id = Guid.NewGuid().ToString();
        }

        public ArchiveReader CreateArchiveReader()
        {
            ArchiveReader output = new ArchiveReader(Path, this);
            output.CurrentPosition = Position;

            return output;
        }
    }
}
