using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Lite;
using DayCare.ViewModels.Dialogs;
using DayCare.ViewModels.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Children
{
	public class ChildrenMainViewModel : ListItemScreen<ChildUI>//ReactivatableScreen
	{
		private Account _account;
		
		public ChildrenMainViewModel(Account account)
		{
			LogManager.GetLog(GetType()).Info("Create");

			this._account = account;
			base.LoadItems();
		}


		protected override void LoadItems()
		{
			LogManager.GetLog(GetType()).Info("Load items");

			Items = (from c in _account.Children
							 where c.Deleted == false
							 select new ChildUI
							 {
								 Name = string.Format("{0} {1}", c.FirstName, c.LastName),
								 Tag = c
							 }).ToList();

			base.LoadItems();
		}

		public void AddAction()
		{
			LogManager.GetLog(GetType()).Info("Add");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.ShowDialog
				{
					Dialog = new AddChildViewModel(_account)
				});
		}

		public void EditAction()
		{
			LogManager.GetLog(GetType()).Info("Edit");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.ShowDialog
				{
					Dialog = new EditChildViewModel(_account, SelectedItem.Tag)
				});
		}

		public void OpenAction(ChildUI child)
		{
			LogManager.GetLog(GetType()).Info("Open");
			SelectItem(child);
			EditAction();
		}

		public void DeleteAction()
		{
			LogManager.GetLog(GetType()).Info("Delete");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Events.ShowDialog
				{
					Dialog = new YesNoDialogViewModel
					{
						Message = "Ben je zeker?",
						Yes = () => DeleteChild()
					}
				});
		}

		public void DeleteChild()
		{
			LogManager.GetLog(GetType()).Info("Delete");

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				SelectedItem.Tag.Deleted = true;
				SelectedItem.Tag.Updated = DateTime.Now;

				db.Children.Update(SelectedItem.Tag);
				//db.Children.Delete(SelectedItem.Tag.Id);
			}
			SelectItem(null);

			LoadItems();
		}
	}
}
