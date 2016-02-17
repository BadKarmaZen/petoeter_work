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

			while (date.Value.DayOfWeek != DayOfWeek.Monday)
			{
				date = date.Value.AddDays(1);				
			}

			return date.Value;
		}
	}
}
