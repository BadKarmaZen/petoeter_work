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
	class ScheduleExceptionViewModel : Screen
	{		
		#region Members

		private Child _child;
		private Schedule _schedule;
		private Schedule _original;

		private DateTime _startdate;
		private DateTime _enddate;
		private bool _showWeekSelection;
		private bool _autoUpdateEnddate;

		private List<WeekScheduleViewModel> _details;
		private DateTime _minStartdate;
		private DateTime _maxStartdate;
		private DateTime _minEnddate;
		private DateTime _maxEnddate;

		private List<ScheduleDetail> _updatedDetails;

		#endregion

		#region Properties

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
			set 
			{
				_autoUpdateEnddate = false;
				_enddate = value.Value; 
				NotifyOfPropertyChange(() => EndDate);
				ReorderWeekDetails();
			}
		}

		public DateTime? StartDate
		{
			get { return _startdate; }
			set 
			{
				var newDate = value.Value;
				if (newDate >= MinStartdate)
				{
					_startdate = newDate; 
					NotifyOfPropertyChange(() => StartDate);

					if (_autoUpdateEnddate)
					{
						EndDate = _startdate.AddDays(4);
						_autoUpdateEnddate = true;
					}

					ReorderWeekDetails();
				}				
			}
		}

		public List<WeekScheduleViewModel> Details
		{
			get { return _details; }
			set { _details = value; NotifyOfPropertyChange(() => Details); }
		}

		#endregion

		public ScheduleExceptionViewModel(Child child, Schedule exception)
		{
			this._child = child;
			this._schedule = exception.MakeCopy();
			this._original = exception;

			_autoUpdateEnddate = true;
			_updatedDetails = _schedule.Details.ToList();

			var today = DateTime.Now.Date;
			if (today.DayOfWeek != DayOfWeek.Monday)
			{
				today = today.PreviousMonday();
			}

			if (today < _original.StartDate)
			{
				today = _original.StartDate;
			}

			this.StartDate = today;
			this.MinStartdate = today;
			this.MaxEnddate = exception.EndDate;


			ReorderWeekDetails();
		}

		public void ReorderWeekDetails()
		{
			Details = GetWeekDetails(_updatedDetails, _startdate, _enddate).ToList();
		}

		public IEnumerable<WeekScheduleViewModel> GetWeekDetails(List<ScheduleDetail> source, DateTime start, DateTime end)
		{
			var weeks = (start - _original.StartDate).Days / 7;
			var count = 1 + (end - start).Days / 7;
			var indexes = Enumerable.Range(0, source.Count);

			var neworder = indexes.Skip(weeks).ToList();
			neworder.AddRange(indexes.Take(weeks));

			int newindex = 1;

			foreach (var index in neworder.Take(count))
			{
				yield return new WeekScheduleViewModel(source[index])
					{
						Header = string.Format("Week {0}", newindex++)
					};
			}
		}

		public void SaveAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Events.ShowDialog());

			var model = ServiceProvider.Instance.GetService<Petoeter>();

			//	4 exceptions possible
			//
			//	1.	[===========]
			//		      [+++]
			//		  [==][+++][==]		org.Left exc org.Right
			//	1.a			[	  ]
			//			[==]     [==]   org.left --- org.right    
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
					_original.Details = (from d in GetWeekDetails(_original.Details, _original.StartDate, _original.EndDate)
															 select d.Schedule).ToList();
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
					_original.Details = (from d in GetWeekDetails(_original.Details, _original.StartDate, _original.EndDate)
															 select d.Schedule).ToList();
					_original.Updated = true;
				}
				else
				{
					//	1
					_schedule.StartDate = _startdate;
					_schedule.EndDate = _enddate;
					_schedule.Details = (from d in Details
															 select d.Schedule).ToList();

					if (!_schedule.IsVoid())
					{
						_child.Schedules.Add(_schedule);
						model.AddSchedule(_schedule);						
					}


					var right = _original.MakeCopy();
					right.StartDate = _schedule.EndDate.NextMonday();
					right.Details = (from d in GetWeekDetails(right.Details, right.StartDate, right.EndDate)
													 select d.Schedule).ToList();

					_child.Schedules.Add(right);
					model.AddSchedule(right);

					_original.EndDate = _schedule.StartDate.PreviousFriday();
					_original.Details = (from d in GetWeekDetails(_original.Details, _original.StartDate, _original.EndDate)
															 select d.Schedule).ToList();
					_original.Updated = true;
				}
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
	}
}
