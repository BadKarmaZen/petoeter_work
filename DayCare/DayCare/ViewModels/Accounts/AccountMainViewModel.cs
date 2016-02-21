using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model;
using DayCare.ViewModels.UICore;
using DayCare.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Accounts
{
	public class AccountMainViewModel : FilteredListItemScreen<AccountUI>
	{
		//private List<AccountUI> _accounts;
		//private AccountUI _selectedAccount;


		/*public AccountUI SelectedAccount
		{
			get { return _selectedAccount; }
			set
			{
				_selectedAccount = value;
				NotifyOfPropertyChange(() => SelectedAccount);
				NotifyOfPropertyChange(() => IsItemSelected);
			}
		}*/

		/*public bool IsItemSelected
		{
			get
			{
				return _selectedAccount != null;
			}
		}*/


		/*private string _filter;

		public List<AccountUI> Accounts
		{
			get { return _accounts; }
			set { _accounts = value; NotifyOfPropertyChange(() => Accounts); NotifyOfPropertyChange(() => FilteredAccounts); }
		}
		*/
		/*public List<AccountUI> FilteredAccounts
		{
			get
			{
				if (string.IsNullOrWhiteSpace(Filter))
				{
					return (from a in _accounts
									let name = a.Name.ToLowerInvariant()
									orderby name
									select a).ToList();
				}

				var f = Filter.ToLowerInvariant();
				return (from a in _accounts
								let name = a.Name.ToLowerInvariant()
								where name.Contains(f)
								orderby name
								select a).ToList();
			}
		}

		public string Filter
		{
			get { return _filter; }
			set { _filter = value; NotifyOfPropertyChange(() => Filter); NotifyOfPropertyChange(() => FilteredAccounts); }
		}
		*/
		//public AccountMainViewModel()
		//{
		//	LoadData();
		//}

		protected override void LoadItems()
		{
			var data = from a in ServiceProvider.Instance.GetService<Petoeter>().GetAccounts()
								 select new AccountUI 
								 {
 									 Name = a.Name,
									 Tag = a
								 };

			Items = data.ToList();
			base.LoadItems();
		}


		public void AddAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
					new Core.Events.ShowDialog
					{
						Dialog = new AddAccountViewModel()
					});
		}

		public void EditAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
					new Core.Events.SwitchTask
					{
						Task = new EditAccountViewModel(SelectedItem.Tag)
					});
		}

		public void OpenAction(AccountUI account)
		{
			SelectItem(account);
			EditAction();
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

			model.DeleteAccount(SelectedItem.Tag);
			SelectItem(null);

			LoadItems();
		}
	}
}
