using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model//.Database.Updates
{
	//public class DatabaseUpdate
	//{
	//	public Petoeter	Database { get; set; }

	//	public DatabaseUpdate(Petoeter database)
	//	{
	//		Database = database;
	//	}

	//	public void Upgrade()
	//	{
	//		From_1_0_To_1_1();
	//	}

	//	public void From_1_0_To_1_1()
	//	{
	//		var newVersion = new Version(1,1,0);
	//		if (Database.Settings.DatabaseVersion < newVersion)
	//		{
	//			var query = string.Format("ALTER TABLE `{0}`.`member` ADD COLUMN `phone` VARCHAR(45) NULL COMMENT '' AFTER `lastname`;", Petoeter.DatabaseName);
	//			Database.ExecuteCommand(query);


	//			Database.Settings.DatabaseVersion = newVersion;
	//			Database.SaveSystemSettings();
	//		}

	//	}
	//}
}
