using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Lite;
using DayCare.ViewModels.Children;
using DayCare.ViewModels.Members;
using DayCare.ViewModels.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Scheduler
{
	public class SchedulerMainViewModel : FilteredListItemScreen<ChildUI>
	{
		protected override void LoadItems()
		{
			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var query = from c in db.Children.FindAll()
										where c.Deleted == false
										orderby c.BirthDay
										select new ChildUI
										{
											Name = string.Format("{0} {1}", c.FirstName, c.LastName),
											Tag = c
										};

				Items = query.ToList();
			}

			base.LoadItems();
		}
		
		public void EditAction()
		{
			//ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
			//	new Core.Events.SwitchTask
			//	{
			//		Task = new EditChildScheduleViewModel(SelectedItem.Tag)
			//	});

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new EditChildCalendarViewModel(SelectedItem.Tag)
				});
		}

		public void OpenAction(ChildUI child)
		{
			SelectItem(child);
			EditAction();
		}
	}
}
