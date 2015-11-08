using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Database
{
    public class Petoeter
    {
        #region Members
        private Dictionary<Type, QueryInfo> Queries = new Dictionary<Type, QueryInfo>();

        public string ConnectionString { get; set; }
        public MySqlConnection DataBase { get; set; }


        private List<Child> _children = new List<Child>();
        private List<Account> _accounts = new List<Account>();
        private List<Member> _members = new List<Member>();
        private List<Schedule> _schedules = new List<Schedule>();
        #endregion

        

        public Petoeter()
        {
            CreateQueries(typeof(Account));
            CreateQueries(typeof(Member));
            CreateQueries(typeof(Child));
						CreateQueries(typeof(Schedule));
						CreateQueries(typeof(Presence), "created = @param_date");

            InitalizeDatabase();
        }

        private void InitalizeDatabase()
        {
			//ConnectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3};", "192.168.1.100", "petoeter", "admin", "666777");
			ConnectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3};", "localhost", "petoeter", "admin", "666777");
			DataBase = new MySqlConnection(ConnectionString);

            LoadData(Queries[typeof(Account)], _accounts);
            LoadData(Queries[typeof(Member)], _members);
            LoadData(Queries[typeof(Child)], _children);
            LoadData(Queries[typeof(Schedule)], _schedules);

            //LinkAccount();
        }

        //private void LinkAccount()
        //{
        //    //  link children to the account
        //    //
        //    foreach (var account in _accounts)
        //    {
        //        account.Children = GetChild(c => c.Account_Id == account.Id).ToList();
        //        //account.Members = GetMember(m => m.Account_Id == account.Id).ToList();
        //    }
        //}

        //private void LoadAccounts()
        //{


        //}

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

				public IEnumerable<Presence> GetPresenceData()
				{
					var today = DateTime.Now.Date;
					var data = new List<Presence>();
					LoadData(Queries[typeof(Presence)], data, Param("@param_date", today));

					if (data.Count == 0)
					{
						data = (from c in GetChild(c => c.Deleted == false)
										from s in GetSchedule(s => s.Child_Id == c.Id && s.ValidDate(today))
										select new Presence
										{
											Id = Guid.NewGuid(),
											Child_Id = c.Id,
											Created = today,
											FullName = c.FirstName,	// string.Format("{0})" c.ToString()
										}).ToList();

						foreach (var p in data)
						{
							SavePresenceItem(p);							
						}
					}

						//Queries[typeof(Presence)];
					//query.SelectQuery.

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
        private void CreateQueries(Type type, string selectClause = null)
        {
            Queries.Add(type, QueryInfo.CreateType(type, selectClause));
        }

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

        internal void SaveRecord(DatabaseRecord record)
        {
                try
                {
                    DataBase.Open();

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
           //}
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

            //var account = GetAccount(a => a.Id == child.Account_Id).FirstOrDefault();
            //if(account != null)
            //{
            //    account.Children.Add(child);
            //}
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
		}
}
