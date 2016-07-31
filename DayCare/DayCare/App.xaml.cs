using Caliburn.Micro;
using Caliburn.Micro.Logging.NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace DayCare
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		static App()
		{
			LogManager.GetLog = _ => new Core.Log(_);
			//LogManager.GetLog = t => new NLogLogger(t);
		}

		public App()
		{
			LogManager.GetLog(GetType()).Info("Start Daycare");

			InitializeComponent();
		}
	}
}
