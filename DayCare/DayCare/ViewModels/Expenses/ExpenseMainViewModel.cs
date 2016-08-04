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
	public class ExpenseUI :  TaggedItemUI<Expense>
	{
		public BitmapImage ImageData
		{
			get
			{
				return PetoeterImageManager.GetImage(FileId);
			}
		}

		public bool ShowImage
		{
			get
			{
				return !string.IsNullOrEmpty(FileId);
			}
		}

		public string FileId { get; set; }
	}

	class ExpenseMainViewModel : ListItemScreen<ExpenseUI>
	{
		public ExpenseMainViewModel()
		{
		}

		protected override void LoadItems()
		{
			var today = DateTimeProvider.Now().Date;

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var query = from p in db.Presences.Find(p => p.Date == today)
										select new ExpenseUI 
										{
											Name = p.Child.GetFullName(),
											FileId = p.Child.FileId,
											Tag = p.Expense
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

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Core.Events.ShowDialog
				{
					Dialog = new ExpenseDetailViewModel(expense)
				});
		}

		public void CloseThisScreen()
		{
		}
	}
}
