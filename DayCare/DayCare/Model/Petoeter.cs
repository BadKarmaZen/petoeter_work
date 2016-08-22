using Caliburn.Micro;
using DayCare.Database.Model;
using DayCare.Model.Lite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model
{
	public class Data : DayCare.Core.XmlBaseConfig<Data>
	{
		public List<Account> Accounts { get; set; }

		public List<Member> Members { get; set; }

		public List<Child> Children { get; set; }

		public List<Presence> Presences { get; set; }

		public List<Date> Holidays { get; set; }

		public SystemSettings Settings { get; set; }
	}

	public class Petoeter
	{
		public const string AdminExportFilename = "export_admin.ldb";
		public const string PresenceExportFilename = "export_presence.ldb";
		//public enum ApplicationMode
		//{
		//	Configuration,
		//	Presence
		//}

		//public DayCare.Database.DatabaseEngine Database { get; set; }
		//public ApplicationMode Mode { get; set; }

		#region Properties
		//public List<Account> Accounts { get; set; }
		//public List<Child> Children { get; set; }
		//public List<Member> Members { get; set; }
		//public List<Schedule> Schedules { get; set; }
		//public List<ScheduleDetail> ScheduleDetails { get; set; }

		//public List<Holiday> Holidays { get; set; }

		//public List<Presence> Presences { get; set; }

		//public SystemSetting Settings { get; set; }

		//public List<ChildPresence> ChildPresences { get; set; }

		#endregion


		public Petoeter()
		{
			//Mode = ApplicationMode.Presence;

			//Database = new DayCare.Database.DatabaseEngine(Mode);

			//	Verify Database
			//	DumpDatabase();

			//LoadData();
		}

		private void DumpDatabase()
		{
			var data = new Data();

			using (var db = new PetoeterDb(@"E:\Temp\petoeter_db\db.ldb"))
			{
				data.Accounts = db.Accounts.FindAll().ToList();
				data.Members = db.Members.FindAll().ToList();
				data.Children = db.Children.FindAll().ToList();
				data.Presences = db.Presences.FindAll().ToList();
				data.Holidays = db.Holidays.FindAll().ToList();
				data.Settings = db.GetSettings();

				data.SaveToFile(@"E:\Temp\petoeter_db\data.xml");
			}
		}

		private void VerifyDatabase()
		{
			//	Sanityze
			//
			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				//db.Pricings.Insert(new PricingInformation 
				//{
				//	Start = new DateTime(2016, 1, 1),
				//	End = DateTime.MaxValue,
				//	FullDay = 23.0,
				//	HalfDay = 15.5,
				//	ExtraMeal = 3.25,
				//	ExtraHour = 1,
				//	Diapers = 0.5,
				//	Medication = 1.0,
				//	ToLate = 5.0,
				//	FullDaySick = 11.5,
				//	HalfDaySick = 7.75
				//});


				//var accounts = (from a in db.Accounts.FindAll()
				//							 //where a.Deleted != false
				//							 select a).ToList();

				//foreach (var account in accounts)
				//{
				//	if (account.Members.Count == 2)
				//	{
				//		var oma = new Member { FirstName = "Oma" };
				//		var opa = new Member { FirstName = "Opa" };
				//		var other = new Member { FirstName = "Andere" };

				//		account.Members.Add(oma);
				//		db.Members.Insert(oma);
				//		account.Members.Add(opa);
				//		db.Members.Insert(opa);
				//		account.Members.Add(other);
				//		db.Members.Insert(other);

				//		db.Accounts.Update(account);						
				//	}
									
				//}
				//var accounts = db.GetCollection<Account>(PetoeterDb.TableAccount);
				//var members = db.GetCollection<Member>(PetoeterDb.TableMember);

				//Debug.WriteLine(accounts.Count());
				//accounts.Delete(new LiteDB.BsonValue(19));
				//Debug.WriteLine(accounts.Count());

				//Debug.WriteLine(members.Count());
				//members.Delete(new LiteDB.BsonValue(37));
				//members.Delete(new LiteDB.BsonValue(77));
				//Debug.WriteLine(members.Count());
				
				//foreach (var account in accounts)
				//{
				//	Debug.WriteLine(string.Format("A [{0}] {1}", account.Id, account.Name));
				//	Debug.WriteLine(account.Members.Count);

				//	foreach (var mi in account.Members)
				//	{
				//		var member = members.FindById(mi.Id);

				//		Debug.WriteLine(string.Format("M [{0}] {1}",member.Id, member.GetFullName()));
				//	}					
				//}
			}


			//	Add Oma and Opa Generic members
			//
			/*using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var member = db.Members.Find(m => m.Phone == "GrandParents");
				if (member.Count() == 0)
				{
					db.Members.Insert(new Lite.Member { FirstName = "Oma", Phone = "GrandParents" });
					db.Members.Insert(new Lite.Member { FirstName = "Opa", Phone = "GrandParents" });					
				}			
	
				member = db.Members.Find(m => m.Phone == "Other");
				if (member.Count() == 0)
				{
					db.Members.Insert(new Lite.Member { FirstName = "Andere", Phone = "Other" });			
				}				
			}*/
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
			//Settings = new SystemSetting
			//{
			//	ImageFolder = @"E:\[Petoeter]\Images"//Database.SystemSettings.PictureFolder
			//};

			return true;
		}


		public void Export(string path)
		{
			//ForceExport();
			if (Properties.Settings.Default.PresenseMode)
			{
				ExportPresence(Path.Combine(path, PresenceExportFilename));
			}
			else
			{
				ExportAdministration(Path.Combine(path, AdminExportFilename));
			}
		}

		public static void ForceExport()
		{
			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var settings = db.GetSettings();
				settings.ExporTimeStamp = new DateTime(2016, 08, 01);
				db.UpdateSystemSettings();
			} 
		}

		private void ExportPresence(string filename)
		{
			var log = LogManager.GetLog(GetType());
			var exportTime = DateTime.Now;
			log.Info("Start presence export ({0}) : {1}.{2:D3}", filename, exportTime, exportTime.Millisecond);

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var settings = db.GetSettings();
				var lastExportTime = settings.ExporTimeStamp;

				using (var export = new PetoeterDb(filename))
				{
					var presence = (from p in db.Presences.Find(p => p.Updated > lastExportTime) select p).ToList();

					presence.ForEach(p =>
					{
						p.Updated = exportTime;
						log.Info("Export presence: {0}. [{1}] {2}", p.Id, p.Date.ToShortDateString(), p.Child.GetFullName());

						db.Presences.Update(p);

						//if (export.Members.Exists(m => m.Id == p.BroughtBy.Id) == false)
						//{
						//	export.Members.Insert(p.BroughtBy);
						//}

						//if (export.Members.Exists(m => m.Id == p.TakenBy.Id) == false)
						//{
						//	export.Members.Insert(p.TakenBy);
						//}
					});

					export.Presences.Insert(presence);
				}

				settings.ExporTimeStamp = exportTime;
				db.UpdateSystemSettings();
			}
		}

		private void ExportAdministration(string filename)
		{
			var log = LogManager.GetLog(GetType());
			var exportTime = DateTime.Now;
			log.Info("Start administration export ({0}) : {1}.{2:D3}", filename, exportTime, exportTime.Millisecond);

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var settings = db.GetSettings();
				var lastExportTime = settings.ExporTimeStamp;

				using (var export = new PetoeterDb(filename))
				{
					var children = (from c in db.Children.FindAll()
													where c.Updated > lastExportTime
													select c).ToList();

					children.ForEach(c =>
					{
						c.Updated = exportTime;
						log.Info("Export child: {0}. {1}", c.Id, c.GetFullName());
					});

					db.Children.Update(children);
					export.Children.Insert(children);

					children.ForEach(c =>
					{
						if (c.FileId != null)
						{
							var file = db.FileStorage.FindById(c.FileId);
							if (file != null)
							{
								export.FileStorage.Upload(c.FileId, file.OpenRead());								
							}					
						}
					});

					var members = (from m in db.Members.FindAll()
												 where m.Updated > lastExportTime
												 select m).ToList();

					members.ForEach(m =>
					{
						m.Updated = exportTime;
						log.Info("Export member: {0}. {1}", m.Id, m.GetFullName());
					});

					db.Members.Update(members);
					export.Members.Insert(members);

					var accounts = (from a in db.Accounts.FindAll()
													where a.Updated > lastExportTime
													select a).ToList();

					accounts.ForEach(a =>
					{
						a.Updated = exportTime;
						log.Info("Export account: {0}. {1}", a.Id, a.Name);
					});

					db.Accounts.Update(accounts);
					export.Accounts.Insert(accounts);

					var holidays = (from h in db.Holidays.FindAll()
													where h.Updated > lastExportTime
													select h).ToList();

					holidays.ForEach(h =>
					{
						h.Updated = exportTime;
						log.Info("Export holiday: {0}. {1}", h.Id, h.Day);
					});

					db.Holidays.Update(holidays);
					export.Holidays.Insert(holidays);
				}

				settings.ExporTimeStamp = exportTime;
				db.UpdateSystemSettings();
			}
		}

		public void Import(string path)
		{
			if (Properties.Settings.Default.PresenseMode)
			{
				ImportAdministration(Path.Combine(path, AdminExportFilename));
			}
			else
			{
 				ImportPresence(Path.Combine(path, PresenceExportFilename));
			}
		}

		private void ImportPresence(string filename)
		{
			var log = LogManager.GetLog(GetType());
			var importTime = DateTime.Now;

			log.Info("Start administration import ({0}) : {1}.{2:D3}", filename, importTime, importTime.Millisecond);

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				using (var import = new PetoeterDb(filename))
				{
					var presences = import.GetCollection<Presence>(PetoeterDb.TablePresence).FindAll().ToList();
					log.Info("Import presences (#{0})", presences.Count);

					foreach (var presence in presences)
					{
						log.Info("Import presence: {0}. [{1}] {2})", presence.Id, presence.Date.ToShortDateString(), presence.Child.GetFullName());
						presence.Updated = importTime;
						db.Presences.Insert(presence);
					}

					import.DropAll();
				}
			}
		}

		private void ImportAdministration(string filename)
		{
			var log = LogManager.GetLog(GetType());
			var importTime = DateTime.Now;

			log.Info("Start administration import ({0}) : {1}.{2:D3}", filename, importTime, importTime.Millisecond);

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				using (var import = new PetoeterDb(filename))
				{
					if (import.Children.Count() != 0)
					{
						var children = (from c in import.Children.FindAll() select c).ToList();
						log.Info("Import children (#{0})", children.Count);

						foreach (var child in children)
						{
							var update_child = db.Children.FindById(child.Id);

							if (update_child == null)
							{
								log.Info("Import child: {0}. {1}", child.Id, child.GetFullName());
								child.Updated = importTime;
								db.Children.Insert(child);
							}
							else
							{
								log.Info("Update child: {0}. {1}", child.Id, child.GetFullName());
								child.Updated = importTime;
								db.Children.Update(child);
							}

							if (string.IsNullOrEmpty(child.FileId) == false)
							{
								var file = import.FileStorage.FindById(child.FileId);
								if (file != null)
								{
									if (db.FileStorage.Exists(child.FileId))
									{
										log.Info("Update child: remove picture");
										db.FileStorage.Delete(child.FileId);
									}
									log.Info("Update child: upload picture");
									db.FileStorage.Upload(child.FileId, file.OpenRead());
								}								
							}
						}
					}

					if (import.Members.Count() != 0)
					{
						var members = (from m in import.Members.FindAll() select m).ToList();
						log.Info("Import members (#{0})", members.Count);

						foreach (var member in members)
						{
							var update_member = db.Members.FindById(member.Id);

							if (update_member == null)
							{
								log.Info("Import member: {0}. {1}", member.Id, member.GetFullName());
								member.Updated = importTime;
								db.Members.Insert(member);
							}
							else
							{
								log.Info("Update member: {0}. {1}", member.Id, member.GetFullName());
								member.Updated = importTime;
								db.Members.Update(member);
							}
						}
					}

					if (import.Accounts.Count() != 0)
					{
						//var accounts = (from a in import.Accounts.FindAll() select a).ToList();
						var accounts = (from a in import.GetCollection<Account>(PetoeterDb.TableAccount).FindAll() select a).ToList();

						log.Info("Import accounts (#{0})", accounts.Count);

						foreach (var account in accounts)
						{
							//	check if account exist
							var updateaccount = db.Accounts.FindById(account.Id);

							if (updateaccount == null)
							{
								log.Info("Import account: #{0}. {1}", account.Id, account.Name);
								account.Updated = importTime;
								db.Accounts.Insert(account);
							}
							else
							{
								log.Info("Update account: #{0}. {1}", account.Id, account.Name);
								account.Updated = importTime;
								db.Accounts.Update(account);
							}
						}
					}

					if (import.Holidays.Count() != 0)
					{
						var holidays = (from h in import.Holidays.FindAll() select h).ToList();

						log.Info("Import holidays (#{0})", holidays.Count);

						foreach (var holiday in holidays)
						{
							//	check if account exist
							var updateholiday = db.Accounts.FindById(holiday.Id);

							if (updateholiday == null)
							{
								log.Info("Import holiday: #{0}. {1}", holiday.Id, holiday.Day);
								holiday.Updated = importTime;
								db.Holidays.Insert(holiday);
							}
							else
							{
								log.Info("Update holiday: #{0}. {1}", holiday.Id, holiday.Day);
								holiday.Updated = importTime;
								db.Holidays.Update(holiday);
							}
						}
					}

					log.Info("Drop all");
					import.DropAll();
				}
			}
		}
	}
}
