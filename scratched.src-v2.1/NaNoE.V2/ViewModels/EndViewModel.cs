using NaNoE.V2.Data;
using NaNoE.V2.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaNoE.V2.ViewModels
{
    class EndViewModel
    {
        public List<IElement> Data { get { return DBManager.Instance.GetElements(); } }
    }
}
