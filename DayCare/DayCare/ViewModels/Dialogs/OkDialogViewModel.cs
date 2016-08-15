using Caliburn.Micro;
using DayCare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Dialogs
{
	public class OkDialogViewModel : Screen
	{
		public string Message { get; set; }
		public System.Action Ok { get; set; }

		public virtual void OkAction()
		{
			LogManager.GetLog(GetType()).Info("Ok Clicked");
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				 new Events.ShowDialog());

			if (Ok != null)
			{
				Ok();
			}
		}
	}
}
