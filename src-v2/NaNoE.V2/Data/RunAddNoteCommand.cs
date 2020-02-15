﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NaNoE.V2.Data
{
    class RunAddNoteCommand : System.Windows.Input.ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (Navigator.Instance.WhereWeAre == "novelend")
            {
                // TODO - rebuild this kind of ID binding we use to fix it for end view for action
                ViewModelLocator.Instance.NovelAddNoteVM.IDAfter = DBManager.Instance.GetEndID();
                ViewModelLocator.Instance.NovelAddNoteVM.Models = DBManager.Instance.GetSurrounded(ViewModelLocator.Instance.NovelAddNoteVM.IDAfter);

                Navigator.Instance.Goto("addnote");
            }
            else if (Navigator.Instance.WhereWeAre == "addnote")
            {
                var vm = ViewModelLocator.Instance.NovelAddNoteVM;
                DBManager.Instance.InsertNote(vm.IDAfter, vm.Text);

                Navigator.Instance.GotoLast();
            }
            else
            {
                throw new NotImplementedException();
            }

        }
    }
}
