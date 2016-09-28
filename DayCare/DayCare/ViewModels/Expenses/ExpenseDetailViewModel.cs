using Caliburn.Micro;
using DayCare.Core;
using DayCare.Model.Lite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DayCare.ViewModels.Expenses
{
	class ExpenseDetailViewModel : Screen
	{
		#region Properties

		public string Name { get; set; }
		public ExpenseUI _info { get; set; }

		public bool ExtraHours
		{
			get
			{
				return _info.Tag.Expense.ExtraHour != 0;
			}
			set
			{
				_info.Tag.Expense.ExtraHour = value ? 1 : 0;
				_info.Tag.Expense.ExtraHourOverride = true;
				NotifyOfPropertyChange(() => ExtraHours);
			}
		}

		public int Diapers
		{
			get { return _info.Tag.Expense.Diapers; }
			set { _info.Tag.Expense.Diapers = value; NotifyOfPropertyChange(() => Diapers); }
		}

		public int Medication
		{
			get { return _info.Tag.Expense.Medication; }
			set { _info.Tag.Expense.Medication = value; NotifyOfPropertyChange(() => Medication); }
		}

		public BitmapImage Image
		{
			get
			{
				return PetoeterImageManager.GetImage(_info.Tag.Child.FileId);
			}
		}

		public bool ExtraMeal
		{
			get { return _info.Tag.Expense.ExtraMeal; }
			set { _info.Tag.Expense.ExtraMeal = value; NotifyOfPropertyChange(()=>ExtraMeal); }
		}

		public bool ToLate
		{
			get { return _info.Tag.Expense.ToLate; }
			set { _info.Tag.Expense.ToLate = value; NotifyOfPropertyChange(()=>ToLate); }
		}

		public bool Sick
		{
			get { return _info.Tag.Expense.Sick; }
			set { _info.Tag.Expense.Sick = value; NotifyOfPropertyChange(() => Sick); }
		}

		public bool SicknessNotNotified
		{
			get { return _info.Tag.Expense.SicknessNotNotified; }
			set { _info.Tag.Expense.SicknessNotNotified = value; NotifyOfPropertyChange(() => SicknessNotNotified); }
		}


		#endregion

		public ExpenseDetailViewModel()
		{
			
		}

		public ExpenseDetailViewModel(ExpenseUI expense)
		{
			if (expense.Tag.Expense == null)
			{
				expense.Tag.Expense = new Expense{};
			}
			_info = expense;

			if (expense.Tag.Expense.ExtraHourOverride == false &&
				  expense.Tag.Expense.ExtraHour == 0)
			{
				//	calculate
				if (expense.Tag.TakenBy != null)
				{
					var timeSpend = expense.Tag.TakenAt - expense.Tag.BroughtAt;

					if (timeSpend > new TimeSpan(0,expense.Tag.TimeCode, 15,0))
					{
						expense.Tag.Expense.ExtraHour = 1;
					}
				}
			}

			Name = expense.Name;
		}


		public void CloseAction()
		{
			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				db.Presences.Update(_info.Tag);
			}

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
			 new Events.ShowDialog());
		}

		//public void DecrementHoursAction()
		//{
		//	if (Hours > 0)
		//	{
		//		Hours--;				
		//	}
		//}

		//public void IncrementHoursAction()
		//{
		//	Hours++;
		//}

		public void DecrementDiapersAction()
		{
			if (Diapers > 0)
			{
				Diapers--;
			}
		}

		public void IncrementDiapersAction()
		{
			Diapers++;
		}

		public void DecrementMedicationAction()
		{
			if (Medication > 0)
			{
				Medication--;
			}
		}

		public void IncrementMedicationAction()
		{
			Medication++;
		}

	}
}
