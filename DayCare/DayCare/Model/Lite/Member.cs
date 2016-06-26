using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Lite
{
	public class Member : LiteRecord, IFullName
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Phone { get; set; }

		public string GetFullName(bool firstName = true)
		{
			return string.Format(firstName ? "{0} {1}" : "{1} {0}", FirstName, LastName);
		}
	}

}
