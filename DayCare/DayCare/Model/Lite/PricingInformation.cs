using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Lite
{
	public class PricingInformation : LiteRecord
	{
		public DateTime Start { get; set; }
		public DateTime End { get; set; }

		public double FullDay { get; set; }
		public double HalfDay { get; set; }
		public double ExtraMeal { get; set; }
		public double ExtraHour { get; set; }
		public double Diapers { get; set; }
		public double Medication { get; set; }
		public double ToLate { get; set; }
		public double FullDaySick { get; set; }
		public double HalfDaySick { get; set; }
	}
}
