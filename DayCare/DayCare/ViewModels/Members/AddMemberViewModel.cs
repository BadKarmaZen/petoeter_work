using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model;
using DayCare.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DayCare.ViewModels.Members
{
	public class AddMemberViewModel : Screen
	{
		private Account _account;

		private string _firstName;
		private string _lastName;
		private string _phone;

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

		public AddMemberViewModel(Account account)
		{
			// TODO: Complete member initialization
			this._account = account;
		}

		public void SaveAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
			 new Events.ShowDialog());

			var member = new Member
			{
				Id = Guid.NewGuid(),
				Account = _account,
				FirstName = this.FirstName,
				LastName = this.LastName,
				Phone = this.Phone
			};

			_account.Members.Add(member);

			ServiceProvider.Instance.GetService<Petoeter>().AddMember(member);
			ServiceProvider.Instance.GetService<Petoeter>().Save();

			/*var newAccount = (from a in ServiceProvider.Instance.GetService<Petoeter>().GetAccounts()
												where a.Id == _account.Id
												select a).FirstOrDefault();*/

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new EditAccountViewModel(_account)
				});
		}

		public void CancelAction()
		{
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
