using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Scheduler
{
	class AddScheduleViewModel : Screen
	{
		private Child _child;
		private Schedule _schedule;

		private DateTime _startdate;
		private DateTime _enddate;
		private bool _mondayMorning;
		private bool _mondayAfternoon;
		private bool _tuesdayMorning;
		private bool _tuesdayAfternoon;
		private bool _wednesdayMorning;
		private bool _wednesdayAfternoon;
		private bool _thursdayMorning;
		private bool _thursdayAfternoon;
		private bool _fridayMorning;
		private bool _fridayAfternoon;

		public bool Edit { get; set; }


		private DateTime _minStartdate;
		private DateTime _maxStartdate;
		private DateTime _minEnddate;
		private DateTime _maxEnddate;

		public DateTime MinStartdate
		{
			get { return _minStartdate; }
			set { _minStartdate = value; NotifyOfPropertyChange(() => MinStartdate); }
		}

		public DateTime MaxStartdate
		{
			get { return _maxStartdate; }
			set { _maxStartdate = value; NotifyOfPropertyChange(() => MaxStartdate); }
		}

		public DateTime MinEnddate
		{
			get { return _minEnddate; }
			set { _minEnddate = value; NotifyOfPropertyChange(() => MinEnddate); }
		}

		public DateTime MaxEnddate
		{
			get { return _maxEnddate; }
			set { _maxEnddate = value; NotifyOfPropertyChange(() => MaxEnddate); }
		}

		public DateTime? EndDate
		{
			get { return _enddate; }
			set { _enddate = value.Value; NotifyOfPropertyChange(() => EndDate); }
		}

		public DateTime? StartDate
		{
			get { return _startdate; }
			set { _startdate = value.Value; NotifyOfPropertyChange(() => StartDate); }
		}

		public bool MondayMorning
		{
			get { return _mondayMorning; }
			set { _mondayMorning = value; NotifyOfPropertyChange(() => MondayMorning); }
		}
		public bool MondayAfternoon
		{
			get { return _mondayAfternoon; }
			set { _mondayAfternoon = value; NotifyOfPropertyChange(() => MondayAfternoon); }
		}

		public bool TuesdayMorning
		{
			get { return _tuesdayMorning; }
			set { _tuesdayMorning = value; NotifyOfPropertyChange(() => TuesdayMorning); }
		}
		public bool TuesdayAfternoon
		{
			get { return _tuesdayAfternoon; }
			set { _tuesdayAfternoon = value; NotifyOfPropertyChange(() => TuesdayAfternoon); }
		}

		public bool WednesdayMorning
		{
			get { return _wednesdayMorning; }
			set { _wednesdayMorning = value; NotifyOfPropertyChange(() => WednesdayMorning); }
		}
		public bool WednesdayAfternoon
		{
			get { return _wednesdayAfternoon; }
			set { _wednesdayAfternoon = value; NotifyOfPropertyChange(() => WednesdayAfternoon); }
		}

		public bool ThursdayMorning
		{
			get { return _thursdayMorning; }
			set { _thursdayMorning = value; NotifyOfPropertyChange(() => ThursdayMorning); }
		}
		public bool ThursdayAfternoon
		{
			get { return _thursdayAfternoon; }
			set { _thursdayAfternoon = value; NotifyOfPropertyChange(() => ThursdayAfternoon); }
		}

		public bool FridayMorning
		{
			get { return _fridayMorning; }
			set { _fridayMorning = value; NotifyOfPropertyChange(() => FridayMorning); }
		}
		public bool FridayAfternoon
		{
			get { return _fridayAfternoon; }
			set { _fridayAfternoon = value; NotifyOfPropertyChange(() => FridayAfternoon); }
		}
		public AddScheduleViewModel(Child child)
		{
			// TODO: Complete member initialization
			this._child = child;

			this.StartDate = DateTime.Now;
			this.EndDate = DateTime.Now;
		}

		public AddScheduleViewModel(Child _child, Schedule schedule)
		{
			// TODO: Complete member initialization
			this._child = _child;
			this._schedule = schedule;


			this.StartDate = _schedule.StartDate;
			this.EndDate = _schedule.EndDate;

			this.MinStartdate = Min(DateTime.Now.Date, _schedule.StartDate);
			this.MaxStartdate = _schedule.EndDate;

			this.MinEnddate = _schedule.StartDate;

			var query = from s in ServiceProvider.Instance.GetService<Petoeter>().GetSchedule(s => s.Child_Id == _schedule.Child_Id)
									where s.StartDate > _schedule.EndDate
									orderby s.StartDate
									select s.StartDate;

			this.MaxEnddate = Max(DateTime.MaxValue, query.FirstOrDefault());

			MondayMorning = _schedule.MondayMorning;
			MondayAfternoon = _schedule.MondayAfternoon;
			TuesdayMorning = _schedule.TuesdayMorning;
			TuesdayAfternoon = _schedule.TuesdayAfternoon;
			WednesdayMorning = _schedule.WednesdayMorning;
			WednesdayAfternoon = _schedule.WednesdayAfternoon;
			ThursdayMorning = _schedule.ThursdayMorning;
			ThursdayAfternoon = _schedule.ThursdayAfternoon;
			FridayMorning = _schedule.FridayMorning;
			FridayAfternoon = _schedule.FridayAfternoon;

			Edit = true;
		}

		public void SaveAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
					new Events.ShowDialog());

			if (Edit == false)
			{
				var schedule = new Schedule
				{
					Id = Guid.NewGuid(),
					StartDate = _startdate,
					EndDate = _enddate,
					Child_Id = _child.Id,

					MondayMorning = this.MondayMorning,
					MondayAfternoon = this.MondayAfternoon,
					TuesdayMorning = this.TuesdayMorning,
					TuesdayAfternoon = this.TuesdayAfternoon,
					WednesdayMorning = this.WednesdayMorning,
					WednesdayAfternoon = this.WednesdayAfternoon,
					ThursdayMorning = this.ThursdayMorning,
					ThursdayAfternoon = this.ThursdayAfternoon,
					FridayMorning = this.FridayMorning,
					FridayAfternoon = this.FridayAfternoon
				};

				ServiceProvider.Instance.GetService<Petoeter>().SaveSchedule(schedule);
			}
			else
			{
				_schedule.StartDate = _startdate;
				_schedule.EndDate = _enddate;

				_schedule.MondayMorning = this.MondayMorning;
				_schedule.MondayAfternoon = this.MondayAfternoon;
				_schedule.TuesdayMorning = this.TuesdayMorning;
				_schedule.TuesdayAfternoon = this.TuesdayAfternoon;
				_schedule.WednesdayMorning = this.WednesdayMorning;
				_schedule.WednesdayAfternoon = this.WednesdayAfternoon;
				_schedule.ThursdayMorning = this.ThursdayMorning;
				_schedule.ThursdayAfternoon = this.ThursdayAfternoon;
				_schedule.FridayMorning = this.FridayMorning;
				_schedule.FridayAfternoon = this.FridayAfternoon;
				
				ServiceProvider.Instance.GetService<Petoeter>().UpdateRecord(_schedule);
			}

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
					new Core.Events.SwitchTask
					{
						Task = new EditChildScheduleViewModel(_child)
					});
		}

		public void CancelAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
					new Events.ShowDialog());

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
					new Core.Events.SwitchTask
					{
						Task = new EditChildScheduleViewModel(_child)
					});
		}

		public DateTime Min(DateTime left, DateTime right)
		{
			if (left < right)
			{
				return left;				
			}
			else
			{
				return right;
			}
		}

		public DateTime Max(DateTime left, DateTime right)
		{
			if (left > right)
			{
				return left;
			}
			else
			{
				return right;
			}
		}
	}
}
