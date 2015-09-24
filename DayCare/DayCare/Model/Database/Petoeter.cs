using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        #endregion

        

        public Petoeter()
        {
            CreateQueries(typeof(Account));
            CreateQueries(typeof(Member));
            CreateQueries(typeof(Child));

            InitalizeDatabase();

            //Account test = new Account { Id = Guid.NewGuid(), Name = "Kurt" };

            //var insert = Queries[typeof(Account)].CreateQuery(QueryKind.Update, test);

            //test.Name = insert;

        }

        private void InitalizeDatabase()
        {
            ConnectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3};", "localhost", "petoeter", "admin", "666777");
            DataBase = new MySqlConnection(ConnectionString);

            LoadData(Queries[typeof(Account)], _accounts);
            LoadData(Queries[typeof(Member)], _members);
            LoadData(Queries[typeof(Child)], _children);
        }

        private void LoadAccounts()
        {


        }

        #region Database Helper
        private void CreateQueries(Type type)
        {
            Queries.Add(type, QueryInfo.CreateType(type));
        }

        //private void LoadData<T>(string cmdtext, List<T> list, Func<MySqlDataReader, object> create)
        private void LoadData<T>(QueryInfo query, List<T> list)
            where T : class
        {
            try
            {
                DataBase.Open();

                if (DataBase.State == ConnectionState.Open)
                {
                    var cmd = DataBase.CreateCommand();
                    cmd.CommandText = query.Select;

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
    }
}
