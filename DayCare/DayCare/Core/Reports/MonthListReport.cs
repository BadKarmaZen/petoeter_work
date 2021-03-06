﻿using ClosedXML.Excel;
using DayCare.Model.Lite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core.Reports
{
	public class MonthListReport
	{
		public static string GetTempFilePathWithExtension(string extension)
		{
			var path = Path.GetTempPath();
			var fileName = string.Format("{0}.{1}", Guid.NewGuid(), extension);
			return Path.Combine(path, fileName);
		}

		public static void Create(int month, int year)
		{
			var Months = new List<string> { "Januari", "Februari", "Maart", "April",
																			"Mei", "Juni", "Juli", "Augustus", 
																			"September", "Oktober", "November", "December" };
			var Days = new List<string> { "Zondag", "Maandag", "Dinsdag", "Woensdag", "Donderdag", "Vrijdag", "Zaterdag" };
			var weekdays = GetDays(month, year).ToList();

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
			foreach (var day in weekdays)
			{
				if (day.Date.DayOfWeek == DayOfWeek.Monday)
				{
					cell = ws.Cell(1, colid.ToString());
					cell.Value = "Aanwezigheidregister";
					cell.Style.Font.FontSize = 14;
					cell.Style.Font.FontColor = color;
					cell.Style.Font.Bold = true;
				}
				else if (day.Date.DayOfWeek == DayOfWeek.Thursday)
				{
					cell = ws.Cell(1, colid.ToString());
					cell.Value = Months[month - 1];
					cell.Style.Font.FontSize = 14;
					cell.Style.Font.FontColor = color;
					cell.Style.Font.Bold = true;
				}
				else if (day.Date.DayOfWeek == DayOfWeek.Friday)
				{
					cell = ws.Cell(1, colid.ToString());
					cell.Value = year.ToString();
					cell.Style.Font.FontSize = 14;
					cell.Style.Font.FontColor = color;
					cell.Style.Font.Bold = true;
				}

				ws.Cell(2, colid.ToString()).Value = Days[(int)(day.Date.DayOfWeek)];
				ws.Cell(3, colid.ToString()).Value = day.Date;

				colid.Increment();
				columns++;
			}

			int rowIndex = 4;
			uint childIndex = 1;
			var model = ServiceProvider.Instance.GetService<DayCare.Model.Petoeter>();

			var period = DatePeriod.MakePeriod(month, year);
			//var holidays = model.GetHolidays().ToList();


			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var fullHolidays = (from d in db.Holidays.FindAll()
													  where d.Morning && d.Afternoon
													  select d).ToList();

				var children = from c in db.Children.FindAll()
											 let month_presence = c.Schedule.Where(d => period.Start <= d.Day).Where(d => d.Day <= period.End).ToList()
											 where c.Deleted == false && month_presence.Count != 0
											 orderby c.BirthDay
											 select new { Child = c, Presence = month_presence };

				foreach (var info in children)
				{
					var child = info.Child;
					info.Presence.RemoveAll(d => d.Afternoon == false && d.Morning == false);
					info.Presence.RemoveAll(d => fullHolidays.Exists(h => h.Day == d.Day));

					if (info.Presence.Count == 0)
					{
						continue;
					}
					
					ws.Cell(rowIndex, "A").Value = string.Format("{0}.", childIndex);

					ws.Cell(rowIndex, "B").Value = string.Format("{0} {1}", child.LastName, child.FirstName);
					ws.Cell(rowIndex, "C").Value = child.BirthDay;

					colid = new ColumnId('D');

					foreach (var day in weekdays)
					{
						var today = db.Holidays.FindOne(h => h.Day == day.Date);

						if (today != null && (today.Morning && today.Afternoon))
						{
							//	FULL holiday
							ws.Cell(rowIndex, colid.ToString()).Style.Fill.BackgroundColor = XLColor.LightGray;
						}
						else
						{
							var text = string.Empty;
							var presence_day = info.Presence.FirstOrDefault(d => d.Day == day.Date);

							if (presence_day != null)
							{
								if (presence_day.Morning && presence_day.Afternoon)
								{
									text = "X";
								}
								else if (presence_day.Morning)
								{
									text = "VM";
								}
								else if (presence_day.Afternoon)
								{
									text = "NM";
								}
								else
								{
									ws.Cell(rowIndex, colid.ToString()).Style.Fill.BackgroundColor = XLColor.LightGray;
								}
							}
							else
							{
								ws.Cell(rowIndex, colid.ToString()).Style.Fill.BackgroundColor = XLColor.LightGray;
							}

							ws.Cell(rowIndex, colid.ToString()).Value = text;
						}

						colid.Increment();
					}

					childIndex++;
					rowIndex++;
				}
			}

			//foreach (var child in from c in model.GetChildren()
			//											where c.Active(period)
			//											orderby c.BirthDay
			//											select c)
			//{
			//	ws.Cell(rowIndex, "A").Value = string.Format("{0}.", childIndex);

			//	ws.Cell(rowIndex, "B").Value = string.Format("{0} {1}", child.LastName, child.FirstName);
			//	ws.Cell(rowIndex, "C").Value = child.BirthDay;

			//	colid = new ColumnId('D');

			//	foreach (var day in weekdays)
			//	{
			//		if (holidays.Exists(h => h.Date == day.Date))
			//		{
			//			ws.Cell(rowIndex, colid.ToString()).Style.Fill.BackgroundColor = XLColor.LightGray;
			//		}
			//		else
			//		{
			//			var schedule = child.FindSchedule(day.Date);
			//			if (schedule != null)
			//			{
			//				var detail = schedule.GetActiveSchedule(day.Date);

			//				var info = string.Empty;

			//				if (detail.ThisMorning(day.Date) && detail.ThisAfternoon(day.Date))
			//				{
			//					info = "X";
			//				}
			//				else if (detail.ThisMorning(day.Date))
			//				{
			//					info = "VM";
			//				}
			//				else if (detail.ThisAfternoon(day.Date))
			//				{
			//					info = "NM";
			//				}
			//				else
			//				{
			//					ws.Cell(rowIndex, colid.ToString()).Style.Fill.BackgroundColor = XLColor.LightGray;
			//				}

			//				ws.Cell(rowIndex, colid.ToString()).Value = info;
			//			}
			//			else
			//			{
			//				ws.Cell(rowIndex, colid.ToString()).Style.Fill.BackgroundColor = XLColor.LightGray;
			//			}
			//		}					
					
			//		colid.Increment();
			//	}

			//	childIndex++;
			//	rowIndex++;
			//}

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
			rng.Style.Font.FontColor = XLColor.Gray;
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

			var file = GetTempFilePathWithExtension("xlsx");
			wb.SaveAs(file);

			System.Diagnostics.Process.Start(file);
		}

		public static IEnumerable<DayInfo> GetDays(int month, int year)
		{
			DateTime start = new DateTime(year, month, 1);
			DateTime end = start.AddMonths(1);
			DateTime startLimit = start;
			DateTime endLimit = end;

			//	ensure full weeks
			if (start.DayOfWeek != DayOfWeek.Saturday && start.DayOfWeek != DayOfWeek.Sunday)
			{
				while (start.DayOfWeek != DayOfWeek.Monday)
				{
					start = start.AddDays(-1);
				}
			}

			if (end.DayOfWeek != DayOfWeek.Saturday && end.DayOfWeek != DayOfWeek.Sunday)
			{
				while (end.DayOfWeek != DayOfWeek.Monday)
				{
					end = end.AddDays(1);
				}
			}

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
