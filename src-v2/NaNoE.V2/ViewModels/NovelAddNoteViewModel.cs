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
    class NovelAddNoteViewModel
    {
        /// <summary>
        /// Initiate VM
        /// </summary>
        public NovelAddNoteViewModel()
        {
            _addNote = new CommandBase(new Action(_run_AddNote));
        }

        /// <summary>
        /// Content for note
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
        private int _idAfter;
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
        /// Connection to Add Note
        /// </summary>
        private ICommand _addNote;
        public ICommand AddNote
        {
            get { return _addNote; }
        }

        /// <summary>
        /// Method to add note
        /// </summary>
        private void _run_AddNote()
        {
            DBManager.Instance.Commands.RunAddNote.Execute(ViewModelLocator.Instance.NovelAddNoteVM);
        }
    }
}
