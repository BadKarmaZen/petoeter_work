using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DayCare.Model.Database
{
	public class Account : DatabaseRecord
	{
		public string Name { get; set; }

		[DatabaseIgnore]
		[XmlIgnore]
		public List<Child> Children { get; set; }

		[DatabaseIgnore]
		[XmlIgnore]
		public List<Member> Members { get; set; }

		public Account()
		{
			Children = new List<Child>();
			Members = new List<Member>();
		}
	}
}
