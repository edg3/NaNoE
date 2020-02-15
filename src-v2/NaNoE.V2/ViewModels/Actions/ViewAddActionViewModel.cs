using NaNoE.V2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NaNoE.V2.ViewModels.Actions
{
    class ViewAddActionViewModel
    {
        /// <summary>
        /// Inisitate commands
        /// </summary>
        public ViewAddActionViewModel()
        {
            _chapterCommand = DBManager.Instance.Commands.RunAddChapter;
            _noteCommand = DBManager.Instance.Commands.RunAddNote;
            _bookmarkCommand = DBManager.Instance.Commands.RunAddBookmark;
            _paragraphCommand = DBManager.Instance.Commands.RunAddParagraph;
            _cancelCommand = new CommandBase(_run_cancelAction);
        }

        // Marking if there's an action to execute
        public enum ActionRan
        {
            True,
            False
        }

        /// <summary>
        /// Mark if there is an action execute
        /// </summary>
        private ActionRan _action = ActionRan.False;
        public ActionRan Action
        {
            get { return _action; }
        }

        /// <summary>
        /// Where naviagtions need to go
        /// </summary>
        private string _actionNavigate = "novelend";
        public string ActionNavigate
        {
            get { return _actionNavigate; }
        }

        /// <summary>
        /// Cancel an action
        /// </summary>
        private void _run_cancelAction()
        {
            Window.Close();
        }

        /// <summary>
        /// ID of selected element
        /// </summary>
        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Insert Chapter command
        /// </summary>
        private ICommand _chapterCommand;
        public ICommand ChapterCommand
        {
            get { return _chapterCommand; }
        }

        /// <summary>
        /// Insert note command
        /// </summary>
        private ICommand _noteCommand;
        public ICommand NoteCommand
        {
            get { return _noteCommand; }
        }

        /// <summary>
        /// Insert bookmark command
        /// </summary>
        private ICommand _bookmarkCommand;
        public ICommand BookmarkCommand
        {
            get { return _bookmarkCommand; }
        }

        /// <summary>
        /// Insert paragraph command
        /// </summary>
        private ICommand _paragraphCommand;
        public ICommand ParagraphCommand
        {
            get { return _paragraphCommand; }
        }

        /// <summary>
        /// Cancel Action command
        /// </summary>
        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get { return _cancelCommand; }
        }

        public ViewAddAction Window { get; internal set; }
    }
}
