using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Database;
using DayCare.Model.UI;
using DayCare.ViewModels.Children;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Precense
{
	public class PresenceUI : TaggedItemUI<Presence>
	{
		private bool _toLate;
		public TimeSpan MaxTime { get; set; }

		public bool ToLate
		{
			get
			{
				return _toLate;
			}

			set
			{
				_toLate = value; NotifyOfPropertyChange(()=>ToLate);
			}
		}

		public void Update()
		{
			if (Tag.ArrivalTime == DateTime.MinValue)
			{
				ToLate = false;
			}
			else
			{
				var time = DateTime.Now;

				if (Tag.DepartureTime != DateTime.MinValue)
					time = Tag.DepartureTime;

				var delta = time - Tag.ArrivalTime;

				ToLate = delta > MaxTime;
			}
		}
	}

	public class PresenceMainViewModel : Screen
	{
		private List<PresenceUI> _presenceList;

		public List<PresenceUI> PresenceList
		{
			get { return _presenceList; }
			set { _presenceList = value; NotifyOfPropertyChange(() => PresenceList); }
		}

		public PresenceMainViewModel()
		{
			var today = DateTime.Now;
			var model = ServiceProvider.Instance.GetService<Petoeter>();
			var data = model.GetPresenceData();

			PresenceList = (from p in data
											from s in model.GetSchedule(s => s.Child_Id == p.Child_Id && s.ValidDate(today))
											orderby p.FullName
											select new PresenceUI
											{
												Name = p.FullName,
												Tag = p,
												MaxTime = new TimeSpan(0, s.GetAllowedTime(DateTime.Now), 0)
											}).ToList();

			PresenceList.ForEach(p => p.Update());
		}

		public void SelectChildAction(PresenceUI child)
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
								new Core.Events.ShowDialog
								{
									Dialog = new EditPrecenseViewModel(child.Tag)
								});
		}
	}
}
