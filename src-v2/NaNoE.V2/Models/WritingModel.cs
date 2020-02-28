using NaNoE.V2.Data;
using NaNoE.V2.ViewModels;
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
                if (value.Length > 0)
                {
                    if (value.Last() == '\n')
                    {
                        var viewmodel = HelperVars.ViewModelToWrite;
                        if (viewmodel is NovelEndViewModel)
                        {
                            (CommandMap.Instance.RunAddParagraph as RunAddParagraphCommand).Content = _content;
                            CommandMap.Instance.RunAddParagraph.Execute(DBManager.Instance.GetEndID());
                            Navigator.Instance.Goto("novelend");
                        }
                        else
                        {
                            if (Navigator.Instance.WhereWeAre == "addnote")
                            {
                                var noteAddVM = ViewModelLocator.Instance.NovelAddNoteVM;
                                noteAddVM.Text = _content;
                                noteAddVM.AddNote.Execute(null);
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
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
}
