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

namespace DayCare.ViewModels.Invoice
{
	public class InvoiceUI : BaseItemUI
	{
		public Child Child { get; set; }
		public List<Presence> Presences { get; set; }

		public BitmapImage ImageData
		{
			get
			{
				return PetoeterImageManager.GetImage(Child.FileId);
			}
		}

		public bool ShowImage
		{
			get
			{
				return !string.IsNullOrEmpty(Child.FileId);
			}
		}
	}

	public class DateInfo : Screen
	{
		private bool _active;
		private bool _isSelected;

		public string Name { get; set; }
		public bool Active 
		{
			get { return _active; }
			set { _active = value; NotifyOfPropertyChange(() => Active); }
		}
		public bool IsSelected 
		{
			get { return _isSelected; }
			set { _isSelected = value; NotifyOfPropertyChange(() => IsSelected); }
		}
		public int Data { get; set; }
	}

	class InvoiceMainViewModel : ListItemScreen<InvoiceUI>
	{
		#region Members

		private List<DateInfo> _years;
		private List<DateInfo> _months;

		private DateInfo _selectedYear;
		private DateInfo _selectedMonth;

		private List<DateTime> _dates;
		#endregion

		#region Properties

		public List<DateInfo> Years
		{
			get { return _years; }
			set { _years = value; NotifyOfPropertyChange(() => Years); }
		}

		public List<DateInfo> Months
		{
			get { return _months; }
			set { _months = value; NotifyOfPropertyChange(() => Months); }
		}

		public DateInfo SelectedMonth
		{
			get { return _selectedMonth; }
			set
			{
				if (_selectedMonth != null)
					_selectedMonth.IsSelected = false;

				_selectedMonth = value;
				_selectedMonth.IsSelected = true;

				LoadItems();
			}
		}

		public DateInfo SelectedYear
		{
			get { return _selectedYear; }
			set { _selectedYear = value; LoadMonths(); }
		}

		#endregion


		public InvoiceMainViewModel()
		{
			var months = new List<DateInfo>();
			months.Add(new DateInfo { Name = "Jan", Data = 1 });
			months.Add(new DateInfo { Name = "Feb", Data = 2 });
			months.Add(new DateInfo { Name = "Maa", Data = 3 });
			months.Add(new DateInfo { Name = "Apr", Data = 4 });
			months.Add(new DateInfo { Name = "Mei", Data = 5 });
			months.Add(new DateInfo { Name = "Jun", Data = 6 });
			months.Add(new DateInfo { Name = "Jul", Data = 7 });
			months.Add(new DateInfo { Name = "Aug", Data = 8 });
			months.Add(new DateInfo { Name = "Sep", Data = 9 });
			months.Add(new DateInfo { Name = "Okt", Data = 10 });
			months.Add(new DateInfo { Name = "Nov", Data = 11 });
			months.Add(new DateInfo { Name = "Dec", Data = 12 });

			Months = months;

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				_dates = (from p in db.Presences.FindAll()
								  select p.Date).ToList();

				var years = from y in (from d in _dates select d.Year).Distinct()
										orderby y
										select new DateInfo { Name = y.ToString(), Data = y };
				Years = years.ToList();

				SelectedYear = Years.Find(y => y.Data == DateTime.Now.Year);
				SelectedYear.IsSelected = true;						
			}

			LoadMonths();
		}

		public void LoadMonths()
		{
			Months.ForEach(m =>
			{
				m.Active = false;
				m.IsSelected = false;
			});
			
			foreach(var month in from m in (from d in _dates
																				where d.Year == SelectedYear.Data
																				select d.Month).Distinct()
														 orderby m
														 select m)
			{
				Months[month-1].Active = true;
			}

			if (SelectedYear.Data == DateTime.Now.Year)
			{
				SelectedMonth = Months.Find(m => m.Data == DateTime.Now.Month);
			}
		}

		protected override void LoadItems()
		{
			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var query = from p in db.Presences.FindAll()
										where p.Date.Year == SelectedYear.Data &&
													p.Date.Month == SelectedMonth.Data
										group p by p.Child.Id into invoices
										select new InvoiceUI
										{
											Name = invoices.First().Child.GetFullName(),
											Child = invoices.First().Child,
											Presences = invoices.ToList()
										};

				Items = query.ToList();
			}
			base.LoadItems();
		}

		public void SelectMonth(DateInfo month)
		{
			SelectedMonth = month;
		}

		public void OpenAction(InvoiceUI invoice)
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
					new Core.Events.ShowDialog
					{
						Dialog = new InvoiceDetailViewModel(invoice)
					}); 
		}
	}
}
