using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Lite
{
	public class Child : LiteRecord
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public DateTime BirthDay { get; set; }

		public List<Date> Schedule { get; set; }

		public Child()
		{
			Schedule = new List<Date>();
		}
	}
}
