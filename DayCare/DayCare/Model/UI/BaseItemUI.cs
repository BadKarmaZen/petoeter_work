using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.UI
{
    public class BaseItemUI : Screen
    {
        private string _name;
        private string _toolTip;
        private bool _selected;

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; NotifyOfPropertyChange(() => Selected); }
        }

        public string ToolTip
        {
            get { return _toolTip; }
            set { _toolTip = value; NotifyOfPropertyChange(() => ToolTip); }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyOfPropertyChange(()=> Name); }
        }
    }
    public class TaggedItemUI<T> : BaseItemUI
        where T: class
    {
        public T Tag { get; set; }
    }
}
