using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NaNoE.V2.Data
{
    internal class RunAddParagraphCommand : System.Windows.Input.ICommand
    {
        public string Content { get; internal set; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            DBManager.Instance.InsertParagraph((int)parameter, Content, false);
        }
    }
}
