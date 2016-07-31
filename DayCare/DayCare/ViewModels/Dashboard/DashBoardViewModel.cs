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
			LogManager.GetLog(GetType()).Info("Close");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.Close());
		}

		public void AdministrationAction()
		{
			LogManager.GetLog(GetType()).Info("Administration");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(AddBackMenu);
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new AccountMainViewModel()
				});
		}

		public void ManageSchedulesAction()
		{
			LogManager.GetLog(GetType()).Info("Schedule");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(AddBackMenu);
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new SchedulerMainViewModel()
				});

		}

		public void StartPrecenseAction()
		{
			LogManager.GetLog(GetType()).Info("Start Presence");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(AddBackMenu);
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new PresenceMainViewModel()
				});
		}

		public void CalendarOverviewAction()
		{
			LogManager.GetLog(GetType()).Info("Calendar");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(AddBackMenu);
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new CalendarMainViewModel()
				});
		}

		public void ReportsAction()
		{
			LogManager.GetLog(GetType()).Info("Reports");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(AddBackMenu);
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new MainReportsViewModel()
				});
		}

		public void DatabaseAction()
		{
			LogManager.GetLog(GetType()).Info("Database");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(AddBackMenu);
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new DayCare.ViewModels.Database.DatabaseViewModel()
				});
		}

		public void ExpensesAction()
		{
			//LogManager.GetLog(GetType()).Info("Expenses");
			//ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(AddBackMenu);
			//ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
			//	new Core.Events.SwitchTask
			//	{
			//		Task = new ExpenseMainViewModel()
			//	});
		}

	}
}
