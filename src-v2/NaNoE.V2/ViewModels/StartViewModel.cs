using NaNoE.V2.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

            OpenNovelCommand = new CommandBase() { Act = new Action(_OpenNovel) };
            NewNovelCommand = new CommandBase() { Act = new Action(_NewNovel) };
            OpenLastNovelCommand = new CommandBase() { Act = new Action(_OpenLastNovel) };
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
            get { return _openNovelCommand; }
            set { _openNovelCommand = value; }
        }
        private void _OpenNovel()
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.FileName = "SQLite Novel";
            ofd.DefaultExt = ".sqlite";
            ofd.Filter = "SQLite Novels (.sqlite)|*.sqlite";

            Nullable<bool> result = ofd.ShowDialog();

            if (result == true)
            {
                DBManager.Instance.Load(ofd.FileName);
                Navigator.Instance.Goto("novelend");
            }
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
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.FileName = "SQLite Novel";
            sfd.DefaultExt = ".sqlite";
            sfd.Filter = "SQLite Novels (.sqlite)|*.sqlite";
            sfd.OverwritePrompt = false;

            Nullable<bool> result = sfd.ShowDialog();

            if (result == true)
            {
                if (File.Exists(sfd.FileName))
                {
                    MessageBox.Show("Can't use name of already created files.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    DBManager.Instance.Create(sfd.FileName);
                    Navigator.Instance.Goto("novelend");
                }
            }
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
            if (LastNovel == "None")
            {
                MessageBox.Show("Can't open 'None'.", "Oops...", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            
            DBManager.Instance.Load(LastNovel);
            // TODO: Open last should go to same position
            Navigator.Instance.Goto("novelend");
        }
    }
}
