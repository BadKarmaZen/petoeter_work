using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model
{
	class Petoeter
	{
		public DayCare.Database.DatabaseEngine	Database { get; set; }

		#region Properties
		public List<Account> Accounts { get; set; }
		public List<Child> Children { get; set; }
		public List<Member> Members { get; set; }
		public List<Schedule> Schedules { get; set; }
		public List<ScheduleDetail> ScheduleDetails { get; set; }
		public SystemSetting Settings { get; set; }
		
		#endregion


		public Petoeter()
		{
			Database = new DayCare.Database.DatabaseEngine();

			LoadData();
		}
		
		public bool LoadData()
		{
			var children = Database.GetData<Database.Model.Child>(/*c => c.Deleted == false*/);
			var members = Database.GetData<Database.Model.Member>(/*m => m.Deleted == false*/);
			var schedules = Database.GetData<Database.Model.Schedule>(s => s.Deleted == false);
			var scheduledetails = Database.GetData<Database.Model.ScheduleDetail>(d => d.Deleted == false);
										 
			Accounts = (from a in Database.GetData<DayCare.Database.Model.Account>(/*a => a.Deleted == false*/)
									select new Account
									{
										 Id = a.Id,
										 Name = a.Name,
										 Deleted = a.Deleted
									}).ToList();

			Children = new List<Child>();
			Members = new List<Member>();
			Schedules = new List<Schedule>();
			ScheduleDetails = new List<ScheduleDetail>();

			foreach (var a in Accounts)
			{
				a.Children = (from c in children
											where c.Account_Id == a.Id
											select new Child 
											{
 												Id = c.Id,
												FirstName = c.FirstName,
												LastName = c.LastName,
												BirthDay = c.BirthDay,
												Account = a,
												Deleted = c.Deleted
											}).ToList();

				a.Members = (from m in members
										 where m.Account_Id == a.Id
										 select new Member 
										 {
 											 Id = m.Id,
											 FirstName = m.FirstName,
											 LastName = m.LastName,
											 Phone = m.Phone,
											 Account = a,
											 Deleted = m.Deleted
										 }).ToList();

				Children.AddRange(a.Children);
				Members.AddRange(a.Members);
			}

			foreach (var child in Children)
			{
				child.Schedules = (from s in schedules
													 where s.Child_Id == child.Id
													 select new Schedule 
													 { 
														 Id = s.Id,
														 StartDate = s.StartDate,
														 EndDate = s.EndDate,
														 Child = child
													 }).ToList();

				Schedules.AddRange(child.Schedules);				
			}

			foreach (var schedule in Schedules)
			{
				schedule.Details = (from d in scheduledetails
														where d.Schedule_Id == schedule.Id
														orderby d.Schedule_Index
														select new ScheduleDetail 
														{
															Id = d.Id,
															MondayMorning = ScheduleDetail.IsMorning(d.Monday),
															MondayAfternoon = ScheduleDetail.IsAfternoon(d.Monday),
															TuesdayMorning = ScheduleDetail.IsMorning(d.Tuesday),
															TuesdayAfternoon = ScheduleDetail.IsAfternoon(d.Tuesday),
															WednesdayMorning = ScheduleDetail.IsMorning(d.Wednesday),
															WednesdayAfternoon = ScheduleDetail.IsAfternoon(d.Wednesday),
															ThursdayMorning = ScheduleDetail.IsMorning(d.Thursday),
															ThursdayAfternoon = ScheduleDetail.IsAfternoon(d.Thursday),
															FridayMorning = ScheduleDetail.IsMorning(d.Friday),
															FridayAfternoon = ScheduleDetail.IsAfternoon(d.Friday),
														}).ToList();

				ScheduleDetails.AddRange(schedule.Details);				
			}

			Settings = new SystemSetting { ImageFolder = @"E:\[Petoeter]\Images" };

			return true;
		}

		public bool Save()
		{
			try
			{
				SaveAccounts();
				SaveChildren();
				SaveMembers();
			}
			catch (Exception ex)
			{
				return false;
			}

			return true;
		}


		#region Account
		public IEnumerable<Account> GetAccounts(bool deleted = false)
		{
			return Accounts.AsEnumerable().Where(a => a.Deleted == deleted);
		}
	
		public void AddAccount(Account account)
		{
			MarkForAdd(account);
			Accounts.Add(account);

			foreach (var member in account.Members)
			{
				AddMember(member);
			}
		}
		
		public void DeleteAccount(Account account)
		{
			account.Updated = true;
			account.Deleted = true;

			foreach (var child in account.Children)
			{
				DeleteChild(child);				
			}

			foreach (var member in account.Members)
			{
				DeleteMember(member);				
			}

		}
		
		public void SaveAccounts()
		{
			foreach (var account in from a in Accounts where a.Updated select a)
			{
				if (account.Deleted)
				{
					Database.DeleteAccount(account.Id);					
				}
				else if(account.Added)
				{
					Database.AddAccount(new Database.Model.Account 
					{
						Id = account.Id,
						Name = account.Name
					});
				}
				else
				{
					Database.UpdateAccount(new Database.Model.Account
					{
						Id = account.Id,
						Name = account.Name
					});
				}
			}
		}

		#endregion

		#region Child
		public IEnumerable<Child> GetChildren(bool deleted = false)
		{
			return Children.AsEnumerable().Where(c => c.Deleted == deleted);
		}

		public void AddChild(Child child)
		{
			MarkForAdd(child);
			Children.Add(child);
		}

		public void DeleteChild(Child child)
		{
			child.Updated = true;
			child.Deleted = true;
		}
		
		private void SaveChildren()
		{
			foreach (var child in from c in Children where c.Updated select c)
			{
				if (child.Deleted)
				{
					Database.DeleteChild(child.Id);
				}
				else if (child.Added)
				{
					Database.AddChild(new Database.Model.Child
					{
						Id = child.Id,
						FirstName = child.FirstName,
						LastName = child.LastName,
						BirthDay = child.BirthDay,
						Account_Id = child.Account.Id
					});
				}
				else
				{
					Database.UpdateChild(new Database.Model.Child
					{
						Id = child.Id,
						FirstName = child.FirstName,
						LastName = child.LastName,
						BirthDay = child.BirthDay,
						Account_Id = child.Account.Id
					});
				}
			}
		}
		#endregion

		#region Member
		public IEnumerable<Member> GetMembers(bool deleted = false)
		{
			return Members.AsEnumerable().Where(m => m.Deleted == deleted);
		}
		public void AddMember(Member member)
		{
			MarkForAdd(member);
			Members.Add(member);
		}

		public void DeleteMember(Member member)
		{
			member.Updated = true;
			member.Deleted = true;
		}
		private void SaveMembers()
		{
			foreach (var member in from m in Members where m.Updated select m)
			{
				if (member.Deleted)
				{
					Database.DeleteMember(member.Id);
				}
				else if (member.Added)
				{
					Database.AddMember(new Database.Model.Member
					{
						Id = member.Id,
						FirstName = member.FirstName,
						LastName = member.LastName,
						Phone = member.Phone,
						Account_Id = member.Account.Id
					});
				}
				else
				{
					Database.UpdateMember(new Database.Model.Member
					{
						Id = member.Id,
						FirstName = member.FirstName,
						LastName = member.LastName,
						Phone = member.Phone,
						Account_Id = member.Account.Id
					});
				}
			}
		}
		#endregion

		#region Schedule
		public IEnumerable<Schedule> GetSchedules()
		{
			return Schedules.AsEnumerable();
		}

		public void AddSchedule(Schedule schedule)
		{
			MarkForAdd(schedule);
			Schedules.Add(schedule);
		}

		public void DeleteSchedule(Schedule schedule)
		{
			MarkForDelete(schedule);
		}

		public void SaveSchedules()
		{ }
		#endregion

		public void MarkForAdd(DataObject data)
		{
			data.Added = true;
			data.Updated = true;
		}

		public void MarkForDelete(DataObject data)
		{
			data.Deleted = true;
			data.Updated = true;
		}

	}
}
