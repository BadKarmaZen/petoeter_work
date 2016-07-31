using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DayCare.Core;
using DayCare.Model.Lite;
using DayCare.Model.UI;

namespace DayCare.ViewModels.Calendar
{
	public class DayUI : Screen
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

		public int MorningCount { get; set; }
		public int AfternoonCount { get; set; }
	}

	public class CalendarMainViewModel : Screen
	{
		#region Members
		private DateTime _startDate;
		private DateTime _endDate;
		private DateTime _currentDate;
		private List<DayUI> _days;
		#endregion

		#region Properties
		public DateTime CurrentDate
		{
			get { return _currentDate; }
			set { _currentDate = value; NotifyOfPropertyChange(() => CurrentDate); }
		}

		public List<DayUI> Days
		{
			get { return _days; }
			set { _days = value; NotifyOfPropertyChange(() => Days); }
		}
		#endregion

		public CalendarMainViewModel()
		{
			_currentDate = DateTime.Now;
		}

		protected override void OnViewLoaded(object view)
		{
			LoadItems();

			base.OnViewLoaded(view);
		}

		#region Helper
		protected void LoadItems()
		{
			LogManager.GetLog(GetType()).Info("Load items");

			CalculateCalendarRegion();

			var days = new List<DayUI>();			

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var children_presence = new List<DayCare.Model.Lite.Date>();
				
				foreach (var child in db.Children.FindAll())
				{
					children_presence.AddRange(child.Schedule);
				}

				var date = _startDate;
				while (date <= _endDate)
				{
					//	var scheduled = _child.Schedule.Where(d => d.Day == date).FirstOrDefault();
					var day = new DayUI
					{
						Day = date,
						ActiveMonth = date.Month == _currentDate.Month,
						Active = (date.DayOfWeek != DayOfWeek.Saturday) &&
										 (date.DayOfWeek != DayOfWeek.Sunday)
					};

					var holiday = db.Holidays.FindOne(d => d.Day == date);
					if (holiday != null)
					{
						day.Morning = holiday.Morning;
						day.Afternoon = holiday.Afternoon;
					}

					//	count children
					day.MorningCount = children_presence.Count(d => d.Day == date && d.Morning);
					day.AfternoonCount = children_presence.Count(d => d.Day == date && d.Afternoon);

					days.Add(day);
					date = date.AddDays(1);
				}
			}

			Days = days;
		}

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

			//_weekCount = (1 + (_endDate - _startDate).Days) / 7;
		}

		private void SaveHolidays()
		{
		}
		#endregion

		#region Actions

		public void ToggleAction(DayUI ui)
		{
			ui.Morning = !ui.Morning;
			ui.Afternoon = ui.Morning;

			SaveHoliday(ui);
		}

		private static void SaveHoliday(DayUI ui)
		{
			LogManager.GetLog(typeof(CalendarMainViewModel)).Info("Save Holiday");

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var day = db.Holidays.FindOne(d => d.Day == ui.Day);

				if (day != null)
				{
					// update
					day.Afternoon = ui.Afternoon;
					day.Morning = ui.Morning;

					db.Holidays.Update(day);
				}
				else
				{
					if (ui.Morning || ui.Afternoon)
					{
						db.Holidays.Insert(new Model.Lite.Date
						{
							Day = ui.Day,
							Morning = ui.Morning,
							Afternoon = ui.Afternoon
						});
					}
				}
			}
		}
		public void ToggleMorningAction(DayUI ui)
		{
			ui.Morning = !ui.Morning;
			SaveHoliday(ui);
		}

		public void ToggleAfternoonAction(DayUI ui)
		{
			ui.Afternoon = !ui.Afternoon;
			SaveHoliday(ui);
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
