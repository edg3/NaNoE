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
        System.Windows.Input.ICommand _runAddChapter;
        public System.Windows.Input.ICommand RunAddChapter
        {
            get { return _runAddChapter; }
        }

        /// <summary>
        /// Add paragraph command
        /// </summary>
        System.Windows.Input.ICommand _runAddParagraph;
        public System.Windows.Input.ICommand RunAddParagraph
        {
            get { return _runAddParagraph; }
        }

        /// <summary>
        /// Add bookmark command
        /// </summary>
        System.Windows.Input.ICommand _runAddBookmark;
        public System.Windows.Input.ICommand RunAddBookmark
        {
            get { return _runAddBookmark; }
        }

        /// <summary>
        /// Add note command
        /// </summary>
        private System.Windows.Input.ICommand _runAddNote;
        public System.Windows.Input.ICommand RunAddNote
        {
            get { return _runAddNote; }
        }

        /// <summary>
        /// Delete command
        /// </summary>
        private System.Windows.Input.ICommand _runDelete;
        public System.Windows.Input.ICommand RunDelete
        {
            get { return _runDelete; }
        }

        /// <summary>
        /// Edit command
        /// </summary>
        private System.Windows.Input.ICommand _runEdit;
        public System.Windows.Input.ICommand RunEdit
        {
            get { return _runEdit; }
        }
    }
}
