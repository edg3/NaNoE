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
        
        public void Execute(object parameter)
        {
            var where = parameter.ToString();
            var end = DBManager.Instance.GetEndID();

            if (where == "-1")
            {
                DBManager.Instance.InsertChapter(end);
                var id = DBManager.Instance.GetMaxId("elements");
                if (end != 0)
                {
                    DBManager.Instance.ExecSQLNonQuery("UPDATE elements " +
                                       "SET idafter = " + end + " " +
                                       "WHERE rowid = " + id);
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            Navigator.Instance.Goto("novelend");
        }
    }
}

