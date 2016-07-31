﻿using Caliburn.Micro;
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

		public void YesAction()
		{
			LogManager.GetLog(GetType()).Info("Yes Clicked");

			if (Yes != null)
			{
				Yes();
			}

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				 new Events.ShowDialog());
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
