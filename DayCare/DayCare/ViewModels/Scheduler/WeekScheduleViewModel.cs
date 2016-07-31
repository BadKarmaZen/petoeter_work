using Caliburn.Micro;
using DayCare.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Scheduler
{
	public class ScheduleDayUI : PropertyChangedBase
	{
		static private string[] _day = new string[5] { "Ma", "Di", "Wo", "Do", "Vr" };

		#region Member
		private bool _morning;
		private bool _afternoon;
		#endregion

		#region Properties
		public bool Morning
		{
			get { return _morning; }
			set { _morning = value; NotifyOfPropertyChange(() => Morning); }
		}

		public bool Afternoon
		{
			get { return _afternoon; }
			set { _afternoon = value; NotifyOfPropertyChange(() => Afternoon); }
		}

		public int Index { get; set; }

		public string Day 
		{
			get
			{
				return _day[Index];
			}
		}
		#endregion
	}

	public class WeekScheduleViewModel : Screen
	{
		private List<ScheduleDayUI> _schedule;
		private int _index;

		public int Index
		{
			get { return _index; }
			set 
			{ 
				_index = value;
				NotifyOfPropertyChange(() => Index);
				NotifyOfPropertyChange(() => Header);
			}
		}

		public string Header 
		{
			get { return string.Format("Week {0}", Index); }
		}

		#region Properties

		public List<ScheduleDayUI> Schedule
		{
			get { return _schedule; }
			set { _schedule = value; NotifyOfPropertyChange(() => Schedule); }
		}

		#endregion

		public WeekScheduleViewModel(int index)
		{
			LogManager.GetLog(GetType()).Info("Create");
			
			_schedule = new List<ScheduleDayUI>(from i in Enumerable.Range(0, 5) select new ScheduleDayUI { Index = i });
			Index = index;
		}

		#region Actions

		public void ToggleAction(ScheduleDayUI ui)
		{
			ui.Morning = !ui.Morning;
			ui.Afternoon = ui.Morning;
		}

		public void ToggleMorningAction(ScheduleDayUI ui)
		{
			ui.Morning = !ui.Morning;
		}

		public void ToggleAfternoonAction(ScheduleDayUI ui)
		{
			ui.Afternoon = !ui.Afternoon;
		}

		#endregion
	}
}
