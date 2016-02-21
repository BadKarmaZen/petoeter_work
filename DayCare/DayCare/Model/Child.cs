using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DayCare.Model
{
	public class Child : DataObject
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime BirthDay { get; set; }
	}
}
