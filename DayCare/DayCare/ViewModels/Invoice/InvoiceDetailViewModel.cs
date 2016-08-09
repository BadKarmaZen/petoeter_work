using Caliburn.Micro;
using DayCare.Model.Lite;
using System;
using System.Collections.Generic;
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
		public InvoiceUI Info { get; set; }
		public ExpenseReport TotalExpense { get; set; }


		public InvoiceDetailViewModel(InvoiceUI invoice)
		{
			Info = invoice;

			Calculate();
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
	}
}
