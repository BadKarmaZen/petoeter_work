using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Lite
{
	public class Presence : LiteRecord
	{
		public DateTime Date { get; set; }
		public Child Child { get; set; }
		public Member BroughtBy { get; set; }
		public DateTime BroughtAt { get; set; }

		public Member TakenBy { get; set; }
		public DateTime TakenAt { get; set; }

		public int TimeCode { get; set; }

	}
}
