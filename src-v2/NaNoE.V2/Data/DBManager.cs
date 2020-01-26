﻿using NaNoE.V2.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaNoE.V2.Data
{
    /// <summary>
    /// DB connection manager
    /// </summary>
    class DBManager
    {
        /// <summary>
        /// Reference to the DB
        /// </summary>
        private static DBManager _instance;
        public static DBManager Instance
        {
            get
            {
                if (_instance == null) _instance = new DBManager();

                return _instance;
            }
        }

        /// <summary>
        /// Instantiate
        /// </summary>
        private DBManager()
        {
            
        }

        /// <summary>
        /// Sqlite Connection
        /// </summary>
        private SQLiteConnection _connection;

        /// <summary>
        /// DB Connected
        /// </summary>
        public bool Connected { get { return _connection != null; } }

        /// <summary>
        /// Load a DB
        /// </summary>
        /// <param name="file">The .sqlite file to load</param>
        public void Load(string file)
        {
            if (_connection != null)
            {
                _connection.Close();
            }

            _connection = new SQLiteConnection("Data Source=" + file);
            _connection.Open();

            GenerateMap();
        }

        /// <summary>
        /// Create a .sqlite file
        /// </summary>
        /// <param name="file">Create a .sqlite file</param>
        public void Create(string file)
        {
            if (_connection != null)
            {
                _connection.Close();
            }

            SQLiteConnection.CreateFile(file);

            _connection = new SQLiteConnection("Data Source=" + file);
            _connection.Open();

            CreateTables();

            GenerateMap();
        }

        /// <summary>
        /// Create database tables
        /// </summary>
        private void CreateTables()
        {
            // 0 - Chapter
            // 1 - Paragraph
            // 2 - Note
            // 3 - Bookmark

            string elementTableCreate = "CREATE TABLE elements (idbefore int, idafter int, type int, externalid int)";
            ExecSQLNonQuery(elementTableCreate);

            string paragraphTableCreate = "CREATE TABLE paragraphs (content text, flagged bool)";
            ExecSQLNonQuery(paragraphTableCreate);

            string noteTableCreate = "CREATE TABLE notes (content text)";
            ExecSQLNonQuery(noteTableCreate);

            string bookmarkTableCreate = "CREATE TABLE bookmarks (content text)";
            ExecSQLNonQuery(bookmarkTableCreate);
        }

        /// <summary>
        /// Execute SQL without return
        /// </summary>
        /// <param name="sql">SQL to execute</param>
        internal void ExecSQLNonQuery(string sql)
        {
            SQLiteCommand cmd = new SQLiteCommand(sql, _connection);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Execute SQL with return
        /// </summary>
        /// <param name="sql">SQL to execute</param>
        /// <param name="answerSize">Size of answers</param>
        /// <returns>List of object[size] answers</returns>
        internal List<object[]> ExecSQLQuery(string sql, int answerSize)
        {
            SQLiteCommand cmd = new SQLiteCommand(sql, _connection);
            List<object[]> returns = new List<object[]>();
            object[] line;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            line = new object[answerSize];
                            reader.GetValues(line);
                            returns.Add(line);
                        }
                        catch { }
                    }
                }
            }

            return returns;
        }

        /// <summary>
        /// Insert an element into the novel
        /// </summary>
        /// <param name="where">The ID where the element will be places</param>
        /// <param name="type">What the type of the element is (see CreateTables)</param>
        /// <param name="external">External element ID to link to</param>
        private void InsertElement(int where, int idafter, int type, int external)
        {
            ExecSQLNonQuery("INSERT INTO elements (idbefore, idafter, type, externalid)" +
                         " VALUES (" +
                            where + "," +
                            idafter + "," +
                            type + "," +
                            external +
                         ")");
            var id = GetMaxId("elements");

            if (where != 0)
            {
                ExecSQLNonQuery("UPDATE elements SET idafter = " + id +
                                " WHERE rowid = " + where);
            }
            if (idafter != 0)
            {
                ExecSQLNonQuery("UPDATE elements SET idbefore = " + id +
                                " WHERE rowid = " + idafter);
            }
        }

        /// <summary>
        /// Insert a chapter into the novel
        /// </summary>
        /// <param name="where">The ID before this position</param>
        public void InsertChapter(int where)
        {
            int idafter = 0;
            if (where != -1)
            {
                var after = ExecSQLQuery("SELECT idafter FROM elements WHERE rowid = " + where, 1);
                idafter = int.Parse((after[0])[0].ToString());
            }
            else
            {
                where = 0;
            }

            InsertElement(where, idafter, 0, 0);
        }

        /// <summary>
        /// Insert a bookmark into the novel
        /// </summary>
        /// <param name="where">The ID before this position</param>
        /// <param name="text">The label of the bookmark</param>
        public void InsertBookmark(int where, string text)
        {
            ExecSQLNonQuery("INSERT INTO bookmarks (text)" +
                            " VALUES ('" + ProcessText(text) + "')");
            var id = GetMaxId("bookmarks");

            InsertElement(where, 0, 3, id);
        }

        /// <summary>
        /// Insert a note into the novel
        /// </summary>
        /// <param name="where">The ID before this position</param>
        /// <param name="text">The note's contents</param>
        public void InsertNote(int where, string text)
        {
            ExecSQLNonQuery("INSERT INTO notes (text)" +
                            " VALUES ('" + ProcessText(text) + "')");
            var id = GetMaxId("notes");

            InsertElement(where, 0, 2, id);
        }

        /// <summary>
        /// Insert a paragraph into the novel
        /// </summary>
        /// <param name="where">The ID before this position</param>
        /// <param name="text">The paragraph text</param>
        /// <param name="flagged">Whether it should be ignored in Edits</param>
        public void InsertParagraph(int where, string text, bool flagged)
        {
            ExecSQLNonQuery("INSERT INTO paragraphs (text, flagged)" +
                            " VALUES ('" + ProcessText(text) + "', " + (flagged ? "True" : "False") + ")");
            var id = GetMaxId("paragraphs");

            InsertElement(where, 0, 1, id);
        }

        /// <summary>
        /// Fix ' characters for SQL
        /// </summary>
        /// <param name="text">Text to process</param>
        /// <returns>Text with double '</returns>
        private string ProcessText(string text)
        {
            return text.Replace("'", "''");
        }

        /// <summary>
        /// Retrieves the end of the novel
        /// </summary>
        /// <returns>End of Novel</returns>
        internal List<ModelBase> GetEnd()
        {
            // TODO: move this to position
            List<ModelBase> answer = new List<ModelBase>();

            var elements = ExecSQLQuery("SELECT rowid, idbefore, idafter, type, externalid FROM elements ORDER BY rowid DESC LIMIT 2", 5);
            for (int i = 0; i < elements.Count; i++)
            {
                switch ((elements[i])[3])
                {
                    case 0: // Chapter
                        {
                            answer.Insert(0, new ModelBase(
                                    int.Parse((elements[i])[0].ToString()),
                                    int.Parse((elements[i])[1].ToString()),
                                    int.Parse((elements[i])[2].ToString()),
                                    int.Parse((elements[i])[3].ToString()),
                                    0
                                ));
                        }
                        break;
                    case 1: // Paragraph
                        {
                            var paragraph = ExecSQLQuery("SELECT content, flagged FROM paragraphs WHERE id = " + int.Parse((elements[i])[3].ToString()), 2);
                            var content = (paragraph[0])[0].ToString();
                            var flagged = bool.Parse((paragraph[0])[1].ToString()) == true;
                            answer.Insert(0, new ParagraphModel(
                                    int.Parse((elements[i])[0].ToString()),
                                    int.Parse((elements[i])[1].ToString()),
                                    int.Parse((elements[i])[2].ToString()),
                                    int.Parse((elements[i])[3].ToString()),
                                    int.Parse((elements[i])[4].ToString()),
                                    content,
                                    flagged
                                ));
                        }
                        break;
                    case 2: // Note
                        {
                            var note = ExecSQLQuery("SELECT content FROM notes WHERE id = " + int.Parse((elements[i])[3].ToString()), 1);
                            var content = (note[0])[0].ToString();
                            answer.Insert(0, new NoteModel(
                                    int.Parse((elements[i])[0].ToString()),
                                    int.Parse((elements[i])[1].ToString()),
                                    int.Parse((elements[i])[2].ToString()),
                                    int.Parse((elements[i])[3].ToString()),
                                    int.Parse((elements[i])[4].ToString()),
                                    content
                                ));
                        }
                        break;
                    case 3: // Bookmark
                        {
                            var bookmark = ExecSQLQuery("SELECT content FROM bookmarks WHERE id = " + int.Parse((elements[i])[3].ToString()), 1);
                            var content = (bookmark[0])[0].ToString();
                            answer.Insert(0, new BookmarkModel(
                                    int.Parse((elements[i])[0].ToString()),
                                    int.Parse((elements[i])[1].ToString()),
                                    int.Parse((elements[i])[2].ToString()),
                                    int.Parse((elements[i])[3].ToString()),
                                    int.Parse((elements[i])[4].ToString()),
                                    content
                                ));
                        }
                        break;
                }
            }
            
            int id = 0;
            if (elements.Count > 0)
            {
                var item = elements[elements.Count - 1];
                id = int.Parse(item[0].ToString());
            }

            answer.Add(new WritingModel(id,"",false));

            return answer;
        }

        /// <summary>
        /// Initial map of elements in DB
        /// </summary>
        private List<int> _map;
        private void GenerateMap()
        {
            _map = new List<int>();

            var test = GetMaxId("elements");

            if (test == 0)
            {
                return;
            }

            var response = ExecSQLQuery("SELECT rowid, idbefore, idafter FROM elements", 3);

            if (response.Count > 0)
            {
                var item = (from i in response
                            where int.Parse(i[2].ToString()) == 0
                            select i).First();
                response.Remove(item);
                _map.Insert(0, int.Parse(item[0].ToString()));

                while (response.Count > 0)
                {
                    item = (from e in response
                            where e[0].ToString() == item[1].ToString()
                            select e).First();
                    response.Remove(item);
                    _map.Insert(0, int.Parse(item[0].ToString()));
                }
            }
        }

        /// <summary>
        /// View position
        /// </summary>
        private int _position;
        public int Position { get { return _position; } }

        /// <summary>
        /// Move position down 1
        /// </summary>
        public void IncreasePosition()
        {
            if (_position < _map.Count - 3)
            {
                ++_position;
            }
        }

        /// <summary>
        /// Get ID of last Element
        /// </summary>
        /// <returns>ID of last Element</returns>
        internal int GetEndID()
        {
            if (_map.Count == 0)
            {
                return 0;
            }

            return _map[_map.Count - 1];
        }

        /// <summary>
        /// Get the Max(rowid) From Table
        /// </summary>
        /// <param name="v">Table parameter</param>
        /// <returns>Max(ID) from Table</returns>
        internal int GetMaxId(string v)
        {
            var cmd = "SELECT Max(rowid) FROM " + v;
            SQLiteCommand sqlCmd = new SQLiteCommand(cmd, _connection);
            object val = sqlCmd.ExecuteScalar();
            if (val.ToString() == "") return 0;
            return int.Parse(val.ToString());
        }
        
        /// <summary>
        /// Get the position of an ID in the novel
        /// </summary>
        /// <returns>Where in the map this ID resides</returns>
        internal int GetMapPosition(int id)
        {
            return _map.FindIndex(a => a == id);
        }
    }
}
