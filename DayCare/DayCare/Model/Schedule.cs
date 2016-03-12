using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model
{
	public class Schedule : DataObject
	{
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public Child Child { get; set; }

		public List<ScheduleDetail> Details { get; set; }

		public Schedule MakeCopy()
		{
			var copy = new Schedule
			{
				Id = Guid.NewGuid(),
				StartDate = this.StartDate,
				EndDate = this.EndDate,
				Child = this.Child
			};
			copy.Details = new List<ScheduleDetail>(
					from d in this.Details
					select new ScheduleDetail
					{
						Id = Guid.NewGuid(),
						Index = d.Index,
						Schedule = copy,
						MondayMorning = d.MondayMorning,
						MondayAfternoon = d.MondayAfternoon,
						TuesdayMorning = d.TuesdayMorning,
						TuesdayAfternoon = d.TuesdayAfternoon,
						WednesdayMorning = d.WednesdayMorning,
						WednesdayAfternoon = d.WednesdayAfternoon,
						ThursdayMorning = d.ThursdayMorning,
						ThursdayAfternoon = d.ThursdayAfternoon,
						FridayMorning = d.FridayMorning,
						FridayAfternoon = d.FridayAfternoon
					});

			return copy;
		}
	}

	public class ScheduleDetail : DataObject
	{
		public const int Morning = 0x01;
		public const int Afternoon = 0x02;

		public int Index { get; set; }

		public Schedule Schedule { get; set; }

		public bool MondayMorning { get; set; }
		public bool MondayAfternoon { get; set; }
		public bool TuesdayMorning { get; set; }
		public bool TuesdayAfternoon { get; set; }
		public bool WednesdayMorning { get; set; }
		public bool WednesdayAfternoon { get; set; }
		public bool ThursdayMorning { get; set; }
		public bool ThursdayAfternoon { get; set; }
		public bool FridayMorning { get; set; }
		public bool FridayAfternoon { get; set; }

		public static bool IsMorning(int day)
		{
			return Morning == (day & Morning);
		}

		public static bool IsAfternoon(int day)
		{
			return Afternoon == (day & Afternoon);
		}

		public static int DayState(bool morning, bool afternoon)
		{
			return	(morning ? Morning : 0) |
							(afternoon ? Afternoon : 0);
		}

	}
}
