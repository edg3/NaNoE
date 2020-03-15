using NaNoE.V2.Data;
using NaNoE.V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NaNoE.V2.ViewModels
{
    class NovelAddParagraphViewModel
    {
        /// <summary>
        /// Initiate VM
        /// </summary>
        public NovelAddParagraphViewModel()
        {
            _addParagraph = new CommandBase(new Action(_run_AddParagraph));
        }

        /// <summary>
        /// Content for paragraph
        /// </summary>
        private string _text = "";
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// The position in map
        /// </summary>
        private int _idAfter = 0;
        public int IDAfter
        {
            get { return _idAfter; }
            set { _idAfter = value; }
        }

        /// <summary>
        /// The view given
        ///  - before
        ///  - writing
        ///  - after
        ///  Shows things around position
        /// </summary>
        List<ModelBase> _models = new List<ModelBase>();
        public List<ModelBase> Models
        {
            get { return _models; }
            set { _models = value; }
        }


        /// <summary>
        /// Connectuib to Add Paragraph
        /// </summary>
        private ICommand _addParagraph;
        public ICommand AddParagraph
        {
            get { return _addParagraph; }
        }

        /// <summary>
        /// Method to add paragraph
        /// </summary>
        private void _run_AddParagraph()
        {
            CommandMap.Instance.RunAddParagraph.Execute(ViewModelLocator.Instance.NovelAddParagraphVM);
        }

        /// <summary>
        /// Cancel command to move back
        /// </summary>
        private ICommand _cancel;
        public ICommand Cancel
        {
            get { return _cancel; }
        }

        /// <summary>
        /// Method to cancel adding paragraphs
        /// </summary>
        private void _run_cancel()
        {
            Navigator.Instance.GotoLast();
        }

    }
}
