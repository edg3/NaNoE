﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaNoE.V2.Models
{
    class BookmarkElement : IElement
    {
        public BookmarkElement(int id) : base(id)
        {

        }


        public string WebView()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "Bookmark";
        }
    }
}
