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

		public List<Account> Accounts { get; set; }
		public List<Child> Children { get; set; }
		public List<Member> Members { get; set; }

		public Petoeter()
		{
			Database = new DayCare.Database.DatabaseEngine();

			LoadData();
		}
		
		public bool LoadData()
		{
			var children = Database.GetData<DayCare.Database.Model.Child>(c => c.Deleted == false);
			var members = Database.GetData<DayCare.Database.Model.Member>(m => m.Deleted == false);
										 
			Accounts = (from a in Database.GetData<DayCare.Database.Model.Account>(a => a.Deleted == false)
									select new Account
									{
										 Id = a.Id,
										 Name = a.Name
									}).ToList();

			Children = new List<Child>();
			Members = new List<Member>();

			foreach (var a in Accounts)
			{
				a.Children = (from c in children
											where c.Account_Id == a.Id
											select new Child 
											{
 												Id = c.Id,
												FirstName = c.FirstName,
												LastName = c.LastName,
												BirthDay = c.BirthDay
											}).ToList();

				a.Members = (from m in members
										 where m.Account_Id == a.Id
										 select new Member 
										 {
 											 Id = m.Id,
											 FirstName = m.FirstName,
											 LastName = m.LastName,
											 Phone = m.Phone,
											 Account = a
										 }).ToList();

				Children.AddRange(a.Children);
				Members.AddRange(a.Members);
			}

			return true;
		}

		public bool Save()
		{
			try
			{
				SaveAccounts();
				//SaveChildren();
				SaveMembers();
			}
			catch (Exception ex)
			{
				return false;
			}

			LoadData();

			return true;
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
						Phone = member.Phone
					});
				}
				else
				{
					Database.UpdateMember(new Database.Model.Member
					{
						Id = member.Id,
						FirstName = member.FirstName,
						LastName = member.LastName,
						Phone = member.Phone
					});
				}
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

		public IEnumerable<Account> GetAccounts()
		{
			return Accounts.AsEnumerable();
		}

		public IEnumerable<Member> GetMembers()
		{
			return Members.AsEnumerable();
		}

		public IEnumerable<Child> GetChildren()
		{
			return Children.AsEnumerable();
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

		public void DeleteMember(Member member)
		{
			member.Updated = true;
			member.Deleted = true;
		}

		public void DeleteChild(Child child)
		{
			child.Updated = true;
			child.Deleted = true;
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

		public void AddMember(Member member)
		{
			MarkForAdd(member);
			Members.Add(member);
		}

		public void MarkForAdd(DataObject data)
		{
			data.Added = true;
			data.Updated = true;
		}

	}
}
