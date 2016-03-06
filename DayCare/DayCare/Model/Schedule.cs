using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model
{
	public	class Schedule : DataObject
	{
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public Child Child { get; set; }

		public List<ScheduleDetail> Details { get; set; }
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
	}
}
