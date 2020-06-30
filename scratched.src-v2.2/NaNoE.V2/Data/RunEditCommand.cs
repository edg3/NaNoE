using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NaNoE.V2.Data
{
    class RunEditCommand : System.Windows.Input.ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            // TODO:
            //  - make edit VM
            //    - ID
            //    - content
            //  - make view, enter submits, or goe "back"
            //  - use similar style as the other "adds" just bind to pre-existing data
            throw new NotImplementedException();
        }
    }
}
