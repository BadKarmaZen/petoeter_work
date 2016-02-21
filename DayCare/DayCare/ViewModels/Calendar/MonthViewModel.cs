using Caliburn.Micro;
using DayCare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Calendar
{
	/*public class DayUI : Screen
	{
		private bool _holiday;

		public bool Holiday
		{
			get { return _holiday; }
			set { _holiday = value; NotifyOfPropertyChange(() => Holiday); }
		}

		public int Day { get { return Date.Day; } }

		public DateTime Date { get; set; }

		public int MorningCount { get; set; }
		public int AfternoonCount { get; set; }

		public bool NotInMonth { get; set; }
		public bool Weekday { get; set; }
		public Holiday Tag { get; set; }
	}

	public class MonthViewModel : Screen
	{
		private DateTime _start;
		private DateTime _end;

		private DateTime _startView;
		private DateTime _endView;

		private List<DayUI> _days;


		public List<DayUI> Days
		{
			get { return _days; }
			set { _days = value; NotifyOfPropertyChange(() => Days); }
		}

		public MonthViewModel(DateTime date)
		{
			_start = new DateTime(date.Year, date.Month, 1);
			_end = _start.AddMonths(1).AddDays(-1);

			_startView = _start;
			_endView = _end;

			while (_startView.DayOfWeek != DayOfWeek.Monday)
			{
				_startView = _startView.AddDays(-1);				
			}

			while (_endView.DayOfWeek != DayOfWeek.Sunday)
			{
				_endView = _endView.AddDays(1);
			}

			var model = ServiceProvider.Instance.GetService<Petoeter>();

			Days = (from d in GetViewableDays()
							let grp = model.GetValidGroupSchedules(d)
							select new DayUI
							{
								Date = d,
								MorningCount = grp.Count(g => g.GetScheduleOn(d).ThisMorning(d)),
								AfternoonCount = grp.Count(g => g.GetScheduleOn(d).ThisAfternoon(d)),
								NotInMonth = d.Month != date.Month,
								Holiday = model.GetHoliday(h => h.Date == d).Count() == 1,
								Weekday = d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday
							}).ToList();

		}

		public IEnumerable<DateTime> GetViewableDays()
		{
			var day = _startView;
			do
			{
				yield return day;
				day = day.AddDays(1);
			} while (day != _endView);
			yield return day;
		}

		public void ToggleHolidayAction(DayUI ui)
		{
			if (ui.Weekday == false)
			{
				return;				
			}

			ui.Holiday = !ui.Holiday;

			var model = ServiceProvider.Instance.GetService<Petoeter>();

			if (ui.Holiday)
			{
				ui.Tag = new Holiday { Date = ui.Date };
				model.SaveHoliday(ui.Tag);
			}
			else
			{
				model.ObliterateRecord(ui.Tag);
			}
		}
	}*/
}
