using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Lite;
using DayCare.Model.Tasks;
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
		private string _name;

		public string Name
		{
			get { return _name; }
			set { _name = value; NotifyOfPropertyChange(() => Name); }
		}
		public void SaveAction()
		{
			LogManager.GetLog(GetType()).Info("Save action");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Events.ShowDialog());

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var account = new Account
				{
					Name = _name,
					Members = new List<Member>()
				};
				
				//	default create members
				//
				var names = _name.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
				var space = new char [] { ' ' };
				foreach (var name in names)
				{
					LogManager.GetLog(GetType()).Info("Add member'{0}'", name);

					var foo = name.Split(space, StringSplitOptions.RemoveEmptyEntries);

					string firstname = string.Empty;
					string lastname = string.Empty;

					if (foo.Length >= 2)
					{
						firstname = foo[foo.Length - 1];
						lastname = string.Join(" ", foo.Take(foo.Length - 1));
					}
					else
					{
						lastname = foo[0];
					}

					//	Add members
					var member = new Member
					{
						FirstName = firstname,
						LastName = lastname
					};

					account.Members.Add(member);
					member.Updated = DateTime.Now;
					db.Members.Insert(member);
				}
				
				account.Updated = DateTime.Now;
				db.Accounts.Insert(account);

				ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
					new Core.Events.SwitchTask
					{
						Task = new EditAccountViewModel(account)
					});
			}
		}

		public void CancelAction()
		{
			LogManager.GetLog(GetType()).Info("Cancel");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Events.ShowDialog());

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new AccountMainViewModel()
				});
		}
	}  
}
