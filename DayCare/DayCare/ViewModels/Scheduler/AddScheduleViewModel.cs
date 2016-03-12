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
		#region Members

		private Child _child;
		private Schedule _schedule;
		private Schedule _original;

		private DateTime _startdate;
		private DateTime _enddate;
		private bool _showWeekSelection;

		private List<WeekScheduleViewModel> _details;
		private DateTime _minStartdate;
		private DateTime _maxStartdate;
		private DateTime _minEnddate;
		private DateTime _maxEnddate;

		#endregion

		#region Properties
		
		public bool ShowWeekSelection
		{
			get { return _showWeekSelection; }
			set { _showWeekSelection = value; NotifyOfPropertyChange(() => ShowWeekSelection); }
		}
		
		public bool Edit { get; set; }

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
			set 
			{
				var newDate = value.Value;
				if (newDate > MinStartdate)
				{
					_startdate = newDate; 
					NotifyOfPropertyChange(() => StartDate);

					EndDate = _startdate.AddDays(5);
				}				
			}
		}

		public List<WeekScheduleViewModel> Details
		{
			get { return _details; }
			set { _details = value; NotifyOfPropertyChange(() => Details); }
		}

		#endregion

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
						Index = 0
					})
				{
					Header = "Week 1"
				} 
			};
			ShowWeekSelection = true;
		}

		public AddScheduleViewModel(Child child, Schedule schedule, Schedule exception = null)
		{
			this._child = child;
			this._schedule = schedule;
			this._original = exception;

			if (this._original != null)
			{
				var today = DateTime.Now.Date;
				if (today.DayOfWeek != DayOfWeek.Monday)
					today = today.PreviousMonday();

				if (today < _original.StartDate)
				{
					today = _original.StartDate;
				}

				this.StartDate = today;
				this.MinStartdate = today;
				this.MaxEnddate = schedule.EndDate;
			}
			else
			{
				this.StartDate = schedule.StartDate;
				this.EndDate = schedule.EndDate;
			}

			var query = from d in schedule.Details
									orderby d.Index
									select new WeekScheduleViewModel(d)
									{
										Header = string.Format("Week {0}", d.Index + 1)
									};
			Details = query.ToList();

			Edit = true;
			ShowWeekSelection = false;
		}

		public void SaveAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
					new Events.ShowDialog());
			
			var model = ServiceProvider.Instance.GetService<Petoeter>();

			if (_original != null)
			{
				//	4 exceptions possible
				//
				//	1.	[===========]
				//		      [+++]
				//		  [==][+++][==]		org.Left exc org.Right
				//
				//	2.	[===========]
				//			[+++]	
				//	    [+++][======]		exc org.Right
				//
				//	3.	[===========]
				//			        [+++]	
				//			[======][+++]		org.left exc
				//	
				//	4.  [===========]
				//			[+++++++++++]
				//			[+++++++++++]		update org
				//

				if (_startdate == _original.StartDate)
				{
					//	2 or 4
					//
					if (_enddate == _original.EndDate)
					{
						//	4
						_original.Updated = true;
						_original.Details = (from d in Details
																 select d.Schedule).ToList();						
					}
					else
					{
						//	2
						_schedule.StartDate = _startdate;
						_schedule.EndDate = _enddate;
						_schedule.Details = (from d in Details
																 select d.Schedule).ToList();

						_child.Schedules.Add(_schedule);
						model.AddSchedule(_schedule);

						_original.StartDate = _schedule.EndDate.NextMonday();
						_original.Updated = true;
					}					
				}
				else
				{
					//	1 or 3
					if (_enddate == _original.EndDate)
					{
						//	3
						_schedule.StartDate = _startdate;
						_schedule.EndDate = _enddate;
						_schedule.Details = (from d in Details
																 select d.Schedule).ToList();

						_child.Schedules.Add(_schedule);
						model.AddSchedule(_schedule);

						_original.EndDate = _schedule.StartDate.PreviousFriday();
						_original.Updated = true;
					}
					else
					{
 						//	1
						_schedule.StartDate = _startdate;
						_schedule.EndDate = _enddate;
						_schedule.Details = (from d in Details
																 select d.Schedule).ToList();
						
						_child.Schedules.Add(_schedule);
						model.AddSchedule(_schedule);

						var right = _original.MakeCopy();
						right.StartDate = _schedule.EndDate.NextMonday();

						_child.Schedules.Add(right);
						model.AddSchedule(right);
						
						_original.EndDate = _schedule.StartDate.PreviousFriday();
						_original.Updated = true;
					}
				}
			}
			else
			{

				if (Edit == false)
				{
					var schedule = new Schedule
					{
						Id = Guid.NewGuid(),
						Child = _child,
						StartDate = _startdate,
						EndDate = _enddate,
						Details = (from d in Details
											 select d.Schedule).ToList()
					};

					_child.Schedules.Add(schedule);
					model.AddSchedule(schedule);
				}
				else
				{
					_schedule.Updated = true;

					_schedule.StartDate = _startdate;
					_schedule.EndDate = _enddate;
					_schedule.Details = (from d in Details
															 select d.Schedule).ToList();

					//foreach (var detail in Details)
					//{
					//	detail.Schedule.Deleted = true;
					//}
				}
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
			Details = new List<WeekScheduleViewModel>(Details.Take(1));
		}

		public void TwoWeekAction()
		{
			var details = new List<WeekScheduleViewModel>(Details.Take(2));

			for (int index = Details.Count; index < 2; index++)
			{
				details.Add(new WeekScheduleViewModel(new ScheduleDetail() { Id = Guid.NewGuid(), Index = index }) { Header = string.Format("Week {0}", index + 1) });
			}

			Details = details;
		}

		public void ThreeWeekAction()
		{
			var details = new List<WeekScheduleViewModel>(Details.Take(2));

			for (int index = Details.Count; index < 3; index++)
			{
				details.Add(new WeekScheduleViewModel(new ScheduleDetail() { Id = Guid.NewGuid(),  Index = index }) { Header = string.Format("Week {0}", index + 1) });
			}

			Details = details;
		}

		public void FourWeekAction()
		{
			var details = new List<WeekScheduleViewModel>(Details.Take(2));

			for (int index = Details.Count; index < 4; index++)
			{
				details.Add(new WeekScheduleViewModel(new ScheduleDetail() { Id = Guid.NewGuid(),  Index = index }) { Header = string.Format("Week {0}", index + 1) });
			}

			Details = details;
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
