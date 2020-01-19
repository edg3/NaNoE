using NaNoE.V2.Models;
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

            string elementTableCreate = "CREATE TABLE elements (id int identity(1,1), idbefore int, idafter int, type int, externalid int)";
            ExecSQLNonQuery(elementTableCreate);

            string paragraphTableCreate = "CREATE TABLE paragraphs (id int identity(1,1), content text, bool flagged)";
            ExecSQLNonQuery(paragraphTableCreate);

            string noteTableCreate = "CREATE TABLE notes (id int identity(1,1), content text)";
            ExecSQLNonQuery(noteTableCreate);

            string bookmarkTableCreate = "CREATE TABLE bookmarks (id int identity(1,1), content text)";
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
            string[] line;
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            line = new string[answerSize];
                            for (int i = 0; i < answerSize; i++)
                            {
                                line[i] = reader.GetString(i);
                            }
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
        private void InsertElement(int where, int type, int external)
        {
            int idafter = 0;
            if (where != 0)
            {
                var afteranswer = ExecSQLQuery("SELECT afterid FROM elements WHERE id = " + where, 1);
                idafter = int.Parse((afteranswer[0])[0].ToString());
            }

            ExecSQLNonQuery("INSERT INTO elements (idbefore, idafter, type, externalid)" +
                         " VALUES (" +
                            where + "," +
                            idafter + "," +
                            type + "," +
                            external +
                         ")");
            var answer = ExecSQLQuery("SELECT max(id) FROM elements", 1);
            var id = int.Parse((answer[0])[0].ToString());

            if (where != 0)
            {
                ExecSQLNonQuery("UPDATE elements SET idafter = " + id +
                                " WHERE id = " + where);
            }
            if (idafter != 0)
            {
                ExecSQLNonQuery("UPDATE elements SET idbefore = " + id +
                                " WHERE id = " + idafter);
            }
        }

        /// <summary>
        /// Insert a chapter into the novel
        /// </summary>
        /// <param name="where">The ID before this position</param>
        public void InsertChapter(int where)
        {
            InsertElement(where, 0, 0);
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
            var answer = ExecSQLQuery("SELECT max(id) FROM bookmarks", 1);
            var id = int.Parse((answer[0])[0].ToString());

            InsertElement(where, 3, id);
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
            var answer = ExecSQLQuery("SELECT max(id) FROM notes", 1);
            var id = int.Parse((answer[0])[0].ToString());

            InsertElement(where, 2, id);
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
            var answer = ExecSQLQuery("SELECT max(id) FROM paragraphs", 1);
            var id = int.Parse((answer[0])[0].ToString());

            InsertElement(where, 1, id);
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

        internal List<ModelBase> GetEnd()
        {
            // TODO: move this to position
            List<ModelBase> answer = new List<ModelBase>();

            var elements = ExecSQLQuery("SELECT id, idbefore, idafter, type, externalid FROM elements", 5);
            for (int i = 0; i < elements.Count; i++)
            {
                switch ((elements[i])[3])
                {
                    case 0: // Chapter
                        {
                            
                        }
                        break;
                    case 1: // Paragraph
                        {

                        }
                        break;
                    case 2: // Note
                        {

                        }
                        break;
                    case 3: // Bookmark
                        {

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

            answer.Add(new WritingModel(id));

            return answer;
        }
    }
}
