using Caliburn.Micro;
using DayCare.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Scheduler
{
	public class WeekScheduleViewModel : Screen
	{
		private ScheduleDetail _schedule;

		public ScheduleDetail Schedule
		{
			get { return _schedule; }
			set { _schedule = value; }
		}

		#region Properties

		public string Header { get; set; }
		
		public bool MondayMorning
		{
			get { return _schedule.MondayMorning; }
			set { _schedule.MondayMorning = value; NotifyOfPropertyChange(() => MondayMorning); }
		}
		public bool MondayAfternoon
		{
			get { return _schedule.MondayAfternoon; }
			set { _schedule.MondayAfternoon = value; NotifyOfPropertyChange(() => MondayAfternoon); }
		}

		public bool TuesdayMorning
		{
			get { return _schedule.TuesdayMorning; }
			set { _schedule.TuesdayMorning = value; NotifyOfPropertyChange(() => TuesdayMorning); }
		}
		public bool TuesdayAfternoon
		{
			get { return _schedule.TuesdayAfternoon; }
			set { _schedule.TuesdayAfternoon = value; NotifyOfPropertyChange(() => TuesdayAfternoon); }
		}

		public bool WednesdayMorning
		{
			get { return _schedule.WednesdayMorning; }
			set { _schedule.WednesdayMorning = value; NotifyOfPropertyChange(() => WednesdayMorning); }
		}
		public bool WednesdayAfternoon
		{
			get { return _schedule.WednesdayAfternoon; }
			set { _schedule.WednesdayAfternoon = value; NotifyOfPropertyChange(() => WednesdayAfternoon); }
		}

		public bool ThursdayMorning
		{
			get { return _schedule.ThursdayMorning; }
			set { _schedule.ThursdayMorning = value; NotifyOfPropertyChange(() => ThursdayMorning); }
		}
		public bool ThursdayAfternoon
		{
			get { return _schedule.ThursdayAfternoon; }
			set { _schedule.ThursdayAfternoon = value; NotifyOfPropertyChange(() => ThursdayAfternoon); }
		}

		public bool FridayMorning
		{
			get { return _schedule.FridayMorning; }
			set { _schedule.FridayMorning = value; NotifyOfPropertyChange(() => FridayMorning); }
		}
		public bool FridayAfternoon
		{
			get { return _schedule.FridayAfternoon; }
			set { _schedule.FridayAfternoon = value; NotifyOfPropertyChange(() => FridayAfternoon); }
		}

		#endregion

		public WeekScheduleViewModel(ScheduleDetail schedule)
		{
			_schedule = schedule;
		}
	}
}
