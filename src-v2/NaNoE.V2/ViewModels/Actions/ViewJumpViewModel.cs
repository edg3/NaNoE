using NaNoE.V2.Data;
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

            var i = DBManager.Instance.ChapterCount();
            for (int j = 0; j < i; ++j)
            {
                _chapters.Add(j + 1);
            }

            // _bookmarks.AddRange(DBManager.Instance.BookmarkCount());

            _stringSize = "0 - " + (MaxPos - 1);
        }

        public int MaxPos
        {
            get { return DBManager.Instance.MapSize; }
        }

        private string _stringSize = "";
        public string StringSize
        {
            get { return _stringSize; }
        }
    }
}
