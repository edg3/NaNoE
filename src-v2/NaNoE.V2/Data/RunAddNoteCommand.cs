using System;
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

        // Bug - why is the first parameter in novelend view 'null'? It should be bound.
        public void Execute(object parameter)
        {
            try
            {
                ViewModelLocator.Instance.NovelAddNoteVM.IDAfter = int.Parse(parameter.ToString());
            }
            catch
            {
                ViewModelLocator.Instance.NovelAddNoteVM.IDAfter = 0;
            }

            if (Navigator.Instance.WhereWeAre == "novelend")
            {
                if (parameter == null || ViewModelLocator.Instance.NovelAddNoteVM.IDAfter == 0)
                {
                    ViewModelLocator.Instance.NovelAddNoteVM.IDAfter = int.Parse(DBManager.Instance.GetEndID().ToString());
                }
                else
                {
                    ViewModelLocator.Instance.NovelAddNoteVM.IDAfter = int.Parse(DBManager.Instance.UsingID);
                }
                ViewModelLocator.Instance.NovelAddNoteVM.Models = DBManager.Instance.GetSurrounded(ViewModelLocator.Instance.NovelAddNoteVM.IDAfter);
                
                Navigator.Instance.Goto("addnote");
            }
            else if (Navigator.Instance.WhereWeAre == "addnote")
            {
                var vm = ViewModelLocator.Instance.NovelAddNoteVM;
                DBManager.Instance.InsertNote(vm.IDAfter, vm.Text);

                Navigator.Instance.GotoLast();
            }
            else if (Navigator.Instance.WhereWeAre == "midnovel")
            {
                ViewModelLocator.Instance.NovelAddNoteVM.IDAfter = int.Parse(DBManager.Instance.UsingID);

                ViewModelLocator.Instance.NovelAddNoteVM.Models = DBManager.Instance.GetSurrounded(ViewModelLocator.Instance.NovelAddNoteVM.IDAfter);

                Navigator.Instance.Goto("addnote");
            }
            else
            {
                throw new NotImplementedException();
            }

        }
    }
}
