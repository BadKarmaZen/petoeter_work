using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Lite;
using DayCare.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DayCare.ViewModels.Members
{
	public class AddMemberViewModel : Screen
	{
		#region Member
		private Account _account;

		private string _firstName;
		private string _lastName;
		private string _phone;
		
		#endregion

		#region Properties
		public string Phone
		{
			get { return _phone; }
			set { _phone = value; NotifyOfPropertyChange(() => Phone); }
		}

		public string FirstName
		{
			get { return _firstName; }
			set { _firstName = value; NotifyOfPropertyChange(() => FirstName); }
		}
		public string LastName
		{
			get { return _lastName; }
			set { _lastName = value; NotifyOfPropertyChange(() => LastName); }
		}
		
		#endregion

		
		public AddMemberViewModel(Account account)
		{
			LogManager.GetLog(GetType()).Info("Create");

			_account = account;
		}

		public void SaveAction()
		{
			LogManager.GetLog(GetType()).Info("Save");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
			 new Events.ShowDialog());

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var member = new Member
				{
					FirstName = this.FirstName,
					LastName = this.LastName,
					Phone = this.Phone,
					Updated = DateTime.Now
				};

				db.Members.Insert(member);
				_account.Members.Add(member);

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
