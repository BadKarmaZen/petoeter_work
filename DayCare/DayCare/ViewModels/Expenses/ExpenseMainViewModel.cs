using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DayCare.ViewModels.Expenses
{
	/*public class ExpenseUI :  TaggedItemUI<Child>
	{
		public BitmapImage ImageData
		{
			get
			{
				var img = ServiceProvider.Instance.GetService<ImageManager>();
				return img.CreateBitmap(img.FindImage(Tag.Id.ToString()));
			}
		}
	}

	class ExpenseMainViewModel : Screen, ICloseScreen
	{
		private List<ExpenseUI> _expenses;

		public List<ExpenseUI> Expenses
		{
			get { return _expenses; }
			set { _expenses = value; NotifyOfPropertyChange(() => Expenses); }
		}

		public ExpenseMainViewModel()
		{
			var model = ServiceProvider.Instance.GetService<Petoeter>();

			Expenses = (from c in model.GetChild(c => c.Deleted == false)
									select new ExpenseUI() { Tag = c }).ToList();

		}

		public void SelectChildAction(ExpenseUI child)
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
								new Core.Events.ShowDialog
								{
									Dialog = new ExpenseDetailViewModel(child.Tag)
								});
		}

		public void CloseThisScreen()
		{
		}
	}*/
}
