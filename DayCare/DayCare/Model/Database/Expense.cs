using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Database
{
	public class Expense : DatabaseRecord
	{
		public Guid ChildID { get; set; }
		public DateTime Date { get; set; }
		public bool Lunch { get; set; }
		public int Diapers { get; set; }
		public int Medication { get; set; }
		public bool ToLate { get; set; }
	}
}
