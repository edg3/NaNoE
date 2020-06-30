using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NaNoE.V2.Data
{
    class RunNavigateUp : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (!DBManager.Instance.MapOverThree()) return;

            if (parameter.ToString() == "-1")
            {
                ViewModelLocator.Instance.MidNovelVM.Position = DBManager.Instance.GetEndMapPosition();
            }
            else if (ViewModelLocator.Instance.MidNovelVM.Position > 2)
            {
                --ViewModelLocator.Instance.MidNovelVM.Position;
            }
            ViewModelLocator.Instance.MidNovelVM.Refresh();
            Navigator.Instance.Goto("midnovel");
        }
    }
}
