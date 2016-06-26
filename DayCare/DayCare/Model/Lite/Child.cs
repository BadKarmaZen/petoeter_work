using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Lite
{
	public class Child : LiteRecord, IFullName
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public DateTime BirthDay { get; set; }

		public List<Date> Schedule { get; set; }

		public string FileId { get; set; }

		public Child()
		{
			Schedule = new List<Date>();
		}

		public string GetFullName(bool firstName = true)
		{
			return string.Format(firstName ? "{0} {1}" : "{1} {0}", FirstName, LastName);
		}
	}
}
