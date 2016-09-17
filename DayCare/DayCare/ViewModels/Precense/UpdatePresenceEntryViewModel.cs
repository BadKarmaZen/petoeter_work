using Caliburn.Micro;
using DayCare.Model.Lite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Precense
{
	public class UpdatePresenceEntryViewModel : Screen
	{
		#region Members

		DateTime _date;
		Member _member;

		#endregion

		#region Properties

		public Member Member
		{
			get { return _member; }
			set { _member = value; }
		}

		public int Hour 
		{
			get { return _date.Hour; }
			set
			{
				_date = new DateTime(_date.Year, _date.Month, _date.Day, value, _date.Minute, 0);
				NotifyOfPropertyChange(() => Hour);
			}
		}

		public int Minute
		{
			get { return _date.Minute; }
			set
			{
				_date = new DateTime(_date.Year, _date.Month, _date.Day, _date.Hour, value, 0);
				NotifyOfPropertyChange(() => Minute);
			}
		}

		#endregion


	}
}
