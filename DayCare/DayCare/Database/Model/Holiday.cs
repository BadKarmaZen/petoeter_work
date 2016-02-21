using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Database.Model
{
	class Holiday : DatabaseRecord
	{
		public DateTime Date { get; set; }

		public Holiday()
		{
			//	Auto assign
			Id = Guid.NewGuid();
		}
	}
}
