using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaNoE.Objective
{
    class ObjectiveDB
    {
        public static SQLiteConnection Connection { get; private set; }
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

            Connection = new SQLiteConnection("Data Source=" + fileName +"; Version=3;");
            Connection.Open();

            Name = fileName;
        }

        public static void TestNew()
        {
            if (!RunCMD("SELECT name FROM sqlite_master WHERE type='table';").HasRows)
            {
                // Create tables
                RunCMD("CREATE TABLE notes (id int primary key, val varchar(600));");
                RunCMD("CREATE TABLE paragraphs (id int primary key, para varchar(100000))"); // <- may be low?
                RunCMD("CREATE TABLE helpers (id int primary key, name varchar(200))");
                RunCMD("CREATE TABLE plots (id int primary key, name varchar(200))");

                RunCMD("CREATE TABLE plotsjoint (id int primary key, plotid int, noteid int)");
                RunCMD("CREATE TABLE helpersjoint (id int primary key, helperid int, noteid int)");
            }
        }

        public static SQLiteDataReader RunCMD(string sql)
        {
            if (Connection == null) return null;

            SQLiteCommand cmd = new SQLiteCommand(sql, Connection);
            var reader = cmd.ExecuteReader();

            return reader;
        }
    }
}
