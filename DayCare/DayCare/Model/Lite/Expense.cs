using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Lite
{
	public class Expense
	{
		public bool FullDay { get; set; }
		public bool ExtraMeal { get; set; }
		public bool ToLate { get; set; }
		public int Diapers { get; set; }
		public int ExtraHour { get; set; }
		public bool ExtraHourOverride { get; set; }
		public bool Sick { get; set; }
		public bool SicknessNotNotified { get; set; }
		public int Medication { get; set; }
	}
}
