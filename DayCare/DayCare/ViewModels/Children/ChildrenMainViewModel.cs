using Caliburn.Micro;
using DayCare.Core;
using DayCare.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Children
{
	/*public class ChildrenMainViewModel : ReactivatableScreen
	{
		private Account _account;
		private List<ChildUI> _children;
		private ChildUI _selectedChild;

		public ChildUI SelectedChild
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


		public ChildrenMainViewModel(Model.Database.Account account)
		{
			this._account = account;
			var model = ServiceProvider.Instance.GetService<Petoeter>();

			//Children = (from c in model.GetChild(c => c.Account_Id == _account.Id)
			//            select new ChildUI
			//            { 
			//                Name = string.Format("{0} {1}", c.FirstName, c.LastName),
			//                Tag = c
			//            }).ToList();
			LoadData();
		}

		private void LoadData()
		{
			var model = ServiceProvider.Instance.GetService<Petoeter>();

			Children = (from c in model.GetChild(c => c.Account_Id == _account.Id && c.Deleted == false)
									select new ChildUI
									{
										Name = string.Format("{0} {1}", c.FirstName, c.LastName),
										Tag = c
									}).ToList();
		}

		//public override void Reactivate()
		//{
		//    Children = (from c in _account.Children
		//                select new ChildUI
		//                {
		//                    Name = string.Format("{0} {1}", c.FirstName, c.LastName),
		//                    Tag = c
		//                }).ToList();
		//}


		public void SelectChild(ChildUI child)
		{
			if (SelectedChild != null)
			{
				SelectedChild.Selected = false;
			}

			SelectedChild = child;

			if (SelectedChild != null)
			{
				SelectedChild.Selected = true;
			}
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
						Dialog = new EditChildViewModel(_account, SelectedChild.Tag)
					});
		}

		public void OpenAction(ChildUI child)
		{
			SelectChild(child);
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
			var model = ServiceProvider.Instance.GetService<Petoeter>();

			model.DeleteRecord(SelectedChild.Tag);
			SelectChild(null);

			LoadData();
		}



	}*/
}
