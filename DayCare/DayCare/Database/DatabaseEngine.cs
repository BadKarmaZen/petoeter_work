using DayCare.Core;
using DayCare.Database.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Database
{
	public partial class DatabaseEngine
	{
		#region Member
		//public string DatabaseName = "petoeter_live";
		//public const string DatabaseName = "petoeter_live_presence";
		public string DatabaseName { get; set; }

		private Dictionary<Type, QueryInfo> Queries = new Dictionary<Type, QueryInfo>();
		private Dictionary<Type, object> Data = new Dictionary<Type, object>();

		private List<Child> _children = new List<Child>();
		private List<Account> _accounts = new List<Account>();
		private List<Member> _members = new List<Member>();
		private List<Schedule> _schedules = new List<Schedule>();
		private List<ScheduleDetail> _scheduleDetails = new List<ScheduleDetail>();
		private List<Holiday> _holidays = new List<Holiday>();
		private List<Presence> _presence = new List<Presence>();
		private SystemSetting _systemSettings = new SystemSetting();


		#endregion

		#region Properties
		public string ConnectionString { get; set; }
		public MySqlConnection DataBase { get; set; }
		public SystemSetting SystemSettings
		{
			get { return _systemSettings; }
			set { _systemSettings = value; }
		}

		public DayCare.Model.Petoeter.ApplicationMode ApplicationMode { get; set; }

		#endregion

		public DatabaseEngine(DayCare.Model.Petoeter.ApplicationMode mode)
		{
			ApplicationMode = mode;

			CreateQueries(typeof(Account));
			CreateQueries(typeof(Member));
			CreateQueries(typeof(Child));
			CreateQueries(typeof(Schedule));
			CreateQueries(typeof(ScheduleDetail));
			CreateQueries(typeof(Holiday));
			CreateQueries(typeof(Presence));

			if (ApplicationMode == DayCare.Model.Petoeter.ApplicationMode.Configuration)
			{
				DatabaseName = 	"petoeter_live";
			}
			else
			{
				DatabaseName = "petoeter_live_presence";
			}

			InitalizeDatabase();
		}

		private void InitalizeDatabase()
		{
			ConnectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3};", "localhost", DatabaseName, "admin", "666777");
			
			DataBase = new MySqlConnection(ConnectionString);

			LoadSystemSettings();

			//var updater = new DatabaseUpdate(this);
			//updater.Upgrade();

			LoadData(Queries[typeof(Account)], _accounts);
			LoadData(Queries[typeof(Member)], _members);
			LoadData(Queries[typeof(Child)], _children);
			LoadData(Queries[typeof(Schedule)], _schedules);
			LoadData(Queries[typeof(ScheduleDetail)], _scheduleDetails);
			LoadData(Queries[typeof(Holiday)], _holidays);
			var param = new Tuple<string, object>("created", DateTime.Today.Date);
			LoadData(Queries[typeof(Presence)], _presence, new Tuple<string, object>("created", DateTime.Today.Date));
			LoadData(_presence);


			//if (ApplicationMode == DayCare.Model.Petoeter.ApplicationMode.Configuration)
			//{
			//	ExportConfigurationData(@"E:\temp");				
			//}
			//else
			//{
			//	ImportConfigurationData(@"E:\temp");
			//}
		}
		
		private void LoadData<T>(List<T> list)
		{
			var lt = typeof(T);
			var t = typeof(List<>);
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

		private void UpdateRecord(DatabaseRecord record, DateTime? updateTime = null)
		{
			try
			{
				DataBase.Open();

				record.Updated = updateTime.HasValue ? updateTime.Value :  DateTimeProvider.Now();

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

		public void AddHoliday(Holiday holiday)
		{
			_holidays.Add(holiday);
			AddRecord(holiday);
		}

		public void UpdateHoliday(Holiday holiday)
		{
			var old = GetData<Holiday>(h => h.Id == holiday.Id).FirstOrDefault();

			if (old == null)
			{
				AddRecord(holiday);
			}
			else
			{
				_holidays.Remove(old);
				_holidays.Add(holiday);
				UpdateRecord(holiday);
			}
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
					SystemSettings.PictureFolder = rdr["picture_folder"] as string;
					SystemSettings.CommunicationFolder = rdr["communication_folder"] as string;

					string version = rdr["version"] as string;
					SystemSettings.SchemaVersion = Version.Parse(version);

					var timestamp = rdr["export_timestamp"];
					SystemSettings.ExporTimeStamp = (timestamp is System.DBNull) ? DateTime.MinValue : Convert.ToDateTime(timestamp);
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
				cmd.CommandText = "Update system set picture_folder = @picture_folder, communication_folder = @communication_folder, export_timestamp = @export_timestamp, version = @version;";

				cmd.Parameters.Add("@picture_folder", SystemSettings.PictureFolder);
				cmd.Parameters.Add("@communication_folder", SystemSettings.CommunicationFolder);
				cmd.Parameters.Add("@export_timestamp", SystemSettings.ExporTimeStamp);
				cmd.Parameters.Add("@version", SystemSettings.SchemaVersion.ToString(3));

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

		/// <summary>
		/// Exports all the configuration data to the precense database
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public void ExportConfigurationData(string path)
		{
			var lastexporttime = _systemSettings.ExporTimeStamp;

			var export = new ExportData
			{
				Accounts = _accounts.Where(r => r.Updated > lastexporttime).ToList(),
				Members = _members.Where(r => r.Updated > lastexporttime).ToList(),
				Children = _children.Where(r => r.Updated > lastexporttime).ToList(),
				Schedules = _schedules.Where(r => r.Updated > lastexporttime).ToList(),
				ScheduleDetails = _scheduleDetails.Where(r => r.Updated > lastexporttime).ToList(),
				Holidays = _holidays.Where(r => r.Updated > lastexporttime).ToList()
			};

			export.SaveToFile(Path.Combine(path, "export_config.xml"));

			var exporttime = DateTimeProvider.Now();

			_accounts.Where(r => r.Updated > lastexporttime).Execute(r => UpdateRecord(r, exporttime));
			_members.Where(r => r.Updated > lastexporttime).Execute(r => UpdateRecord(r, exporttime));
			_children.Where(r => r.Updated > lastexporttime).Execute(r => UpdateRecord(r, exporttime));
			_schedules.Where(r => r.Updated > lastexporttime).Execute(r => UpdateRecord(r, exporttime));
			_scheduleDetails.Where(r => r.Updated > lastexporttime).Execute(r => UpdateRecord(r, exporttime));
			_holidays.Where(r => r.Updated > lastexporttime).Execute(r => UpdateRecord(r, exporttime));

			_systemSettings.ExporTimeStamp = exporttime;
			SaveSystemSettings();
		}

		public void ImportConfigurationData(string path)
		{
			var file = Path.Combine(path, "export_config.xml");

			if (File.Exists(file))
			{
				ExportData data = ExportData.LoadFromFile(file);

				data.Accounts.Execute(r => UpdateAccount(r));
				data.Members.Execute(r => UpdateMember(r));
				data.Children.Execute(r => UpdateChild(r));
				data.Schedules.Execute(r => UpdateSchedule(r));

				var scheduleDetails = from d in data.ScheduleDetails
															select d.Schedule_Id;

				scheduleDetails.Distinct().Execute(id =>
					{
						DeleteScheduleDetails(id);
					});

				data.ScheduleDetails.Execute(r => AddScheduleDetail(r));

				File.Delete(file);
			}
		}

		public void ExportPresenceData(string path) { }
		public void ImportPresenceData(string path) { }
		#endregion


	}

	public static class extention
	{
		public static void Execute<T>(this IEnumerable<T> data, Action<T> action)
			//where T: DatabaseRecord
		{
			if (data != null && action != null)
			{
				foreach (var item in data)
				{
					action(item);					
				}
			}
		}
	}
}
