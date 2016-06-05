using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Model.Lite
{
	public class PetoeterDb : LiteDatabase
	{
		public PetoeterDb(string file)
			: base(file)
		{
		}

		public const string FileName = @"E:\petoeter_lite.ldb";

		protected override void OnModelCreating(BsonMapper mapper)
		{
			base.OnModelCreating(mapper);

			mapper.Entity<Account>()
				.DbRef(a => a.Members, "member")
				.DbRef(a => a.Children, "children");
		}

		public LiteCollection<Account> Accounts
		{
			get
			{
				return GetCollection<Account>("account")
					.Include(a => a.Members)
					.Include(a => a.Children);
			}
		}

		public LiteCollection<Member> Members
		{
			get
			{
				return GetCollection<Member>("member");
			}
		}

		public LiteCollection<Child> Children
		{
			get
			{
				return GetCollection<Child>("children");
			}
		}

		public LiteCollection<Date> Holidays
		{
			get
			{
				return GetCollection<Date>("holidays");
			}
		}

		public SystemSettings Settings { get; set; }
	}

}
