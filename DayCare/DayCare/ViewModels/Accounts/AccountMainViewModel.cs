using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model;
using DayCare.ViewModels.UICore;
using DayCare.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DayCare.Model.Lite;

namespace DayCare.ViewModels.Accounts
{
	public class AccountMainViewModel : FilteredListItemScreen<AccountUI>
	{
		private bool _showDeleted;

		public bool ShowDeleted
		{
			get { return _showDeleted; }
			set 
			{
				_showDeleted = value;
				NotifyOfPropertyChange(() => ShowDeleted);
				NotifyOfPropertyChange(() => DeletedLabel);
				NotifyOfPropertyChange(() => ShowReactivate);
			}
		}

		public string DeletedLabel
		{
			get { return _showDeleted ? "Verberg gewiste" : "Toon gewiste"; }
		}

		private bool ShowReactivate
		{
			get { return _showDeleted && this.IsItemSelected; }
		}

		protected override void LoadItems()
		{
			LogManager.GetLog(GetType()).Info("Load items");

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var query = from a in db.Accounts.FindAll()
										where a.Deleted == _showDeleted
										select new AccountUI
										{
											Name = a.Name,
											Tag = a
										};

				Items = query.ToList();
			}

			base.LoadItems();
		}


		public void AddAction()
		{
			LogManager.GetLog(GetType()).Info("Add action");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.ShowDialog
				{
					Dialog = new AddAccountViewModel()
				});
		}

		public void EditAction()
		{
			LogManager.GetLog(GetType()).Info("Edit Action");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.SwitchTask
				{
					Task = new EditAccountViewModel(SelectedItem.Tag)
				});
		}

		public void OpenAction(AccountUI account)
		{
			LogManager.GetLog(GetType()).Info("Open Action");

			SelectItem(account);
			EditAction();
		}

		public void DeleteAction()
		{
			LogManager.GetLog(GetType()).Info("Delete Action");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Events.ShowDialog
				{
					Dialog = new YesNoDialogViewModel
					{
						Message = "Ben je zeker?",
						Yes = () => DeleteAccount()
					}
				});
		}

		public void DeleteAccount()
		{
			LogManager.GetLog(GetType()).Info("Delete account");

			//	No real delete
			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var update = DateTime.Now;

				SelectedItem.Tag.Deleted = true;
				SelectedItem.Tag.Updated = update;

				foreach (var child in SelectedItem.Tag.Children)
				{
					child.Deleted = true;
					child.Updated = update;
				}

				db.Accounts.Update(SelectedItem.Tag);
			}
			
			SelectItem(null);
			LoadItems();
		}

		public void ToggleDeletedAction()
		{
			ShowDeleted = !ShowDeleted;
			SelectItem(null);
			
			LoadItems();
		}

		public void ReactivateAction()
		{ 
		}
	}
}
