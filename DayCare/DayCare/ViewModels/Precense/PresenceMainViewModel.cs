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
			var model = ServiceProvider.Instance.GetService<Petoeter>();
			var data = model.GetPresenceData();

			PresenceList = (from p in data
											orderby p.FullName
											select new PresenceUI
											{
												Name = p.FullName,
												Tag = p
											}).ToList();
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
