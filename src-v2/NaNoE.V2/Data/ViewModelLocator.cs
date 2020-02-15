using NaNoE.V2.ViewModels;
using NaNoE.V2.ViewModels.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaNoE.V2.Data
{
    /// <summary>
    /// Locator for finding ViewModels
    /// </summary>
    class ViewModelLocator
    {
        /// <summary>
        /// Initiate the static instance of the Locator
        /// </summary>
        public ViewModelLocator()
        {
            _instance = this;
        }

        /// <summary>
        /// Static reference for the view model locator
        /// </summary>
        private static ViewModelLocator _instance;
        public static ViewModelLocator Instance
        {
            get { return _instance; }
            set
            {
                if (null != _instance) throw new Exception("ViewModelLocator Instance is already set.");
                _instance = value;
            }
        }

        /// <summary>
        /// Instances of all ViewModels we are using
        /// </summary>
        
        private StartViewModel _startVM = new StartViewModel();
        public StartViewModel StartVM
        {
            get { return _startVM; }
        }

        private NovelEndViewModel _novelEndVM = new NovelEndViewModel();
        public NovelEndViewModel NovelEndVM
        {
            get
            {
                HelperVars.Position = DBManager.Instance.GetEndID();
                return _novelEndVM;
            }
        }

        private NovelAddNoteViewModel _novelAddNoteVM = new NovelAddNoteViewModel();
        public NovelAddNoteViewModel NovelAddNoteVM
        {
            get { return _novelAddNoteVM; }
        }

        private MidNovelViewModel _midNovelVM = new MidNovelViewModel();
        public MidNovelViewModel MidNovelVM
        {
            get
            {
                return _midNovelVM;
            }
        }

        private ViewAddActionViewModel _viewAddActionsVM = new ViewAddActionViewModel();
        public ViewAddActionViewModel ViewAddActionVM
        {
            get { return _viewAddActionsVM; }
        }
    }
}
