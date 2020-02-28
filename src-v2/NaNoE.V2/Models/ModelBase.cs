using NaNoE.V2.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NaNoE.V2.Models
{
    /// <summary>
    /// Note: ModelBase implies 'element'
    /// </summary>
    class ModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Initiate Element structure
        /// </summary>
        /// <param name="id">Element's ID</param>
        /// <param name="before">ID of previous Element</param>
        /// <param name="after">ID of following Element</param>
        /// <param name="elType">Type of Element</param>
        /// <param name="external">External ID</param>
        public ModelBase(int id, int before, int after, int elType, int external)
        {
            _id = id;
            _idBefore = before;
            _idAfter = after;
            _elementType = elType;
            _externalID = external;
        }

        /// <summary>
        /// Everything in the DB has an ID
        ///  - Note: This can differ from tables
        /// </summary>
        private int _id;
        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                Changed("ID");
            }
        }

        /// <summary>
        /// This is used as part of data binding
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Helps make it easier to notify when properties change so forms will update their binding value
        /// </summary>
        /// <param name="name">Name of item updated</param>
        protected void Changed(string name)
        {
            if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// ID for previous Element
        /// </summary>
        private int _idBefore;
        public int IDBefore
        {
            get { return _idBefore; }
            set
            {
                _idBefore = value;
                Changed("IDBefore");
            }
        }

        /// <summary>
        /// ID for next Element
        /// </summary>
        private int _idAfter;
        public int IDAfter
        {
            get { return _idAfter; }
            set
            {
                _idAfter = value;
                Changed("IDAfter");
            }
        }

        /// <summary>
        /// Type of element
        ///  - 0 = Chapter
        ///  - 1 = Paragraph
        ///  - 2 = Note
        ///  - 3 = Bookmark
        /// </summary>
        private int _elementType;
        public int ElementType
        {
            get { return _elementType; }
            // Note: Not changeable
        }

        /// <summary>
        /// For Elements 1, 2, and 3 this indicates where the data is stored
        /// </summary>
        private int _externalID;
        public int ExternalID
        {
            get { return _externalID; }
            // Note: Not changeable
        }

        /// <summary>
        /// Find Map position
        /// </summary>
        public int WhereOnMap
        {
            get { return DBManager.Instance.GetEndID(); }
        }

        /// <summary>
        /// Command mapping
        /// </summary>
        public CommandMap Commands
        {
            get { return CommandMap.Instance; }
        }
    }
}
