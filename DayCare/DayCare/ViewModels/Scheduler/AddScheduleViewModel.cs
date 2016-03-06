using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model;
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
		private bool _showWeekSelection;

		public bool ShowWeekSelection
		{
			get { return _showWeekSelection; }
			set { _showWeekSelection = value; NotifyOfPropertyChange(() => ShowWeekSelection); }
		}

		private List<WeekScheduleViewModel> _details;

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

		public List<WeekScheduleViewModel> Details
		{
			get { return _details; }
			set { _details = value; NotifyOfPropertyChange(() => Details); }
		}

		public AddScheduleViewModel(Child child)
		{
			this._child = child;

			this.StartDate = DateTime.Now;
			this.EndDate = DateTime.Now;

			Details = new List<WeekScheduleViewModel> 
			{ 
				new WeekScheduleViewModel(new ScheduleDetail() 
					{ 
						Id = Guid.NewGuid(),
						Index = 1
					})
				{
					Header = "Week 1"
				} 
			};
			ShowWeekSelection = true;
		}

		public AddScheduleViewModel(Child child, Schedule schedule)
		{
			this._child = child;
			this._schedule = schedule;

			this.StartDate = schedule.StartDate;
			this.EndDate = schedule.EndDate;

			var query = from d in schedule.Details
									orderby d.Index
									select new WeekScheduleViewModel(d)
									{
										Header = string.Format("Week {0}", d.Index + 1)
									};
			Details = query.ToList();

			Edit = true;
			ShowWeekSelection = false;

			/*
			this.MinStartdate = Min(DateTime.Now.Date, grpSchedule.StartDate);
			this.MaxStartdate = grpSchedule.EndDate;

			this.MinEnddate = grpSchedule.StartDate;

			var query = from s in grpSchedule.Schedules
									where s.StartDate > _schedule.EndDate
									orderby s.StartDate
									select s.StartDate;

			this.MaxEnddate = Max(DateTime.MaxValue, query.FirstOrDefault());
			Schedules = (from s in grpSchedule.Schedules
									 orderby s.Group_Index
									 select new WeekScheduleViewModel(s) { Header = string.Format("Week {0}", s.Group_Index + 1) }).ToList();

			Edit = true;
			ShowWeekSelection = false;*/
		}

		public void SaveAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
					new Events.ShowDialog());
			
			var model = ServiceProvider.Instance.GetService<Petoeter>();

			if (Edit == false)
			{
				var schedule = new Schedule
				{
					Id = Guid.NewGuid(),
					StartDate = _startdate,
					EndDate = _enddate,
					Details = (from d in Details
										 select d.Schedule).ToList()
				};

				model.AddSchedule(schedule);
			}
			else
			{
				_schedule.Updated = true;

				_schedule.StartDate = _startdate;
				_schedule.EndDate = _enddate;

				foreach (var detail in Details)
				{
					detail.Schedule.Deleted = true;
				}

			//	_schedule.Details = (from d in Details select d.Schedule).ToList()

			}
		
			model.SaveSchedules();

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

		public void OneWeekAction()
		{
			//Schedules = new List<WeekScheduleViewModel>(Schedules.Take(1));
		}

		public void TwoWeekAction()
		{
			//var schedules = new List<WeekScheduleViewModel>(Schedules.Take(2));

			//for (int index = Schedules.Count; index < 2; index++)
			//{
			//	schedules.Add(new WeekScheduleViewModel(new Schedule() { Group_Index = index }) { Header = string.Format("Week {0}", index + 1) });
			//}				

			//Schedules = schedules;
		}

		public void ThreeWeekAction()
		{
			//var schedules = new List<WeekScheduleViewModel>(Schedules.Take(2));
		
			//for (int index = Schedules.Count; index < 3; index++)
			//{
			//	schedules.Add(new WeekScheduleViewModel(new Schedule() { Group_Index = index }) { Header = string.Format("Week {0}", index + 1) });
			//}	

			//Schedules = schedules;
		}

		public void FourWeekAction()
		{
			//var schedules = new List<WeekScheduleViewModel>(Schedules.Take(2));

			//for (int index = Schedules.Count; index < 4; index++)
			//{
			//	schedules.Add(new WeekScheduleViewModel(new Schedule() { Group_Index = index }) { Header = string.Format("Week {0}", index + 1) });
			//}

			//Schedules = schedules;
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
