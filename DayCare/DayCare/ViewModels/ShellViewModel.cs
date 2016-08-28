using Caliburn.Micro;
using Caliburn.Micro.Logging.NLog;
using DayCare.Core;
using DayCare.ViewModels.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels
{
	public class ShellViewModel : Screen,
			IHandle<Events.SwitchTask>,
			IHandle<Events.Close>,
			IHandle<Events.ShowDialog>,
		  IHandle<Events.ShowSnapshot>
	{
		#region Members

		private Screen _task;
		private Screen _menu;
		private Screen _dialog;
		private Screen _snapshots;
		private string _applicationVersion;

		#endregion

		#region Properties

		public Screen Dialog
		{
			get { return _dialog; }
			set { _dialog = value; NotifyOfPropertyChange(() => Dialog); NotifyOfPropertyChange(() => ShowDialog); }
		}

		public bool ShowDialog
		{
			get { return _dialog != null; }
		}

		public Screen Menu
		{
			get { return _menu; }
			set
			{
				_menu = value;
				NotifyOfPropertyChange(() => Menu);
			}
		}

		public Screen SnapshotViewer
		{
			get { return _snapshots; }
			set
			{
				_snapshots = value;
				NotifyOfPropertyChange(() => SnapshotViewer);
				NotifyOfPropertyChange(() => ShowSnapshotViewer);
			}
		}

		public bool ShowSnapshotViewer
		{
			get
			{
				return _snapshots != null;
			}
		}

		public Screen Task
		{
			get
			{
				return _task;
			}

			set
			{
				_task = value;
				NotifyOfPropertyChange(() => Task);
			}
		}

		public string ApplicationVersion
		{
			get { return _applicationVersion; }
			set { _applicationVersion = value; NotifyOfPropertyChange(() => ApplicationVersion); }
		}

		#endregion


		public ShellViewModel()
		{
			LogManager.GetLog(GetType()).Info("Start Shell");
			ServiceProvider.Instance.GetService<EventAggregator>().Subscribe(this);

			Task = new Dashboard.DashBoardViewModel();
			Menu = new Menu.MenuBarViewModel();

			ApplicationVersion = string.Format("DayCare V{0} - © 2016, Kurt Pattyn", Assembly.GetExecutingAssembly().GetName().Version.ToString(3));
		}

		public void Handle(Events.SwitchTask message)
		{
			if (Task is ICloseScreen)
			{
				LogManager.GetLog(GetType()).Info("Close Screen");
				((ICloseScreen)Task).CloseThisScreen();
			}

			LogManager.GetLog(GetType()).Info("Swith to task: {0}", message.Task.ToString());

			Task = message.Task;

			var reactivateTask = Task as ReactivatableScreen;
			if (reactivateTask != null)
			{
				reactivateTask.Reactivate();
			}
		}

		public void Handle(Events.Close message)
		{
			LogManager.GetLog(GetType()).Info("TryClose");
			TryClose();
		}

		public void Handle(Events.ShowDialog message)
		{
			LogManager.GetLog(GetType()).Info("Show Dialog: {0}", message.Dialog);
			Dialog = message.Dialog;
		}

		public void Handle(Events.ShowSnapshot message)
		{
			if (string.IsNullOrWhiteSpace(message.FileName))
			{
				SnapshotViewer = null;
			}
			else
			{
				SnapshotViewer = new PasportImageViewModel(message);
			}
		}
	}
}
