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
		//private List<ChildUI> _children;
		//private ChildUI _selectedChild;

		/*public ChildUI SelectedChild
		{
			get { return _selectedChild; }
			set
			{
				_selectedChild = value; NotifyOfPropertyChange(() => SelectedChild);
				NotifyOfPropertyChange(() => IsItemSelected);
			}
		}

		public bool IsItemSelected
		{
			get
			{
				return _selectedChild != null;
			}
		}

		public List<ChildUI> Children
		{
			get { return _children; }
			set { _children = value; NotifyOfPropertyChange(() => Children); }
		}
		*/

		public ChildrenMainViewModel(Account account)
		{
			this._account = account;
			//var model = ServiceProvider.Instance.GetService<Petoeter>();

			////Children = (from c in model.GetChild(c => c.Account_Id == _account.Id)
			////            select new ChildUI
			////            { 
			////                Name = string.Format("{0} {1}", c.FirstName, c.LastName),
			////                Tag = c
			////            }).ToList();
			//LoadData();

			base.LoadItems();
		}


		protected override void LoadItems()
		{
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
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.ShowDialog
				{
					Dialog = new AddChildViewModel(_account)
				});
		}

		public void EditAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.ShowDialog
				{
					Dialog = new EditChildViewModel(_account, SelectedItem.Tag)
				});
		}

		public void OpenAction(ChildUI child)
		{
			SelectItem(child);
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
						 Yes = () => DeleteChild()
					 }
				 });
		}

		public void DeleteChild()
		{
			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				SelectedItem.Tag.Deleted = true;
				db.Children.Update(SelectedItem.Tag);
				//db.Children.Delete(SelectedItem.Tag.Id);
			}
			SelectItem(null);

			LoadItems();
		}
	}
}
