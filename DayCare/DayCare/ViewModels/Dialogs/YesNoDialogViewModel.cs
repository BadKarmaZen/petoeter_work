using Caliburn.Micro;
using DayCare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Dialogs
{
	public class YesNoDialogViewModel : Screen
	{
		public string Message { get; set; }
		public System.Action Yes { get; set; }
		public System.Action No { get; set; }

		public virtual void YesAction()
		{
			LogManager.GetLog(GetType()).Info("Yes Clicked");
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				 new Events.ShowDialog());

			if (Yes != null)
			{
				Yes();
			}
		}

		public void NoAction()
		{
			LogManager.GetLog(GetType()).Info("No Clicked");

			if (No != null)
			{
				No();
			}

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				 new Events.ShowDialog());
		}

	}
}
