using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Lite;
using DayCare.Model.UI;
using DayCare.ViewModels.Dialogs;
using DayCare.ViewModels.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Members
{

	public class MembersMainViewModel : ListItemScreen<MemberUI>
	{
		private Account _account;

		public MembersMainViewModel(Account account)
		{
			LogManager.GetLog(GetType()).Info("Create");
		
			this._account = account;
		}

		protected override void LoadItems()
		{
			LogManager.GetLog(GetType()).Info("Load items");

			Items = (from m in _account.Members
							 where m.Deleted == false
							 select new MemberUI 
							 {
								 Name = string.Format("{0} {1}", m.FirstName, m.LastName),
 								 Tag = m
							 }).ToList();

			base.LoadItems();
		}

		public void AddAction()
		{
			LogManager.GetLog(GetType()).Info("Add");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.ShowDialog
				{
					Dialog = new AddMemberViewModel(_account)
				});
		}

		public void EditAction()
		{
			LogManager.GetLog(GetType()).Info("Edit");

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.ShowDialog
				{
				Dialog = new EditMemberViewModel(_account, SelectedItem.Tag)
				});
		}


		public void OpenAction(MemberUI member)
		{
			LogManager.GetLog(GetType()).Info("Open");

			SelectItem(member);
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
						Yes = () => DeleteItem()
					}
				});
		}

		protected override void DeleteItem()
		{
			LogManager.GetLog(GetType()).Info("Delete Item");

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				db.Members.Delete(SelectedItem.Tag.Id);
			}

			base.DeleteItem();
		}
	}
}
