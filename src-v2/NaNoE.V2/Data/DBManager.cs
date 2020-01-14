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

        internal List<IElement> GetElements(int v)
        {
            var answer = new List<IElement>();

            var response = ExecSQLQuery("SELECT * FROM elements elements ORDER BY id DESC LIMIT 3", 5);
            for (int i = response.Count - 1; i >= 0; i++)
            {
                var what = (int)((response[i])[3]);
                switch (what)
                {
                    case 0: // Chapter
                        {
                            answer.Add(new ChapterElement((int)((response[i])[0])));
                        } break;
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
    }
}
