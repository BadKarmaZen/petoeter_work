using DayCare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Database
{
	public class MasterExport : XmlBaseConfig<MasterExport>
	{
		private List<Account> _accounts;
		
		public List<Account> Accounts
		{
			get { return _accounts; }
			set { _accounts = value; }
		}

		public MasterExport()
		{
			_accounts = new List<Account>();
		}
	}

	public static class Synchronizer
	{
		public static bool ImportInMaster(string fileName)
		{
			return true;
		}

		public static bool ImportInSlave(string fileName)
		{

			return true;
		}
		public static bool ExportFromMaster(string fileName)
		{
			var export = new MasterExport();

			export.Accounts.Add(new Account { Id = Guid.NewGuid(), Name = "hello" });
			export.Accounts.Add(new Account { Id = Guid.NewGuid(), Name = "world" });

			MasterExport.SaveToFile(fileName, export);

			return true;
		}

		public static bool ExportFromSlave(string fileName)
		{
			var data = new List<Presence> 
			{
				new Presence { Id = Guid.NewGuid(), Child_Id = Guid.NewGuid(), ArrivalTime = DateTime.Now},
				new Presence { Id = Guid.NewGuid(), Child_Id = Guid.NewGuid(), ArrivalTime = DateTime.Now},
				new Presence { Id = Guid.NewGuid(), Child_Id = Guid.NewGuid(), ArrivalTime = DateTime.Now}
			};

			//XmlBaseConfig<List<Presence>>.SaveToFile(fileName, data);

			return true;
		}
		

	}
}
