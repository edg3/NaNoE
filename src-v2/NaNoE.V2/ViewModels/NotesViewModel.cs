using NaNoE.V2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaNoE.V2.ViewModels
{
    class NotesViewModel
    {
        internal void Refresh()
        {
            _notes = DBManager.Instance.GetNotes();
        }

        List<string> _notes = new List<string>();
        public List<string> Notes
        {
            get { return _notes; }
        }
    }
}
