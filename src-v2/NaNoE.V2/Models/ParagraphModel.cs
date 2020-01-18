using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaNoE.V2.Models
{
    class ParagraphModel : ModelBase
    {
        /// <summary>
        /// Paragraph contents
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

        /// <summary>
        /// Flagged implies ignored in Edits
        /// </summary>
        private bool _flagged;
        public bool Flagged
        {
            get { return _flagged; }
            set
            {
                _flagged = value;
                Changed("Flagged");
            }
        }
    }
}
