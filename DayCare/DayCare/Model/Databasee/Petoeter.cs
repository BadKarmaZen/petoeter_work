using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model//.Database
{
	public class Petoeterex
	{
	/*	//public const string DatabaseName = "petoeter";
		public const string DatabaseName = "petoeter_live";
		#region Members


		public static readonly Schedule EmptySchedule = new Schedule();


		private Dictionary<Type, QueryInfo> Queries = new Dictionary<Type, QueryInfo>();

		


		private List<Child> _children = new List<Child>();
		private List<Account> _accounts = new List<Account>();
		private List<Member> _members = new List<Member>();
		private List<Schedule> _schedules = new List<Schedule>();
		private List<Holiday> _holydays = new List<Holiday>();
		private List<Expense> _expenses = new List<Expense>();
		#endregion

		public SystemSetting Settings { get; set; }



		public Petoeterex()
		{
			CreateQueries(typeof(Member));
			CreateQueries(typeof(Child));
			CreateQueries(typeof(Schedule));
			CreateQueries(typeof(Presence), "created = @param_date");
			CreateQueries(typeof(Holiday));
			CreateQueries(typeof(Expense), "date >= @from_date and date <= @to_date");

			InitalizeDatabase();
		}

		private void InitalizeDatabase()
		{
			//ConnectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3};", "192.168.1.100", "petoeter", "admin", "666777");
			ConnectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3};", "localhost", DatabaseName, "admin", "666777");
			DataBase = new MySqlConnection(ConnectionString);

			LoadSystemSettings();

			var updater = new DatabaseUpdate(this);
			updater.Upgrade();

			LoadData(Queries[typeof(Account)], _accounts);
			LoadData(Queries[typeof(Member)], _members);
			LoadData(Queries[typeof(Child)], _children);
			LoadData(Queries[typeof(Schedule)], _schedules);
			LoadData(Queries[typeof(Holiday)], _holydays);
		}

		private bool LoadSystemSettings()
		{
			try
			{
				DataBase.Open();

				var cmd = DataBase.CreateCommand();
				cmd.CommandText = "select * from system";

				var rdr = cmd.ExecuteReader();

				if (rdr.Read())
				{
					Settings = new SystemSetting();
					Settings.ImageFolder = rdr["picture_folder"] as string;

					string version = rdr["version"] as string;
					Settings.DatabaseVersion = Version.Parse(version);

					var timestamp = rdr["export_timestamp"];
					Settings.ExporTimeStamp = (timestamp is System.DBNull) ? DateTime.MinValue : Convert.ToDateTime(timestamp);
				}

				return true;
			}
			catch (Exception ex)
			{
			}
			finally
			{
				DataBase.Close();
			}

			return false;
		}

		public bool SaveSystemSettings()
		{
			try
			{
				DataBase.Open();

				var cmd = DataBase.CreateCommand();
				cmd.CommandText = "Update system set picture_folder = @picture_folder, export_timestamp = @export_timestamp, version = @version;";

				cmd.Parameters.Add("@picture_folder", Settings.ImageFolder);
				cmd.Parameters.Add("@export_timestamp", Settings.ExporTimeStamp);
				cmd.Parameters.Add("@version", Settings.DatabaseVersion.ToString(3));

				cmd.ExecuteNonQuery();

				return true;
			}
			catch (Exception ex)
			{
			}
			finally
			{
				DataBase.Close();
			}

			return false;
		}


		public IEnumerable<Account> GetAccount(Expression<Func<Account, bool>> predicate = null)
		{
			var query = _accounts.AsEnumerable();

			if (predicate != null)
			{
				query = query.Where(predicate.Compile());
			}

			return query.AsEnumerable();
		}

		public IEnumerable<Child> GetChild(Expression<Func<Child, bool>> predicate = null)
		{
			var query = _children.AsEnumerable();

			if (predicate != null)
			{
				query = query.Where(predicate.Compile());
			}

			return query.AsEnumerable();
		}

		public IEnumerable<Member> GetMember(Expression<Func<Member, bool>> predicate = null)
		{
			var query = _members.AsEnumerable();

			if (predicate != null)
			{
				query = query.Where(predicate.Compile());
			}

			return query.AsEnumerable();
		}

		public IEnumerable<Schedule> GetSchedule(Expression<Func<Schedule, bool>> predicate = null)
		{
			var query = _schedules.AsEnumerable();

			if (predicate != null)
			{
				query = query.Where(predicate.Compile());
			}

			return query.AsEnumerable();
		}

		public GroupSchedule GetGroupSchedule(Guid groupId)
		{
			var grpSchedule = new GroupSchedule(from s in GetSchedule(s => s.Group_Id == groupId)
																					orderby s.Group_Index
																					select s);

			return grpSchedule;
		}

		public IEnumerable<GroupSchedule> GetGroupSchedules()
		{
			var grpIds = (from s in _schedules
										select s.Group_Id).Distinct();

			return from id in grpIds select GetGroupSchedule(id);
		}

		public IEnumerable<GroupSchedule> GetValidGroupSchedules(DateTime date)
		{
			var grpIds = (from s in _schedules
										where s.ValidDate(date)
										select s.Group_Id).Distinct();

			return from id in grpIds select GetGroupSchedule(id);
		}

		public IEnumerable<Holiday> GetHoliday(Expression<Func<Holiday, bool>> predicate = null)
		{
			var query = _holydays.AsEnumerable();

			if (predicate != null)
			{
				query = query.Where(predicate.Compile());
			}

			return query.AsEnumerable();
		}

		public IEnumerable<Presence> GetPresenceData()
		{
			var today = DateTimeProvider.Now().Date;
			var data = new List<Presence>();
			LoadData(Queries[typeof(Presence)], data, Param("@param_date", today));

			if (data.Count == 0)
			{
				var children = GetChild(c => c.Deleted == false);

				foreach (var child in children)
				{
					var schedule = GetCurrentSchedule(child, today);

					if (schedule != EmptySchedule)
					{
						var timeCode = schedule.GetAllowedTime(today);

						if (timeCode != 0)
						{
							data.Add(new Presence
							{
								Id = Guid.NewGuid(),
								Child_Id = child.Id,
								Created = today,
								FullName = child.FirstName,
								TimeCode = timeCode
							});
						}
					}
				}

				foreach (var p in data)
				{
					SavePresenceItem(p);
				}
			}

			return data;
		}

		private void SavePresenceItem(Presence presence)
		{
			try
			{
				DataBase.Open();

				var command = Queries[typeof(Presence)].InsertQuery(DataBase, presence);
				int result = command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
			}
			finally
			{
				DataBase.Close();
			}
		}

		private Tuple<string, object> Param(string param, object value)
		{
			return new Tuple<string, object>(param, value);
		}

		#region Database Helper


		//private void LoadData<T>(string cmdtext, List<T> list, Func<MySqlDataReader, object> create)
		private void LoadData<T>(QueryInfo query, List<T> list, params Tuple<string, object>[] parameters)
				where T : class
		{
			try
			{
				DataBase.Open();

				if (DataBase.State == ConnectionState.Open)
				{
					var cmd = query.SelectQuery(DataBase);

					if (parameters != null && parameters.Length != 0)
					{
						foreach (var parameter in parameters)
						{
							cmd.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
						}
					}

					var rdr = cmd.ExecuteReader();

					while (rdr.Read())
					{
						list.Add(query.Creator(rdr) as T);
					}
				}
			}
			catch (Exception ex)
			{
				//throw;
			}
			finally
			{
				DataBase.Close();
			}

		}
		#endregion

		public void UpdateRecord(DatabaseRecord record)
		{
			try
			{
				DataBase.Open();

				record.Updated = DateTimeProvider.Now();

				var command = Queries[record.GetType()].UpdateQuery(DataBase, record);
				int result = command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
			}
			finally
			{
				DataBase.Close();
			}
		}

		internal void DeleteRecord(DatabaseRecord record)
		{
			record.Deleted = true;

			try
			{
				DataBase.Open();

				var command = Queries[record.GetType()].DeleteQuery(DataBase, record);
				int result = command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
			}
			finally
			{
				DataBase.Close();
			}
		}

		internal void ObliterateRecord(DatabaseRecord record)
		{
			try
			{
				DataBase.Open();

				var command = Queries[record.GetType()].ObliterateQuery(DataBase, record);
				int result = command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
			}
			finally
			{
				DataBase.Close();
			}
		}

		internal void SaveRecord(DatabaseRecord record)
		{
			try
			{
				DataBase.Open();

				record.Updated = DateTimeProvider.Now();

				var command = Queries[record.GetType()].InsertQuery(DataBase, record);
				int result = command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
			}
			finally
			{
				DataBase.Close();
			}
		}

		internal void SaveHoliday(Holiday day)
		{
			_holydays.Add(day);
			SaveRecord(day);
		}

		internal void ObliterateHoliday(Holiday day)
		{
			_holydays.Remove(day);
			ObliterateRecord(day);
		}

		public void SaveAccount(Account account)
		{
			SaveRecord(account);
			_accounts.Add(account);
		}

		internal void SaveChild(Child child)
		{
			SaveRecord(child);
			_children.Add(child);
		}

		internal void SaveMember(Member member)
		{
			SaveRecord(member);
			_members.Add(member);
		}

		internal void SaveSchedule(Schedule schedule)
		{
			var curSchedules = GetSchedule(s => s.Child_Id == schedule.Child_Id);
			Schedule newEnd = null;

			foreach (var sched in curSchedules)
			{
				if (sched.InPeriod(schedule))
				{
					newEnd = sched.Copy() as Schedule;

					//  begin part
					sched.EndDate = schedule.StartDate.AddSeconds(-1);
					UpdateRecord(sched);

					//  End part
					newEnd.StartDate = schedule.EndDate.AddSeconds(1);
				}
			}

			SaveRecord(schedule);
			_schedules.Add(schedule);

			if (newEnd != null)
			{
				SaveRecord(newEnd);
				_schedules.Add(newEnd);
			}
		}


		internal void UpdatePresence(Presence presence)
		{
			try
			{
				DataBase.Open();

				var command = Queries[presence.GetType()].UpdateQuery(DataBase, presence);
				int result = command.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
			}
			finally
			{
				DataBase.Close();
			}
		}

		internal Schedule GetCurrentSchedule(Child child, DateTime day)
		{
			var ids = from s in _schedules
								where s.Child_Id == child.Id &&
											s.ValidDate(day)
								select s.Group_Id;

			var groups = (from id in ids.Distinct()
										select GetGroupSchedule(id)).ToList();

			if (groups.Count != 0)
			{
				return groups[0].GetScheduleOn(day);
			}

			return EmptySchedule;
		}


		internal void DeleteAccount(Account account)
		{
			//	first delete child and family

			var children = (from c in _children
											where c.Account_Id == account.Id
											select c).ToList();

			foreach (var child in children)
			{
				child.Deleted = true;
				DeleteChild(child);
			}

			var members = (from m in _members
										 where m.Account_Id == account.Id
										 select m).ToList();

			foreach (var member in members)
			{
				member.Deleted = true;
				DeleteMemeber(member);
			}

			DeleteRecord(account);
		}

		private void DeleteMemeber(Member member)
		{
			DeleteRecord(member);
		}

		private void DeleteChild(Child child)
		{
			//	delete schedules
			var schedules = (from s in _schedules
											 where s.Child_Id == child.Id
											 select s).ToList();

			foreach (var schedule in _schedules)
			{
				schedule.Deleted = true;
				DeleteRecord(schedule);
			}

			DeleteRecord(child);
		}

		//internal void ExecuteCommand(string query)
		//{
		//	try
		//	{
		//		DataBase.Open();

		//		var cmd = DataBase.CreateCommand();
		//		cmd.CommandText = query;
		//		cmd.ExecuteNonQuery();
		//	}
		//	catch (Exception ex)
		//	{
		//	}
		//	finally
		//	{
		//		DataBase.Close();
		//	}
		//}*/
	}
}
