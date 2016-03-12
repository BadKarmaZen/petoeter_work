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
		internal void DeleteSchedule(Guid scheduleId)
		{
			DeleteScheduleDetails(scheduleId);
			DeleteRecord(GetData<Schedule>(s => s.Id == scheduleId).FirstOrDefault());
		}

		internal void DeleteScheduleDetails(Guid scheduleId)
		{ 
			foreach (var detail in GetData<ScheduleDetail>(sd => sd.Schedule_Id == scheduleId))
			{
				ObliterateRecord(detail);
			}
		}

		internal void AddSchedule(Schedule schedule)
		{
			_schedules.Add(schedule);
			AddRecord(schedule);
		}

		internal void UpdateSchedule(Schedule schedule)
		{
			var old = GetData<Schedule>(s => s.Id == schedule.Id).FirstOrDefault();

			if (old == null)
			{
				AddSchedule(schedule);
			}
			else
			{
				_schedules.Remove(old);
				_schedules.Add(schedule);
				UpdateRecord(schedule);
			}
		}

		internal void AddScheduleDetail(ScheduleDetail scheduleDetail)
		{
			_scheduleDetails.Add(scheduleDetail);
			AddRecord(scheduleDetail);
		}
	}
}