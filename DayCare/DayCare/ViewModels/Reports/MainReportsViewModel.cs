using Caliburn.Micro;
using ClosedXML.Excel;
using DayCare.Core;
using DayCare.Core.Reports;
using DayCare.Model.Database;
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
			var Months = new List<string> { "Januari", "Februari", "Maart", "April",
																			"Mei", "Juni", "Juli", "Augustus", 
																			"September", "Oktober", "November", "December" };
			var Days = new List<string> { "Zondag", "Maandag", "Dinsdag", "Woensdag", "Donderdag", "Vrijdag", "Zaterdag" };

			var model = ServiceProvider.Instance.GetService<Petoeter>();

			var file = @"E:\temp\data.xlsx";

			var wb = new XLWorkbook();

			var ws = wb.Worksheets.Add("Aanwezigheid");

			ws.Style.Font.FontName = "Calibri";
			ws.Style.Font.FontSize = 10;
			
			var cell = ws.Cell(2, "C");

			cell.Value = "Geboorte-";
			cell = ws.Cell(3, "C");
			cell.Value = "datum";
			

			ColumnId colid = new ColumnId('D');
			uint columns = 0;
			var color = XLColor.FromArgb(0, 51, 102);
			foreach (var day in MonthList.GetWeekDays())
			{
				if (day.DayOfWeek == DayOfWeek.Monday)
				{
					cell = ws.Cell(1, colid.ToString());
					cell.Value = "Aanwezigheidregister";
					cell.Style.Font.FontSize = 14;
					cell.Style.Font.FontColor = color;
					cell.Style.Font.Bold = true;
				}
				else if (day.DayOfWeek == DayOfWeek.Thursday)
				{
					cell = ws.Cell(1, colid.ToString());
					cell.Value = Months[day.Month - 1];
					cell.Style.Font.FontSize = 14;
					cell.Style.Font.FontColor = color;
					cell.Style.Font.Bold = true;
				}
				else if (day.DayOfWeek == DayOfWeek.Friday)
				{
					cell = ws.Cell(1, colid.ToString());
					cell.Value = day.Year.ToString();
					cell.Style.Font.FontSize = 14;
					cell.Style.Font.FontColor = color;
					cell.Style.Font.Bold = true;
				}

				ws.Cell(2, colid.ToString()).Value = Days[(int)(day.DayOfWeek)];
				ws.Cell(3, colid.ToString()).Value = day;

				colid.Increment();
				columns++;
			}
			
			int rowIndex = 4;
			uint childIndex = 1;

			foreach (var child in model.GetChild(c => c.Deleted == false))
			{
				ws.Cell(rowIndex, "A").Value = string.Format("{0}.", childIndex);
		
				ws.Cell(rowIndex, "B").Value = string.Format("{0} {1}", child.LastName, child.FirstName);
				ws.Cell(rowIndex, "C").Value = child.BirthDay;

				colid = new ColumnId('D');

				foreach (var day in MonthList.GetWeekDays())
				{
					var schedule = model.GetCurrentSchedule(child, day);
					var info = string.Empty;

					if (schedule.ThisMorning(day) && schedule.ThisAfternoon(day))
					{
						info = "X";
					}
					else if (schedule.ThisMorning(day))
					{
						info = "VM";
					}
					else if (schedule.ThisAfternoon(day))
					{
						info = "NM";
					}

					ws.Cell(rowIndex, colid.ToString()).Value = info;
					colid.Increment();
				}

				childIndex++;
				rowIndex++;
			}

			var mergedcell = ws.Range(2, 1, 3, 2).Merge();
			mergedcell.Value = "Naam";
			mergedcell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
			mergedcell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
			mergedcell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
			mergedcell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
			mergedcell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
			mergedcell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;

			//	styling
			ws.Column(1).Width = 3.14;
			ws.Column(2).Width = 18.15;
			ws.Column(3).Width = 10;
			ws.Columns(4, (int)(4 + columns + 1)).Width = 20.43;

			for (int index = 3; index <= 3 + columns; index++)
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
			ws.Rows(4, (int)(2 + childIndex)).Height = 25.50;

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

			wb.SaveAs(file);
		}
	}
}
