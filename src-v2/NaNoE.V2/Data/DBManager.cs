using NaNoE.V2.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NaNoE.V2.Data
{
    class DBManager
    {
        private static DBManager _instance;
        public static DBManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new DBManager();
                }

                return _instance;
            }
        }

        private DBManager()
        {
            _runAddChapter = new RunAddChapterCommand();
        }

        private SQLiteConnection _connection;
        public bool Connected { get { return _connection != null; } }
        public void Load(string file)
        {
            if (_connection != null)
            {
                _connection.Close();
            }

            _connection = new SQLiteConnection("Data Source=" + file);
            _connection.Open();

            GenerateMap();

            _position = _map.Count - 2;
            if (_position < 0)
            {
                _position = 0;
            }
        }

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

            _position = 0;
        }

        private void CreateTables()
        {
            // 0 - Chapter
            // 1 - Paragraph
            // 2 - Note
            // 3 - Bookmark

            string elementTableCreate = "CREATE TABLE elements (id int identity(1,1), idbefore int, idafter int, type int, externalid int)";
            ExecSQLNonQuery(elementTableCreate);

            string paragraphTableCreate = "CREATE TABLE paragraphs (id int identity(1,1), content text)";
            ExecSQLNonQuery(paragraphTableCreate);

            string noteTableCreate = "CREATE TABLE notes (id int identity(1,1), content text)";
            ExecSQLNonQuery(noteTableCreate);

            string bookmarkTableCreate = "CREATE TABLE bookmarks (id int identity(1,1), content text)";
            ExecSQLNonQuery(bookmarkTableCreate);
        }

        internal void ExecSQLNonQuery(string sql)
        {
            SQLiteCommand cmd = new SQLiteCommand(sql, _connection);
            cmd.ExecuteNonQuery();
        }

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

        internal List<IElement> GetElements()
        {
            var answer = new List<IElement>();

            List<int> prepare = new List<int>();
            for (int i = _position; i < _map.Count && i < _position + 3; i++)
            {
                prepare.Add(_map[_position]);
            }

            for (int i = 0; i < prepare.Count; i++)
            {
                var response = ExecSQLQuery("SELECT id, idbefore, idafter, type, externalid FROM elements elements WHERE id = " + prepare[i], 5);
                var what = (int)((response[i])[3]);
                switch (what)
                {
                    case 0: // Chapter
                        {
                            answer.Add(new ChapterElement((int)((response[i])[0])));
                        }
                        break;
                    case 1: // Paragraph
                        {
                            answer.Add(new ParagraphElement((int)((response[i])[0])));
                        }
                        break;
                    case 2: // Note
                        {
                            answer.Add(new NoteElement((int)((response[i])[0])));
                        }
                        break;
                    case 4: // Bookmark
                        {
                            answer.Add(new BookmarkElement((int)((response[i])[0])));
                        }
                        break;
                }
            }

            int max = -1;
            var findMax = ExecSQLQuery("SELECT MAX(id) FROM elements", 1);
            if (findMax.Count != 0)
            {
                max = (int)((findMax[0])[0]);
            }
            if (answer.Count == 0)
            {
                answer.Add(new WritingElement(-1));
            }
            else if (answer[answer.Count - 1].ID == max)
            {
                answer.Add(new WritingElement(-1));
            }

            return answer;
        }

        ICommand _runAddChapter;
        public ICommand RunAddChapter
        {
            get { return _runAddChapter; }
        }

        private List<int> _map;
        private void GenerateMap()
        {
            _map = new List<int>();

            var test = ExecSQLQuery("SELECT COUNT(id) FROM elements", 1);

            if (test.Count == 0)
            {
                return;
            }

            var response = ExecSQLQuery("SELECT id, afterid FROM elements", 2);

            if (response.Count > 0)
            {
                var item = response[0];
                response.Remove(item);
                _map.Add((int)(item[0]));

                while (response.Count > 0)
                {
                    item = (from element in response
                            where response[0] == item[1]
                            select element).First();
                    response.Remove(item);
                    _map.Add((int)(item[0]));
                }
            }
        }

        private int _position;
        public int Position { get { return _position; } }

        public void IncreasePosition()
        {
            if (_position < _map.Count - 3)
            {
                ++_position;
            }
        }
    }
}
