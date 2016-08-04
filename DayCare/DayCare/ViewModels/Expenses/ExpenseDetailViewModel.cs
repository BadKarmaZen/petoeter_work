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
		public string Name { get; set; }
		public ExpenseUI _info { get; set; }

		public int Hours
		{
			get { return _info.Tag.ExtraHour; }
			set { _info.Tag.ExtraHour = value; NotifyOfPropertyChange(() => Hours); }
		}

		public int Diapers
		{
			get { return _info.Tag.Diapers; }
			set { _info.Tag.Diapers = value; NotifyOfPropertyChange(() => Diapers); }
		}

		public int Medication
		{
			get { return _info.Tag.Medication; }
			set { _info.Tag.Medication = value; NotifyOfPropertyChange(() => Medication); }
		}

		public ExpenseDetailViewModel(ExpenseUI expense)
		{
			if (expense.Tag == null)
			{
				expense.Tag = new Expense{};
			}
			_info = expense;

			Name = expense.Name;
		}
		public BitmapImage Image
		{
			get
			{
				return PetoeterImageManager.GetImage(_info.FileId);
			}
		}

		public void CloseAction()
		{
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
