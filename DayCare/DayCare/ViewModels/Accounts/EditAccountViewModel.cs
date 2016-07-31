using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Lite;
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
		#region Members
		private Account _account;
		
		#endregion

		#region Properties
		public AccountDetailViewModel Detail { get; set; }
		public ChildrenMainViewModel ChildrenDetail { get; set; }
		public MembersMainViewModel MembersDetail { get; set; }
		
		#endregion


		public EditAccountViewModel(Account account)
		{
			LogManager.GetLog(GetType()).Info("Create");

			Menu = new BackMenu(Menu, "d75c9da9-10a3-427e-b9fe-517033674336", CancelAction);

			_account = account;

			Detail = new AccountDetailViewModel(_account);

			ChildrenDetail = new ChildrenMainViewModel(_account);
			MembersDetail = new MembersMainViewModel(_account);
		}


		public void CancelAction()
		{
			LogManager.GetLog(GetType()).Info("Cancel");

			//ServiceProvider.Instance.GetService<TaskManager>().EndTask();
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new AccountMainViewModel()
				});
		}
	}
}
