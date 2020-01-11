using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaNoE.V2.NovelElements
{
    class ParagraphElement : INovelElement
    {
        public ParagraphElement(int id, string content)
        {
            _id = id;
            _content = content;
        }

        private string _content;
        public string Content { get { return _content; } }

        private int _id;
        public int ID { get { return _id; } }

        public string GetWeb()
        {
            return _content;
        }
    }
}
