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
			this._account = account;
		}

		protected override void LoadItems()
		{
			/*var model = ServiceProvider.Instance.GetService<Petoeter>();

			Items = (from m in model.GetMember(m => m.Account_Id == _account.Id && m.Deleted == false)
							 select new MemberUI
							 {
								 Name = string.Format("{0} {1}", m.FirstName, m.LastName),
								 Tag = m
							 }).ToList();*/

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
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.ShowDialog
				{
					Dialog = new AddMemberViewModel(_account)
				});
		}

		public void EditAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.ShowDialog
				{
				Dialog = new EditMemberViewModel(_account, SelectedItem.Tag)
				});
		}


		public void OpenAction(MemberUI member)
		{
			SelectItem(member);
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
						Yes = () => DeleteItem()
					}
				});
		}

		protected override void DeleteItem()
		{
			using (var db = new PetoeterDb(@"E:\petoeter_lite.ldb"))
			{
				db.Members.Delete(SelectedItem.Tag.Id);
			}

			base.DeleteItem();
		}
	}
}
