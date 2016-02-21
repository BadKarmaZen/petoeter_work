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
		internal void DeleteAccount(Guid accountId)
		{
			DeleteRecord(GetData<Account>(a => a.Id == accountId).FirstOrDefault());
		}

		internal void AddAccount(Account account)
		{
			AddRecord(account);
		}


		internal void UpdateAccount(Account account)
		{
			var old = GetData<Account>(a => a.Id == account.Id).FirstOrDefault();
			
			if (old == null)
			{
				AddAccount(account);				
			}
			else
			{
				_accounts.Remove(old);
				_accounts.Add(account);
				UpdateRecord(account);
			}
		}
	}
}