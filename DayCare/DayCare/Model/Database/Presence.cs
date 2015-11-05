using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Database
{
	public class Presence
	{
		public Guid Id { get; set; }

		public Guid Child_Id { get; set; }

		public DateTime Created { get; set; }

		public string FullName { get; set; }
		public DateTime ArrivalTime { get; set; }

		public Guid ArrivalMember_Id { get; set; }		
		
		public DateTime DepartureTime { get; set; }

		public Guid DepartureMember_Id { get; set; }
	}
}
