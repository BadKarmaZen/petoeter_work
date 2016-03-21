using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model
{
	public class Holiday :  DataObject
	{
		public const int MaskMorning = 0x01;
		public const int MaskAfternoon = 0x02;

		public DateTime Date { get; set; }
		public bool Morning { get; set; }
		public bool Afternoon { get; set; }
		public bool FullDay 
		{
			get
			{
				return Morning && Afternoon;
			}
			set
			{
				Morning = value;
				Afternoon = value;
			}
		}
	
		public int Mask
		{
			get
			{
				return (Morning ? MaskMorning : 0) | (Afternoon ? MaskAfternoon : 0);
			}
		}

		internal static bool IsMorning(int mask)
		{
			return MaskMorning == (mask & MaskMorning);
		}

		internal static bool IsAfternoon(int mask)
		{
			return MaskAfternoon == (mask & MaskAfternoon);
		}


	}
}
