using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Accounts
{
    public class AccountDetailViewModel : Screen
    {
        private Account _account;
        private string _name;

        public string Name 
        {
            get { return _name; }
            set { _name = value; NotifyOfPropertyChange(() => Name); } 
        }

        public AccountDetailViewModel(Account account)
        {
            _name = account.Name;
            _account = account;
        }

        public void RenameAction()
        {
            ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
                new Core.Events.ShowDialog
                {
                    Dialog = new RenameAccountViewModel(_account)
                });
        }
    }
}
