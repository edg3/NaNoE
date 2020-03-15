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
    class NovelAddBookmarkViewModel
    {
        /// <summary>
        ///  Initiate VM
        /// </summary>
        public NovelAddBookmarkViewModel()
        {
            _addBookmark = new CommandBase(new Action(_run_add));
            _cancel = new CommandBase(new Action(_run_cancel));
        }

        /// <summary>
        /// Bookmark name
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
        /// Cancel command to move back
        /// </summary>
        private ICommand _cancel;
        public ICommand Cancel
        {
            get { return _cancel; }
        }

        /// <summary>
        /// Method to cancel adding a note
        /// </summary>
        private void _run_cancel()
        {
            Navigator.Instance.GotoLast();
        }

        /// <summary>
        /// Method for adding a bookmark
        /// </summary>
        private ICommand _addBookmark;
        public ICommand AddBookmark
        {
            get { return _addBookmark; }
        }

        /// <summary>
        /// Method to add a bookmark
        /// </summary>
        private void _run_add()
        {
            try
            {
                ViewModelLocator.Instance.NovelAddNoteVM.IDAfter = ViewModelLocator.Instance.RunAddActionID;
            }
            catch
            {
                ViewModelLocator.Instance.NovelAddNoteVM.IDAfter = 0;
            }

            if (Navigator.Instance.WhereWeAre == "novelend")
            {
                ViewModelLocator.Instance.NovelAddBookmarkVM.IDAfter = ViewModelLocator.Instance.RunAddActionID;
                ViewModelLocator.Instance.NovelAddBookmarkVM.Models = DBManager.Instance.GetSurrounded(ViewModelLocator.Instance.NovelAddNoteVM.IDAfter);

                Navigator.Instance.Goto("addbookmark");
            }
            else if (Navigator.Instance.WhereWeAre == "addbookmark")
            {
                var vm = ViewModelLocator.Instance.NovelAddBookmarkVM;
                DBManager.Instance.InsertBookmark(vm.IDAfter, vm.Text);

                Navigator.Instance.GotoLast();
            }
            else if (Navigator.Instance.WhereWeAre == "midnovel")
            {
                ViewModelLocator.Instance.NovelAddNoteVM.IDAfter = ViewModelLocator.Instance.RunAddActionID;
                ViewModelLocator.Instance.NovelAddNoteVM.Models = DBManager.Instance.GetSurrounded(ViewModelLocator.Instance.NovelAddNoteVM.IDAfter);

                Navigator.Instance.Goto("addbookmark");
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
