using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core
{
	public class Events
	{
		public class Close { }

		public class SwitchTask
		{
			public Screen Task { get; set; }
		}

		public class ShowDialog
		{
			public Screen Dialog { get; set; }
		}

		public class RegisterMenu
		{
			public bool Add { get; set; }
			public string Id { get; set; }
			public string Caption { get; set; }
			public System.Action Action { get; set; }
		}

		public class ShowSnapshot
		{
			public string FileName { get; set; }
			public string FileId { get; set; }
		}

		public class SaveSnapshot
		{
			public string FileId { get; set; }
		}
	}
}
