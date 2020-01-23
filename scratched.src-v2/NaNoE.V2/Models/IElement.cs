using NaNoE.V2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NaNoE.V2.Models
{
    class IElement
    {
        private int _id;
        public int ID { get { return _id; } }

        public IElement(int id)
        {
            _id = id;
        }

        string WebView()
        {
            throw new NotImplementedException();
        }

        public ICommand RunAddChapter
        {
            get { return DBManager.Instance.RunAddChapter; }
        }
    }
}
