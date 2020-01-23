using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaNoE.V2.ViewModels
{
    class ViewModelLocator
    {
        private static ViewModelLocator _instance;
        private EndViewModel _endViewModel;
        public EndViewModel EndViewModel { get { return _endViewModel; } }

        public static ViewModelLocator Instance
        {
            get { return _instance; }
        }

        public ViewModelLocator()
        {
            _instance = this;

            _endViewModel = new EndViewModel();
        }
    }
}
