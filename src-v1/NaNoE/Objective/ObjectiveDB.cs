using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NaNoE.Objective
{
    public class ObjectiveDB
    {
        public static SQLiteConnection Connection { get; private set; }
        public static ObjectiveDB ConnectionHelper { get; private set; }
        public static string Name { get; private set; }
        public ObjectiveDB(string fileName)
        {
            if (!File.Exists(fileName))
            {
                SQLiteConnection.CreateFile(fileName);
            }

            if (Connection != null)
            {
                Connection.Close();
            }

            Connection = new SQLiteConnection("Data Source=" + fileName + "; Version=3;");
            Connection.Open();

            Name = fileName;

            ConnectionHelper = this;
        }

        public static void TestNew()
        {
            if (!RunCMD("SELECT name FROM sqlite_master WHERE type='table';").HasRows)
            {
                // Create tables
                RunCMD("CREATE TABLE notes (id integer primary key AUTOINCREMENT, val varchar(600));");
                RunCMD("CREATE TABLE paragraphs (id integer primary key AUTOINCREMENT, para varchar(100000))"); // <- may be low?
                RunCMD("CREATE TABLE helpers (id integer primary key AUTOINCREMENT, name varchar(200))");
                RunCMD("CREATE TABLE plots (id integer primary key AUTOINCREMENT, name varchar(200))");

                RunCMD("CREATE TABLE plotsjoint (id integer primary key AUTOINCREMENT, plotid int, noteid int)");
                RunCMD("CREATE TABLE helpersjoint (id integer primary key AUTOINCREMENT, helperid int, noteid int)");

                DBCount();
            }
        }


        public static int WordCount { get; private set; }
        private static Thread threadUsed { get; set; }
        public static void DBCount()
        {
            try
            {
                CountWords(); // Error
            }
            catch { }
        }

        private static void CountWords()
        {
            WordCount = 0;
            var paras = RunCMD("SELECT para FROM paragraphs WHERE para != '[chapter]'");
            if (paras != null)
            {
                if (paras.HasRows)
                {
                    paras.Read();
                    do
                    {
                        var para = paras.GetString(0);
                        var split = para.Split(' ');
                        WordCount += split.Length;
                    }
                    while (paras.Read());
                }
            }

            threadUsed = null;
        }

        public static int GetParaID(string line)
        {
            if (line == "[chapter]") return -1;
            var para = RunCMD("SELECT id FROM paragraphs WHERE para = '" + StringReplacement(line) + "'");
            para.Read();
            return para.GetInt32(0);
        }

        public static string GetParaFromID(int i)
        {
            var para = RunCMD("SELECT para FROM paragraphs WHERE id=" + i);
            para.Read();
            return para.GetString(0);
        }

        public static SQLiteDataReader RunCMD(string sql)
        {
            if (Connection == null) return null;

            SQLiteCommand cmd = new SQLiteCommand(sql, Connection);
            var reader = cmd.ExecuteReader();

            return reader;
        }

        internal static int CountParagraphs()
        {
            var val = RunCMD("SELECT count(id) FROM paragraphs WHERE para != '[chapter]';");
            if (val != null)
            {
                val.Read();
                return val.GetInt32(0);
            }
            return -1;
        }

        internal static void UpdatePara(int i, string content)
        {
            RunCMD("UPDATE paragraphs SET para = '" + StringReplacement(content) + "' WHERE id = " + i.ToString());
        }

        internal static string StringReplacement(string s)
        {
            return s.Replace("'", "''");
        }

        internal static void DeleteParagraph(int i)
        {
            RunCMD("DELETE FROM paragraphs WHERE id = " + i + ";");
        }
    }
}
