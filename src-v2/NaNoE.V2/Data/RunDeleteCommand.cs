﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NaNoE.V2.Data
{
    class RunDeleteCommand : System.Windows.Input.ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var answer = MessageBox.Show("Are you sure you want to delete that element?\n\n" + DBManager.Instance.GetID((int)parameter), "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        }
    }
}
