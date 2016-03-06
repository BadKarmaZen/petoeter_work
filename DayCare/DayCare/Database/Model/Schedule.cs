using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Database.Model
{
	public class Schedule : DatabaseRecord
	{
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public Guid Child_Id { get; set; }
	}
}
