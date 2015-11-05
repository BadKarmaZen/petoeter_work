using Caliburn.Micro;
using DayCare.Model.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Children
{
    public class ChildDetailViewModel : Screen
    {
        private string _firstName;
        private string _lastName;

        #region Properties
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; NotifyOfPropertyChange(() => FirstName); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; NotifyOfPropertyChange(() => FirstName); }
        }
        #endregion

        public ChildDetailViewModel(Child child = null)
        {
            if (child != null)
            {
                _firstName = child.FirstName;
                _lastName = child.LastName;                
            }
        }

        internal void GetData(Child child)
        {
            child.FirstName = _firstName;
            child.LastName = _lastName;
        }
    }
}
