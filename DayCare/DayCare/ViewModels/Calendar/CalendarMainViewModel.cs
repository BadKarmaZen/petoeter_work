using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Calendar
{
	public class CalendarMainViewModel : Screen
	{
		private MonthViewModel _month;
		private DateTime _selectedDate;

		public DateTime SelectedDate
		{
			get { return _selectedDate; }
			set { _selectedDate = value; NotifyOfPropertyChange(() => SelectedDate); NotifyOfPropertyChange(() => SelectedMonth); }
		}

		public MonthViewModel Month
		{
			get { return _month; }
			set { _month = value; NotifyOfPropertyChange(() => Month); }
		}

		public string SelectedMonth
		{
			get
			{
				return SelectedDate.ToString("MMMM yyyy");
			}
		}

		public CalendarMainViewModel()
		{
			SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
			Month = new MonthViewModel(SelectedDate);
		}

		public void NextMonthAction()
		{
			SelectedDate = SelectedDate.AddMonths(1);
			Month = new MonthViewModel(SelectedDate);
		}

		public void PrevousMonthAction()
		{
			SelectedDate = SelectedDate.AddMonths(-1);
			Month = new MonthViewModel(SelectedDate);
		}
	}
}
