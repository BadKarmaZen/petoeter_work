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
		internal void DeleteMember(Guid memberId)
		{
			DeleteRecord(GetData<Member>(m => m.Id == memberId).FirstOrDefault());
		}

		internal void AddMember(Member member)
		{
			_members.Add(member);
			AddRecord(member);
		}


		internal void UpdateMember(Member member)
		{
			var old = GetData<Member>(m => m.Id == member.Id).FirstOrDefault();

			if (old == null)
			{
				AddRecord(member);
			}
			else
			{
				_members.Remove(old);
				_members.Add(member);
				UpdateRecord(member);
			}
		}
	}
}