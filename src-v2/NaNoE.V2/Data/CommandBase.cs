using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NaNoE.V2.Data
{
    public class CommandBase : ICommand
    {
        /// <summary>
        /// Command to execute
        /// </summary>
        private Action _action;
        public Action Act
        {
            get { return _action; }
            set { _action = value; }
        }
        
        public CommandBase(Action action)
        {
            _action = action;
        }

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// If this command can execute under current conditions
        /// </summary>
        /// <param name="parameter">The parameter to check</param>
        /// <returns>True</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Run the command assigned
        /// </summary>
        /// <param name="parameter">Unused</param>
        public void Execute(object parameter)
        {
            if (null != _action) _action();
        }
    }
}
