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

            Connection = new SQLiteConnection(fileName);
        }
    }
}
