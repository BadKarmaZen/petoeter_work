using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core
{
	public class DatePeriod
	{
		public DateTime Start { get; set; }
		public DateTime End { get; set; }

		public static bool Collision(DatePeriod period1, DatePeriod period2)
		{
			if (period2.Start < period1.Start)
			{
				return period2.End >= period1.Start;
			}

			if (period2.End >= period1.End)
			{
				return period2.Start < period1.End;				
			}

			return true;
		}

		public bool Collision(DatePeriod period)
		{
			return Collision(this, period);
		}

		public static DatePeriod MakePeriod(int month, int year)
		{
			return new DatePeriod 
			{
				Start = new DateTime(year,month,1),
				End = new DateTime(year,month,1).AddMonths(1).AddDays(-1)
			};
		}
	}
}
