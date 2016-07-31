using Caliburn.Micro;
using DayCare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Reports
{
	public class SelectMonthViewModel : Screen
	{
		private int _month;
		private List<string> _months;
		private List<int> _years;
		private int _year;
        public System.Action Yes { get; set; }

		public int Year
		{
			get { return _year; }
			set { _year = value; NotifyOfPropertyChange(()=>Year); }
		}

		public List<int> Years
		{
			get { return _years; }
			set { _years = value; NotifyOfPropertyChange(()=>Years);}
		}

		public List<string> Months
		{
			get { return _months; }
			set { _months = value; NotifyOfPropertyChange(() => Months); }
		}

		public int Month
		{
			get { return _month; }
			set { _month = value; NotifyOfPropertyChange(() => Month); }
		}
		public SelectMonthViewModel ()
		{
			LogManager.GetLog(GetType()).Info("Create");

			var date = DateTimeProvider.Now().Date.AddMonths(1);

			Years = Enumerable.Range(date.Year - 2, 22).ToList();
			Year = date.Year;

			Months = new List<string> { "Januari", "Februari", "Maart", "April",
																			"Mei", "Juni", "Juli", "Augustus", 
																			"September", "Oktober", "November", "December" };
			Month = date.Month - 1;
		}

		public void YesAction()
		{
			if (Yes != null)
			{
				Yes();
			}

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Events.ShowDialog());
		}

		public void NoAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Events.ShowDialog());
		}
	}
}
