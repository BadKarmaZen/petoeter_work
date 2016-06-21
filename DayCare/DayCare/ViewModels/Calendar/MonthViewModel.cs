using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model;
using DayCare.Model.Lite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Calendar
{
	

	//public class MonthViewModel : Screen
	//{
	//	private DateTime _start;
	//	private DateTime _end;

	//	private DateTime _startView;
	//	private DateTime _endView;

	//	private List<DayUI> _days;


	//	public List<DayUI> Days
	//	{
	//		get { return _days; }
	//		set { _days = value; NotifyOfPropertyChange(() => Days); }
	//	}

	//	public MonthViewModel(DateTime date)
	//	{
	//		_start = new DateTime(date.Year, date.Month, 1);
	//		_end = _start.AddMonths(1).AddDays(-1);

	//		_startView = _start.PreviousMonday();
	//		_endView = _end.NextFriday();

	//		using (var db = new PetoeterDb(PetoeterDb.FileName))
	//		{
	//			var holydays = from h in db.Holidays.FindAll()
	//										 where _startView <= h.Day && _endView >= h.Day
	//										 select h;

	//			var weekdays = from d in GetViewableDays()
	//										 select new DayUI 
	//										 {
	//											 Date = d,
	//											 NotInMonth = d.Month != date.Month,
	//											 MorningHoliday = holydays.Where(h => h.Day == d && h.Morning).Count() == 1,
	//											 AfternoonHoliday = holydays.Where(h => h.Day == d && h.Afternoon).Count() == 1
	//										 };

	//			Days = weekdays.ToList();

	//		}

	//		//var model = ServiceProvider.Instance.GetService<DayCare.Model.Petoeter>();

	//		//var weekdays = (from d in GetViewableDays()
	//		//								select new DayUI
	//		//								{
	//		//									Date = d,
	//		//									NotInMonth = d.Month != date.Month,
	//		//									MorningHoliday = model.GetHolidays().Where(h => h.Date == d && h.Morning).Count() == 1,
	//		//									AfternoonHoliday = model.GetHolidays().Where(h => h.Date == d && h.Afternoon).Count() == 1,
	//		//									Weekday = d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday
	//		//								}).ToList();

	//		var period = DatePeriod.MakePeriod(date.Month, date.Year);

	//		//foreach (var child in from c in model.GetChildren()
	//		//											where c.Active(period)
	//		//											orderby c.BirthDay
	//		//											select c)
	//		//{
	//		//	foreach (var weekday in weekdays)
	//		//	{
	//		//		var schedule = child.FindSchedule(weekday.Date);
	//		//		if (schedule != null)
	//		//		{
	//		//			var detail = schedule.GetActiveSchedule(weekday.Date);
						
	//		//			if (detail.ThisMorning(weekday.Date))
	//		//			{
	//		//				weekday.MorningCount++;
	//		//			}
						
	//		//			if (detail.ThisAfternoon(weekday.Date))
	//		//			{
	//		//				weekday.AfternoonCount++;
	//		//			}
	//		//		}
	//		//	}
	//		//}

	//	}

	//	public IEnumerable<DateTime> GetViewableDays()
	//	{
	//		var day = _startView;
	//		do
	//		{
	//			yield return day;
	//			day = day.AddDays(1);
	//		} while (day != _endView);
	//		yield return day;
	//	}

	//	public void ToggleMorningHolidayAction(DayUI ui)
	//	{
	//		if (ui.Weekday == false)
	//		{
	//			return;
	//		}

	//		ui.MorningHoliday = !ui.MorningHoliday;
			
	//		//	TODO

	//		//var model = ServiceProvider.Instance.GetService<Petoeter>();
	//		//model.SetHoliday(ui.Date, ui.MorningHoliday, ui.AfternoonHoliday);
	//	}

	//	public void ToggleAfternoonHolidayAction(DayUI ui)
	//	{
	//		if (ui.Weekday == false)
	//		{
	//			return;
	//		}

	//		ui.AfternoonHoliday = !ui.AfternoonHoliday;


	//		//	TODO
	//		//var model = ServiceProvider.Instance.GetService<Petoeter>();
	//		//model.SetHoliday(ui.Date, ui.MorningHoliday, ui.AfternoonHoliday);
	//	}

	//	public void ToggleHolidayAction(DayUI ui)
	//	{
	//		if (ui.Weekday == false)
	//		{
	//			return;
	//		}

	//		ui.MorningHoliday = !ui.MorningHoliday;
	//		ui.AfternoonHoliday = !ui.AfternoonHoliday;

	//		//	TODO
	//		//var model = ServiceProvider.Instance.GetService<Petoeter>();
	//		//model.SetHoliday(ui.Date, ui.MorningHoliday, ui.AfternoonHoliday);
	//	}
	//}
}
