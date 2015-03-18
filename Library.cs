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
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS COMICS (
                                        GBL_ID, 
                                        PATH, 
                                        TITLE, 
                                        POSITION, 
                                        CREATION_DATE, 
                                        LAST_EDIT_DATE,
                                        ZOOM)";
                cmd.ExecuteNonQuery();

                /*
                 * OPERATION_TYPE values:
                 * I: Insert
                 * D: Delete
                 * U: Update
                 */

                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS COMICS_LOG (
                                        GBL_ID, 
                                        PATH, 
                                        TITLE, 
                                        POSITION, 
                                        CREATION_DATE, 
                                        OPERATION_TYPE, 
                                        OPERATION_DATE, 
                                        ZOOM)";
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
                    {
                        cmd.CommandText = @"INSERT INTO COMICS (GBL_ID, 
                                                                PATH, 
                                                                TITLE, 
                                                                POSITION, 
                                                                CREATION_DATE, 
                                                                LAST_EDIT_DATE,
                                                                ZOOM) 
                                                        VALUES (@gbl_id, 
                                                                @path, 
                                                                @title, 
                                                                @position, 
                                                                @creation_date, 
                                                                @creation_date,
                                                                @zoom)";

                        cmd.Parameters.AddWithValue("@creation_date", DateTime.Now);
                    }
                    else
                        //TODO: don't insert a new log if the most recent one points to the same path
                        cmd.CommandText = @"INSERT INTO COMICS_LOG (
                                                   GBL_ID, 
                                                   PATH, 
                                                   TITLE, 
                                                   POSITION, 
                                                   CREATION_DATE, 
                                                   OPERATION_TYPE, 
                                                   OPERATION_DATE, 
                                                   ZOOM)
                                            SELECT GBL_ID, 
                                                   PATH, 
                                                   TITLE, 
                                                   POSITION, 
                                                   CREATION_DATE, 
                                                   'U', 
                                                   @operation_date,
                                                   ZOOM
                                            FROM COMICS WHERE GBL_ID = @gbl_id;

                                             UPDATE COMICS 
                                                SET PATH = @path, 
                                                    TITLE = @title, 
                                                    POSITION = @position,
                                                    LAST_EDIT_DATE = @operation_date,
                                                    ZOOM = @zoom
                                                WHERE GBL_ID = @gbl_id";

                    cmd.Parameters.AddWithValue("@operation_date", DateTime.Now);
                    cmd.Parameters.AddWithValue("@path", c.Path);
                    cmd.Parameters.AddWithValue("@title", c.Title);
                    cmd.Parameters.AddWithValue("@gbl_id", c.Id);
                    cmd.Parameters.AddWithValue("@position", c.Position);
                    cmd.Parameters.AddWithValue("@zoom", c.Zoom);
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

                    cmd.CommandText = @"INSERT INTO COMICS_LOG (
                                                    GBL_ID, 
                                                    PATH, 
                                                    TITLE, 
                                                    POSITION, 
                                                    CREATION_DATE, 
                                                    OPERATION_TYPE, 
                                                    OPERATION_DATE,
                                                    ZOOM)
                                                SELECT GBL_ID, 
                                                    PATH, 
                                                    TITLE, 
                                                    POSITION, 
                                                    CREATION_DATE, 
                                                    'D', 
                                                    @operation_date, 
                                                    ZOOM
                                                FROM COMICS 
                                                WHERE GBL_ID = @gbl_id; ";
                    cmd.Parameters.AddWithValue("@gbl_id", id);
                    cmd.Parameters.AddWithValue("@operation_date", DateTime.Now);

                    cmd.ExecuteNonQuery();
                }

                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    cmd.Transaction = trans;

                    cmd.CommandText = "DELETE FROM COMICS WHERE GBL_ID = @gbl_id;";
                    cmd.Parameters.AddWithValue("@gbl_id", id);
                    cmd.ExecuteNonQuery();
                }

                trans.Commit();
            }
        }

        public List<Comic> GetComicList(string id = null)
        {
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"SELECT C.GBL_ID, 
                                           C.PATH, 
                                           C.TITLE, 
                                           C.CREATION_DATE,
                                           C.LAST_EDIT_DATE,
                                           C.ZOOM,
                                           C.POSITION
                                      FROM COMICS C
                                     WHERE GBL_ID = IFNULL(@id, GBL_ID)
                                  ORDER BY LAST_EDIT_DATE DESC, C.CREATION_DATE, UPPER(C.TITLE)";
                if (string.IsNullOrEmpty(id))
                    cmd.Parameters.AddWithValue("@id", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@id", id);

                SQLiteDataReader reader = cmd.ExecuteReader();

                List<Comic> output = new List<Comic>();

                while (reader.Read())
                {
                    Comic c = new Comic();
                    c.Id = reader["GBL_ID"].ToString();
                    c.Path = reader["PATH"].ToString();
                    c.Title = reader["TITLE"].ToString();
                    c.Position = Convert.ToInt32(reader["POSITION"]);
                    c.CreationDate = Convert.ToDateTime(reader["CREATION_DATE"]);
                    c.Zoom = Convert.ToDouble(reader["ZOOM"]);
                    c.Saved = true;
                    output.Add(c);
                }

                return output;
            }
        }

        [Serializable]
        public class NotExistingComicException : Exception
        {
        }

        public Comic GetComic(string id)
        {
            List<Comic> l = GetComicList(id);
            if (l.Count == 0)
                throw new NotExistingComicException();
            return l[0];
        }
    }
}
