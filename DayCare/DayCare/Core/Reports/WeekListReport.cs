using ClosedXML.Excel;
using DayCare.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core.Reports
{
	public class WeekListReport
	{
		static public void Create(DateTime date)
		{
			var Months = new List<string> { "Januari", "Februari", "Maart", "April",
																			"Mei", "Juni", "Juli", "Augustus", 
																			"September", "Oktober", "November", "December" };
			var Days = new List<string> { "Zondag", "Maandag", "Dinsdag", "Woensdag", "Donderdag", "Vrijdag", "Zaterdag" };
			var weekdays = GetDays(date).ToList();

			int month = date.Month;
			int year = date.Year;

			var wb = new XLWorkbook();
			var ws = wb.Worksheets.Add("Aanwezigheid");

			ws.Style.Font.FontName = "Calibri";
			ws.Style.Font.FontSize = 10;


			var color = XLColor.FromArgb(0, 51, 102);
			var cell = ws.Cell(1, "D");

			cell.Value = "Aanwezigheidregister";
			cell.Style.Font.FontSize = 14;
			cell.Style.Font.FontColor = color;
			cell.Style.Font.Bold = true;

			cell = ws.Cell(1, "M");
			cell.Value = Months[month - 1];
			cell.Style.Font.FontSize = 14;
			cell.Style.Font.FontColor = color;
			cell.Style.Font.Bold = true;

			cell = ws.Cell(1, "P");
			cell.Value = string.Format("'{0}", year);
			cell.Style.Font.FontSize = 14;
			cell.Style.Font.FontColor = color;
			cell.Style.Font.Bold = true;

			ColumnId colid = new ColumnId('D');
			uint columns = 0;
			int dayCol = 4;

			foreach (var day in weekdays)
			{
				var daycell = ws.Range(2, dayCol, 2, dayCol + 2).Merge();
				daycell.Value = string.Format("'{0} {1}", Days[(int)(day.Date.DayOfWeek)], day.Date.ToShortDateString());

				daycell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
				daycell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
				Enbox(daycell.Style);

				cell = ws.Cell(3, colid.ToString());
				cell.Value = "uur";
				Enbox(cell.Style);
				colid.Increment();

				cell = ws.Cell(3, colid.ToString());
				cell.Value = "wie";
				Enbox(cell.Style);
				colid.Increment();

				cell = ws.Cell(3, colid.ToString());
				cell.Value = "paraaf";
				Enbox(cell.Style);
				colid.Increment();

				dayCol += 3;
				columns++;
			}

			int rowIndex = 4;
			uint childIndex = 1;
			var model = ServiceProvider.Instance.GetService<Petoeter>();

			//var thisWeek = DatePeriod.MakePeriod(month, year);

			var children = from c in model.GetChildren()
										 where c.Deleted == false && c.HasValidPeriod(date)
										 orderby c.BirthDay
										 select c;

			//var data = from c in model.GetChild(c => c.Deleted == false)
			//var child_ids = (from gs in model.GetGroupSchedules()
			//								 where period.Collision(new DatePeriod { Start = gs.StartDate, End = gs.EndDate })
			//								 select gs.ChildId).Distinct();

			foreach (var child in children)
			{
				var nr = ws.Range(rowIndex, 1, rowIndex + 1, 1).Merge();
				nr.Value = string.Format("{0}.", childIndex);
				nr.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
				nr.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
				Enbox(nr.Style);


				var cname = ws.Range(rowIndex, 2, rowIndex + 1, 2).Merge();
				cname.Value = string.Format("{0} {1}", child.LastName, child.FirstName);
				cname.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
				cname.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
				cname.Style.Alignment.WrapText = true;
				Enbox(cname.Style);

				cell = ws.Cell(rowIndex, "C");
				cell.Value = "Aankomst";
				Enbox(cell.Style);
				ws.Row(rowIndex).Height = 27.0;

				cell = ws.Cell(rowIndex + 1, "C");
				cell.Value = "Vertrek";
				Enbox(cell.Style);
				ws.Row(rowIndex + 1).Height = 27.0;

				colid = new ColumnId('D');
				for (int index = 0; index < 15; index++)
				{
					cell = ws.Cell(rowIndex, colid.ToString());
					Enbox(cell.Style);
					cell = ws.Cell(rowIndex + 1, colid.ToString());
					Enbox(cell.Style);

					colid.Increment();
				}

				childIndex++;
				rowIndex += 2;
			}

			var mergedcell = ws.Range(2, 1, 3, 3).Merge();
			mergedcell.Value = "Naam";
			mergedcell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
			mergedcell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
			mergedcell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
			mergedcell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
			mergedcell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
			mergedcell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

			//	styling
			ws.Column(1).Width = 2.0;
			ws.Column(2).Width = 13.0;
			ws.Column(3).Width = 6.43;

			ws.Column(3).Style.Font.FontSize = 8;

			for (int day = 0; day < 5; day++)
			{
				ws.Column(4 + (day * 3)).Width = 5.43;
				ws.Column((int)(4 + (day * 3) + 1)).Width = 10.14;
				ws.Column((int)(4 + (day * 3) + 2)).Width = 5.43;
			}

			/*for (int index = 3; index <= 3 + columns; index++)
			{
				cell = ws.Cell(2, index);
				cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
				cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
				cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
				cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

				cell = ws.Cell(3, index);
				cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
				cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
				cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
				cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
			}

			var rng = ws.Range(4, 3, (int)(2 + childIndex), (int)(3 + columns));
			rng.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
			rng.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
			rng.Style.Font.FontColor = XLColor.LightGray;
			rng.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
			rng.Style.Border.RightBorder = XLBorderStyleValues.Thin;
			rng.Style.Border.TopBorder = XLBorderStyleValues.Thin;
			rng.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

			int col = 0;
			foreach (var column in rng.Columns().Skip(1))
			{
				if (!weekdays[col++].Valid)
				{
					column.Style.Fill.BackgroundColor = XLColor.LightGray;
				}
			}

			rng.FirstColumn().Style.Font.FontColor = XLColor.Black;

			rng = ws.Range(4, 1, (int)(2 + childIndex), 2);
			rng.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
			rng.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
			rng.Style.Border.RightBorder = XLBorderStyleValues.Thin;
			rng.Style.Border.TopBorder = XLBorderStyleValues.Thin;
			rng.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

			ws.Row(1).Height = 18.75;
			ws.Row(2).Height = 12.75;
			ws.Row(3).Height = 12.75;
			ws.Rows(4, (int)(2 + childIndex)).Height = 25.50;*/

			ws.PageSetup.PageOrientation = XLPageOrientation.Landscape;
			ws.PageSetup.PaperSize = XLPaperSize.A4Paper;
			ws.PageSetup.VerticalDpi = 600;
			ws.PageSetup.HorizontalDpi = 600;

			ws.PageSetup.Margins.Bottom = 0;
			ws.PageSetup.Margins.Top = 0;
			ws.PageSetup.Margins.Left = 0.25;
			ws.PageSetup.Margins.Right = 0.25;

			ws.PageSetup.SetColumnsToRepeatAtLeft(1, 3);
			ws.PageSetup.SetRowsToRepeatAtTop(1, 3);

			var file = GetTempFilePathWithExtension("xlsx");
			wb.SaveAs(file);

			System.Diagnostics.Process.Start(file);

		}

		private static void Enbox(IXLStyle style)
		{
			style.Border.LeftBorder = XLBorderStyleValues.Thin;
			style.Border.RightBorder = XLBorderStyleValues.Thin;
			style.Border.TopBorder = XLBorderStyleValues.Thin;
			style.Border.BottomBorder = XLBorderStyleValues.Thin;
		}

		public static string GetTempFilePathWithExtension(string extension)
		{
			var path = Path.GetTempPath();
			var fileName = string.Format("{0}.{1}", Guid.NewGuid(), extension);
			return Path.Combine(path, fileName);
		}

		public static IEnumerable<DayInfo> GetDays(DateTime date)
		{
			DateTime start = date;
			DateTime startLimit = start;

			//	ensure full weeks
			if (start.DayOfWeek != DayOfWeek.Saturday && start.DayOfWeek != DayOfWeek.Sunday)
			{
				while (start.DayOfWeek != DayOfWeek.Monday)
				{
					start = start.AddDays(-1);
				}
			}

			DateTime end = start.AddDays(5);
			DateTime endLimit = end;

			var current = start;

			while (current < end)
			{
				if (current.DayOfWeek != DayOfWeek.Saturday && current.DayOfWeek != DayOfWeek.Sunday)
				{
					yield return new DayInfo
					{
						Date = current,
						Valid = current < endLimit && current >= startLimit
					};
				}
				current = current.AddDays(1);
			};
		}
	}
}
