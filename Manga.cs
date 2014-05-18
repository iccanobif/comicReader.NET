using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace comicReader.NET
{
    class Manga
    {
        public string title { get; set; }
        public string path { get; set; }

        private DateTime _creationTime;
        public DateTime creationTime
        {
            get { return _creationTime; }
        }

        public Manga()
        {
            _creationTime = DateTime.Now;
        }

    }
}
