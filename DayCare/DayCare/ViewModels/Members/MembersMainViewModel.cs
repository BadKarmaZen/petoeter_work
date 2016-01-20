using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Database;
using DayCare.Model.UI;
using DayCare.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Members
{
	public class ListItemScreen<T> : Screen
			where T : BaseItemUI
	{
		protected List<T> _items;
		protected T _selectedItem;

		public T SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				_selectedItem = value; NotifyOfPropertyChange(() => SelectedItem);
				NotifyOfPropertyChange(() => IsItemSelected);
			}
		}

		public bool IsItemSelected
		{
			get
			{
				return _selectedItem != null;
			}
		}

		public List<T> Items
		{
			get { return _items; }
			set { _items = value; ItemsUpdated(); }
		}

		public virtual void ItemsUpdated()
		{
			NotifyOfPropertyChange(() => Items);
		}


		public ListItemScreen()
		{

		}

		protected override void OnViewLoaded(object view)
		{
			LoadItems();

			base.OnViewLoaded(view);
		}

		public void SelectItem(T item = null)
		{
			if (SelectedItem != null)
			{
				SelectedItem.Selected = false;
			}

			SelectedItem = item;

			if (SelectedItem != null)
			{
				SelectedItem.Selected = true;
			}
		}

		protected virtual void LoadItems()
		{ }

		protected virtual void DeleteItem()
		{
			SelectItem();
			LoadItems();
		}
	}

	public class FilteredListItemScreen<T> : ListItemScreen<T>
		where T : BaseItemUI
	{
		private string _filter;

		public string Filter
		{
			get { return _filter; }
			set { _filter = value; NotifyOfPropertyChange(() => Filter); NotifyOfPropertyChange(() => FilteredItems); }
		}

		public List<T> FilteredItems
		{
			get 
			{
				if (_items == null)
				{
					return _items;					
				}

				return (from i in _items
								where i.IsValidFilter(_filter)
								orderby i.OrderBy()
								select i).ToList(); 
			}
		}

		public override void ItemsUpdated()
		{
			base.ItemsUpdated();

			NotifyOfPropertyChange(() => FilteredItems);
		}
	}

	public class MemberUI : TaggedItemUI<Member>
	{

	}

	public class MembersMainViewModel : ListItemScreen<MemberUI>
	{
		private Account _account;

		public MembersMainViewModel(Account account)
		{
			this._account = account;
		}

		protected override void LoadItems()
		{
			var model = ServiceProvider.Instance.GetService<Petoeter>();

			Items = (from m in model.GetMember(m => m.Account_Id == _account.Id && m.Deleted == false)
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
			var model = ServiceProvider.Instance.GetService<Petoeter>();

			model.DeleteRecord(SelectedItem.Tag);

			base.DeleteItem();
		}
	}
}
