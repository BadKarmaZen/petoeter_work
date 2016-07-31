using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Lite;
using DayCare.Model.UI;
using DayCare.ViewModels.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Scheduler
{
	public class ChildDayUI : PropertyChangedBase
	{
		#region Member
		private bool _morning;
		private bool _afternoon;

		#endregion

		#region Properties
		public bool Morning
		{
			get { return _morning; }
			set { _morning = value; NotifyOfPropertyChange(() => Morning); }
		}

		public bool Afternoon
		{
			get { return _afternoon; }
			set { _afternoon = value; NotifyOfPropertyChange(() => Afternoon); }
		}

		//	Not in weekend
		//	Not in month
		public bool Active { get; set; }
		public bool ActiveMonth { get; set; }
		public DateTime Day { get; set; }

		#endregion
	}

	public class WeekUI : PropertyChangedBase
	{
		public DateTime From { get; set; }
		public DateTime To { get; set; }

		private bool _morning;
		private bool _afternoon;

		public bool Morning
		{
			get { return _morning; }
			set { _morning = value; NotifyOfPropertyChange(() => Morning); }
		}

		public bool Afternoon
		{
			get { return _afternoon; }
			set { _afternoon = value; NotifyOfPropertyChange(() => Afternoon); }
		}
	}

	public class EditChildCalendarViewModel : BaseScreen
	{
		private Child _child;

		private DateTime _startDate;
		private DateTime _endDate;
		private DateTime _currentDate;
		private int _weekCount;

		public DateTime CurrentDate
		{
			get { return _currentDate; }
			set { _currentDate = value; NotifyOfPropertyChange(() => CurrentDate); }
		}

		public string FullName
		{
			get
			{
				return string.Format("{0} {1}", _child.FirstName, _child.LastName);
			}
		}

		private List<ChildDayUI> _days;
		private List<WeekUI> _weeks;

		public List<WeekUI> Weeks
		{
			get { return _weeks; }
			set { _weeks = value; NotifyOfPropertyChange(() => Weeks); }
		}

		public List<ChildDayUI> Days
		{
			get { return _days; }
			set { _days = value; NotifyOfPropertyChange(() => Days); }
		}

		public EditChildCalendarViewModel(Child child)
		{
			LogManager.GetLog(GetType()).Info("Create");
			_child = child;

			Menu = new BackMenu(Menu, "98d040fb-0e97-4eaf-bee4-8f455650493b", BackAction);

			_currentDate = DateTime.Now;
		}

		protected override void OnViewLoaded(object view)
		{
			LoadItems();

			base.OnViewLoaded(view);
		}

		public void BackAction()
		{
			LogManager.GetLog(GetType()).Info("Back");

			SaveHolidays();

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new SchedulerMainViewModel()
				});
		}

		protected void LoadItems()
		{
			LogManager.GetLog(GetType()).Info("Load items");
			CalculateCalendarRegion();

			var days = new List<ChildDayUI>();

			var date = _startDate;
			while (date <= _endDate)
			{
				var scheduled = _child.Schedule.Where(d => d.Day == date).FirstOrDefault();
				var day = new ChildDayUI
				{
					Day = date,
					ActiveMonth = date.Month == _currentDate.Month,
					Active = (date.DayOfWeek != DayOfWeek.Saturday) &&
									 (date.DayOfWeek != DayOfWeek.Sunday)
				};

				if (scheduled != null)
				{
					day.Morning = scheduled.Morning;
					day.Afternoon = scheduled.Afternoon;
				}

				days.Add(day);
				date = date.AddDays(1);
			}

			Days = days;

			BuildWeeks();
		}

		private void BuildWeeks()
		{
			LogManager.GetLog(GetType()).Info("Build weeks");
			var date = _startDate;
			var weeks = new List<WeekUI>();

			while (date < _endDate)
			{
				var week = new WeekUI
				{
					From = date,
					To = date.AddDays(4)
				};

				//	calculate the morning and afternoon
				var days = from d in _days
									 where week.From <= d.Day && d.Day <= week.To
									 select d;

				week.Morning = days.Where(d => d.Morning).Count() > 2;
				week.Afternoon = days.Where(d => d.Afternoon).Count() > 2;

				weeks.Add(week);
				date = date.AddDays(7);
			}

			Weeks = weeks;
		}

		#region Helper

		private void CalculateCalendarRegion()
		{
			//	Start date = The Monday of the week of the 1st of the month
			//
			_startDate = new DateTime(_currentDate.Year, _currentDate.Month, 1);
			switch (_startDate.DayOfWeek)
			{
				case DayOfWeek.Sunday:
					_startDate = _startDate.AddDays(1);
					break;
				case DayOfWeek.Saturday:
					_startDate = _startDate.AddDays(2);
					break;
				case DayOfWeek.Monday:
					//	do nothing
					break;
				default:
					_startDate = _startDate.PreviousMonday();
					break;
			}

			//	End date
			//			
			_endDate = new DateTime(_currentDate.Year, _currentDate.Month, 1).AddMonths(1).AddDays(-1);
			switch (_endDate.DayOfWeek)
			{
				case DayOfWeek.Sunday:
					//	Do nothing
					break;
				case DayOfWeek.Saturday:
					_endDate = _endDate.AddDays(1);
					break;
				case DayOfWeek.Friday:
					_endDate = _endDate.AddDays(2);
					break;
				default:
					_endDate = _endDate.NextFriday().AddDays(2);
					break;
			}

			_weekCount = (1 + (_endDate - _startDate).Days) / 7;
		}

		private void SaveHolidays()
		{
			LogManager.GetLog(GetType()).Info("Save");
			//	Remove all scheduled
			_child.Schedule.RemoveAll(d => _startDate <= d.Day && d.Day <= _endDate);

			var query = from d in Days
									where d.Afternoon || d.Morning
									select new DayCare.Model.Lite.Date
									{
										Morning = d.Morning,
										Afternoon = d.Afternoon,
										Day = d.Day
									};

			_child.Schedule.AddRange(query);

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				_child.Updated = DateTime.Now;
				db.Children.Update(_child);
			}
		}

		#endregion

		#region Actions
		public void ToggleAction(ChildDayUI ui)
		{
			ui.Morning = !ui.Morning;
			ui.Afternoon = ui.Morning;
			UpdateWeek(ui.Day);
		}
		public void ToggleMorningAction(ChildDayUI ui)
		{
			ui.Morning = !ui.Morning;
			UpdateWeek(ui.Day);
		}

		public void ToggleAfternoonAction(ChildDayUI ui)
		{
			ui.Afternoon = !ui.Afternoon;
			UpdateWeek(ui.Day);
		}

		public void UpdateWeek(DateTime date)
		{
			var week = (from w in Weeks
									where w.From <= date && date <= w.To
									select w).FirstOrDefault();

			if (week != null)
			{
				var days = from d in _days
									 where week.From <= d.Day && d.Day <= week.To
									 select d;
				week.Morning = days.Where(d => d.Morning).Count() > 2;
				week.Afternoon = days.Where(d => d.Afternoon).Count() > 2;
			}
		}

		public void ToggleAfternoonWeekAction(WeekUI week)
		{
			week.Afternoon = !week.Afternoon;

			//	update week
			var days = from d in _days
								 where week.From <= d.Day && d.Day <= week.To
								 select d;

			foreach (var day in days)
			{
				day.Afternoon = week.Afternoon;
			}

			week.Morning = days.Where(d => d.Morning).Count() > 2;
			week.Afternoon = days.Where(d => d.Afternoon).Count() > 2;
		}

		public void ToggleMorningWeekAction(WeekUI week)
		{
			week.Morning = !week.Morning;

			//	update week
			var days = from d in _days
								 where week.From <= d.Day && d.Day <= week.To
								 select d;

			foreach (var day in days)
			{
				day.Morning = week.Morning;
			}
		}

		public void AddPatternAction()
		{
			LogManager.GetLog(GetType()).Info("Add Pattern");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.ShowDialog
				{
					Dialog = new AddScheduleViewModel(_child)
				});
		}

		public List<ChildDayUI> CopyWeek { get; set; }

		public void CopyWeekAction(WeekUI week)
		{
			LogManager.GetLog(GetType()).Info("Copy week");

			CopyWeek = (from d in _days
									where week.From <= d.Day && d.Day <= week.To
									select new ChildDayUI { Day = d.Day, Morning = d.Morning, Afternoon = d.Afternoon }).ToList();
		}

		public void PasteWeekAction(WeekUI week)
		{
			if (CopyWeek != null)
			{
				LogManager.GetLog(GetType()).Info("Paste week");
				var daysTo = (from d in _days
											where week.From <= d.Day && d.Day <= week.To
											select d).ToList();

				for (int index = 0; index < CopyWeek.Count; index++)
				{
					daysTo[index].Morning = CopyWeek[index].Morning;
					daysTo[index].Afternoon = CopyWeek[index].Afternoon;
				}

				UpdateWeek(week.From);
			}
			else
			{
				LogManager.GetLog(GetType()).Warn("Paste week - no source");
			}
		}

		public void FastPrevAction()
		{
			SaveHolidays();
			CurrentDate = CurrentDate.AddMonths(-3);
			LoadItems();
		}

		public void PrevAction()
		{
			SaveHolidays();
			CurrentDate = CurrentDate.AddMonths(-1);
			LoadItems();
		}

		public void NextAction()
		{
			SaveHolidays();
			CurrentDate = CurrentDate.AddMonths(1);
			LoadItems();
		}
		public void FastNextAction()
		{
			SaveHolidays();
			CurrentDate = CurrentDate.AddMonths(3);
			LoadItems();
		}

		#endregion

	}
}
