using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Lite;
using DayCare.Model.UI;
using DayCare.ViewModels.UICore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DayCare.ViewModels.Expenses
{
	public class ExpenseUI :  TaggedItemUI<DayCare.Model.Lite.Presence>
	{
		public BitmapImage ImageData
		{
			get
			{
				return PetoeterImageManager.GetImage(Tag.Child.FileId);
			}
		}

		public bool ShowImage
		{
			get
			{
				return !string.IsNullOrEmpty(Tag.Child.FileId);
			}
		}
	}

	class ExpenseMainViewModel : ListItemScreen<ExpenseUI>
	{
		#region members

		private DateTime _editDate;

		#endregion

		#region Properties
		public DateTime EditDate
		{
			get { return _editDate; }
			set 
			{
				_editDate = value; 
				NotifyOfPropertyChange(()=>EditDate);
				LoadItems();
			}
		}

		#endregion


		public ExpenseMainViewModel()
		{
			EditDate = DateTime.Now.Date;
		}

		protected override void LoadItems()
		{
			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var query = from p in db.Presences.Find(p => p.Date == EditDate)
										select new ExpenseUI 
										{
											Name = p.Child.GetFullName(),
											Tag = p
										};
				Items = query.ToList();
			}
			base.LoadItems();
		}

		public void EditExpenseAction(ExpenseUI expense)
		{
			if (expense == null)
			{
				expense = SelectedItem;
			}

			if (expense != null)
			{
				ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
					new Core.Events.ShowDialog
					{
						Dialog = new ExpenseDetailViewModel(expense)
					});
			}
		}

		public void DecrementDateAction()
		{
			EditDate = EditDate.AddDays(-1);
		}

		public void IncrementDateAction()
		{
			EditDate = EditDate.AddDays(1);
		}

		public void CloseThisScreen()
		{
		}
	}
}
