using Caliburn.Micro;
using ClosedXML.Excel;
using DayCare.Core;
using DayCare.Core.Invoicing;
using DayCare.Model.Lite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Invoice
{
	//public class ExpenseReport
	//{
	//	public int FullDay { get; set; }
	//	public int HalfDay { get; set; }
	//	public int FullSickDay { get; set; }
	//	public int HalfSickDay { get; set; }
	//	public int ExtraMeal { get; set; }
	//	public int ExtraHour { get; set; }
	//	public int Diapers { get; set; }
	//	public int Medication { get; set; }
	//	public int ToLatePickup { get; set; }		
	//}

	class InvoiceDetailViewModel : Screen
	{
		#region Properties

		public InvoiceUI Info { get; set; }
		public DateTime InvoicePeriod { get; set; }
		//public ExpenseReport TotalExpense { get; set; }
		public ExpenseRecord ExpensesCount { get; set; }

		public double FullDayPrice { get; set; }
		public double HalfDayPrice { get; set; }
		public double MealPrice { get; set; }
		public double ExtraHourPrice { get; set; }
		public double DiapersPrice { get; set; }
		public double MedicationPrice { get; set; }
		public double ToLatePrice { get; set; }
		public double SickFullPrice { get; set; }
		public double SickHalfPrice { get; set; }

		public double FullDayCount { get; set; }
		public double HalfDayCount { get; set; }
		public double MealCount { get; set; }
		public double ExtraHourCount { get; set; }
		public double DiapersCount { get; set; }
		public double MedicationCount { get; set; }
		public double ToLateCount { get; set; }
		public double SickFullCount { get; set; }
		public double SickHalfCount { get; set; }

		public double FullDayTotal { get; set; }
		public double HalfDayTotal { get; set; }
		public double MealTotal { get; set; }
		public double ExtraHourTotal { get; set; }
		public double DiapersTotal { get; set; }
		public double MedicationTotal { get; set; }
		public double ToLateTotal { get; set; }
		public double SickFullTotal { get; set; }
		public double SickHalfTotal { get; set; }

		public double FullResultTotal { get; set; }

		#endregion

		public InvoiceDetailViewModel(ExpenseRecord expenses, DateTime period)
		{
			InvoicePeriod = period;
			ExpensesCount = expenses;

			//	count
			FullDayCount = ExpensesCount.FullDay;
			HalfDayCount = ExpensesCount.HalfDay;
			MealCount = ExpensesCount.ExtraMeal;
			ExtraHourCount = ExpensesCount.ExtraHour;
			DiapersCount = ExpensesCount.Diapers;
			MedicationCount = ExpensesCount.Medication;
			ToLateCount = ExpensesCount.ToLatePickup;
			SickFullCount = ExpensesCount.FullSickDay;
			SickHalfCount = ExpensesCount.HalfSickDay;

			// pricing
			var mgr = new InvoiceManager();
			var prices = mgr.GetCurrentPrices();

			FullDayPrice = prices.FullDay;
			HalfDayPrice = prices.HalfDay;
			MealPrice = prices.ExtraMeal;
			ExtraHourPrice = prices.ExtraHour;
			DiapersPrice = prices.Diapers;
			MedicationPrice = prices.Medication;
			ToLatePrice = prices.ToLate;
			SickFullPrice = prices.FullDaySick;
			SickHalfPrice = prices.HalfDaySick;


			//	Total
			FullDayTotal = FullDayCount * FullDayPrice;
			HalfDayTotal = HalfDayCount * HalfDayPrice;
			MealTotal = MealCount * MealPrice;
			ExtraHourTotal = ExtraHourCount * ExtraHourPrice;
			DiapersTotal = DiapersCount * DiapersPrice;
			MedicationTotal = MedicationCount * MedicationPrice;
			ToLateTotal = ToLateCount * ToLatePrice;
			SickFullTotal = SickFullCount * SickFullPrice;
			SickHalfTotal = SickHalfCount * SickHalfPrice;

			FullResultTotal = FullDayTotal + HalfDayTotal + MealTotal +
												ExtraHourTotal + DiapersTotal + MedicationTotal +
												ToLateTotal + SickFullTotal + SickHalfTotal;
		}

		private ExpenseRecord Calculate()
		{
			var calculator = new InvoiceCalculator();

			foreach (var presence in Info.Presences)
			{
				calculator.AddExpenses(presence);
			}

			return calculator.Expenses;
		}

		public void OKAction()
		{
			ServiceProvider.Instance.GetService<EventAggregator>().PublishOnUIThread(
				new Events.ShowDialog());
		}

		public string Folder = @"E:\Temp\Invoice";

		public void DocumentAction()
		{
			var mgr = new InvoiceManager();
			var invoice = mgr.FindInvoice(Info.Child, InvoicePeriod.Year);

			if (string.IsNullOrEmpty(invoice.File) == false)
			{
				invoice.SetMonth(InvoicePeriod.Month, ExpensesCount);
			}
		}
	}
}
