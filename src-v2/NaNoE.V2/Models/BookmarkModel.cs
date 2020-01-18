using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaNoE.V2.Models
{
    class BookmarkModel : ModelBase
    {
        /// <summary>
        /// Bookmark's Text
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
