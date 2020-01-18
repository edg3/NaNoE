using NaNoE.V2.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NaNoE.V2.ViewModels
{
    public class StartViewModel
    {
        /// <summary>
        /// Initiate the View
        /// </summary>
        public StartViewModel()
        {
            // TODO: 'info' saved data
            //        - LastNovel
            LastNovel = "None";

            OpenNovelCommand = new CommandBase(new Action(_OpenNovel));
            NewNovelCommand = new CommandBase(new Action(_NewNovel));
            OpenLastNovelCommand = new CommandBase(new Action(_OpenLastNovel));
        }

        /// <summary>
        /// Last novel 
        /// </summary>
        private string _lastNovel;
        public string LastNovel
        {
            get { return _lastNovel; }
            set { _lastNovel = value; }
        }

        /// <summary>
        /// Function to open an existing novel database
        /// </summary>
        private CommandBase _openNovelCommand;
        public CommandBase OpenNovelCommand
        {
            get { return _openLastNovelCommand; }
            set { _openLastNovelCommand = value; }
        }
        private void _OpenNovel()
        {
            Debug.Assert(false, "Open Novel command");
        }

        /// <summary>
        /// Function to create a new novel database
        /// </summary>
        private CommandBase _newNovelCommand;
        public CommandBase NewNovelCommand
        {
            get { return _newNovelCommand; }
            set { _newNovelCommand = value; }
        }
        private void _NewNovel()
        {
            Debug.Assert(false, "New Novel command");
        }

        /// <summary>
        /// Opens the last novel we used
        /// </summary>
        private CommandBase _openLastNovelCommand;
        public CommandBase OpenLastNovelCommand
        {
            get { return _openLastNovelCommand; }
            set { _openLastNovelCommand = value; }
        }
        private void _OpenLastNovel()
        {
            Debug.Assert(false, "Open Last Novel command");
        }
    }
}
