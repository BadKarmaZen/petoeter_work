using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Database
{
    public class Petoeter
    {
        public enum QueryKind
        {
            Select,
            Insert,
            Update,
            Delete,
            Obliterate
        }

        public class QueryInfo
        {
            public string Select { get; set; }
            public string Insert { get; set; }
            public string Update { get; set; }
            public string Delete { get; set; }
            public string Obliterate { get; set; }

            public PropertyInfo[] Properties { get; set; }

            public string CreateQuery(QueryKind kind, object data)
            {
                switch (kind)
                {
                    case QueryKind.Select:
                        return Select;
                    case QueryKind.Insert:
                        return CreateInsert(data);
                    case QueryKind.Update:
                        return CreateUpdate(data);
                    case QueryKind.Delete:
                        break;
                    case QueryKind.Obliterate:
                        return Select;
                }

                return string.Empty;
            }

            private string CreateInsert(object data)
            {
                List<object> values = new List<object>();
                foreach (var property in Properties)
                {
                    values.Add(property.GetMethod.Invoke(data, null));
                }

                return string.Format(Insert, values.ToArray());
            }

            private string CreateUpdate(object data)
            {
                var updates = from p in Properties
                              let name = p.Name.ToLower()
                              where name.Equals("id") == false
                              select p;

                List<object> values = new List<object>();
                foreach (var property in updates)
                {
                    values.Add(property.GetMethod.Invoke(data, null));
                }

                values.Add(Properties.First(p => p.Name.ToLower().Equals("id")).GetMethod.Invoke(data, null));

                return string.Format(Update, values.ToArray());
            }

            /*var updates = from p in properties
                          let name = p.Name.ToLower()
                          where name.Equals("id") == false
                          select new { Name = name, Type = p.PropertyType.ToString() };*/
        }

        private Dictionary<Type, QueryInfo> Queries = new Dictionary<Type, QueryInfo>();

        public void CreateQueries(Type type)
        {
            var properties = type.GetProperties();
            var info = new QueryInfo { Properties = properties };

            var table = type.Name.ToLower();

            info.Select = string.Format("select * from {0};", table);

            info.Insert = CreateInsertQuery(properties, table);
            info.Update = CreateUpdateQuery(properties, table);
            info.Delete = string.Format("update {0} set delete = 1 where id like '{{0}}';", table);
            info.Obliterate = string.Format("delete from {0} where id like '{{0}}';", table);

            Queries.Add(type, info);
        }

        private string CreateInsertQuery(PropertyInfo[] properties, string table)
        {
            var propNames = from p in properties
                        select p.Name.ToLower();

            var valueHolders = new List<string>();
            int valueIndex = 0;

            foreach (var property in properties)
            {
                switch (property.PropertyType.ToString())
                {
                    case "System.Int32":
                    case "System.Boolean":
                        valueHolders.Add(string.Format("{{{0}}}", valueIndex++));
                        break;
                    default:
                        valueHolders.Add(string.Format("'{{{0}}}'", valueIndex++));
                        break;
                }
            }

            return string.Format("insert into {0} ({1}) values ({2});", table,
                string.Join(", ", propNames), string.Join(", ", valueHolders));
        }

        private string CreateUpdateQuery(PropertyInfo[] properties, string table)
        {
            var updates = from p in properties
                          let name = p.Name.ToLower()
                          where name.Equals("id") == false
                          select new { Name = name, Type = p.PropertyType.ToString() };

            var updateHolders = new List<string>();
            int updateIndex = 0;

            foreach (var property in updates)
            {
                switch (property.Type)
                {
                    case "System.Int32":
                    case "System.Boolean":
                        updateHolders.Add(string.Format("{0} = {{{1}}}", property.Name, updateIndex++));
                        break;
                    default:
                        updateHolders.Add(string.Format("{0} = '{{{1}}}'", property.Name, updateIndex++));
                        break;
                }
            }

            return string.Format("update {0} set {1} where id like '{{{2}}}';", table, string.Join(", ", updateHolders), updateIndex);
        }

        public Petoeter()
        {
            CreateQueries(typeof(Account));
            CreateQueries(typeof(Member));

            Account test = new Account { Id = Guid.NewGuid(), Name = "Kurt" };

            var insert = Queries[typeof(Account)].CreateQuery(QueryKind.Update, test);

            test.Name = insert;

        }
    }
}
