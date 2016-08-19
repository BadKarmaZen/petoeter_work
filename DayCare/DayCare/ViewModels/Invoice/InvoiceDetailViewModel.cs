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
	public class ExpenseReport
	{
		public int FullDay { get; set; }
		public int HalfDay { get; set; }
		public int FullSickDay { get; set; }
		public int HalfSickDay { get; set; }
		public int ExtraMeal { get; set; }
		public int ExtraHour { get; set; }
		public int Diapers { get; set; }
		public int Medication { get; set; }
		public int ToLatePickup { get; set; }		
	}

	class InvoiceDetailViewModel : Screen
	{
		#region Properties

		public InvoiceUI Info { get; set; }
		public DateTime InvoicePeriod { get; set; }
		public ExpenseReport TotalExpense { get; set; }

		public double FullDayPrice { get; set; }
		public double HalfDayPrice{ get; set; }
		public double MealPrice{ get; set; }
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
			
#endregion

		public InvoiceDetailViewModel(InvoiceUI invoice, DateTime period)
		{
			InvoicePeriod = period;
			Info = invoice;

			Calculate();

			//	count
			FullDayCount = TotalExpense.FullDay;
			HalfDayCount = TotalExpense.HalfDay;
			MealCount = TotalExpense.ExtraMeal;
			ExtraHourCount = TotalExpense.ExtraHour;
			DiapersCount = TotalExpense.Diapers;
			MedicationCount = TotalExpense.Medication;
			ToLateCount = TotalExpense.ToLatePickup;
			SickFullCount = TotalExpense.FullSickDay;
			SickHalfCount = TotalExpense.HalfSickDay;

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
			ToLateCount = ToLateCount * ToLatePrice;
			SickFullTotal = SickFullCount * SickFullPrice;
			SickHalfTotal = SickHalfCount * SickHalfPrice;
		}

		private void Calculate()
		{
			TotalExpense = new ExpenseReport();

			foreach (var presence in Info.Presences)
			{
				if (presence.Expense == null)
					presence.Expense = new Expense();

				//	check time present
				TimeSpan period = new TimeSpan (presence.TimeCode,0,0);
				var closeTime = presence.Date.AddHours(18);

				if (presence.BroughtBy != null)
				{
					if (presence.TakenBy != null)
					{
						period = presence.TakenAt - presence.BroughtAt;
					}
					else
					{
						//	asume until 18:00
						period = closeTime - presence.BroughtAt;
					}
				}
				else
				{
					if (presence.TakenBy != null)
					{
						//	asume from 07:00
		 				var morning = presence.Date.AddHours(7);
					}
				}

				if (presence.TimeCode == 9 || period.TotalHours > 6)
				{
					if (presence.Expense.Sick)
					{
						TotalExpense.FullSickDay++;
					}
					else
					{
						TotalExpense.FullDay++;
					}
				}
				else
				{
					if (presence.Expense.Sick)
					{
						TotalExpense.HalfSickDay++;
					}
					else
					{
						TotalExpense.HalfDay++;
					}
				}

				if (presence.TakenBy != null && presence.TakenAt > closeTime)
				{
					//	every started 15 minutes
					TotalExpense.ToLatePickup += 1 + (int)((presence.TakenAt - closeTime).TotalMinutes / 15);					
				}

				var extra = period.TotalHours;
				while (extra > 9.0)
				{
					extra -= 1.0;
					TotalExpense.ExtraHour++;					
				}

				if (presence.Expense.ExtraMeal)
				{
					TotalExpense.ExtraMeal++;					
				}

				TotalExpense.Diapers += presence.Expense.Diapers;
				TotalExpense.Medication += presence.Expense.Diapers;
			}
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
				invoice.SetMonth(InvoicePeriod.Month, TotalExpense);
			}
		}
	}
}
