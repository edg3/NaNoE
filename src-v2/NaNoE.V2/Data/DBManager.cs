using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaNoE.V2.Data
{
    class DBManager
    {
        private static DBManager _instance;
        public static DBManager Instance
        {
            get
            {
                if (_instance == null) _instance = new DBManager();

                return _instance;
            }
        }

        private DBManager()
        {

        }
    }
}
