using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model//.Database
{
	public class SystemSetting
	{
		public Version DatabaseVersion { get; set; }
		public string ImageFolder { get; set; }
		public DateTime ExporTimeStamp { get; set; }
	}
}
