using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaNoE.V2.ViewModels.Actions
{
    class ViewJumpViewModel
    {
        private List<int> _chapters = new List<int>();
        public List<int> Chapters
        {
            get { return _chapters; }
        }

        private List<(int, string)> _bookmarks = new List<(int, string)>();
        public List<(int, string)> Bookmarks
        {
            get { return _bookmarks; }
        }

        public void Refresh()
        {
            _chapters.Clear();
            _bookmarks.Clear();
        }
    }
}
