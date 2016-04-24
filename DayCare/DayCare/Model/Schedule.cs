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

		public ScheduleDetail GetActiveSchedule(DateTime date)
		{
			var weeks = (date - StartDate).Days / 7;
			var index = weeks % Details.Count;

			return Details[index];
		}

		public bool IsVoid()
		{
			return Details.All(d => d.IsVoid());
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
			return (morning ? Morning : 0) |
							(afternoon ? Afternoon : 0);
		}


		internal bool ThisMorning(DateTime date)
		{
			switch (date.DayOfWeek)
			{
				case DayOfWeek.Friday:
					return FridayMorning;
				case DayOfWeek.Monday:
					return MondayMorning;
				case DayOfWeek.Saturday:
					return false;
				case DayOfWeek.Sunday:
					return false;
				case DayOfWeek.Thursday:
					return ThursdayMorning;
				case DayOfWeek.Tuesday:
					return TuesdayMorning;
				case DayOfWeek.Wednesday:
					return WednesdayMorning;
			}
			return false;
		}

		internal bool ThisAfternoon(DateTime date)
		{
			switch (date.DayOfWeek)
			{
				case DayOfWeek.Friday:
					return FridayAfternoon;
				case DayOfWeek.Monday:
					return MondayAfternoon;
				case DayOfWeek.Saturday:
					return false;
				case DayOfWeek.Sunday:
					return false;
				case DayOfWeek.Thursday:
					return ThursdayAfternoon;
				case DayOfWeek.Tuesday:
					return TuesdayAfternoon;
				case DayOfWeek.Wednesday:
					return WednesdayAfternoon;
			}
			return false;
		}

		public bool IsVoid()
		{
			return !(MondayMorning || MondayAfternoon || TuesdayMorning || TuesdayAfternoon || WednesdayMorning || WednesdayAfternoon || ThursdayMorning || ThursdayAfternoon || FridayMorning || FridayAfternoon);
		}

		public int GetTimeCode(DateTime today)
		{
			var fullDay = 9;
			var halfDay = 6;

			return ThisAfternoon(today) && ThisMorning(today) ? fullDay :
				ThisAfternoon(today) || ThisMorning(today) ? halfDay : 0;
		}
	}
}
