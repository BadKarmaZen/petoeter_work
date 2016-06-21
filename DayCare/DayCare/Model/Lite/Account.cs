using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Lite
{
	public class Account : LiteRecord
	{
		public string Name { get; set; }

		public List<Member> Members { get; set; }
		public List<Child> Children { get; set; }

		public Account()
		{
			Members = new List<Member>();
			Children = new List<Child>();
		}
	}

}
