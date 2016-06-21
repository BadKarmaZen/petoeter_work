using DayCare.Database.Model;
using DayCare.Model.Lite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model
{
	public class Petoeter
	{
		public enum ApplicationMode
		{
			Configuration,
			Presence
		}

		//public DayCare.Database.DatabaseEngine Database { get; set; }
		public ApplicationMode Mode { get; set; }

		#region Properties
		//public List<Account> Accounts { get; set; }
		//public List<Child> Children { get; set; }
		//public List<Member> Members { get; set; }
		//public List<Schedule> Schedules { get; set; }
		//public List<ScheduleDetail> ScheduleDetails { get; set; }

		//public List<Holiday> Holidays { get; set; }

		public List<Presence> Presences { get; set; }

		public SystemSetting Settings { get; set; }

		//public List<ChildPresence> ChildPresences { get; set; }

		#endregion


		public Petoeter()
		{
			Mode = ApplicationMode.Configuration;

			//Database = new DayCare.Database.DatabaseEngine(Mode);

			LoadData();
		}

		public bool LoadData()
		{
			//var children = Database.GetData<Database.Model.Child>(/*c => c.Deleted == false*/);
			//var members = Database.GetData<Database.Model.Member>(/*m => m.Deleted == false*/);
			//var schedules = Database.GetData<Database.Model.Schedule>(s => s.Deleted == false);
			//var scheduledetails = Database.GetData<Database.Model.ScheduleDetail>(d => d.Deleted == false);

			//Accounts = (from a in Database.GetData<DayCare.Database.Model.Account>(/*a => a.Deleted == false*/)
			//						select new Account
			//						{
			//							Id = a.Id,
			//							Name = a.Name,
			//							Deleted = a.Deleted
			//						}).ToList();

			////Children = new List<Child>();
			//Members = new List<Member>();
			//Schedules = new List<Schedule>();
			//ScheduleDetails = new List<ScheduleDetail>();

			//foreach (var a in Accounts)
			//{
			//	a.Children = (from c in children
			//								where c.Account_Id == a.Id
			//								select new Child
			//								{
			//									Id = c.Id,
			//									FirstName = c.FirstName,
			//									LastName = c.LastName,
			//									BirthDay = c.BirthDay,
			//									Account = a,
			//									Deleted = c.Deleted
			//								}).ToList();

			//	a.Members = (from m in members
			//							 where m.Account_Id == a.Id
			//							 select new Member
			//							 {
			//								 Id = m.Id,
			//								 FirstName = m.FirstName,
			//								 LastName = m.LastName,
			//								 Phone = m.Phone,
			//								 Account = a,
			//								 Deleted = m.Deleted
			//							 }).ToList();

			//	Children.AddRange(a.Children);
			//	Members.AddRange(a.Members);
			//}

			//foreach (var child in Children)
			//{
			//	child.Schedules = (from s in schedules
			//										 where s.Child_Id == child.Id
			//										 select new Schedule
			//										 {
			//											 Id = s.Id,
			//											 StartDate = s.StartDate,
			//											 EndDate = s.EndDate,
			//											 Child = child
			//										 }).ToList();

			//	Schedules.AddRange(child.Schedules);
			//}

			//foreach (var schedule in Schedules)
			//{
			//	schedule.Details = (from d in scheduledetails
			//											where d.Schedule_Id == schedule.Id
			//											orderby d.Schedule_Index
			//											select new ScheduleDetail
			//											{
			//												Id = d.Id,
			//												Index = d.Schedule_Index,
			//												MondayMorning = ScheduleDetail.IsMorning(d.Monday),
			//												MondayAfternoon = ScheduleDetail.IsAfternoon(d.Monday),
			//												TuesdayMorning = ScheduleDetail.IsMorning(d.Tuesday),
			//												TuesdayAfternoon = ScheduleDetail.IsAfternoon(d.Tuesday),
			//												WednesdayMorning = ScheduleDetail.IsMorning(d.Wednesday),
			//												WednesdayAfternoon = ScheduleDetail.IsAfternoon(d.Wednesday),
			//												ThursdayMorning = ScheduleDetail.IsMorning(d.Thursday),
			//												ThursdayAfternoon = ScheduleDetail.IsAfternoon(d.Thursday),
			//												FridayMorning = ScheduleDetail.IsMorning(d.Friday),
			//												FridayAfternoon = ScheduleDetail.IsAfternoon(d.Friday),
			//											}).ToList();

			//	ScheduleDetails.AddRange(schedule.Details);
			//}

			//
			//	ConvertSchedule();

			//Holidays = (from h in Database.GetData<Database.Model.Holiday>(h => h.Deleted == false && h.Date > DateTime.Today.AddYears(-1))
			//						select new Holiday
			//						{
			//							Id = h.Id,
			//							Date = h.Date,
			//							Morning = Holiday.IsMorning(h.Mask),
			//							Afternoon = Holiday.IsAfternoon(h.Mask)
			//						}).ToList();


			//var query = from c in GetChildren()
			//						orderby c.BirthDay
			//						select c;

			//var days = new List<DateTime>();
			//var enddate = new DateTime(2019, 1, 1);
			//var date = new DateTime(2016, 1, 1);

			//while (date < enddate)
			//{
			//	if (date.DayOfWeek != DayOfWeek.Saturday &&
			//			date.DayOfWeek != DayOfWeek.Sunday)
			//	{
			//		days.Add(date);
			//	}
			//	date = date.AddDays(1);
			//}

			//using (var db = new Lite.PetoeterDb(@"E:\petoeter_lite.ldb"))
			//{
			//	//	add member
			//	foreach (var m in Members)
			//	{
			//		db.Members.Insert(new Lite.Member
			//		{
			//			Deleted = m.Deleted,
			//			FirstName = m.FirstName,
			//			LastName = m.LastName,
			//			Phone = m.Phone,
			//			Updated = DateTime.Now
			//		});
			//	}

			//	foreach (var c in Children)
			//	{
			//		var s = new List<Lite.Date>();

			//		foreach (var d in days)
			//		{
			//			var sched = c.FindSchedule(d);
			//			if (sched != null)
			//			{
			//				var detail = sched.GetActiveSchedule(d);
			//				if (detail != null)
			//				{
			//					s.Add(new Lite.Date
			//					{
			//						Day = d,
			//						Morning = detail.ThisMorning(d),
			//						Afternoon = detail.ThisAfternoon(d)
			//					});
			//				}
			//			}
			//		}

			//		db.Children.Insert(new Lite.Child
			//		{
			//			BirthDay = c.BirthDay,
			//			Deleted = c.Deleted,
			//			FirstName = c.FirstName,
			//			LastName = c.LastName,
			//			Updated = DateTime.Now,
			//			Schedule = s
			//		});
			//	}

			//	foreach (var h in Holidays)
			//	{
			//		db.Holidays.Insert(new Lite.Date
			//		{
			//			Day = h.Date,
			//			Morning = h.Morning,
			//			Afternoon = h.Afternoon
			//		});
			//	}

			//	foreach (var a in Accounts)
			//	{
			//		var account = new Lite.Account
			//		{
			//			Name = a.Name,
			//			Updated = DateTime.Now,
			//			Deleted = a.Deleted,

			//			Members = new List<Lite.Member>(),
			//			Children = new List<Lite.Child>()
			//		};

			//		foreach (var m in a.Members)
			//		{
			//			var member = db.Members.FindOne(x => x.FirstName.Equals(m.FirstName) && x.LastName.Equals(m.LastName));
			//			account.Members.Add(member);
			//		}

			//		foreach (var c in a.Children)
			//		{
			//			var child = db.Children.FindOne(x => x.FirstName.Equals(c.FirstName) && x.LastName.Equals(c.LastName));
			//			account.Children.Add(child);
			//		}

			//		db.Accounts.Insert(account);
			//	}
			//}

/*
			Presences = (from p in Database.GetData<Database.Model.Presence>()
									 select new Presence
									 {
										 Id = p.Id,
										 Child = Children.Where(c => c.Id == p.Child_Id).FirstOrDefault(),
										 Arriving = Members.Where(m => m.Id == p.ArrivalMember_Id).FirstOrDefault(),
										 ArrivingTime = p.ArrivalTime,
										 Leaving = Members.Where(m => m.Id == p.DepartureMember_Id).FirstOrDefault(),
										 LeavingTime = p.DepartureTime,
										 TimeCode = p.TimeCode
									 }).ToList();

			if (Presences == null || Presences.Count == 0)
			{
				var today = DateTime.Today.Date;
				var query = from c in Children
										let schedule = c.FindSchedule(today)
										where schedule != null
										let week = schedule.GetActiveSchedule(today)
										where week != null
										let time = week.GetTimeCode(today)
										where c.HasValidPeriod(today) && time != 0
										select new Presence
										{
											Id = Guid.NewGuid(),
											Child = c,
											TimeCode = time,
											Added = true
										};
				Presences = query.ToList();
			}

			ChildPresences = (from cp in Database.GetData<Database.Model.ChildPresence>()
												select new ChildPresence 
												{
													Id = cp.Id,
													Child = Children.Where(c => c.Id == cp.Child_Id).FirstOrDefault(),
													Date = cp.Day,
													Morning = Holiday.IsMorning(cp.Mask),
													Afternoon = Holiday.IsAfternoon(cp.Mask)
												}).ToList();
			*/
			//	TODO
			Settings = new SystemSetting
			{
				ImageFolder = @"E:\[Petoeter]\Images"//Database.SystemSettings.PictureFolder
			};

			return true;
		}


		public void Export(string path)
		{
			string filename = Path.Combine(path, "export.ldb");

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var settings = db.GetSettings();
				var lastExportTime = settings.ExporTimeStamp;
				var exportTime = DateTime.Now;

				using (var export = new PetoeterDb(filename))
				{
					var children = (from c in db.Children.FindAll()
													where c.Updated < lastExportTime
													select c).ToList();

					children.ForEach(c => c.Updated = exportTime);

					db.Children.Update(children);
					export.Children.Insert(children);


					var members = (from m in db.Members.FindAll()
												 where m.Updated < lastExportTime
												 select m).ToList();

					members.ForEach(m => m.Updated = exportTime);

					db.Members.Update(members);
					export.Members.Insert(members);

					var accounts = (from a in db.Accounts.FindAll()
													where a.Updated < lastExportTime
													select a).ToList();

					accounts.ForEach(a => a.Updated = exportTime);

					db.Accounts.Update(accounts);
					export.Accounts.Insert(accounts);

					var holidays = (from h in db.Holidays.FindAll()
													where h.Updated < lastExportTime
													select h).ToList();

					holidays.ForEach(h => h.Updated = exportTime);

					db.Holidays.Update(holidays);
					export.Holidays.Insert(holidays);
				}

				settings.ExporTimeStamp = DateTime.Now;
				db.UpdateSystemSettings();
			}
		}

		private void ExportPresenceToConfig(string path)
		{
		}

		private void ExportConfigToPresence(string path)
		{
		}

		public void Import(string path)
		{
			//if (Mode == Petoeter.ApplicationMode.Configuration)
			//{
			//	Database.ImportPresenceData(path);
			//}
			//else
			//{
			//	Database.ImportConfigurationData(path);
			//}

			//LoadData();
		}
	}
}
