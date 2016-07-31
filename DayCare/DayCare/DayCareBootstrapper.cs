using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model;
using DayCare.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DayCare
{
	class DayCareBootstrapper : BootstrapperBase
	{
		public DayCareBootstrapper()
		{
			Initialize();

			ServiceProvider.Instance.RegisterService(new EventAggregator());
			ServiceProvider.Instance.RegisterService(new TaskManager());
			ServiceProvider.Instance.RegisterService(new Petoeter());
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			//  base.OnStartup(sender, e);

			DisplayRootViewFor<ShellViewModel>();
		}

		protected override void OnUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			LogManager.GetLog(GetType()).Error(e.Exception);
			/*using (StreamWriter w = File.AppendText("log.txt"))
			{
				w.WriteLine(e.Exception);
				w.WriteLine(e.Exception.Message);
				if (e.Exception.InnerException != null)
				{
					w.WriteLine(e.Exception.InnerException);
					w.WriteLine(e.Exception.InnerException.Message);
				}
			}*/
		}
	}
}
