using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model;
using DayCare.ViewModels;
using System;
using System.Collections.Generic;
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
			ServiceProvider.Instance.RegisterService(new ImageManager());
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			//  base.OnStartup(sender, e);

			DisplayRootViewFor<ShellViewModel>();
		}
	}
}
