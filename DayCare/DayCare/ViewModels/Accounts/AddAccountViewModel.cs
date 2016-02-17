using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Database;
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
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Events.ShowDialog());

			var account = new Account
			{
				Id = Guid.NewGuid(),
				Name = _name
			};

			ServiceProvider.Instance.GetService<Petoeter>().SaveAccount(account);

			var names = _name.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
			var space = new char [] { ' ' };
			foreach (var name in names)
			{
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

				var member = new Member { Account_Id = account.Id, FirstName = firstname, LastName = lastname, Id = Guid.NewGuid() };
				ServiceProvider.Instance.GetService<Petoeter>().SaveMember(member);
			}


			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
					new Core.Events.SwitchTask
					{
						Task = new EditAccountViewModel(account)
					});
		}

		public void CancelAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Events.ShowDialog());

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
					new Core.Events.SwitchTask
					{
						Task = new AccountMainViewModel()
					});
		}


	}
	//public class AddAccountViewModel : ReactivatableScreen
	//{
	//    private Account _account;
	//    public AccountDetailViewModel Detail { get; set; }
	//    public ChildrenMainViewModel ChildrenDetail { get; set; } 

	//    public AddAccountViewModel(Account account = null)
	//    {
	//        if (account == null)
	//        {
	//            _account = new Account { Id = Guid.NewGuid() };                
	//        }
	//        else
	//        {
	//            _account = account;
	//        }

	//        Detail = new AccountDetailViewModel(_account);
	//        ChildrenDetail = new ChildrenMainViewModel(_account); 

	//        var tasks = ServiceProvider.Instance.GetService<TaskManager>();
	//        tasks.StartTask(new AddAccountTask
	//        {
	//            ReturnAction = () =>
	//                ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
	//                    new Core.Events.SwitchTask
	//                    {
	//                        Task = new AddAccountViewModel(_account)
	//                    })
	//        });
	//    }

	//    public override void Reactivate()
	//    {
	//        ChildrenDetail.Reactivate();
	//    }

	//    public void SaveAction()
	//    {
	//        //  get data
	//        _account.Name = Detail.Name;
	//        _account.Children = (from c in ChildrenDetail.Children
	//                             select c.Tag).ToList();

	//        ServiceProvider.Instance.GetService<Petoeter>().SaveRecord(_account);
	//        ServiceProvider.Instance.GetService<TaskManager>().EndTask();
	//        ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
	//            new Core.Events.SwitchTask
	//            {
	//                Task = new AccountMainViewModel()
	//            });
	//    }

	//    public void CancelAction()
	//    {
	//        ServiceProvider.Instance.GetService<TaskManager>().EndTask();
	//        ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
	//            new Core.Events.SwitchTask
	//            {
	//                Task = new AccountMainViewModel()
	//            });
	//    }
	//}    
}
