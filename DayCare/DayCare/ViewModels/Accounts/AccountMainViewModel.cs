using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Database;
using DayCare.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Accounts
{
    public class AccountMainViewModel : Screen
    {
        private List<AccountUI> _accounts;  
        private AccountUI _selectedAccount;


        public AccountUI SelectedAccount
        {
            get { return _selectedAccount; }
            set { _selectedAccount = value; 
                NotifyOfPropertyChange(() => SelectedAccount);
                NotifyOfPropertyChange(() => IsItemSelected);
            }
        }

        public bool IsItemSelected
        {
            get 
            {
                return _selectedAccount != null;
            }
        }

              
        private string _filter;        
        
        public List<AccountUI> Accounts
        {
            get { return _accounts; }
            set { _accounts = value; NotifyOfPropertyChange(() => Accounts); NotifyOfPropertyChange(() => FilteredAccounts); }
        }

        public List<AccountUI> FilteredAccounts
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(Filter))
                {
                    return _accounts;
                }

                var f = Filter.ToLowerInvariant();
                return (from a in _accounts
                        let name = a.Name.ToLowerInvariant()
                        where name.Contains(f)
                        select a).ToList();
            }
        }

        public string Filter
        {
            get { return _filter; }
            set { _filter = value; NotifyOfPropertyChange(() => Filter); NotifyOfPropertyChange(() => FilteredAccounts); }
        }

        public AccountMainViewModel()
        {
            LoadData();
        }

        private void LoadData()
        {
            var model = ServiceProvider.Instance.GetService<Petoeter>();

            Accounts = (from a in model.GetAccount(a => a.Deleted == false)
                        select new AccountUI { Name = a.Name, Tag = a }).ToList();
        }

        public void AddAction ()
        {
            ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
                  new Core.Events.SwitchTask
                  {
                      Task = new AddAccountViewModel()
                  });            
        }

        public void EditAction()
        {

            ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
                  new Core.Events.SwitchTask
                  {
                      Task = new EditAccountViewModel(SelectedAccount.Tag)
                  });
        }

        public void DeleteAction()
        {
            ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
                new Events.ShowDialog
                {
                    Dialog = new YesNoDialogViewModel
                    {
                        Message = "Ben je zeker?",
                        Yes = () => DeleteAccount()
                    }
                });
        }

        public void DeleteAccount()
        {
            var model = ServiceProvider.Instance.GetService<Petoeter>();

            model.DeleteRecord(SelectedAccount.Tag);
            SelectAction(null);

            LoadData();
        }

        public void SelectAction(AccountUI account)
        {
            if (SelectedAccount != null)
            {
                SelectedAccount.Selected = false;                
            }

            SelectedAccount = account;

            if (SelectedAccount != null)
            {
                SelectedAccount.Selected = true;                
            }
        }
    }
}
