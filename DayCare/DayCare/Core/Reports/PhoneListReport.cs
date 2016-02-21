using ClosedXML.Excel;
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
			//try
			//{
			//	var model = ServiceProvider.Instance.GetService<Petoeter>();
			//	var wb = new XLWorkbook();
			//	var ws = wb.Worksheets.Add("Telefoon lijst");

			//	ws.Style.Font.FontName = "Calibri";
			//	ws.Style.Font.FontSize = 10;

			//	var cell = ws.Cell(1,1);
			//	cell.Value = "Account";

			//	int row = 2;
			//	int maxcolumn = 3;
			//	foreach (var account in from m in model.GetAccount(m => m.Deleted == false)
			//													orderby m.Name ascending
			//													select m)
			//	{
			//		cell = ws.Cell(row, 1);
			//		cell.Value = account.Name;

			//		var members = from m in model.GetMember(m => m.Deleted == false && m.Account_Id == account.Id)
			//								where !string.IsNullOrWhiteSpace(m.Phone)
			//								select m;

			//		int column = 2;

			//		foreach (var member in members)
			//		{
			//			cell = ws.Cell(row, column++);
			//			cell.Value = string.Format("{0} {1}", member.FirstName, member.LastName);

			//			if (column > maxcolumn)
			//			{
			//				maxcolumn = column; 
			//			}
						
			//			cell = ws.Cell(row, column++);
			//			cell.Value = member.Phone;
			//			cell.SetDataType(XLCellValues.Text);		
			//		}

			//		row++;
			//	}

			//	//	style
			//	ws.Column(1).Width = 42.0;
			//	ws.Columns(2, maxcolumn).Width = 13.0;


			//	var file = GetTempFilePathWithExtension("xlsx");
			//	wb.SaveAs(file);

			//	System.Diagnostics.Process.Start(file);
			//}
			//catch (Exception)
			//{
			//}
		}

		public static string GetTempFilePathWithExtension(string extension)
		{
			var path = Path.GetTempPath();
			var fileName = string.Format("{0}.{1}", Guid.NewGuid(), extension);
			return Path.Combine(path, fileName);
		}
	}
}
