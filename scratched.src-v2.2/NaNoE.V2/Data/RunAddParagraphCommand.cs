using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NaNoE.V2.Data
{
    internal class RunAddParagraphCommand : System.Windows.Input.ICommand
    {
        public string Content { get; internal set; }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        private bool failed;

        public void Execute(object parameter)
        {
            failed = true;
            try
            {
                if (int.Parse(parameter.ToString()) == DBManager.Instance.GetEndID())
                {
                    DBManager.Instance.InsertParagraph((int)parameter, Content, false);
                    failed = false;
                }
            }
            catch
            {
            }
            
            if (failed)
            {
                try
                {
                    ViewModelLocator.Instance.NovelAddParagraphVM.IDAfter = ViewModelLocator.Instance.RunAddActionID;
                }
                catch
                {
                    ViewModelLocator.Instance.NovelAddParagraphVM.IDAfter = 0;
                }

                if (Navigator.Instance.WhereWeAre == "novelend")
                {
                    ViewModelLocator.Instance.NovelAddParagraphVM.IDAfter = ViewModelLocator.Instance.RunAddActionID;
                    ViewModelLocator.Instance.NovelAddParagraphVM.Models = DBManager.Instance.GetSurrounded(ViewModelLocator.Instance.NovelAddParagraphVM.IDAfter);

                    Navigator.Instance.Goto("addparagraph");
                }
                else if (Navigator.Instance.WhereWeAre == "addparagraph")
                {
                    var vm = ViewModelLocator.Instance.NovelAddParagraphVM;
                    DBManager.Instance.InsertParagraph(vm.IDAfter, vm.Text, false);

                    Navigator.Instance.GotoLast();
                }
                else if (Navigator.Instance.WhereWeAre == "midnovel")
                {
                    ViewModelLocator.Instance.NovelAddParagraphVM.IDAfter = ViewModelLocator.Instance.RunAddActionID;

                    ViewModelLocator.Instance.NovelAddParagraphVM.Models = DBManager.Instance.GetSurrounded(ViewModelLocator.Instance.NovelAddParagraphVM.IDAfter);

                    Navigator.Instance.Goto("addparagraph");
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
