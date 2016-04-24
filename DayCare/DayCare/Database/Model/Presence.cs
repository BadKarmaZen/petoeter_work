using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Database.Model
{
	public class Presence : DatabaseRecord
	{
		public Guid Child_Id { get; set; }

		public DateTime Created { get; set; }
		public string FullName { get; set; }
		public DateTime ArrivalTime { get; set; }
		public Guid ArrivalMember_Id { get; set; }
		public DateTime DepartureTime { get; set; }
		public Guid DepartureMember_Id { get; set; }
		public int TimeCode { get; set; }
	}
}
