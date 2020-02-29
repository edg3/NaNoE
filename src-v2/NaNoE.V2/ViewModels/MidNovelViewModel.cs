using NaNoE.V2.Data;
using NaNoE.V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NaNoE.V2.ViewModels
{
    class MidNovelViewModel
    {
        private int _position = 0;
        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }

        internal void Refresh()
        {
            var items = DBManager.Instance.GetMapElements(Position);
            _view = items;
        }

        private List<ModelBase> _view;
        public List<ModelBase> View
        {
            get { return _view; }
        }

        public MidNovelViewModel()
        {

        }

        public ICommand NavUp
        {
            get { return CommandMap.Instance.RunNavigateUp; }
        }

        public ICommand NavDown
        {
            get { return CommandMap.Instance.RunNavigateDown; }
        }

        public int BottomID
        {
            get { return _view[_view.Count - 1].ID; }
        }

        public int TopID
        {
            get { return _view[0].ID; }
        }
    }
}
