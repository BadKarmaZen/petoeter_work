using Caliburn.Micro;
using DayCare.Core;
using DayCare.ViewModels.Accounts;
using DayCare.ViewModels.Calendar;
using DayCare.ViewModels.Expenses;
using DayCare.ViewModels.Precense;
using DayCare.ViewModels.Reports;
using DayCare.ViewModels.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Dashboard
{
	public class DashBoardViewModel : Screen
	{
		private static Events.RegisterMenu AddBackMenu;
		private static Events.RegisterMenu RemoveBackMenu;

		static DashBoardViewModel()
		{
			AddBackMenu = new Events.RegisterMenu
			{
				Caption = "Home",
				Id = "Menu.Home",
				Add = true,
				Action = () =>
				{
					ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
					new Core.Events.SwitchTask
					{
						Task = new DashBoardViewModel()
					});
					ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(RemoveBackMenu);
				}
			};

			RemoveBackMenu = new Events.RegisterMenu
			{
				Id = "Menu.Home",
				Add = false
			};
		}

		public void CloseAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.Close());
		}

		public void AdministrationAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(AddBackMenu);
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new AccountMainViewModel()
				});
		}

		public void ManageSchedulesAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(AddBackMenu);
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new SchedulerMainViewModel()
				});
		}

		public void StartPrecenseAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(AddBackMenu);
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new PresenceMainViewModel()
				});
		}

		public void CalendarOverviewAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(AddBackMenu);
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new CalendarMainViewModel()
				});
		}

		public void ReportsAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(AddBackMenu);
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new MainReportsViewModel()
				});
		}

		public void ExpensesAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(AddBackMenu);
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new ExpenseMainViewModel()
				});
		}

	}
}
