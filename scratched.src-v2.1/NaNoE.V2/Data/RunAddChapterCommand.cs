using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NaNoE.V2.Data
{
    class RunAddChapterCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }
        //string elementTableCreate = "CREATE TABLE elements (id int identity(1,1), idbefore int, idafter int, type int, externalid int)";
        public void Execute(object parameter)
        {
            if (parameter.ToString() == "-1")
            {
                var where = DBManager.Instance.GetEndID();

                DBManager.Instance.ExecSQLNonQuery("INSERT INTO elements (type, idafter, externalid) VALUES (0, " + (where + 1) + ", 0)");
                var tmp = DBManager.Instance.ExecSQLQuery("SELECT MAX(id) FROM elements", 1);
                var id = (int)(((tmp)[0])[0]);
                if (where != 0)
                {
                    DBManager.Instance.ExecSQLNonQuery("UPDATE elements " +
                                       "SET idafter = " + id + " " +
                                       "WHERE id = " + where);
                }
            }
            else
            {
                // Insert Element
                DBManager.Instance.ExecSQLNonQuery("INSERT INTO elements (type, idafter, externalid) VALUES (0, " + parameter.ToString() + ", 0)");
                var id = (int)(((DBManager.Instance.ExecSQLQuery("SELECT MAX(id) FROM elements", 1))[0])[0]);

                // Update Elements
                var afterId = (int)(((DBManager.Instance.ExecSQLQuery("SELECT idafter FROM elements WHERE id = " + parameter, 1))[0])[0]);
                DBManager.Instance.ExecSQLNonQuery("UPDATE elements " +
                                                   "SET idafter = " + id + " " +
                                                   "WHERE id = " + parameter);
                DBManager.Instance.ExecSQLNonQuery("UPDATE elements " +
                                                   "SET idbefore = " + id + " " +
                                                   "WHERE id = " + afterId);
            }
        }
    }
}
