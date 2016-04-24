using DayCare.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model//.Database
{
	public class MasterExport : XmlBaseConfig<MasterExport>
	{
	//	private List<Account> _accounts;
	//	private List<Child> _children;
	//	private List<Member> _members;
	//	private List<Schedule> _schedules;
	//	private List<Holiday> _holydays;

	//	public List<Holiday> Holydays
	//	{
	//		get { return _holydays; }
	//		set { _holydays = value; }
	//	}

	//	public List<Schedule> Schedules
	//	{
	//		get { return _schedules; }
	//		set { _schedules = value; }
	//	}

	//	public List<Member> Members
	//	{
	//		get { return _members; }
	//		set { _members = value; }
	//	}

	//	public List<Child> Children
	//	{
	//		get { return _children; }
	//		set { _children = value; }
	//	}
		
	//	public List<Account> Accounts
	//	{
	//		get { return _accounts; }
	//		set { _accounts = value; }
	//	}

	//	public MasterExport()
	//	{
	//		_accounts = new List<Account>();
	//		_children = new List<Child>();
	//		_members = new List<Member>();
	//		_schedules = new List<Schedule>();
	//		_holydays = new List<Holiday>();
	//	}
	//}

	//public static class Synchronizer
	//{
	//	public static bool ImportFromMaster(string fileName, Petoeter model)
	//	{
	//		var export = MasterExport.LoadFromFile(fileName);

	//		SynchronizeAccounts(model, export); 
	//		SynchronizeChildren(model, export);
	//		SynchronizeRecords(model, export.Members, from m in model.GetMember() select m.Id, r => model.SaveMember(r));
	//		SynchronizeRecords(model, export.Schedules, from m in model.GetSchedule() select m.Id, r => model.SaveSchedule(r));
	//		SynchronizeRecords(model, export.Holydays, from m in model.GetHoliday() select m.Id, r => model.SaveHoliday(r));

	//		return true;
	//	}

	//	private static void SynchronizeAccounts(Petoeter model, MasterExport export)
	//	{
	//		var record_ids = (from a in model.GetAccount() select a.Id).ToList();

	//		var new_records = (from a in export.Accounts
	//											 where record_ids.Contains(a.Id) == false
	//												select a).ToList();
	//		var update_records = (from a in export.Accounts
	//													 where record_ids.Contains(a.Id) == true
	//													 select a).ToList();

	//		foreach (var account in new_records)
	//		{
	//			model.SaveAccount(account);
	//		}

	//		foreach (var account in update_records)
	//		{
	//			model.UpdateRecord(account);
	//		}
	//	}

	//	private static void SynchronizeChildren(Petoeter model, MasterExport export)
	//	{
	//		var record_ids = (from c in model.GetChild() select c.Id).ToList();

	//		var new_records = (from a in export.Accounts
	//											 where record_ids.Contains(a.Id) == false
	//												select a).ToList();
	//		var update_records = (from a in export.Accounts
	//													 where record_ids.Contains(a.Id) == true
	//													 select a).ToList();

	//		foreach (var account in new_records)
	//		{
	//			model.SaveAccount(account);
	//		}

	//		foreach (var account in update_records)
	//		{
	//			model.UpdateRecord(account);
	//		}
	//	}

	//	private static void SynchronizeRecords<T>(Petoeter model, IEnumerable<T> Source, IEnumerable<Guid> record_ids, Action<T> storeAction)
	//		where T: DatabaseRecord
	//	{
	//		var new_records = (from a in Source
	//											 where record_ids.Contains(a.Id) == false
	//											 select a).ToList();

	//		var update_records = (from a in Source
	//													where record_ids.Contains(a.Id) == true
	//													select a).ToList();

	//		foreach (var record in new_records)
	//		{
	//			storeAction(record);
	//		}

	//		foreach (var record in update_records)
	//		{
	//			model.UpdateRecord(record);
	//		}
	//	}

	//	public static bool ImportInSlave(string fileName)
	//	{

	//		return true;
	//	}
	//	public static bool ExportFromMaster(string fileName, Petoeter model)
	//	{
	//		var export = new MasterExport();

	//		DateTime lastExport = model.Settings.ExporTimeStamp;
		//		model.Settings.ExporTimeStamp = DateTimeProvider.Now();

	//		export.Accounts.AddRange(model.GetAccount(a => a.Updated >= lastExport));
	//		export.Children.AddRange(model.GetChild(c => c.Updated >= lastExport));
	//		export.Members.AddRange(model.GetMember(m => m.Updated >= lastExport));
	//		export.Schedules.AddRange(model.GetSchedule(s => s.Updated >= lastExport));
	//		export.Holydays.AddRange(model.GetHoliday(h => h.Updated >= lastExport));

	//		MasterExport.SaveToFile(fileName, export);

	//		return true;
	//	}

	//	public static bool ExportFromSlave(string fileName)
	//	{
	//		var data = new List<Presence> 
	//		{
		//			new Presence { Id = Guid.NewGuid(), Child_Id = Guid.NewGuid(), ArrivalTime = DateTimeProvider.Now()},
		//			new Presence { Id = Guid.NewGuid(), Child_Id = Guid.NewGuid(), ArrivalTime = DateTimeProvider.Now()},
		//			new Presence { Id = Guid.NewGuid(), Child_Id = Guid.NewGuid(), ArrivalTime = DateTimeProvider.Now()}
	//		};

	//		//XmlBaseConfig<List<Presence>>.SaveToFile(fileName, data);

	//		return true;
	//	}
		

	}
}
