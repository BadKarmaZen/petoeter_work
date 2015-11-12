using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Database
{
	public class GroupSchedule
	{
		public List<Schedule> Schedules { get; set; }

		public Schedule CurrentSchedule { get { return GetScheduleOn(DateTime.Today); } }

		public int ScheduleCount { get{ return Schedules.Count; } }

		public GroupSchedule(IEnumerable<Schedule> schedules)
		{
			Schedules = new List<Schedule>(schedules);

			//	first
			var start = Schedules[0].StartDate;
			while (start.DayOfWeek != DayOfWeek.Monday)
				start = start.AddDays(-1);

			StartDate = start;

			var end = Schedules[0].EndDate;
			while (end.DayOfWeek != DayOfWeek.Friday)
				end = end.AddDays(1);

			EndDate = end;
		}

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public Schedule GetScheduleOn(DateTime date)
		{
			var index = ((date - StartDate).Days / 7) % ScheduleCount;

			return Schedules[index];
		}
	}
}
