using NaNoE.V2.Data;
using NaNoE.V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NaNoE.V2.ViewModels
{
    class NovelEndViewModel
    {
        /// <summary>
        /// Data to View
        /// </summary>
        public List<ModelBase> View
        {
            get { return DBManager.Instance.GetEnd(); }
        }

        /// <summary>
        /// The last ID in the map update by the View Model Locator
        /// </summary>
        public int EndPosition
        {
            get { return HelperVars.Position; }
        }

        /// <summary>
        /// Initiate the View Model
        /// </summary>
        public NovelEndViewModel()
        {
            
        }

        public ICommand NavUp
        {
            get { return CommandMap.Instance.RunNavigateUp; }
        }
    }
}
