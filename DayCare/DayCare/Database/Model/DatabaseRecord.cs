using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Database.Model
{
	public class DatabaseRecord
	{
		public Guid Id { get; set; }
		public bool Deleted { get; set; }
		public DateTime Updated { get; set; }
	}
}
