﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaNoE.V2.Models
{
    class NoteModel : ModelBase
    {
        /// <summary>
        /// Initiate Note
        /// </summary>
        /// <param name="id">Element ID</param>
        /// <param name="before">ID Before</param>
        /// <param name="after">ID After</param>
        /// <param name="elType">Element type</param>
        /// <param name="external">External ID</param>
        /// <param name="text">Text</param>
        public NoteModel(int id, int before, int after, int elType, int external, string text) : base(id, before, after, elType, external)
        {
            _text = text;
        }

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
