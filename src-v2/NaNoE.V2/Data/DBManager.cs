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
        /// Map of the commands available
        /// </summary>
        private CommandMap _commands;
        public CommandMap Commands
        {
            get { return _commands; }
        }
        
        /// <summary>
        /// Remove novel Element
        /// </summary>
        /// <param name="parameter">ID to remove</param>
        internal void DeleteElement(int id)
        {
            // TODO
            var el = GetElement(id);
            // - get idbefore
            // - get idafter
            // - update idbefore and idafter on those 2 elements
            // - remove id
            //   - from db aswell
            // - refresh "_map"
            throw new NotImplementedException();
            // Note: perhaps we should also "track" these "kinds" of changes so we can "undo" a delete. Perhaps with a limit?
        }

        /// <summary>
        /// Instantiate
        /// </summary>
        private DBManager()
        {
            _commands = new CommandMap();
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

            int post = 0;
            for (int i = 0; i < _map.Count; i++)
            {
                if (_map[i] == where)
                {
                    post = i + 1;
                    break;
                }
            }
            _map.Insert(post, id);
        }

        /// <summary>
        /// Insert a chapter into the novel
        /// </summary>
        /// <param name="where">The ID before this position</param>
        public void InsertChapter(int where)
        {
            int idafter = 0;
            if (where != 0)
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
            ExecSQLNonQuery("INSERT INTO bookmarks (content)" +
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
            ExecSQLNonQuery("INSERT INTO notes (content)" +
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
            ExecSQLNonQuery("INSERT INTO paragraphs (content, flagged)" +
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

            List<object[]> elements = null;
            if (_map.Count == 1)
            {
                elements = ExecSQLQuery("SELECT rowid, idbefore, idafter, type, externalid FROM elements WHERE rowid = " + _map[0], 5);
            }
            else if (_map.Count > 1)
            {
                elements = ExecSQLQuery("SELECT rowid, idbefore, idafter, type, externalid FROM elements WHERE rowid = " + _map[_map.Count - 2], 5);
                
                var last = ExecSQLQuery("SELECT rowid, idbefore, idafter, type, externalid FROM elements WHERE rowid = " + _map[_map.Count - 1], 5);
                elements.Add(last[0]);
            }

            if (elements != null)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    switch ((elements[i])[3])
                    {
                        case 0: // Chapter
                            {
                                answer.Add(new ModelBase(
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
                                var paragraph = ExecSQLQuery("SELECT content, flagged FROM paragraphs WHERE rowid = " + int.Parse((elements[i])[4].ToString()), 2);
                                var content = (paragraph[0])[0].ToString();
                                var flagged = bool.Parse((paragraph[0])[1].ToString()) == true;
                                answer.Add(new ParagraphModel(
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
                                var note = ExecSQLQuery("SELECT content FROM notes WHERE rowid = " + int.Parse((elements[i])[4].ToString()), 1);
                                var content = (note[0])[0].ToString();
                                answer.Add(new NoteModel(
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
                                var bookmark = ExecSQLQuery("SELECT content FROM bookmarks WHERE rowid = " + int.Parse((elements[i])[4].ToString()), 1);
                                var content = (bookmark[0])[0].ToString();
                                answer.Add(new BookmarkModel(
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
            }
            else
            {
                elements = new List<object[]>();
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

            var response = ExecSQLQuery("SELECT rowid, idbefore FROM elements", 2);

            if (response.Count > 0)
            {
                var item = (from i in response
                            /* idbefore == 0 => start */
                            where int.Parse(i[1].ToString()) == 0
                            select i).First();
                response.Remove(item);
                _map.Insert(0, int.Parse(item[0].ToString()));

                while (response.Count > 0)
                {
                    item = (from i in response
                                /* idbefore = end of _map */
                            where int.Parse(i[1].ToString()) == int.Parse(item[0].ToString())
                            select i).First();
                    response.Remove(item);
                    _map.Add(int.Parse(item[0].ToString()));
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
            if (_connection == null) return 0;

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

        internal ModelBase GetID(int id)
        {
            if (_map.Count > 0)
            {
                return GetModel(id);
            }

            return null;
        }

        private ModelBase GetModel(int id)
        {
            if (id == 0) return null;

            var el = ExecSQLQuery("SELECT rowid, idbefore, idafter, type, externalid FROM elements WHERE rowid = " + id, 5);
            ModelBase answer = null;
            switch ((el[0])[3])
            {
                case 1:
                    {
                        var paragraph = ExecSQLQuery("SELECT content, flagged FROM paragraphs WHERE rowid = " + int.Parse((el[0])[4].ToString()), 2);
                        var content = (paragraph[0])[0].ToString();
                        var flagged = bool.Parse((paragraph[0])[1].ToString()) == true;
                        answer = new ParagraphModel(
                                int.Parse((el[0])[0].ToString()),
                                int.Parse((el[0])[1].ToString()),
                                int.Parse((el[0])[2].ToString()),
                                int.Parse((el[0])[3].ToString()),
                                int.Parse((el[0])[4].ToString()),
                                content,
                                flagged
                            );
                    } break;
                case 2:
                    {
                        var note = ExecSQLQuery("SELECT content FROM notes WHERE rowid = " + int.Parse((el[0])[4].ToString()), 1);
                        var content = (note[0])[0].ToString();
                        answer = new NoteModel(
                                int.Parse((el[0])[0].ToString()),
                                int.Parse((el[0])[1].ToString()),
                                int.Parse((el[0])[2].ToString()),
                                int.Parse((el[0])[3].ToString()),
                                int.Parse((el[0])[4].ToString()),
                                content
                            );
                    } break;
                case 3:
                    {
                        var bookmark = ExecSQLQuery("SELECT content FROM bookmarks WHERE rowid = " + int.Parse((el[0])[4].ToString()), 1);
                        var content = (bookmark[0])[0].ToString();
                        answer = new BookmarkModel(
                                int.Parse((el[0])[0].ToString()),
                                int.Parse((el[0])[1].ToString()),
                                int.Parse((el[0])[2].ToString()),
                                int.Parse((el[0])[3].ToString()),
                                int.Parse((el[0])[4].ToString()),
                                content
                            );
                    } break;
                default:
                    {
                        answer = new ModelBase(
                                int.Parse((el[0])[0].ToString()),
                                int.Parse((el[0])[1].ToString()),
                                int.Parse((el[0])[2].ToString()),
                                int.Parse((el[0])[3].ToString()),
                                0
                            );
                    }
                    break;
            }

            return answer;
        }

        /// <summary>
        /// Retrieve elements at a position and after
        /// </summary>
        /// <param name="id">The position id</param>
        /// <returns>Element with id, and the element from idafter</returns>
        internal List<ModelBase> GetSurrounded(int id)
        {
            List<ModelBase> answer = new List<ModelBase>();

            var element1 = GetID(id);
            if (element1 != null)
            {
                answer.Add(element1);
            }

            answer.Add(new WritingModel(id, "", false));

            if (element1 != null)
            {
                if (element1.IDAfter != 0)
                {
                    var element2 = GetID(element1.IDAfter);
                    if (element2 != null)
                    {
                        answer.Add(element2);
                    }
                }
            }

            return answer;
        }

        /// <summary>
        /// Using the map system to retrieve 3 elements in a row from the last element up
        /// </summary>
        /// <param name="mapPos">The last element id.</param>
        /// <returns>A list of the elements above a position, limit 3</returns>
        internal List<ModelBase> GetMapElements(int mapPos)
        {
            List<ModelBase> answer = new List<ModelBase>();

            for (int i = mapPos; i >= 0 && i > mapPos -3; i--)
            {
                if (answer.Count == 0)
                {
                    answer.Add(DBManager.Instance.GetModel(_map[i]));
                }
                else if (answer[0].IDBefore != 0)
                {
                    answer.Insert(0, DBManager.Instance.GetModel(answer[0].IDBefore));
                }
            }

            return answer;
        }

        /// <summary>
        /// Get the IDBefore from the element
        /// </summary>
        /// <param name="v">The id we want the idbefore of</param>
        /// <returns></returns>
        internal int GetPreviousID(int v)
        {
            var current = GetModel(v);
            return current.IDBefore;
        }

        private ModelBase GetElement(int id)
        {
            ModelBase answer = null;
            var elements = ExecSQLQuery("SELECT rowid, idbefore, idafter, type, externalid FROM elements WHERE rowid = " + _map[0], 5);

            switch ((elements[0])[3])
            {
                case 0: // Chapter
                    {
                        answer = new ModelBase(
                                int.Parse((elements[0])[0].ToString()),
                                int.Parse((elements[0])[1].ToString()),
                                int.Parse((elements[0])[2].ToString()),
                                int.Parse((elements[0])[3].ToString()),
                                0
                            );
                    }
                    break;
                case 1: // Paragraph
                    {
                        var paragraph = ExecSQLQuery("SELECT content, flagged FROM paragraphs WHERE rowid = " + int.Parse((elements[0])[4].ToString()), 2);
                        var content = (paragraph[0])[0].ToString();
                        var flagged = bool.Parse((paragraph[0])[1].ToString()) == true;
                        answer = new ParagraphModel(
                                int.Parse((elements[0])[0].ToString()),
                                int.Parse((elements[0])[1].ToString()),
                                int.Parse((elements[0])[2].ToString()),
                                int.Parse((elements[0])[3].ToString()),
                                int.Parse((elements[0])[4].ToString()),
                                content,
                                flagged
                            );
                    }
                    break;
                case 2: // Note
                    {
                        var note = ExecSQLQuery("SELECT content FROM notes WHERE rowid = " + int.Parse((elements[0])[4].ToString()), 1);
                        var content = (note[0])[0].ToString();
                        answer = new NoteModel(
                                int.Parse((elements[0])[0].ToString()),
                                int.Parse((elements[0])[1].ToString()),
                                int.Parse((elements[0])[2].ToString()),
                                int.Parse((elements[0])[3].ToString()),
                                int.Parse((elements[0])[4].ToString()),
                                content
                            );
                    }
                    break;
                case 3: // Bookmark
                    {
                        var bookmark = ExecSQLQuery("SELECT content FROM bookmarks WHERE rowid = " + int.Parse((elements[0])[4].ToString()), 1);
                        var content = (bookmark[0])[0].ToString();
                        answer = new BookmarkModel(
                                int.Parse((elements[0])[0].ToString()),
                                int.Parse((elements[0])[1].ToString()),
                                int.Parse((elements[0])[2].ToString()),
                                int.Parse((elements[0])[3].ToString()),
                                int.Parse((elements[0])[4].ToString()),
                                content
                            );
                    }
                    break;
            }

            return answer;
        }

        /// <summary>
        /// Run map of elements again
        ///  - TODO: this may be slow eventually?
        ///  - made as function as we may need adjustments
        /// </summary>
        internal void RunMap()
        {
            GenerateMap();
        }
    }
}
