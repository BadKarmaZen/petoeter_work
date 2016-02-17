using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Database;
using DayCare.Model.Tasks;
using DayCare.ViewModels.Children;
using DayCare.ViewModels.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Accounts
{
	public class EditAccountViewModel : Screen
	{
		private Events.RegisterMenu AddBackMenu { get; set; }
		private Events.RegisterMenu RemoveBackMenu { get; set; }

		private Account _account;
		public AccountDetailViewModel Detail { get; set; }
		public ChildrenMainViewModel ChildrenDetail { get; set; }

		public MembersMainViewModel MembersDetail { get; set; }



		public EditAccountViewModel(Account account)
		{
			_account = account;

			Detail = new AccountDetailViewModel(_account);
			ChildrenDetail = new ChildrenMainViewModel(_account);
			MembersDetail = new MembersMainViewModel(_account);

			ServiceProvider.Instance.GetService<TaskManager>().StartTask(new EditAccountTask
			{
				ReturnAction = () =>
						ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
								new Core.Events.SwitchTask
								{
									Task = new EditAccountViewModel(_account)
								})
			});

			AddBackMenu = new Events.RegisterMenu
			{
				Caption = "Terug",
				Id = "Menu.AccountView.Back",
				Add = true,
				Action = () =>
				{
					CancelAction();
				}
			};

			RemoveBackMenu = new Events.RegisterMenu
			{
				Id = "Menu.AccountView.Back",
				Add = false
			};

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(AddBackMenu);
		}

		//public void SaveAction()
		//{
		//    _account.Name = Detail.Name;
		//    _account.Children = (from c in ChildrenDetail.Children
		//                         select c.Tag).ToList();

		//    ServiceProvider.Instance.GetService<Petoeter>().UpdateRecord(_account);
		//    ServiceProvider.Instance.GetService<TaskManager>().EndTask();
		//    ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
		//        new Core.Events.SwitchTask
		//        {
		//            Task = new AccountMainViewModel()
		//        });
		//}

		public void CancelAction()
		{
			ServiceProvider.Instance.GetService<TaskManager>().EndTask();
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
					new Core.Events.SwitchTask
					{
						Task = new AccountMainViewModel()
					});


			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(RemoveBackMenu);
		}
	}
}
