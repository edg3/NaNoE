﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NaNoE.V2.Data
{
    class RunAddChapterCommand : System.Windows.Input.ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }
        
        public void Execute(object parameter)
        {
            // Where on map
            var pos = int.Parse(parameter.ToString());

            DBManager.Instance.InsertChapter(pos);

            Navigator.Instance.Goto("novelend");
        }
    }
}

