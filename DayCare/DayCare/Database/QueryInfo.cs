using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Database
{
	class QueryInfo
	{
		#region Converter
		public static Dictionary<Type, Func<object, object>> Convertion;

		static QueryInfo()
		{
			Convertion = new Dictionary<Type, Func<object, object>>();
			Convertion.Add(typeof(Guid), (data) => Guid.Parse(data.ToString()));
			Convertion.Add(typeof(bool), (data) => Convert.ToBoolean(data));
			Convertion.Add(typeof(DateTime), (data) => (data is System.DBNull) ? DateTime.MinValue : Convert.ToDateTime(data));
			Convertion.Add(typeof(string), (data) => (data is System.DBNull) ? string.Empty : Convert.ToString(data));
		}
		#endregion

		#region Properties
		private string Select { get; set; }
		private string Insert { get; set; }
		private string Update { get; set; }
		private string Delete { get; set; }
		private string Obliterate { get; set; }
		private PropertyInfo[] Properties { get; set; }
		private PropertyInfo ID { get; set; }
		public Func<MySqlDataReader, object> Creator { get; set; }
		#endregion

		#region Queries
		public MySqlCommand SelectQuery(MySqlConnection database)
		{
			var command = database.CreateCommand();
			command.CommandText = Select;

			return command;
		}

		public MySqlCommand InsertQuery(MySqlConnection database, object data)
		{
			var command = database.CreateCommand();
			command.CommandText = Insert;

			foreach (var property in Properties)
			{
				command.Parameters.AddWithValue(string.Format("@param_{0}", property.Name),
					 property.GetMethod.Invoke(data, null));
			}

			command.Parameters.AddWithValue("@param_id", ID.GetMethod.Invoke(data, null));

			return command;
		}

		public MySqlCommand UpdateQuery(MySqlConnection database, object data)
		{
			var command = database.CreateCommand();
			command.CommandText = Update;

			foreach (var property in Properties)
			{
				command.Parameters.AddWithValue(string.Format("@param_{0}", property.Name),
					 property.GetMethod.Invoke(data, null));
			}
			command.Parameters.AddWithValue("@param_id", ID.GetMethod.Invoke(data, null));

			return command;
		}

		public MySqlCommand DeleteQuery(MySqlConnection database, object data)
		{
			var command = database.CreateCommand();
			command.CommandText = Delete;

			foreach (var property in Properties)
			{
				command.Parameters.AddWithValue(string.Format("@param_{0}", property.Name),
					 property.GetMethod.Invoke(data, null));
			}

			command.Parameters.AddWithValue("@param_id", ID.GetMethod.Invoke(data, null));

			return command;
		}

		public MySqlCommand ObliterateQuery(MySqlConnection database, object data)
		{
			var command = database.CreateCommand();
			command.CommandText = Obliterate;

			foreach (var property in Properties)
			{
				command.Parameters.AddWithValue(string.Format("@param_{0}", property.Name),
					 property.GetMethod.Invoke(data, null));
			}

			command.Parameters.AddWithValue("@param_id", ID.GetMethod.Invoke(data, null));

			return command;
		}

		#endregion

		#region Factories

		public static QueryInfo CreateType(Type type, string selectClause = null)
		{
			var table = type.Name.ToLower();
			var properties = from p in type.GetProperties()
											 let a = p.GetCustomAttribute<DatabaseIgnoreAttribute>()
											 where a == null
											 select p;

			var info = new QueryInfo
			{
				ID = properties.First(p => p.Name.Equals("Id")),
				Properties = properties.Where(p => !p.Name.Equals("Id")).ToArray()
			};

			if (string.IsNullOrWhiteSpace(selectClause))
			{
				info.Select = string.Format("select * from {0};", table);
			}
			else
			{
				info.Select = string.Format("select * from {0} where {1};", table, selectClause);
			}

			CreateInsertQuery(info, table);
			CreateUpdateQuery(info, table);
			CreateDeleteQuery(info, table);
			CreateObliterateQuery(info, table);
			CreateObjectCreator(info, type);

			return info;
		}

		private static void CreateObjectCreator(QueryInfo info, Type type)
		{
			info.Creator = row =>
			{
				var data = Activator.CreateInstance(type);

				//  ID
				info.ID.SetValue(data, QueryInfo.Convertion[info.ID.PropertyType](row["id"]));

				//  Properties
				foreach (var property in info.Properties)
				{
					if (QueryInfo.Convertion.ContainsKey(property.PropertyType))
					{
						property.SetValue(data, QueryInfo.Convertion[property.PropertyType](row[property.Name]));
					}
					else
					{
						property.SetValue(data, row[property.Name]);
					}
				}
				return data;
			};
		}

		private static void CreateInsertQuery(QueryInfo query, string table)
		{
			var propNames = new List<string> { "id" };
			propNames.AddRange(from p in query.Properties
												 select p.Name.ToLower());

			var paramHolder = new List<string>();

			foreach (var name in propNames)
			{
				paramHolder.Add(string.Format("@param_{0}", name));
			}

			query.Insert = string.Format("insert into {0} ({1}) values ({2});", table,
					string.Join(", ", propNames), string.Join(", ", paramHolder));
		}

		private static void CreateUpdateQuery(QueryInfo query, string table)
		{
			var updateHolders = (from p in query.Properties
													 select string.Format("{0} = @param_{0}", p.Name.ToLower())).ToList();

			query.Update = string.Format("update {0} set {1} where id like @param_id;",
					table, string.Join(", ", updateHolders));
		}

		private static void CreateDeleteQuery(QueryInfo query, string table)
		{
			query.Delete = string.Format("update {0} set deleted = 1 where id like @param_id;", table);
		}

		private static void CreateObliterateQuery(QueryInfo query, string table)
		{
			query.Obliterate = string.Format("delete from {0} where id like @param_id;", table);
		}


		#endregion
	}
}
