using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Lite
{
	public class Member : LiteRecord
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Phone { get; set; }
	}

}
