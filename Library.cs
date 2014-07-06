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
                cmd.CommandText = "CREATE TABLE IF NOT EXISTS COMICS (GBL_ID, PATH, TITLE, POSITION)";
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Add a new Comic object to the database and returns its Global Id
        /// </summary>
        /// <param name="c">The Comic object to insert to the database</param>
        /// <returns>The Global Id of the new comic</returns>
        public string SaveComic(Comic c)
        {
            int count;

            //TODO: Use SQlite's "INSERT OR REPLACE" statement
            using (SQLiteTransaction trans = conn.BeginTransaction())
            {
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.Transaction = trans;

                    //Check if row already exists

                    cmd.CommandText = "SELECT COUNT(1) FROM COMICS WHERE GBL_ID = @gbl_id";
                    cmd.Parameters.AddWithValue("@gbl_id", c.Id);
                    count = Convert.ToInt32(cmd.ExecuteScalar());

                }

                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.Transaction = trans;

                    if (count == 0)
                        cmd.CommandText = @"INSERT INTO COMICS (GBL_ID, PATH, TITLE, POSITION) 
                                                        VALUES (@gbl_id, @path, @title, @position)";
                    else
                        cmd.CommandText = @"UPDATE COMICS 
                                            SET PATH = @path, 
                                                TITLE = @title, 
                                                POSITION = @position
                                            WHERE GBL_ID = @gbl_id";
                    
                    cmd.Parameters.AddWithValue("@path", c.Path);
                    cmd.Parameters.AddWithValue("@title", c.Title);
                    cmd.Parameters.AddWithValue("@gbl_id", c.Id);
                    cmd.Parameters.AddWithValue("@position", c.Position);
                    cmd.ExecuteNonQuery();
                }

                trans.Commit();
            }
            return c.Id;
        }

        public void DeleteComic(string id)
        {
            using (SQLiteTransaction trans = conn.BeginTransaction())
            {
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.Transaction = trans;

                    cmd.CommandText = "DELETE FROM COMICS WHERE GBL_ID = @gbl_id; COMMIT;";
                    cmd.Parameters.AddWithValue("@gbl_id", id);
                    cmd.ExecuteNonQuery();

                    //trans.Commit();
                }
            }
        }

        public List<Comic> GetComicList()
        {
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT GBL_ID, PATH, TITLE FROM COMICS ORDER BY UPPER(TITLE)";
                SQLiteDataReader reader = cmd.ExecuteReader();

                List<Comic> output = new List<Comic>();

                while (reader.Read())
                {
                    Comic c = new Comic();
                    c.Id = reader["GBL_ID"].ToString();
                    c.Path = reader["PATH"].ToString();
                    c.Title = reader["TITLE"].ToString();
                    output.Add(c);
                }

                return output;
            }
        }

        [Serializable]
        public class NotExistingComicException : Exception
        {
            //public NotExistingComicException() { }
            //public NotExistingComicException(string message) : base(message) { }
            //public NotExistingComicException(string message, Exception inner) : base(message, inner) { }
            //protected NotExistingComicException(
            //  System.Runtime.Serialization.SerializationInfo info,
            //  System.Runtime.Serialization.StreamingContext context)
            //    : base(info, context) { }
        }

        public Comic GetComic(string id)
        {
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT PATH, TITLE, POSITION FROM COMICS WHERE GBL_ID = @id";
                cmd.Parameters.AddWithValue("@id", id);
                SQLiteDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    throw new NotExistingComicException();
                }

                Comic c = new Comic();
                reader.Read();
                c.Id = id;
                c.Path = reader["PATH"].ToString();
                c.Title = reader["TITLE"].ToString();
                c.Position = Convert.ToInt32(reader["POSITION"]);

                return c;
            }
        }


    }
}
