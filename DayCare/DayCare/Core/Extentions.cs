using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DayCare.Core
{
	static class Date
	{
		public static DateTime NextMonday(DateTime? date = null)
		{
			if (!date.HasValue)
			{
				date = DateTime.Now;				
			}

			return date.Value.NextMonday();
		}

		public static DateTime NextMonday(this DateTime date)
		{
			while (date.DayOfWeek != DayOfWeek.Monday)
			{
				date = date.AddDays(1);
			}

			return date;
		}

		public static DateTime NextFriday(this DateTime? date)
		{
			if (!date.HasValue)
			{
				date = DateTime.Now;
			}

			return date.Value.NextFriday();
		}

		public static DateTime NextFriday(this DateTime date)
		{
			while (date.DayOfWeek != DayOfWeek.Friday)
			{
				date = date.AddDays(1);
			}

			return date;

		}

		public static DateTime PreviousMonday(DateTime? date = null)
		{
			if (!date.HasValue)
			{
				date = DateTime.Now;
			}

			return date.Value.PreviousMonday();
		}

		public static DateTime PreviousMonday(this DateTime date)
		{
			while (date.DayOfWeek != DayOfWeek.Monday)
			{
				date = date.AddDays(-1);
			}

			return date;
		}

		public static DateTime PreviousFriday(this DateTime? date)
		{
			if (!date.HasValue)
			{
				date = DateTime.Now;
			}

			return date.Value.PreviousFriday();
		}

		public static DateTime PreviousFriday(this DateTime date)
		{
			while (date.DayOfWeek != DayOfWeek.Friday)
			{
				date = date.AddDays(-1);
			}

			return date;

		}
	}
}
