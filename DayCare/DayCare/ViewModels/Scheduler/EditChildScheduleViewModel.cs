﻿using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model;
using DayCare.Model.Lite;
using DayCare.Model.UI;
using DayCare.ViewModels.Dialogs;
using DayCare.ViewModels.Members;
using DayCare.ViewModels.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Scheduler
{
	public class ScheduleUI : TaggedItemUI<Schedule>
	{
	}

	public class EditChildScheduleViewModel : ListItemScreen<ScheduleUI>
	{
		private Child _child;

		public string Name { get { return string.Format("{0} {1}", _child.FirstName, _child.LastName);} }

		public EditChildScheduleViewModel(Child child)
		{
			Menu = new BackMenu(Menu, "98d040fb-0e97-4eaf-bee4-8f455650493b", BackAction);

			this._child = child;
		}

		protected override void LoadItems()
		{
			try
			{
				//Items = (from s in _child.Schedules
				//				 where s.Deleted == false
				//				 orderby s.StartDate
				//				 select new ScheduleUI
				//				 {
				//					 Name = string.Format("{0} - {1}", s.StartDate.ToShortDateString(), s.EndDate.ToShortDateString()),
				//					 Tag = s
				//				 }).ToList();
			}
			catch(Exception e)
			{
			}

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
					//Dialog = new AddScheduleViewModel(_child, SelectedItem.Tag)
				});
		}

		public void DeleteAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				 new Events.ShowDialog
				 {
					 Dialog = new YesNoDialogViewModel
					 {
						 Message = "Ben je zeker?",
						 Yes = () => DeleteItem()
					 }
				 });
		}

		public void ExceptionAction()
		{
 			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.ShowDialog
				{
					//Dialog = new ScheduleExceptionViewModel(_child, SelectedItem.Tag)
				});
		}

		protected override void DeleteItem()
		{
			var model = ServiceProvider.Instance.GetService<Petoeter>();

			//_child.Schedules.RemoveAll(s => s.Id == SelectedItem.Tag.Id);

			model.DeleteSchedule(SelectedItem.Tag);
			model.Save();

			SelectItem(null);

			LoadItems();
			
			base.DeleteItem();
		}
	}
}
