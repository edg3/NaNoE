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
            var mapPos = DBManager.Instance.GetMapPosition(Position);
            var items = DBManager.Instance.GetMapElements(mapPos);
            _view = items;
        }

        private List<ModelBase> _view;
        public List<ModelBase> View
        {
            get { return _view; }
        }

        public MidNovelViewModel()
        {
            _navUp = DBManager.Instance.Commands.RunNavigateUp;
            _navDown = DBManager.Instance.Commands.RunNavigateDown;
        }

        private ICommand _navUp;
        public ICommand NavUp
        {
            get { return _navUp; }
        }

        private ICommand _navDown;
        public ICommand NavDown
        {
            get { return _navDown; }
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
