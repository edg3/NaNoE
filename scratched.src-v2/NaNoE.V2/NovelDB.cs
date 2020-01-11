using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using NaNoE.V2.NovelElements;

namespace NaNoE.V2
{
    static class NovelDB
    {
        /*
         Novel
          - Element
            - id
            - idbefore
            - idafter
            - type
            - externalid
         Paragraph
            - id
            - content
         Chapter
            - id
         */
        private static SQLiteConnection _connection;
        public static bool Connected { get { return _connection != null; } }
        public static void Load(string file)
        {
            if (_connection != null)
            {
                _connection.Close();
            }

            _connection = new SQLiteConnection("Data Source=" + file);
            _connection.Open();
        }

        public static void Create(string file)
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

        private static void CreateTables()
        {
            string elementTableCreate = "CREATE TABLE elements (id int identity(1,1), idbefore int, idafter int, type int(1), externalid int)";
            ExecSQLNonQuery(elementTableCreate);

            string paragraphTableCreate = "CREATE TABLE paragraphs (id int identity(1,1), content text)";
            ExecSQLNonQuery(paragraphTableCreate);

            string chapterTableCreate = "CREATE TABLE chapters (id int identity(1,1))";
            ExecSQLNonQuery(chapterTableCreate);
        }

        private static void ExecSQLNonQuery(string sql)
        {
            SQLiteCommand cmd = new SQLiteCommand(sql, _connection);
            cmd.ExecuteNonQuery();
        }

        private static List<string[]> ExecSQLQuery(string sql, int answerSize)
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

        private static int _visID = 0;
        public static int VisID { get { return _visID; } }

        public static List<INovelElement> GetView()
        {
            List<INovelElement> answer = new List<INovelElement>();

            List<string[]> data = ExecSQLQuery("SELECT id, idbefore, idafter, type, externalid FROM elements",5);

            for (int i = 0; i < data.Count; i++)
            {
                if ((data[i])[3] == "0")
                {
                    // Chapter
                    answer.Add(new ChapterElement());
                }
                else
                {
                    // Paragraph
                    var content = ExecSQLQuery("SELECT content FROM paragraphs WHERE id = " + (data[i])[4], 1);
                    answer.Add(new ParagraphElement(int.Parse((data[i])[4]), (content[0])[0]));
                }
            }

            return answer;
        }
    }
}
