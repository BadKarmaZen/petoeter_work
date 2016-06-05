using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Lite
{
	public class Date : LiteRecord
	{
		public DateTime Day { get; set; }
		public bool Afternoon { get; set; }
		public bool Morning { get; set; }
	}

}
