using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core.Invoicing
{
	public class ExpenseRecord
	{
		public int FullDay { get; set; }
		public int HalfDay { get; set; }
		public int FullSickDay { get; set; }
		public int HalfSickDay { get; set; }
		public int ExtraMeal { get; set; }
		public int ExtraHour { get; set; }
		public int Diapers { get; set; }
		public int Medication { get; set; }
		public int ToLatePickup { get; set; }		
	}
}
