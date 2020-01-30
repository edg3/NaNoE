using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NaNoE.V2.Data
{
    // Herpa derp
    internal class RunAddParagraphCommand : CommandBase
    {
        public RunAddParagraphCommand() : base()
        {
            base.Act = this.FnAct;
        }

        public void FnAct()
        {
            var test = 1;
        }
    }
}
