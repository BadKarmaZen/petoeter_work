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
        private Child _child;

        #region Properties
        public string FirstName
        {
            get { return _child.FirstName; }
            set { _child.FirstName = value; NotifyOfPropertyChange(() => FirstName); }
        }

        public string LastName
        {
            get { return _child.LastName; }
            set { _child.LastName = value; NotifyOfPropertyChange(() => FirstName); }
        }
        #endregion

        public ChildDetailViewModel(Child child)
        {
            _child = child;
        }
    }
}
