using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DayCare.Model
{
	public class Member : DataObject
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Phone { get; set; }

		public Account Account { get; set; }
	}
}
