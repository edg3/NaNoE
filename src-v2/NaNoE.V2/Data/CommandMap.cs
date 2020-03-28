using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NaNoE.V2.Data
{
    class CommandMap
    {
        private static CommandMap _instance;
        public static CommandMap Instance
        {
            get { return _instance; }
            private set { _instance = value; }
        }

        /// <summary>
        /// Instantiate the commands
        /// </summary>
        public CommandMap()
        {
            _runAddChapter = new RunAddChapterCommand();
            _runAddParagraph = new RunAddParagraphCommand();
            _runAddBookmark = new RunAddBookmarkCommand();
            _runAddNote = new RunAddNoteCommand();
            _runDelete = new RunDeleteCommand();
            _runEdit = new RunEditCommand();
            _runNavigateUp = new RunNavigateUp();
            _runNavigateDown = new RunNavigateDown();
            _runAction = new RunAction();
            _runNoteView = new RunNoteView();

            _instance = this;
        }

        /// <summary>
        /// Add Chapter Command
        /// </summary>
        private ICommand _runAddChapter;
        public ICommand RunAddChapter
        {
            get { return _runAddChapter; }
        }

        /// <summary>
        /// Add paragraph command
        /// </summary>
        private ICommand _runAddParagraph;
        public ICommand RunAddParagraph
        {
            get { return _runAddParagraph; }
        }

        /// <summary>
        /// Add bookmark command
        /// </summary>
        ICommand _runAddBookmark;
        public ICommand RunAddBookmark
        {
            get { return _runAddBookmark; }
        }

        /// <summary>
        /// Add note command
        /// </summary>
        private ICommand _runAddNote;
        public ICommand RunAddNote
        {
            get { return _runAddNote; }
        }

        /// <summary>
        /// Delete command
        /// </summary>
        private ICommand _runDelete;
        public ICommand RunDelete
        {
            get { return _runDelete; }
        }

        /// <summary>
        /// Edit command
        /// </summary>
        private ICommand _runEdit;

        public ICommand RunEdit
        {
            get { return _runEdit; }
        }

        /// <summary>
        /// Navigate up command
        /// </summary>
        private ICommand _runNavigateUp;
        public ICommand RunNavigateUp
        {
            get { return _runNavigateUp; }
        }

        /// <summary>
        /// Navigate down command
        /// </summary>
        private ICommand _runNavigateDown;
        public ICommand RunNavigateDown
        {
            get { return _runNavigateDown; }
        }

        /// <summary>
        /// Run an action command
        /// </summary>
        private ICommand _runAction;

        public ICommand RunAction
        {
            get { return _runAction; }
        }

        /// <summary>
        /// Open notes
        /// </summary>
        private RunNoteView _runNoteView;

        public ICommand RunNoteView
        {
            get { return _runNoteView; }
        }
    }
}
