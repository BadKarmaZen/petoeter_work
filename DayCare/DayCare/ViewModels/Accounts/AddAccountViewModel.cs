using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Database;
using DayCare.ViewModels.Children;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Accounts
{
    public class AddAccountViewModel : Screen
    {
        private Account _account;
        public AccountDetailViewModel Detail { get; set; }
        public ChildrenMainViewModel ChildrenDetail { get; set; } 

        public AddAccountViewModel()
        {
            _account = new Account();

            Detail = new AccountDetailViewModel(_account);
            ChildrenDetail = new ChildrenMainViewModel(_account);
        }
    }    
}
