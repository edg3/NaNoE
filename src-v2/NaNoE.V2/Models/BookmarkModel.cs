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
        /// Initiate Bookmark
        /// </summary>
        /// <param name="id">Element ID</param>
        /// <param name="before">ID Before</param>
        /// <param name="after">ID After</param>
        /// <param name="elType">Element type</param>
        /// <param name="external">External ID</param>
        /// <param name="text">Text</param>
        public BookmarkModel(int id, int before, int after, int elType, int external, string text) : base(id, before, after, elType, external)
        {
            _text = text;
        }

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
