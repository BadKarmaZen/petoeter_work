using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DayCare.Model
{
	public class DataObject
	{
		public Guid Id { get; set; }
		public bool Deleted { get; set; }
		public bool Updated { get; set; }
		public bool Added { get; set; }
	}
}
