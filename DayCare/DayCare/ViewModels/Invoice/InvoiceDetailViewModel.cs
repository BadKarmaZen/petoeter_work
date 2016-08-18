using Caliburn.Micro;
using ClosedXML.Excel;
using DayCare.Core;
using DayCare.Model.Lite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.ViewModels.Invoice
{
	class ExpenseReport
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
		public DateTime Month { get; set; }
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

		public InvoiceDetailViewModel(InvoiceUI invoice, DateTime month)
		{
			Month = month;
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
			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var pricing = (from p in db.Pricings.FindAll()
											 where p.Start.Date <= month.Date && month.Date <= p.End.Date
											 select p).FirstOrDefault();

				if (pricing != null)
				{
					FullDayPrice =  pricing.FullDay;
					HalfDayPrice =  pricing.HalfDay;
					MealPrice =  pricing.ExtraMeal;
					ExtraHourPrice =  pricing.ExtraHour;
					DiapersPrice =  pricing.Diapers;
					MedicationPrice =  pricing.Medication;
					ToLatePrice =  pricing.ToLate;
					SickFullPrice =  pricing.FullDaySick;
					SickHalfPrice = pricing.HalfDaySick;					
				}				
			}

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
 			//
			var invoiceFile = Path.Combine(Folder, string.Format("{0}_{1}.xlsx", Info.Child.FirstName, Info.Child.LastName));

			if (File.Exists(invoiceFile) == false)
			{
				File.Copy("InvoiceTemplate.xlsx", invoiceFile);				
			}

			var Months = new List<string> { "Januari", "Februari", "Maart", "April",
																			"Mei", "Juni", "Juli", "Augustus", 
																			"September", "Oktober", "November", "December" };

			var wb = new XLWorkbook(invoiceFile);
			
			IXLWorksheet wsYear;
			wb.Worksheets.TryGetWorksheet("Jaaroverzicht", out wsYear);

			var cell = wsYear.Cell(12, "G");
			cell.Value = Info.Child.GetFullName();

			wsYear.Cell(19, "C").Value = FullDayPrice;
			wsYear.Cell(19, "E").Value = HalfDayPrice;
			wsYear.Cell(19, "G").Value = MealPrice;
			wsYear.Cell(19, "I").Value = ExtraHourPrice;
			wsYear.Cell(19, "K").Value = DiapersPrice;
			wsYear.Cell(19, "M").Value = MedicationPrice;
			wsYear.Cell(19, "O").Value = ToLatePrice;
			wsYear.Cell(19, "Q").Value = SickFullPrice;
			wsYear.Cell(19, "S").Value = SickHalfPrice;


			//	
			IXLWorksheet wsMonth;
			wb.Worksheets.TryGetWorksheet(Months[Month.Month-1], out wsMonth);

			wsMonth.Cell(25, "G").Value = FullDayCount;
			wsMonth.Cell(27, "G").Value = HalfDayCount;
			wsMonth.Cell(29, "G").Value = MealCount;
			wsMonth.Cell(31, "G").Value = ExtraHourCount;
			wsMonth.Cell(33, "G").Value = DiapersCount;
			wsMonth.Cell(35, "G").Value = MedicationCount;
			wsMonth.Cell(37, "G").Value = ToLateCount;
			wsMonth.Cell(40, "G").Value = SickFullCount;
			wsMonth.Cell(42, "G").Value = SickHalfCount;

			wb.Save();
		}
	}
}
