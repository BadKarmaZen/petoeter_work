using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Database
{
	public class Holiday : DatabaseRecord
	{
		public DateTime Date { get; set; }

		public Holiday()
		{
			Id = Guid.NewGuid();
		}
	}
}
