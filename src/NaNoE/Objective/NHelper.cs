using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaNoE.Objective
{
    class NHelper
    {
        public string Name { get; set; }
        public NHelper(string name)
        {
            Name = name;
        }

        public List<NNote> Notes = new List<NNote>();
    }
}
