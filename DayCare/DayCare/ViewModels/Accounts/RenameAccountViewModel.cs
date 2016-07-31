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
	public class RenameAccountViewModel : Screen
	{
		private Account _account;
		private string _name;

		public string Name
		{
			get { return _name; }
			set { _name = value; NotifyOfPropertyChange(() => Name); }
		}

		public RenameAccountViewModel(Account account)
		{
			LogManager.GetLog(GetType()).Info("Create");

			_account = account;
			Name = _account.Name;
		}


		public void SaveAction()
		{
			LogManager.GetLog(GetType()).Info("Save");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Events.ShowDialog());

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				_account.Name = Name;
				_account.Updated = DateTime.Now;

				db.Accounts.Update(_account);
			}

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new EditAccountViewModel(_account)
				});
		}

		public void CancelAction()
		{
			LogManager.GetLog(GetType()).Info("Cancel");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Events.ShowDialog());

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new EditAccountViewModel(_account)
				});
		}
	}
}
