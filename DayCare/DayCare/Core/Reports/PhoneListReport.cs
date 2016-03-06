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
	class PhoneListReport
	{
		internal static void Create()
		{
			try
			{
				var model = ServiceProvider.Instance.GetService<Petoeter>();
				var wb = new XLWorkbook();
				var ws = wb.Worksheets.Add("Telefoon lijst");

				ws.Style.Font.FontName = "Calibri";
				ws.Style.Font.FontSize = 10;

				var title = ws.Range(1, 1, 1, 6).Merge();
				title.Value = "Overzichtlijst kinderen";
				title.Style.Font.Bold = true;
				title.Style.Font.FontSize = 14;
				title.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


				ws.Cell(2, 1).Value = "Naam";
				ws.Cell(2, 2).Value = "Geb. Datum";
				ws.Cell(2, 3).Value = "Ouder";
				ws.Cell(2, 4).Value = "Telefoon";
				ws.Cell(2, 5).Value = "Ouder";
				ws.Cell(2, 6).Value = "Telefoon";

				int row = 3;
				int maxcolumn = 3;
				foreach (var account in from m in model.GetAccounts()
																where m.Deleted == false
																orderby m.Name ascending
																select m)
				{
					foreach (var child in from c in account.Children
																where c.Deleted == false
																orderby c.FirstName
																select c)
					{
						var cell = ws.Cell(row, 1);
						cell.Value = string.Format("{0} {1}", child.LastName, child.FirstName);

						cell = ws.Cell(row, 2);
						cell.Value = child.BirthDay;
						
						int column = 3;
						var members = from m in account.Members
													where m.Deleted == false && !string.IsNullOrWhiteSpace(m.Phone)
													select m;

						foreach (var member in members.Take(2))
						{
							cell = ws.Cell(row, column++);
							cell.Value = string.Format("{0} {1}", member.FirstName, member.LastName);

							cell = ws.Cell(row, column++);
							cell.Value = member.Phone;
							cell.SetDataType(XLCellValues.Text);
						}

						row++;
					}
				}

				//	style
				ws.Column(1).Width = 20.0;
				ws.Column(2).Width = 13.0;
				ws.Column(3).Width = 20.0;
				ws.Column(4).Width = 13.0;
				ws.Column(5).Width = 20.0;
				ws.Column(6).Width = 13.0;

				ws.Column(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

				var header = ws.Range(2, 1, 2, 6).Style;
				header.Font.Bold = true;
				header.Border.BottomBorder = XLBorderStyleValues.Thin;

				ws.PageSetup.PageOrientation = XLPageOrientation.Landscape;
				ws.PageSetup.PaperSize = XLPaperSize.A4Paper;

				var file = GetTempFilePathWithExtension("xlsx");
				wb.SaveAs(file);

				System.Diagnostics.Process.Start(file);
			}
			catch (Exception)
			{
			}
		}

		public static string GetTempFilePathWithExtension(string extension)
		{
			var path = Path.GetTempPath();
			var fileName = string.Format("{0}.{1}", Guid.NewGuid(), extension);
			return Path.Combine(path, fileName);
		}
	}
}
