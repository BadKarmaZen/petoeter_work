using ClosedXML.Excel;
using DayCare.ViewModels.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core.Invoicing
{
	public class Invoice
	{
		public string File { get; set; }

		public void SetMonth(int month, ExpenseRecord expense)
		{
			var Months = new List<string> { "Januari", "Februari", "Maart", "April",
																			"Mei", "Juni", "Juli", "Augustus", 
																			"September", "Oktober", "November", "December" };

			var wb = new XLWorkbook(File);
			IXLWorksheet wsMonth;
			wb.Worksheets.TryGetWorksheet(Months[month-1], out wsMonth);

			wsMonth.Cell(25, "G").Value = expense.FullDay;
			wsMonth.Cell(27, "G").Value = expense.HalfDay;
			wsMonth.Cell(29, "G").Value = expense.ExtraMeal;
			wsMonth.Cell(31, "G").Value = expense.ExtraHour;
			wsMonth.Cell(33, "G").Value = expense.Diapers;
			wsMonth.Cell(35, "G").Value = expense.Medication;
			wsMonth.Cell(37, "G").Value = expense.ToLatePickup;
			wsMonth.Cell(40, "G").Value = expense.FullSickDay;
			wsMonth.Cell(42, "G").Value = expense.HalfSickDay;

			wb.Save();
		}
	}
}
