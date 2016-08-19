using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core.Invoicing
{
	public class InvoicePriceDetails
	{
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
