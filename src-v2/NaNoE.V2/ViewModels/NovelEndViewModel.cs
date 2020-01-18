using NaNoE.V2.Data;
using NaNoE.V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaNoE.V2.ViewModels
{
    class NovelEndViewModel
    {
        public List<ModelBase> View
        {
            get { return DBManager.Instance.GetEnd(); }
        }
    }
}
