using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Lite
{
	public class SystemSettings : LiteRecord
	{
		public bool Maintenance { get; set; }
		public string ExportFolder { get; set; }
		public DateTime ExporTimeStamp { get; set; }
	}

}
