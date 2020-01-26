using NaNoE.V2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaNoE.V2.Models
{
    class WritingModel : ModelBase
    {
        public WritingModel(int before, string content, bool flag) : base(0,before,0,0,0)
        {
            _content = content;
        }

        private string _content;
        public string Content
        {
            get { return _content; }
            set 
            {
                if (value.Last() == '\n')
                {
                    throw new NotImplementedException(); 
                }
                else
                {
                    _content = value;
                    Changed("Content");
                }
            }
        }
    }
}
