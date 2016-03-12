using DayCare.Database.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Database
{
	public partial class DatabaseEngine
	{
		#region Member
		public const string DatabaseName = "petoeter_live";

		private Dictionary<Type, QueryInfo> Queries = new Dictionary<Type, QueryInfo>();
		private Dictionary<Type, object> Data = new Dictionary<Type, object>();

		private List<Child> _children = new List<Child>();
		private List<Account> _accounts = new List<Account>();
		private List<Member> _members = new List<Member>();
		private List<Schedule> _schedules = new List<Schedule>();
		private List<ScheduleDetail> _scheduleDetails = new List<ScheduleDetail>();
		private List<Holiday> _holydays = new List<Holiday>();
		#endregion

		#region Properties
		public string ConnectionString { get; set; }
		public MySqlConnection DataBase { get; set; }
		public Version SchemaVersion { get; set; }
		

		#endregion

		public DatabaseEngine()
		{
			CreateQueries(typeof(Account));
			CreateQueries(typeof(Member));
			CreateQueries(typeof(Child));
			CreateQueries(typeof(Schedule));
			CreateQueries(typeof(ScheduleDetail));
			CreateQueries(typeof(Holiday));

			InitalizeDatabase();
		}

		private void InitalizeDatabase()
		{
			ConnectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3};", "localhost", DatabaseName, "admin", "666777");
			
			DataBase = new MySqlConnection(ConnectionString);

			//LoadSystemSettings();

			//var updater = new DatabaseUpdate(this);
			//updater.Upgrade();

			LoadData(Queries[typeof(Account)], _accounts);
			LoadData(Queries[typeof(Member)], _members);
			LoadData(Queries[typeof(Child)], _children);
			LoadData(Queries[typeof(Schedule)], _schedules);
			LoadData(Queries[typeof(ScheduleDetail)], _scheduleDetails);
			LoadData(Queries[typeof(Holiday)], _holydays);
		}


		#region HelperFunctions
		private void LoadData<T>(QueryInfo query, List<T> list, params Tuple<string, object>[] parameters)
				where T : class
		{
			try
			{
				DataBase.Open();

				if (DataBase.State == ConnectionState.Open)
				{
					var cmd = query.SelectQuery(this.DataBase);

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
				Data.Add(typeof(T), list);
				DataBase.Close();
			}

		}

		private void DeleteRecord(DatabaseRecord record)
		{
			if (record == null)
			{
				return;				
			}

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

		private void ObliterateRecord(DatabaseRecord record)
		{
			if (record == null)
			{
				return;
			}

			//	record.Deleted = true;

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

		private void AddRecord(DatabaseRecord record)
		{
			try
			{
				DataBase.Open();

				record.Updated = DateTime.Now;

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

		private void UpdateRecord(DatabaseRecord record)
		{
			try
			{
				DataBase.Open();

				record.Updated = DateTime.Now;

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

		internal void ExecuteCommand(string query)
		{
			try
			{
				DataBase.Open();

				var cmd = DataBase.CreateCommand();
				cmd.CommandText = query;
				cmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
			}
			finally
			{
				DataBase.Close();
			}
		}

		private void CreateQueries(Type type, string selectClause = null)
		{
			Queries.Add(type, QueryInfo.CreateType(type, selectClause));
		}

		public IEnumerable<T> GetData<T>(Expression<Func<T, bool>> predicate = null)
		{
			var query = Data[typeof(T)] as IEnumerable<T>;

			if (query != null)
			{
				if (predicate != null)
				{
					query = query.Where(predicate.Compile());					
				}

				return query.AsEnumerable();
			}

			return new List<T>();
		}

		//private bool LoadSystemSettings()
		//{
		//	try
		//	{
		//		DataBase.Open();

		//		var cmd = DataBase.CreateCommand();
		//		cmd.CommandText = "select * from system";

		//		var rdr = cmd.ExecuteReader();

		//		if (rdr.Read())
		//		{
		//			Settings = new SystemSetting();
		//			Settings.ImageFolder = rdr["picture_folder"] as string;

		//			string version = rdr["version"] as string;
		//			Settings.DatabaseVersion = Version.Parse(version);

		//			var timestamp = rdr["export_timestamp"];
		//			Settings.ExporTimeStamp = (timestamp is System.DBNull) ? DateTime.MinValue : Convert.ToDateTime(timestamp);
		//		}

		//		return true;
		//	}
		//	catch (Exception ex)
		//	{
		//	}
		//	finally
		//	{
		//		DataBase.Close();
		//	}

		//	return false;
		//}

		//public bool SaveSystemSettings()
		//{
		//	try
		//	{
		//		DataBase.Open();

		//		var cmd = DataBase.CreateCommand();
		//		cmd.CommandText = "Update system set picture_folder = @picture_folder, export_timestamp = @export_timestamp, version = @version;";

		//		cmd.Parameters.Add("@picture_folder", Settings.ImageFolder);
		//		cmd.Parameters.Add("@export_timestamp", Settings.ExporTimeStamp);
		//		cmd.Parameters.Add("@version", Settings.DatabaseVersion.ToString(3));

		//		cmd.ExecuteNonQuery();

		//		return true;
		//	}
		//	catch (Exception ex)
		//	{
		//	}
		//	finally
		//	{
		//		DataBase.Close();
		//	}

		//	return false;
		//}
		#endregion


	}
}
