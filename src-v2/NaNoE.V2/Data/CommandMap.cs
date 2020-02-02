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
        }

        /// <summary>
        /// Add Chapter Command
        /// </summary>
        ICommand _runAddChapter;
        public ICommand RunAddChapter
        {
            get { return _runAddChapter; }
        }

        /// <summary>
        /// Add paragraph command
        /// </summary>
        ICommand _runAddParagraph;
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
    }
}
