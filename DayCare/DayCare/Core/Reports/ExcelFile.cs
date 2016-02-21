
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core.Reports
{
	public class ExcelFile
	{
		public CustomStylesheet Style { get; set; }
		public string FileName { get; set; }

		public void Create(string fileName)
		{
			FileName = fileName;

			using (SpreadsheetDocument doc = SpreadsheetDocument.Create(FileName, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
			{
				var workbookPart = doc.AddWorkbookPart();
				var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();

				//	Create Styles
				var stylesPart = doc.WorkbookPart.AddNewPart<WorkbookStylesPart>();
				Style = new CustomStylesheet();
				LoadCustomFonts();
				LoadCustomBorders();
				LoadCustomStyles();
				Style.Save(stylesPart);

				string relId = workbookPart.GetIdOfPart(worksheetPart);

				//	create workbook and sheet
				var workbook = new Workbook();
				var worksheet = new Worksheet();
				var fileVersion = new FileVersion { ApplicationName = "Microsoft Office Excel" };

				//	create columns
				var columns = new Columns();
				CreateColumns(columns); 
				worksheet.Append(columns);

				//	create Sheet
				var sheets = new Sheets();
				var sheet = new Sheet { Name = "My sheet", SheetId = 1, Id = relId };
				sheets.Append(sheet);

				workbook.Append(fileVersion);
				workbook.Append(sheets);

				var sheetData = new SheetData();
				LoadData(sheetData);
				worksheet.Append(sheetData);

				worksheetPart.Worksheet = worksheet;
				worksheetPart.Worksheet.Save();

				doc.WorkbookPart.Workbook = workbook;
				doc.WorkbookPart.Workbook.Save();
				doc.Close();
			}
		}

		protected virtual void LoadData(SheetData sheetData)
		{
		}

		protected virtual void CreateColumns(Columns columns)
		{
		}

		protected virtual void LoadCustomStyles()
		{
		}

		protected virtual void LoadCustomBorders()
		{ }

		protected virtual void LoadCustomFonts()
		{ }
	}

	//public class MonthList : ExcelFile
	//{
	//	#region Styles
	//	private uint TextAllBorder;
	//	private uint TextLTRBorder;
	//	private uint TextLBRBorder;
	//	private uint DateAllBorder;
	//	private uint DateLBRBorder;
	//	private uint TextAllBorderLight;
	//	#endregion

	//	//public static IEnumerable<DateTime> GetDays()
	//	//{
	//	//	DateTime start = new DateTime(2015, 11, 1);
	//	//	DateTime end = new DateTime(2015, 11, 30);

	//	//	var current = start;

	//	//	while (current < end)
	//	//	{
	//	//		yield return current;
	//	//		current = current.AddDays(1);
	//	//	};

	//	//	yield return end;
	//	//}

	//	//public static IEnumerable<DateTime> GetWeekDays()
	//	//{
	//	//	return from d in GetDays()
	//	//				 where d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday
	//	//				 select d;
	//	//}

	//	protected override void CreateColumns(Columns columns)
	//	{
	//		var days = GetDays();

	//		var count = 4 + days.Count();

	//		var colSize = new List<double> { 3.14, 11.29, 6.86, 10 };
	//		for (int column = 0; column < 4; column++)
	//		{
	//			columns.Append(new Column() { Width = colSize[column], Min = (uint)column + 1, Max = (uint)(count + 1), CustomWidth = true });
	//		}

	//		for (int column = 4; column < count; column++)
	//		{
	//			columns.Append(new Column() { Width = 20.43, Min = (uint)column + 1, Max = (uint)(count + 1), CustomWidth = true });
	//		}

	//		base.CreateColumns(columns);
	//	}

	//	protected override void LoadData(SheetData sheetData)
	//	{
	//		var model = ServiceProvider.Instance.GetService<Petoeter>();

	//		var row2 = new Row { RowIndex = 2 };
	//		var row3 = new Row { RowIndex = 3 };

	//		ColumnId colid = new ColumnId('D');

	//		row2.Append(new TextCell(colid, "Geboorte-", 2) { StyleIndex = TextLTRBorder });
	//		row3.Append(new TextCell(colid, "datum", 3) { StyleIndex = TextLBRBorder });
	//		colid.Increment();

	//		uint columns = 0;
	//		foreach (var day in GetWeekDays())
	//		{
	//			row2.Append(new TextCell(colid, day.DayOfWeek.ToString(), 2) { StyleIndex = TextLTRBorder });
	//			row3.Append(new DateCell(colid, day, 3) { StyleIndex = DateLBRBorder });

	//			colid.Increment();
	//			columns++;
	//		}
		
	//		sheetData.Append(row2);
	//		sheetData.Append(row3);

	//		uint rowIndex = 4;
	//		uint childIndex = 1;

	//		foreach (var child in model.GetChild(c => c.Deleted == false))
	//		{
	//			var row = new Row { RowIndex = rowIndex };

	//			row.Append(new TextCell("A", string.Format("{0}.", childIndex), rowIndex) { StyleIndex = TextAllBorder });
	//			row.Append(new TextCell("B", string.Format("{0} {1}", child.LastName, child.FirstName), rowIndex) { StyleIndex = TextAllBorder });
	//			row.Append(new DateCell("D", child.BirthDay, rowIndex) { StyleIndex = DateAllBorder });

	//			colid = new ColumnId('E');

	//			foreach (var day in GetWeekDays())
	//			{
	//				var schedule = model.GetCurrentSchedule(child, day);
	//				var info = string.Empty;

	//				if (schedule.ThisMorning(day) && schedule.ThisAfternoon(day))
	//				{
	//					info = "X";
	//				}
	//				else if (schedule.ThisMorning(day))
	//				{
	//					info = "VM";
	//				}
	//				else if (schedule.ThisAfternoon(day))
	//				{
	//					info = "NM";
	//				}

	//				row.Append(new TextCell(colid, info, rowIndex) { StyleIndex = TextAllBorderLight });
	//				colid.Increment();					
	//			}

	//			childIndex++;
	//			rowIndex++;

	//			sheetData.Append(row);
	//		}

	//		base.LoadData(sheetData);
	//	}

	//	protected override void LoadCustomFonts()
	//	{
	//		Style.RegisterFont("Light", new Font
	//		{
	//			FontName = new FontName { Val = StringValue.FromString("Calibri") },
	//			FontSize = new FontSize { Val = DoubleValue.FromDouble(10) },
	//			Color = new Color { Rgb = new HexBinaryValue { Value = "FFA0A0A0" } }
	//		});

	//		base.LoadCustomFonts();
	//	}

	//	protected override void LoadCustomBorders()
	//	{
	//		Style.RegisterBorder("All", BorderStyleValues.Thin, BorderStyleValues.Thin, BorderStyleValues.Thin, BorderStyleValues.Thin);
	//		Style.RegisterBorder("LeftTopRight", BorderStyleValues.Thin, BorderStyleValues.Thin, BorderStyleValues.Thin, BorderStyleValues.None);
	//		Style.RegisterBorder("LeftBottomRight", BorderStyleValues.Thin, BorderStyleValues.None, BorderStyleValues.Thin, BorderStyleValues.Thin);

	//		base.LoadCustomBorders();
	//	}

	//	protected override void LoadCustomStyles()
	//	{
	//		TextAllBorder = Style.BuildCellFormat("All", CustomStylesheet.NumberStyle.ForcedText);
	//		TextLTRBorder = Style.BuildCellFormat("LeftTopRight", CustomStylesheet.NumberStyle.ForcedText);
	//		TextLBRBorder = Style.BuildCellFormat("LeftBottomRight", CustomStylesheet.NumberStyle.ForcedText);
																						
	//		DateAllBorder = Style.BuildCellFormat("All", CustomStylesheet.NumberStyle.Date);
	//		DateLBRBorder = Style.BuildCellFormat("LeftBottomRight", CustomStylesheet.NumberStyle.Date);

	//		TextAllBorderLight = Style.BuildCellFormat("All", CustomStylesheet.NumberStyle.ForcedText, "Light");
	//		base.LoadCustomStyles();
	//	}
	//}

	
	public class ExcelFileTester
	{
		public static void Test()
		{
			var file = @"E:\temp\data.xlsx";

		//	MonthList lst = new MonthList();
			//lst.Create(file);

			/*using (SpreadsheetDocument doc = SpreadsheetDocument.Create(file, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
			{
				var workbookPart = doc.AddWorkbookPart();
				var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
 
				// Create Styles and Insert into Workbook
				var stylesPart = doc.WorkbookPart.AddNewPart<WorkbookStylesPart>();
				Style = new CustomStylesheet();
				Style.BuildCellFormat(CustomStylesheet.BorderStyle.All, CustomStylesheet.NumberStyle.ForcedText);
				Style.Save(stylesPart);

				

				//	create workbook and sheet
				string relId = workbookPart.GetIdOfPart(worksheetPart);

				var workbook = new Workbook();
				var worksheet = new Worksheet();				
				var fileVersion = new FileVersion { ApplicationName = "Microsoft Office Excel" };

				//	
				var columns = CreateColumns();

				worksheet.Append(columns);

				var sheets = new Sheets();
				var sheet = new Sheet { Name = "My sheet", SheetId = 1, Id = relId };
				sheets.Append(sheet);
				
				
				workbook.Append(fileVersion);
				workbook.Append(sheets);

				var sheetData = GenerateData();
				worksheet.Append(sheetData);

        worksheetPart.Worksheet = worksheet;
        worksheetPart.Worksheet.Save();

        doc.WorkbookPart.Workbook = workbook;
        doc.WorkbookPart.Workbook.Save();
        doc.Close();
			}*/
		}



		private static SheetData GenerateData()
		{
			var sheetData = new SheetData();

			//var row = new Row { RowIndex = 2 };
			//ColumnId colid = new ColumnId('e');

			//row.Append(new TextCell(colid, "hello", 2));


			//var styleId = ExcelFile.Style.BuildCellFormat(CustomStylesheet.BorderStyle.All, CustomStylesheet.NumberStyle.ForcedText);

			//colid.Increment();
			//row.Append(new TextCell(colid, "hello", 2) { StyleIndex = styleId });

			//sheetData.Append(row);



		/*	var row2 = new Row() { RowIndex = 2 };
			var row3 = new Row() { RowIndex = 3 };


			ColumnId colid = new ColumnId('e');

			var dateBorder = ExcelFile.Style.BuildCellFormat(CustomStylesheet.BorderStyle.LeftBottomRight, CustomStylesheet.NumberStyle.Date);

			var days = GetWeekDays();
			foreach (var day in days)
			{
				row2.Append(new TextCell(colid.ToString(), day.ToString("dddd"), 2));
				//row3.Append(new DateCell(colid, day, 3) { StyleIndex = dateBorder });

				colid.Increment();
			}

			sheetData.Append(row2);
			//sheetData.Append(row3);
			*/

			return sheetData;
		}

		private static IEnumerable<DateTime> GetDays()
		{
			DateTime start = new DateTime(2015, 11, 1);
			DateTime end = new DateTime(2015,11, 30);

			var current = start;

			while (current < end)
			{
				yield return current;
				current = current.AddDays(1);
			};

			yield return end;
		}

		private static IEnumerable<DateTime> GetWeekDays()
		{
			return from d in GetDays()
						 where d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday
						 select d;
		}
	}
}
