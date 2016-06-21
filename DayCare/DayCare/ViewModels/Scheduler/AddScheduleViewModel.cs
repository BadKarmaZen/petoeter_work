using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Lite;
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
		private DateTime _startdate;
		private DateTime _enddate;

		private List<WeekScheduleViewModel> _details;

		#endregion

		#region Properties

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
				_startdate = value.Value;
				NotifyOfPropertyChange(() => StartDate);
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

			this.StartDate = DateTimeProvider.Now();
			this.EndDate = DateTimeProvider.Now();

			Details = new List<WeekScheduleViewModel>
			{
				new WeekScheduleViewModel(1)
			};
		}

		public void SaveAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
					new Events.ShowDialog());

			int weekIndex = 0;
			var startDate = StartDate.Value.Date;
			var endDate = EndDate.Value.Date;

			//	make full weeks
			if (startDate.DayOfWeek != DayOfWeek.Monday)
			{
				startDate = startDate.PreviousMonday();				
			}
			if (endDate.DayOfWeek != DayOfWeek.Friday)
			{
				endDate = endDate.NextFriday();
			}

			var weekDate = startDate;

			while (weekDate < endDate)
			{
				//	add week
				var period = _details[weekIndex % _details.Count];

				//	monday to friday
				for (int day = 0; day < 5; day++)
				{
					if (StartDate.Value.Date <= weekDate && weekDate <= EndDate.Value.Date)
					{
						if (period.Schedule[day].Morning || period.Schedule[day].Afternoon)
						{
							_child.Schedule.RemoveAll(d => d.Day == weekDate);
							_child.Schedule.Add(new Model.Lite.Date 
							{
 								Day = weekDate,
								Morning = period.Schedule[day].Morning,
								Afternoon = period.Schedule[day].Afternoon
							});
						}						
					}
					
					weekDate = weekDate.AddDays(1);	
				}
				//	increment week
				weekIndex++;
				weekDate = weekDate.AddDays(2);				
			}

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				db.Children.Update(_child);
			}
			
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new EditChildCalendarViewModel(_child)
				});
		}

		public void CancelAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Events.ShowDialog());
						
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new EditChildCalendarViewModel(_child)
				});
		}

		public void OneWeekAction()
		{
			Details = new List<WeekScheduleViewModel>(Details.Take(1));
		}

		public void TwoWeekAction()
		{
			var details = new List<WeekScheduleViewModel>(Details.Take(2));

			for (int index = Details.Count + 1; index <= 2; index++)
			{
				details.Add(new WeekScheduleViewModel(index));
			}

			Details = details;
		}

		public void ThreeWeekAction()
		{
			var details = new List<WeekScheduleViewModel>(Details.Take(2));

			for (int index = Details.Count + 1; index <= 3; index++)
			{
				details.Add(new WeekScheduleViewModel(index));
			}

			Details = details;
		}

		public void FourWeekAction()
		{
			var details = new List<WeekScheduleViewModel>(Details.Take(3));

			for (int index = Details.Count + 1; index <= 4; index++)
			{
				details.Add(new WeekScheduleViewModel(index));
			}

			Details = details;
		}


		//public DateTime Min(DateTime left, DateTime right)
		//{
		//	if (left < right)
		//	{
		//		return left;
		//	}
		//	else
		//	{
		//		return right;
		//	}
		//}

		//public DateTime Max(DateTime left, DateTime right)
		//{
		//	if (left > right)
		//	{
		//		return left;
		//	}
		//	else
		//	{
		//		return right;
		//	}
		//}
	}
}
