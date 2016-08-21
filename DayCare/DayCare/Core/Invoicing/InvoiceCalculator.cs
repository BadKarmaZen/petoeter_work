using DayCare.Model.Lite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core.Invoicing
{
	public class InvoiceCalculator
	{
		private readonly TimeSpan ReserveTime;

		public ExpenseRecord Expenses { get; set; }

		public InvoiceCalculator()
		{
			Expenses = new ExpenseRecord();
			ReserveTime = new TimeSpan(0, 0, 5, 0);	//	5 minutes
		}

		public void AddExpenses(Presence record)
		{
			if (record == null)
			{
				return;
			}
			else if(record.Expense == null)
			{
				record.Expense = new Expense();
			}

			//	total presence time
			//
			TimeSpan period = new TimeSpan(record.TimeCode, 0, 0);
			var closeTime = record.Date.AddHours(18);

			if (record.BroughtBy != null)
			{
				if (record.TakenBy != null)
				{
					period = record.TakenAt - record.BroughtAt;
				}
				else
				{
					//	asume until 18:00
					period = closeTime - record.BroughtAt;
				}
			}
			else 
			{
				//	Not brought
				if (record.Expense.Sick)
				{
					if (record.TimeCode == 9)
					{
						Expenses.FullSickDay++;
					}
					else
					{
						Expenses.HalfSickDay++;
					}				
				}
			}

			if (record.TakenBy != null && record.TakenAt > closeTime)
			{
				var minutesToLate = (record.TakenAt - closeTime).TotalMinutes;
				Expenses.ToLatePickup += (1 + (int)(minutesToLate / 15));				
			}

			if (period.TotalHours > 6)	//	
			{
				Expenses.FullDay++;				
			}
			else if(period.TotalHours > 0)
 			{
				Expenses.HalfDay++;
			}

			var extra = (period - ReserveTime).TotalHours;
			while (extra > 9.0)
			{
				extra -= 1.0;
				Expenses.ExtraHour++;
			}

			if (record.Expense.ExtraMeal)
			{
				Expenses.ExtraMeal++;
			}

			Expenses.Diapers += record.Expense.Diapers;
			Expenses.Medication += record.Expense.Diapers;
		}
	}
}
