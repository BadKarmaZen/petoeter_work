using DayCare.Core;
using DayCare.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Database
{
	public class ExportData : XmlBaseConfig<ExportData>
	{
		public List<Account> Accounts { get; set; }

		public List<Member> Members { get; set; }

		public List<Child> Children { get; set; }

		public List<Schedule> Schedules { get; set; }

		public List<ScheduleDetail> ScheduleDetails { get; set; }

		public List<Holiday> Holidays { get; set; }
	}
}
