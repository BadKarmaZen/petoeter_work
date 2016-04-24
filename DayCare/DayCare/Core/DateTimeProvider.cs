using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core
{
	static class DateTimeProvider
	{
		static public Func<DateTime> Now { get; set; }

		static DateTimeProvider()
		{
			Now = () => DateTime.Now;
		}
	}
}
