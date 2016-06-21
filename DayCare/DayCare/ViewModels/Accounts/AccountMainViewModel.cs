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
			using (var db = new PetoeterDb(@"E:\petoeter_lite.ldb"))
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
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
					new Core.Events.ShowDialog
					{
						Dialog = new AddAccountViewModel()
					});
		}

		public void EditAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
					new Core.Events.SwitchTask
					{
						Task = new EditAccountViewModel(SelectedItem.Tag)
					});
		}

		public void OpenAction(AccountUI account)
		{
			SelectItem(account);
			EditAction();
		}

		public void DeleteAction()
		{
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
			//	No real delete
			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				SelectedItem.Tag.Deleted = true;
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
