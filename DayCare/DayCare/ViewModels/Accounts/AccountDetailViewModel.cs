using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Lite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Accounts
{
	public class AccountDetailViewModel : Screen
	{
		#region Member
		private Account _account;
		private string _name;		
		#endregion

		#region Properties
		public string Name
		{
			get { return _name; }
			set { _name = value; NotifyOfPropertyChange(() => Name); }
		}

		public AccountDetailViewModel(Account account)
		{
			LogManager.GetLog(GetType()).Info("Create");

			_name = account.Name;
			_account = account;
		}		
		#endregion

		public void RenameAction()
		{
			LogManager.GetLog(GetType()).Info("Rename action");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.ShowDialog
				{
					Dialog = new RenameAccountViewModel(_account)
				});
		}
	}
}
