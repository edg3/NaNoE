using NaNoE.V2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaNoE.V2.Data
{
    /// <summary>
    /// Locator for finding ViewModels
    /// </summary>
    class ViewModelLocator
    {
        /// <summary>
        /// Initiate the static instance of the Locator
        /// </summary>
        public ViewModelLocator()
        {
            _instance = this;
        }

        /// <summary>
        /// Static reference for the view model locator
        /// </summary>
        private static ViewModelLocator _instance;
        public static ViewModelLocator Instance
        {
            get { return _instance; }
            set
            {
                if (null != _instance) throw new Exception("ViewModelLocator Instance is already set.");
                _instance = value;
            }
        }

        /// <summary>
        /// Instances of all ViewModels we are using
        /// </summary>
        private StartViewModel _startVM = new StartViewModel();
        public StartViewModel StartVM
        {
            get { return _startVM; }
        }
    }
}
