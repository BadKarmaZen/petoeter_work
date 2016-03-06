using DayCare.Database.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Database
{
	public partial class DatabaseEngine
	{
		internal void DeleteChild(Guid memberId)
		{
			DeleteRecord(GetData<Child>(m => m.Id == memberId).FirstOrDefault());
		}

		internal void AddChild(Child child)
		{
			_children.Add(child);
			AddRecord(child);
		}


		internal void UpdateChild(Child child)
		{
			var old = GetData<Child>(c => c.Id == child.Id).FirstOrDefault();

			if (old == null)
			{
				AddRecord(child);
			}
			else
			{
				_children.Remove(old);
				_children.Add(child);
				UpdateRecord(child);
			}
		}
	}
}
