using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model;
using DayCare.Model.Tasks;
using DayCare.ViewModels.Children;
using DayCare.ViewModels.Members;
using DayCare.ViewModels.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Accounts
{
	public class EditAccountViewModel : BaseScreen
	{
		private Account _account;
		public AccountDetailViewModel Detail { get; set; }
		public ChildrenMainViewModel ChildrenDetail { get; set; }

		public MembersMainViewModel MembersDetail { get; set; }

		public EditAccountViewModel(Account account)
		{
			Menu = new BackMenu(Menu, "d75c9da9-10a3-427e-b9fe-517033674336", CancelAction);
		
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
			//ServiceProvider.Instance.GetService<TaskManager>().EndTask();
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new AccountMainViewModel()
				});
		}

		//public override void BackAction()
		//{
		//	CancelAction();

		//	base.BackAction();
		//}
	}
}
