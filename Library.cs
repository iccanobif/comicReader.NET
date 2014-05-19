using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace comicReader.NET
{
    public class Library
    {
        SQLiteConnection conn = new SQLiteConnection("Data Source=comicLibrary.db;Version=3");

        public Library()
        {
            conn.Open();
            InitializeDb();
        }
        
        private void InitializeDb()
        {
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS COMICS (PATH, TITLE)";
                cmd.ExecuteNonQuery();
            }
        }

        public void AddComic(Comic c)
        {
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO COMICS (PATH, TITLE) VALUES (@path, @title)";
                cmd.Parameters.AddWithValue("@path", c.Path);
                cmd.Parameters.AddWithValue("@title", c.Title);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Comic> GetComicList()
        {
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT ROWID, PATH, TITLE FROM COMICS";
                SQLiteDataReader reader = cmd.ExecuteReader();

                List<Comic> output = new List<Comic>();

                while (reader.Read())
                {
                    Comic c = new Comic();
                    c.Id = (long)(reader["ROWID"]);
                    c.Path = reader["PATH"].ToString();
                    c.Title = reader["TITLE"].ToString();
                    output.Add(c);
                }

                return output;
            }
        }

        public Comic GetComic(long id)
        {
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT PATH, TITLE FROM COMICS WHERE ROWID = @id";
                cmd.Parameters.AddWithValue("@id", id);
                SQLiteDataReader reader = cmd.ExecuteReader();
                Comic c = new Comic();
                reader.Read();
                c.Id = id;
                c.Path = reader["PATH"].ToString();
                c.Title = reader["TITLE"].ToString();

                return c;
            }
        }


    }
}
