using ClosedXML.Excel;
using DayCare.Model.Lite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core.Reports
{
	public class MonthInfo
	{
		public DateTime Date { get; set; }

		public List<Child> Coming { get; set; }
		public List<Child> Leaving { get; set; }
	}

	public class ExitReport : BaseReport
	{
		public static void Create()
		{
			using(var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var today = DateTime.Now.Date;
				var firstOfMonth = new DateTime(today.Year, today.Month, 1);

				var children = from c in db.Children.FindAll()
											 where c.Deleted == false
											 orderby c.BirthDay
											 select c;

				var months = new List<MonthInfo>();
				
				foreach (var child in children)
				{
					var leave_date = DateTime.MaxValue;
					var entry_date = DateTime.MinValue;

					if (child.Schedule != null && child.Schedule.Count != 0 )
					{
						leave_date = child.Schedule.OrderBy(d => d.Day).Last().Day;
						entry_date = child.Schedule.OrderBy(d => d.Day).First().Day;
					}

					if (leave_date >= firstOfMonth)
					{
						var month = months.FirstOrDefault(m => m.Date.Year == leave_date.Year && m.Date.Month == leave_date.Month);
						if (month == null)
						{
							month = new MonthInfo
							{
								Date = new DateTime(leave_date.Year, leave_date.Month, 1),
								Coming = new List<Child>(),
								Leaving = new List<Child>()
							};

							months.Add(month);						
						}

						month.Leaving.Add(child);
					}

					if (entry_date >= firstOfMonth)
					{
						var month = months.FirstOrDefault(m => m.Date.Year == entry_date.Year && m.Date.Month == entry_date.Month);
						if (month == null)
						{
							month = new MonthInfo
							{
								Date = new DateTime(entry_date.Year, entry_date.Month, 1),
								Coming = new List<Child>(),
								Leaving = new List<Child>()
							};

							months.Add(month);
						}

						month.Coming.Add(child);
					}
				}

				months.Sort((m1, m2) => m1.Date.CompareTo(m2.Date));

				var Months = new List<string> { "", 
					"Januari", "Februari", "Maart", "April",
					"Mei", "Juni", "Juli", "Augustus", 
					"September", "Oktober", "November", "December" };
				
				var wb = new XLWorkbook();
				var ws = wb.Worksheets.Add("Uitschrijvingen");

				ws.Style.Font.FontName = "Calibri";
				ws.Style.Font.FontSize = 10;

				var range = ws.Range(1, 1, 1, 4).Merge();
				range.Value = "Uitschrijvingen / Inschrijvingen";
				range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
				range.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
				range.Style.Font.FontSize = 16;


				int currentYear = 0;
				int currentMonth = 0;

				int row = 3;
				int monthRow = 0;

				foreach (var month in months)
				{
					if (currentMonth != month.Date.Month || currentYear != month.Date.Year)
					{
						//	New Month
						//
						currentMonth = month.Date.Month;
						currentYear = month.Date.Year;

						range = ws.Range(row, 1, row, 4).Merge();

						if (month.Date.Year == DateTime.MaxValue.Year)
						{
							range.Value = "Zonder schema";							
						}
						else 
						{
							range.SetValue<string>(string.Format("{0} {1}", Months[month.Date.Month], month.Date.Year));
						}

						range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
						range.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

						SetBorder(range.Style);

						row++;
						monthRow = row;
					}

					foreach (var child in month.Leaving)
					{
						var child_cell = ws.Cell(row, "A");
						child_cell.Value = child.GetFullName();
						
						child_cell = ws.Cell(row, "B");
						child_cell.Value = child.PresenceInfo;
						
						row++;
					}

					row = monthRow;

					foreach (var child in month.Coming)
					{
						var child_cell = ws.Cell(row, "C");
						child_cell.Value = child.GetFullName();

						child_cell = ws.Cell(row, "D");
						child_cell.Value = child.PresenceInfo;

						row++;
					}

					var size = Math.Max(month.Coming.Count, month.Leaving.Count);
					for (int index = 0; index < size; index++)
					{
						ws.Cell(monthRow + index, "A").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
						ws.Cell(monthRow + index, "B").Style.Border.RightBorder = XLBorderStyleValues.Thin;
						ws.Cell(monthRow + index, "D").Style.Border.RightBorder = XLBorderStyleValues.Thin;
					}
					row = monthRow + size;

					range = ws.Range(row, 1, row, 4).Merge();
					range.Style.Border.TopBorder = XLBorderStyleValues.Thin;					
				}

				ws.Column(1).Width = 25.0;
				ws.Column(2).Width = 15.0;
				ws.Column(3).Width = 25.0;
				ws.Column(4).Width = 15.0;

				var file = GetTempFilePathWithExtension("xlsx");
				wb.SaveAs(file);

				System.Diagnostics.Process.Start(file);
			}
		}
	}
}
