using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Database;
using DayCare.Model.UI;
using DayCare.ViewModels.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Scheduler
{
	public class ScheduleUI : TaggedItemUI<Schedule>
	{ }

	public class EditChildScheduleViewModel : ListItemScreen<ScheduleUI>
	{
		private Child _child;

		public string Name { get { return string.Format("{0} {1}", _child.FirstName, _child.LastName);} }

		public EditChildScheduleViewModel(Child child)
		{
			this._child = child;
		}

		protected override void LoadItems()
		{
			var model = ServiceProvider.Instance.GetService<Petoeter>();

			Items = (from s in model.GetSchedule(s => s.Child_Id == _child.Id && s.Deleted == false)
							 orderby s.StartDate
							 select new ScheduleUI
							 {
								 Name = string.Format("{0} - {1}", s.StartDate.ToShortDateString(), s.EndDate.ToShortDateString()),
								 Tag = s
							 }).ToList();

			base.LoadItems();
		}

		public void AddAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.ShowDialog
				{
					Dialog = new AddScheduleViewModel(_child)
				});
		}

		public void BackAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new SchedulerMainViewModel()
				});
		}

		public void EditAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.ShowDialog
				{
					Dialog = new AddScheduleViewModel(_child, SelectedItem.Tag)
				});
		}

	}
}
