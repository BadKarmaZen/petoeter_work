using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Lite;
using DayCare.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Children
{
	public class AddChildViewModel : Screen
	{
		private Account _account;

		public ChildDetailViewModel Detail { get; set; }


		public AddChildViewModel(Account account)
		{
			LogManager.GetLog(GetType()).Info("Create");

			_account = account;
			Detail = new ChildDetailViewModel() 
			{
				BirthDay = DateTimeProvider.Now()
			};
		}

		public void SaveAction()
		{
			LogManager.GetLog(GetType()).Info("Save");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Events.ShowDialog());

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var child = new Child();
			
				Detail.GetData(child);
				child.Updated = DateTime.Now;
				db.Children.Insert(child);
				
				_account.Children.Add(child);
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
