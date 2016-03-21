using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model
{
	public class Account : DataObject
	{
		public string Name { get; set; }
		public List<Child> Children { get; set; }
		public List<Member> Members { get; set; }

		public Account()
		{
			Children = new List<Child>();
			Members = new List<Member>();
		}
	}
}
