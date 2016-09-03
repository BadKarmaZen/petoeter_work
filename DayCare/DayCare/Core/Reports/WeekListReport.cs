using ClosedXML.Excel;
using DayCare.Model;
using DayCare.Model.Lite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
				SetBorder(daycell.Style);

				cell = ws.Cell(3, colid.ToString());
				cell.Value = "uur";
				SetBorder(cell.Style);
				colid.Increment();

				cell = ws.Cell(3, colid.ToString());
				cell.Value = "wie";
				SetBorder(cell.Style);
				colid.Increment();

				cell = ws.Cell(3, colid.ToString());
				cell.Value = "paraaf";
				SetBorder(cell.Style);
				colid.Increment();

				dayCol += 3;
				columns++;
			}

			int rowIndex = 4;
			uint childIndex = 1;

			var startDate = weekdays.Min(d => d.Date);
			var endDate = weekdays.Max(d => d.Date);

			using (var db = new PetoeterDb(PetoeterDb.FileName))
			{
				var presenceData = db.Presences.Find(p => p.Date >= startDate.Date && p.Date <= endDate.Date).ToList();

				if (presenceData != null && presenceData.Count != 0)
				{
					var presences = from p in presenceData
													orderby p.Child.BirthDay
													group p by p.Child.Id into presencegroup
													//orderby presencegroup.Key.BirthDay
													select presencegroup;
					var count = presences.Count();
					var rowSize = count > 30 ? 24.75 : 27.0;

					foreach (var presence in presences)
					{
						var nr = ws.Range(rowIndex, 1, rowIndex + 1, 1).Merge();
						nr.Value = string.Format("{0}.", childIndex);
						nr.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
						nr.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
						SetBorder(nr.Style);


						var cname = ws.Range(rowIndex, 2, rowIndex + 1, 2).Merge();
						cname.Value = presence.First().Child.GetFullName();
						//Ch.GetFullName();		//	string.Format("{0} {1}", child.LastName, child.FirstName);
						cname.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
						cname.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
						cname.Style.Alignment.WrapText = true;
						SetBorder(cname.Style);

						cell = ws.Cell(rowIndex, "C");
						cell.Value = "Aankomst";
						SetBorder(cell.Style);
						ws.Row(rowIndex).Height = rowSize;

						cell = ws.Cell(rowIndex + 1, "C");
						cell.Value = "Vertrek";
						SetBorder(cell.Style);
						ws.Row(rowIndex + 1).Height = rowSize;

						foreach (var day in from p in presence orderby p.Date select p)
						{
							int dateindex = (day.Date - startDate).Days;

							if (day.BroughtBy != null)
							{
								colid = new ColumnId('D');
								colid.Increment(dateindex * 3);

								cell = ws.Cell(rowIndex, colid.ToString());
								cell.SetValue<string>(day.BroughtAt.ToString("HH:MM"));

								colid.Increment(1);
								cell = ws.Cell(rowIndex, colid.ToString());
								cell.Value = day.BroughtBy.GetFullName();
							}

							if (day.TakenBy != null)
							{
								colid = new ColumnId('D');
								colid.Increment(dateindex * 3);

								cell = ws.Cell(rowIndex + 1, colid.ToString()); 
								cell.SetValue<string>(day.TakenAt.ToString("HH:MM"));

								colid.Increment(1);
								cell = ws.Cell(rowIndex + 1, colid.ToString());
								cell.Value = day.TakenBy.GetFullName();
							}
						}

						childIndex++;
						rowIndex += 2;
					}
				}
				else
				{
					//var query = from c in db.Children.FindAll()
					//						orderby c.BirthDay descending
					//						select c;

					var children = from c in db.Children.FindAll()
												 where c.Schedule.Any(d => weekdays.Any(wd => wd.Date == d.Day))
												 orderby c.BirthDay ascending
												 select c;

					var rowSize = children.Count() > 30 ? 24.75 : 27.0;

					foreach (var child in children)
					{
						var nr = ws.Range(rowIndex, 1, rowIndex + 1, 1).Merge();
						nr.Value = string.Format("{0}.", childIndex);
						nr.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
						nr.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
						SetBorder(nr.Style);


						var cname = ws.Range(rowIndex, 2, rowIndex + 1, 2).Merge();
						cname.Value = string.Format("{0} {1}", child.LastName, child.FirstName);
						cname.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
						cname.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
						cname.Style.Alignment.WrapText = true;
						SetBorder(cname.Style);

						cell = ws.Cell(rowIndex, "C");
						cell.Value = "Aankomst";
						SetBorder(cell.Style);
						ws.Row(rowIndex).Height = rowSize;

						cell = ws.Cell(rowIndex + 1, "C");
						cell.Value = "Vertrek";
						SetBorder(cell.Style);
						ws.Row(rowIndex + 1).Height = rowSize;

						colid = new ColumnId('D');
						for (int index = 0; index < 15; index++)
						{
							cell = ws.Cell(rowIndex, colid.ToString());
							SetBorder(cell.Style);
							cell = ws.Cell(rowIndex + 1, colid.ToString());
							SetBorder(cell.Style);

							colid.Increment();
						}

						childIndex++;
						rowIndex += 2;
					}
				}
			}

			//var model = ServiceProvider.Instance.GetService<Petoeter>();
			//var thisWeek = DatePeriod.MakePeriod(month, year);
			
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

		private static void SetBorder(IXLStyle style, XLBorderStyleValues borderstyle = XLBorderStyleValues.Thin)
		{
			style.Border.LeftBorder = borderstyle;
			style.Border.RightBorder = borderstyle;
			style.Border.TopBorder = borderstyle;
			style.Border.BottomBorder = borderstyle;
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
						Date = current.Date,
						Valid = current < endLimit && current >= startLimit
					};
				}
				current = current.AddDays(1);
			};
		}
	}
}
