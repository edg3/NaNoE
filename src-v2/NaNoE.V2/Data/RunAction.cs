using NaNoE.V2.ViewModels.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NaNoE.V2.Data
{
    class RunAction : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewAddAction action = new ViewAddAction();
            action.Show();

            if (ViewModelLocator.Instance.ViewAddActionVM.Action != ViewAddActionViewModel.ActionRan.False)
            {
                switch (ViewModelLocator.Instance.ViewAddActionVM.ActionNavigate)
                {
                    case "novelend":
                        Navigator.Instance.Goto("novelend");
                        break;
                    default: throw new NotImplementedException();
                }
            }
        }
    }
}
