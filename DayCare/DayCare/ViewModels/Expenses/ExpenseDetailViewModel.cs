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

		public int Hours
		{
			get { return _info.Tag.Expense.ExtraHour; }
			set { _info.Tag.Expense.ExtraHour = value; NotifyOfPropertyChange(() => Hours); }
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

		#endregion
		

		public ExpenseDetailViewModel(ExpenseUI expense)
		{
			if (expense.Tag.Expense == null)
			{
				expense.Tag.Expense = new Expense{};
			}
			_info = expense;

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

		public void DecrementHoursAction()
		{
			if (Hours > 0)
			{
				Hours--;				
			}
		}

		public void IncrementHoursAction()
		{
			Hours++;
		}

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
