using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Children
{
    public class ChildrenMainViewModel : Screen
    {
        private Account _account;
        private List<ChildUI> _children;
        private ChildUI _selectedChild;

        public ChildUI SelectedChild
        {
            get { return _selectedChild; }
            set
            {
                _selectedChild = value; NotifyOfPropertyChange(() => SelectedChild);
                NotifyOfPropertyChange(() => IsItemSelected);
            }
        }

        public bool IsItemSelected
        {
            get
            {
                return _selectedChild != null;
            }
        }

        public List<ChildUI> Children
        {
            get { return _children; }
            set { _children = value; }
        }


        public ChildrenMainViewModel(Model.Database.Account account)
        {
            this._account = account;
            var model = ServiceProvider.Instance.GetService<Petoeter>(); 
            
            Children = (from c in model.GetChild(c => c.Account_Id == _account.Id)
                        select new ChildUI
                        { 
                            Name = string.Format("{0} {1}", c.FirstName, c.LastName),
                            Tag = c
                        }).ToList();
        }

        public void SelectChild(ChildUI child)
        {
            if (SelectedChild != null)
            {
                SelectedChild.Selected = false;                
            }

            SelectedChild = child;
            SelectedChild.Selected = true;
        }

        public void AddAction()
        {
            ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
                  new Core.Events.SwitchTask
                  {
                      Task = new AddChildViewModel(_account)
                  });
        }
    }
}
