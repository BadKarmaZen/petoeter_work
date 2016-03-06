using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model;
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
			var model = ServiceProvider.Instance.GetService<Petoeter>();

			Items = (from c in model.GetChildren()
							 where c.Deleted == false
							 orderby c.FirstName
							 select new ChildUI
							 {
								 Name = string.Format("{0} {1}", c.FirstName, c.LastName),
								 Tag = c
							 }).ToList();

			base.LoadItems();
		}

		//public void AddAction()
		//{
		//    ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
		//        new Core.Events.ShowDialog
		//        {
		//            Dialog = new Edi(_account)
		//        });
		//}

		public void EditAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new EditChildScheduleViewModel(SelectedItem.Tag)
				});
		}

		public void OpenAction(ChildUI child)
		{
			SelectItem(child);
			EditAction();
		}

		//public void DeleteAction()
		//{
		//    ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
		//       new Events.ShowDialog
		//       {
		//           Dialog = new YesNoDialogViewModel
		//           {
		//               Message = "Ben je zeker?",
		//               Yes = () => DeleteItem()
		//           }
		//       });
		//}
	}
}
