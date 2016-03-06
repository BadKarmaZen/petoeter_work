using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model;
using DayCare.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Children
{
	public class EditChildViewModel : Screen
	{
		private Account _account;
		private Child _child;
		public ChildDetailViewModel Detail { get; set; }


		public EditChildViewModel(Account _account, Child child)
		{
			this._account = _account;

			this._child = child;

			Detail = new ChildDetailViewModel(_child);
		}

		public void SaveAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
			 new Events.ShowDialog());

			Detail.GetData(_child);
			
			_child.Updated = true;
			ServiceProvider.Instance.GetService<Petoeter>().Save();

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
