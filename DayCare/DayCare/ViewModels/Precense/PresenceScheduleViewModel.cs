using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Lite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Precense
{
	class PresenceScheduleViewModel : Screen, ICloseScreen
	{
		#region Members
		private List<PresenceUI> _presenceList;
		private DateTime _date;
		
		#endregion

		#region Properties
		public DateTime Date
		{
			get { return _date; }
			set { _date = value; NotifyOfPropertyChange(() => Date); }
		}

		public List<PresenceUI> PresenceList
		{
			get { return _presenceList; }
			set { _presenceList = value; NotifyOfPropertyChange(() => PresenceList); }
		}
		
		#endregion

		public PresenceScheduleViewModel()
		{
			LogManager.GetLog(GetType()).Info("Create");

			Date = DateTime.Now.Date.AddDays(1);

			LoadPresenceData();
		}

		public void CloseThisScreen()
		{
		}

		private void LoadPresenceData()
		{
			LogManager.GetLog(GetType()).Info("LoadPresenceData({0})", _date.ToShortDateString());
			
			//	create a presence record for all children for today
			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var query = from c in db.Children.FindAll()
										where c.Schedule.Exists(d => d.Day == _date)
										orderby c.LastName
										orderby c.FirstName
										select new PresenceUI
										{
											Tag = new Presence { Child = c },
											Name = c.GetFullName()
										};

				PresenceList = query.ToList();
			}
		}

		#region Actions	

		public void NextDayAction()
		{
			Date = Date.AddDays(1);
			if (Date.DayOfWeek == DayOfWeek.Saturday)
			{
				Date = Date.AddDays(2);
			}
			LoadPresenceData();
		}

		public void PreviousDayAction()
		{
			Date = Date.AddDays(-1);
			if (Date.DayOfWeek == DayOfWeek.Sunday)
			{
				Date = Date.AddDays(-2);
			}
			LoadPresenceData();
		}

		#endregion
	}
}
