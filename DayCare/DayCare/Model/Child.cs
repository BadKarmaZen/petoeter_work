using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DayCare.Model
{
	public class Child : DataObject
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime BirthDay { get; set; }
		public Account Account { get; set; }

		public List<Schedule> Schedules { get; set; }

		public bool HasValidPeriod(DateTime dayInPeriod)
		{
			var schedule = FindSchedule(dayInPeriod);
			return schedule != null;
		}

		public Schedule FindSchedule(DateTime dayInPeriod)
		{
			return (from s in Schedules
						 where s.StartDate <= dayInPeriod && s.EndDate >= dayInPeriod
						 select s).FirstOrDefault();
		}
	}
}
