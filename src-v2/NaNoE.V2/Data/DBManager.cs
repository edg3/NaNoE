using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private DBManager() { }

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
            string elementTableCreate = "CREATE TABLE elements (id int identity(1,1), idbefore int, idafter int, type int(1), externalid int)";
            ExecSQLNonQuery(elementTableCreate);

            string paragraphTableCreate = "CREATE TABLE paragraphs (id int identity(1,1), content text)";
            ExecSQLNonQuery(paragraphTableCreate);

            string chapterTableCreate = "CREATE TABLE chapters (id int identity(1,1))";
            ExecSQLNonQuery(chapterTableCreate);
        }

        private void ExecSQLNonQuery(string sql)
        {
            SQLiteCommand cmd = new SQLiteCommand(sql, _connection);
            cmd.ExecuteNonQuery();
        }

        private List<string[]> ExecSQLQuery(string sql, int answerSize)
        {
            SQLiteCommand cmd = new SQLiteCommand(sql, _connection);
            List<string[]> returns = new List<string[]>();
            string[] line;
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    line = new string[answerSize];
                    for (int i = 0; i < answerSize; i++)
                    {
                        line[i] = reader.GetString(i);
                    }
                    returns.Add(line);
                }
            }

            return returns;
        }
    }
}
