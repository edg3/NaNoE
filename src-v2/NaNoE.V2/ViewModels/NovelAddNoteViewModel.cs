using NaNoE.V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaNoE.V2.ViewModels
{
    class NovelAddNoteViewModel
    {
        /// <summary>
        /// The position in map
        /// </summary>
        private int _idAfter;
        public int IDAfter
        {
            get { return _idAfter; }
            set { _idAfter = value; }
        }

        /// <summary>
        /// The view given
        ///  - before
        ///  - writing
        ///  - after
        ///  Shows things around position
        /// </summary>
        List<ModelBase> _models = new List<ModelBase>();
        public List<ModelBase> Models
        {
            get { return _models; }
            set { _models = value; }
        }
    }
}
