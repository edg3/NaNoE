using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaNoE.V2.Data
{
    static class HelperVars
    {
        private static int _position;
        public static int Position
        {
            get { return _position; }
            set { _position = value; }
        }
    }
}
