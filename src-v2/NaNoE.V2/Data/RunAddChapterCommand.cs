﻿using System;
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
            if (parameter.ToString() == "-1")
            {
                var where = DBManager.Instance.GetEndID();

                DBManager.Instance.InsertChapter(where + 1);
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
                throw new NotImplementedException();
            }

            Navigator.Instance.Goto("novelend");
        }
    }
}
