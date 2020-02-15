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

        public void Execute(object parameter)
        {
            if (Navigator.Instance.WhereWeAre == "novelend")
            {
                ViewModelLocator.Instance.NovelAddNoteVM.IDAfter = DBManager.Instance.GetEndID();
            }
            else
            {
                throw new NotImplementedException();
            }

            ViewModelLocator.Instance.NovelAddNoteVM.Models = DBManager.Instance.GetSurrounded(ViewModelLocator.Instance.NovelAddNoteVM.IDAfter);

            Navigator.Instance.Goto("addnote");
        }
    }
}
