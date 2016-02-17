using Caliburn.Micro;
using ClosedXML.Excel;
using DayCare.Core;
using DayCare.Core.Reports;
using DayCare.Model.Database;
using DayCare.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Reports
{
	public class MainReportsViewModel : Screen
	{
		public void MonthListAction()
		{
			var dlg = new SelectMonthViewModel();
			dlg.Yes = () => MonthListReport.Create(dlg.Month + 1, dlg.Year);

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Events.ShowDialog
				 {
					 Dialog = dlg
				 });
		}

		public void WeekListAction()
		{
			var dlg = new DateDialogViewModel() { Date = Date.NextMonday()};
			dlg.Yes = () => WeekListReport.Create(dlg.Date);

			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Events.ShowDialog
				 {
					 Dialog = dlg
				 });
		}

		public void ParentPhoneAction()
		{
			PhoneListReport.Create();
		}
	}
}
