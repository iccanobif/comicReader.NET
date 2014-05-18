using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace comicReader.NET
{
    class Library
    {
        SQLiteConnection conn = new SQLiteConnection("Data Source=comicLibrary.db;Version=3");

        public Library()
        {
            conn.Open();
            InitializeDb();
        }
        
        private void InitializeDb()
        {
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS COMICS (ID, PATH, TITLE)";
            cmd.ExecuteNonQuery();
        }


        
    }
}
