using NaNoE.V2.ViewModels;
using NaNoE.V2.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace NaNoE.V2.Data
{/// <summary>
/// Navigator for moving between views
/// </summary>
    public class Navigator
    {
        /// <summary>
        /// Static reference for Navigation
        /// </summary>
        private static Navigator _instance;
        public static Navigator Instance
        {
            get { return _instance; }
            private set
            {
                if (null != _instance) throw new Exception("Navigator already has an instance.");
                _instance = value;
            }
        }

        /// <summary>
        /// Reference to where we place content
        /// </summary>
        private Frame _host;

        /// <summary>
        /// Initiate how we will change the view
        /// </summary>
        /// <param name="hostFrame">The Frame to place Views into</param>
        public Navigator(Frame hostFrame)
        {
            _host = hostFrame;
            _host.JournalOwnership = System.Windows.Navigation.JournalOwnership.OwnsJournal;
            _host.Navigated += _host_Navigated;

            Instance = this;
        }

        /// <summary>
        /// Fix for App.xaml
        /// </summary>
        public Navigator()
        {
            throw new Exception("Navigator needs a host fram.");
        }

        /// <summary>
        /// Keeps the Views from stacking up
        /// </summary>
        private void _host_Navigated(object sender, NavigationEventArgs e)
        {
            _host.RemoveBackEntry();
        }

        /// <summary>
        /// Reference to the name of where we are
        /// </summary>
        private string _whereWeAre;
        public string WhereWeAre
        {
            get { return _whereWeAre; }
            private set { _whereWeAre = value; }
        }

        /// <summary>
        /// Move to a view of a specific name
        /// </summary>
        /// <param name="name">The name of the view we want to go to</param>
        public void Goto(string name)
        {
            Window window = null;

            switch (name)
            {
                case "start": window = new StartView(); HelperVars.ViewModelToWrite = ViewModelLocator.Instance.StartVM; break;
                case "novelend": window = new NovelEndView(); HelperVars.ViewModelToWrite = ViewModelLocator.Instance.NovelEndVM; break;
                case "addnote": window = new AddNoteView(); HelperVars.ViewModelToWrite = ViewModelLocator.Instance.NovelAddNoteVM; break;
                default: throw new NotImplementedException();
            }

            if (null != window)
            {
                _host.Content = new ContentControl() { Content = window.Content };
                window.Close();
            }
        }
    }
}
