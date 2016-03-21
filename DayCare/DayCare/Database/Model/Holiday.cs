using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Database.Model
{
	public class Holiday : DatabaseRecord
	{
		public DateTime Date { get; set; }
		public int Mask { get; set; }

		public Holiday()
		{
			//	Auto assign
			Id = Guid.NewGuid();
		}
	}
}
