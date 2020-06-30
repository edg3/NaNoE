using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NaNoE.V2.Data
{
    class RunAddBookmarkCommand : System.Windows.Input.ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            try
            {
                ViewModelLocator.Instance.NovelAddBookmarkVM.IDAfter = int.Parse(parameter.ToString());
            }
            catch
            {
                ViewModelLocator.Instance.NovelAddBookmarkVM.IDAfter = 0;
            }

            if (Navigator.Instance.WhereWeAre == "novelend")
            {
                if (parameter == null || ViewModelLocator.Instance.NovelAddBookmarkVM.IDAfter == 0)
                {
                    ViewModelLocator.Instance.NovelAddBookmarkVM.IDAfter = int.Parse(DBManager.Instance.GetEndID().ToString());
                }
                else
                {
                    ViewModelLocator.Instance.NovelAddBookmarkVM.IDAfter = int.Parse(parameter.ToString());
                }
                ViewModelLocator.Instance.NovelAddBookmarkVM.Models = DBManager.Instance.GetSurrounded(ViewModelLocator.Instance.NovelAddBookmarkVM.IDAfter);

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
                ViewModelLocator.Instance.NovelAddBookmarkVM.Models = DBManager.Instance.GetSurrounded(ViewModelLocator.Instance.NovelAddBookmarkVM.IDAfter);

                Navigator.Instance.Goto("addbookmark");
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
