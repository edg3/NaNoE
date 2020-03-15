﻿using NaNoE.V2.Data;
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

        private string _count = "0 words";
        public string Count
        {
            get { return _count; }
            set { _count = value; }
        }

        private string _content;
        public string Content
        {
            get { return _content; }
            set 
            {
                var simplified = _content;
                while (simplified.Contains("  "))
                {
                    simplified = simplified.Replace("  ", " ");
                }
                var c = simplified.Split(' ').Length;
                _count = c + " words";
                Changed("Count");

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
                            else if (Navigator.Instance.WhereWeAre == "addbookmark")
                            {
                                var bookmarkAddVM = ViewModelLocator.Instance.NovelAddBookmarkVM;
                                bookmarkAddVM.Text = _content;
                                bookmarkAddVM.AddBookmark.Execute(null);
                            }
                            else if (Navigator.Instance.WhereWeAre == "addparagraph")
                            {
                                var paragraphVM = ViewModelLocator.Instance.NovelAddParagraphVM;
                                paragraphVM.Text = _content;
                                paragraphVM.AddParagraph.Execute(null);
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
