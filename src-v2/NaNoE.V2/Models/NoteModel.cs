using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaNoE.V2.Models
{
    class NoteModel : ModelBase
    {
        /// <summary>
        /// Note's text
        /// </summary>
        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                Changed("Text");
            }
        }
    }
}
