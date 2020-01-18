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
        /// Initiate Paragraph
        /// </summary>
        /// <param name="id">Element ID</param>
        /// <param name="before">ID Before</param>
        /// <param name="after">ID After</param>
        /// <param name="elType">Element type</param>
        /// <param name="external">External ID</param>
        /// <param name="text">Text</param>
        /// <param name="flagged">Flagged</param>
        public ParagraphModel(int id, int before, int after, int elType, int external, string text, bool flagged) : base(id, before, after, elType, external)
        {
            _text = text;
            _flagged = flagged;
        }

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
