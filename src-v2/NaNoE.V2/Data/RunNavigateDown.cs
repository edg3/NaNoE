using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NaNoE.V2.Data
{
    class RunNavigateDown : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (ViewModelLocator.Instance.MidNovelVM.Position == DBManager.Instance.GetEndMapPosition())
            {
                Navigator.Instance.Goto("novelend");
            }
            else
            {
                ++ViewModelLocator.Instance.MidNovelVM.Position;
                ViewModelLocator.Instance.MidNovelVM.Refresh();
                Navigator.Instance.Goto("midnovel");
            }
        }
    }
}
