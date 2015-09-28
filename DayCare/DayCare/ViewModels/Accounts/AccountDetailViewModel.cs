using Caliburn.Micro;
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

        public string Name 
        {
            get { return _account.Name; }
            set { _account.Name = value; NotifyOfPropertyChange(() => Name); } 
        }

        public AccountDetailViewModel(Account account)
        {
            // TODO: Complete member initialization
            this._account = account;
        }
    }
}
