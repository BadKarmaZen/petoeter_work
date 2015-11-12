using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core.Reports
{
	public class CustomStylesheet : Stylesheet
	{
		public enum NumberStyle : uint
		{
			Date,
			DateTime,
			Decimal4,
			Decimal2,
			ForcedText
		}

		public class CellFormatId
		{
			public string BorderStyle { get; set; }
			public string FontStyle { get; set; }
			public NumberStyle NumberStyle { get; set; }

			public CellFormatId(string borderStyle, NumberStyle numberStyle, string fontStyle)
			{
				BorderStyle = borderStyle;
				NumberStyle = numberStyle;
				FontStyle = fontStyle;
			}

			public override bool Equals(object obj)
			{
				CellFormatId id = obj as CellFormatId;

				if (id == null)
				{
					return false;
				}

				return this.BorderStyle == id.BorderStyle && this.NumberStyle == id.NumberStyle && this.FontStyle == id.FontStyle;
			}

			public override int GetHashCode()
			{
				return BorderStyle.GetHashCode() ^ NumberStyle.GetHashCode() ^ FontStyle.GetHashCode();
			}
		}

		public Dictionary<string, uint> BorderStyles { get; set; }
		public Borders Borders  { get; set; }

		public Dictionary<string, uint> FontStyles { get; set; }
		public Fonts Fonts { get; set; }

		public Dictionary<NumberStyle, NumberingFormat> NumberStyles { get; set; }
		public Dictionary<CellFormatId, uint> CellFormatStyles { get; set; }
		public CellFormats MyCellFormats { get; set; }

		public CustomStylesheet()
		{
			NumberStyles = new Dictionary<NumberStyle, NumberingFormat>();
			CellFormatStyles = new Dictionary<CellFormatId, uint>();
			
			Borders = new Borders();
			BorderStyles = new Dictionary<string, uint>();

			Fonts = new Fonts();
			FontStyles = new Dictionary<string, uint>();

			RegisterBorder("None");
			RegisterFont("Default", new Font 
			{
 				FontName = new FontName { Val = StringValue.FromString("Calibri") },
				FontSize = new FontSize { Val = DoubleValue.FromDouble(10) }
			});

			var fills = CreateFills();

			
			var numberingFormats = CreateNumberFormats();

			MyCellFormats = new DocumentFormat.OpenXml.Spreadsheet.CellFormats();
			MyCellFormats.Append(new CellFormat
			{
				NumberFormatId = 0,
				FontId = 0,
				FillId = 0,
				BorderId = 0,
				FormatId = 0
			});

			//BuildCellFormat(BorderStyle.All, NumberStyle.ForcedText);




			var cellStyleFormats = new CellStyleFormats();

			var cellFormat = new CellFormat
			{
				Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center },
				NumberFormatId = 0,
				FontId = 0,
				FillId = 0,
				BorderId = 0
			};
			cellStyleFormats.Append(cellFormat);
			cellStyleFormats.Count =
				 UInt32Value.FromUInt32((uint)cellStyleFormats.ChildElements.Count);





			//var cellFormats = CreateCellFormats();


			this.Append(numberingFormats);
			this.Append(Fonts);	//	OK
			this.Append(fills);
			this.Append(Borders);	//	OK
			this.Append(cellStyleFormats);
			this.Append(MyCellFormats);
			var css = new CellStyles();
			var cs = new CellStyle
			{
				Name = StringValue.FromString("Normal"),
				FormatId = 0,
				BuiltinId = 0
			};
			css.Append(cs);
			css.Count = UInt32Value.FromUInt32((uint)css.ChildElements.Count);
			this.Append(css);
			var dfs = new DifferentialFormats { Count = 0 };
			this.Append(dfs);
			var tss = new TableStyles
			{
				Count = 0,
				DefaultTableStyle = StringValue.FromString("TableStyleMedium9"),
				DefaultPivotStyle = StringValue.FromString("PivotStyleLight16")
			};
			this.Append(tss);
		}

		public void RegisterFont(string name, Font font)
		{
			if (!FontStyles.ContainsKey(name))
			{
				FontStyles.Add(name, (uint)Fonts.ChildElements.Count);
				Fonts.Append(font);
			}
		}

		public void RegisterBorder(string name, BorderStyleValues left = BorderStyleValues.None, BorderStyleValues top = BorderStyleValues.None, 
																						BorderStyleValues right = BorderStyleValues.None, BorderStyleValues bottom = BorderStyleValues.None)
		{
			RegisterBorder(name, new Border
			{
				LeftBorder = new LeftBorder { Style = left },
				RightBorder = new RightBorder { Style = right },
				TopBorder = new TopBorder { Style = top },
				BottomBorder = new BottomBorder { Style = bottom },
				DiagonalBorder = new DiagonalBorder() 
			});
		}

		public void RegisterBorder(string name, Border border)
		{
			if (!BorderStyles.ContainsKey(name))
			{
				BorderStyles.Add(name, (uint)Borders.ChildElements.Count);
				Borders.Append(border);
			}
		}

		public uint BuildCellFormat(string borderstyle, NumberStyle numberstyle, string fontstyle = "Default")
		{
			CellFormatId id = new CellFormatId(borderstyle, numberstyle, fontstyle);

			uint borderStyleId = 0;
			BorderStyles.TryGetValue(borderstyle, out borderStyleId);

			uint fontStyleId = 0;
			FontStyles.TryGetValue(fontstyle, out fontStyleId);

			if (CellFormatStyles.ContainsKey(id) == false)
			{
				var cell = new CellFormat
				{
					NumberFormatId = NumberStyles[numberstyle].NumberFormatId,
					FontId = fontStyleId,
					FillId = 0,
					BorderId = borderStyleId,
					FormatId = 0,
					ApplyNumberFormat = BooleanValue.FromBoolean(true)
				};

				CellFormatStyles.Add(id, (uint)MyCellFormats.ChildElements.Count);
				MyCellFormats.Append(cell);
			}

			return CellFormatStyles[id];
		}

		private DocumentFormat.OpenXml.Spreadsheet.CellFormats CreateCellFormats()
		{
			var cellFormats = new CellFormats();

			/*CellFormatStyles.Add(CellFormatStyle.Empty, new CellFormat
			{
				NumberFormatId = 0,
				FontId = 0,
				FillId = 0,
				BorderId = 0,
				FormatId = 0
			});
			cellFormats.Append(CellFormatStyles[CellFormatStyle.Empty]);*/

			// index 1
			// Cell Standard Date format 
			/*				CellFormatStyles.Add(CellFormatStyle.Date, new CellFormat
							{
								Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center },
								NumberFormatId = 14,
								FontId = 0,
								FillId = 0,
								BorderId = 0,
								FormatId = 0,
								ApplyNumberFormat = BooleanValue.FromBoolean(true)
							});
							cellFormats.Append(CellFormatStyles[CellFormatStyle.Date]);

							// Index 2
							// Cell Standard Number format with 2 decimal placing
							CellFormatStyles.Add(CellFormatStyle.NumberDecimal2, new CellFormat
							{
								NumberFormatId = 4,
								FontId = 0,
								FillId = 0,
								BorderId = 0,
								FormatId = 0,
								ApplyNumberFormat = BooleanValue.FromBoolean(true)
							});
							cellFormats.Append(CellFormatStyles[CellFormatStyle.NumberDecimal2]);

							// Index 3
							// Cell Date time custom format
							CellFormatStyles.Add(CellFormatStyle.DateTime, new CellFormat
							{
								NumberFormatId = NumberStyles[NumberStyle.Date].NumberFormatId,
								FontId = 0,
								FillId = 0,
								BorderId = 0,
								FormatId = 0,
								ApplyNumberFormat = BooleanValue.FromBoolean(true)
							});
							cellFormats.Append(CellFormatStyles[CellFormatStyle.DateTime]);

							// Index 4
							// Cell 4 decimal custom format
							CellFormatStyles.Add(CellFormatStyle.NumberDecimal4, new CellFormat
							{
								NumberFormatId = NumberStyles[NumberStyle.Decimal4].NumberFormatId,
								FontId = 0,
								FillId = 0,
								BorderId = 0,
								FormatId = 0,
								ApplyNumberFormat = BooleanValue.FromBoolean(true)
							});
							cellFormats.Append(CellFormatStyles[CellFormatStyle.NumberDecimal4]);

							CellFormatStyles.Add(CellFormatStyle.DateBorder, new CellFormat
							{
								Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center },
								NumberFormatId = NumberStyles[NumberStyle.Date].NumberFormatId,
								FontId = 0,
								FillId = 0,
								BorderId = 3,
								FormatId = 0,
								ApplyNumberFormat = BooleanValue.FromBoolean(true)
							});
							cellFormats.Append(CellFormatStyles[CellFormatStyle.DateBorder]);

							/*
							// Index 5
							// Cell 2 decimal custom format
							cellFormat = new CellFormat
							{
								NumberFormatId = NumberStyles[NumberStyle.Decimal2].NumberFormatId,
								FontId = 0,
								FillId = 0,
								BorderId = 0,
								FormatId = 0,
								ApplyNumberFormat = BooleanValue.FromBoolean(true)
							};
							cellFormats.Append(cellFormat);
							// Index 6
							// Cell forced number text custom format
							cellFormat = new CellFormat
							{
								NumberFormatId = NumberStyles[NumberStyle.ForcedText].NumberFormatId,
								FontId = 0,
								FillId = 0,
								BorderId = 0,
								FormatId = 0,
								ApplyNumberFormat = BooleanValue.FromBoolean(true)
							};
							cellFormats.Append(cellFormat);
							// Index 7
							// Cell text with font 12 
							cellFormat = new CellFormat
							{
								NumberFormatId = NumberStyles[NumberStyle.ForcedText].NumberFormatId,
								FontId = 1,
								FillId = 0,
								BorderId = 0,
								FormatId = 0,
								ApplyNumberFormat = BooleanValue.FromBoolean(true)
							};
							cellFormats.Append(cellFormat);
							// Index 8
							// Cell text
							cellFormat = new CellFormat
							{
								Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center },
								NumberFormatId = NumberStyles[NumberStyle.ForcedText].NumberFormatId,
								FontId = 0,
								FillId = 0,
								BorderId = 1,
								FormatId = 0,
								ApplyNumberFormat = BooleanValue.FromBoolean(true)
							};
							cellFormats.Append(cellFormat);
							// Index 9
							// Coloured 2 decimal cell text
							cellFormat = new CellFormat
							{
								NumberFormatId = NumberStyles[NumberStyle.ForcedText].NumberFormatId,
								FontId = 0,
								FillId = 2,
								BorderId = 2,
								FormatId = 0,
								ApplyNumberFormat = BooleanValue.FromBoolean(true)
							};
							cellFormats.Append(cellFormat);
							// Index 10
							// Coloured cell text
							cellFormat = new CellFormat
							{
								NumberFormatId = NumberStyles[NumberStyle.ForcedText].NumberFormatId,
								FontId = 0,
								FillId = 2,
								BorderId = 2,
								FormatId = 0,
								ApplyNumberFormat = BooleanValue.FromBoolean(true)
							};
							cellFormats.Append(cellFormat);
							// Index 11
							// Coloured cell text
							cellFormat = new CellFormat
							{
								NumberFormatId = NumberStyles[NumberStyle.ForcedText].NumberFormatId,
								FontId = 1,
								FillId = 3,
								BorderId = 2,
								FormatId = 0,
								ApplyNumberFormat = BooleanValue.FromBoolean(true)
							};
							cellFormats.Append(cellFormat);*/


			cellFormats.Count = UInt32Value.FromUInt32((uint)cellFormats.ChildElements.Count);
			return cellFormats;
		}

		private DocumentFormat.OpenXml.Spreadsheet.NumberingFormats CreateNumberFormats()
		{
			uint iExcelIndex = 164;
			var numberingFormats = new NumberingFormats();

			NumberStyles.Add(NumberStyle.Date, new NumberingFormat
			{
				NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++),
				FormatCode = StringValue.FromString("dd/mm/yyyy")
			});

			NumberStyles.Add(NumberStyle.DateTime, new NumberingFormat
			{
				NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++),
				FormatCode = StringValue.FromString("dd/mm/yyyy hh:mm:ss")
			});

			numberingFormats.Append(NumberStyles[NumberStyle.Date]);

			NumberStyles.Add(NumberStyle.Decimal4, new NumberingFormat
			{
				NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++),
				FormatCode = StringValue.FromString("#,##0.0000")
			});

			numberingFormats.Append(NumberStyles[NumberStyle.Decimal4]);

			NumberStyles.Add(NumberStyle.Decimal2, new NumberingFormat
			{
				NumberFormatId = UInt32Value.FromUInt32(iExcelIndex++),
				FormatCode = StringValue.FromString("#,##0.00")
			});
			numberingFormats.Append(NumberStyles[NumberStyle.Decimal2]);

			NumberStyles.Add(NumberStyle.ForcedText, new NumberingFormat
			{
				NumberFormatId = UInt32Value.FromUInt32(iExcelIndex),
				FormatCode = StringValue.FromString("@")
			});
			numberingFormats.Append(NumberStyles[NumberStyle.ForcedText]);
			numberingFormats.Count = UInt32Value.FromUInt32((uint)numberingFormats.ChildElements.Count);
			return numberingFormats;
		}

		/*private static DocumentFormat.OpenXml.Spreadsheet.Borders CreateBorders()
		{
			var borders = new Borders();
			var border = new Border
			{
				LeftBorder = new LeftBorder(),
				RightBorder = new RightBorder(),
				TopBorder = new TopBorder(),
				BottomBorder = new BottomBorder(),
				DiagonalBorder = new DiagonalBorder()
			};
			borders.Append(border);

			//	All Boarder Index 1
			border = new Border
			{
				LeftBorder = new LeftBorder { Style = BorderStyleValues.Thin },
				RightBorder = new RightBorder { Style = BorderStyleValues.Thin },
				TopBorder = new TopBorder { Style = BorderStyleValues.Thin },
				BottomBorder = new BottomBorder { Style = BorderStyleValues.Thin },
				DiagonalBorder = new DiagonalBorder()
			};
			borders.Append(border);

			//	Top and Bottom Boarder Index 2
			border = new Border
			{
				LeftBorder = new LeftBorder(),
				RightBorder = new RightBorder(),
				TopBorder = new TopBorder { Style = BorderStyleValues.Thin },
				BottomBorder = new BottomBorder { Style = BorderStyleValues.Thin },
				DiagonalBorder = new DiagonalBorder()
			};
			borders.Append(border);

			//	Left Right
			border = new Border
			{
				LeftBorder = new LeftBorder { Style = BorderStyleValues.Thin },
				RightBorder = new RightBorder { Style = BorderStyleValues.Thin },
				TopBorder = new TopBorder(),
				BottomBorder = new BottomBorder(),
				DiagonalBorder = new DiagonalBorder()
			};
			borders.Append(border);

			//	Left Top Right
			border = new Border
			{
				LeftBorder = new LeftBorder { Style = BorderStyleValues.Thin },
				RightBorder = new RightBorder { Style = BorderStyleValues.Thin },
				TopBorder = new TopBorder { Style = BorderStyleValues.Thin },
				BottomBorder = new BottomBorder(),
				DiagonalBorder = new DiagonalBorder()
			};
			borders.Append(border);

			//	Left Bottom Right
			border = new Border
			{
				LeftBorder = new LeftBorder { Style = BorderStyleValues.Thin },
				RightBorder = new RightBorder { Style = BorderStyleValues.Thin },
				TopBorder = new TopBorder(),
				BottomBorder = new BottomBorder { Style = BorderStyleValues.Thin },
				DiagonalBorder = new DiagonalBorder()
			};
			borders.Append(border);

			//	Bottom
			border = new Border
			{
				LeftBorder = new LeftBorder(),
				RightBorder = new RightBorder(),
				TopBorder = new TopBorder(),
				BottomBorder = new BottomBorder { Style = BorderStyleValues.Thin },
				DiagonalBorder = new DiagonalBorder()
			};
			borders.Append(border);

			borders.Count = UInt32Value.FromUInt32((uint)borders.ChildElements.Count);
			return borders;
		}*/

		private static DocumentFormat.OpenXml.Spreadsheet.Fills CreateFills()
		{
			var fills = new Fills();
			var fill = new Fill();
			var patternFill = new PatternFill { PatternType = PatternValues.None };
			fill.PatternFill = patternFill;
			fills.Append(fill);
			fill = new Fill();
			patternFill = new PatternFill { PatternType = PatternValues.Gray125 };
			fill.PatternFill = patternFill;
			fills.Append(fill);
			//Fill index  2
			fill = new Fill();
			patternFill = new PatternFill
			{
				PatternType = PatternValues.Solid,
				ForegroundColor = new ForegroundColor()
			};
			patternFill.ForegroundColor =
				 TranslateForeground(System.Windows.Media.Colors.LightBlue);
			patternFill.BackgroundColor =
					new BackgroundColor { Rgb = patternFill.ForegroundColor.Rgb };
			fill.PatternFill = patternFill;
			fills.Append(fill);
			//Fill index  3
			fill = new Fill();
			patternFill = new PatternFill
			{
				PatternType = PatternValues.Solid,
				ForegroundColor = new ForegroundColor()
			};
			patternFill.ForegroundColor =
				 TranslateForeground(System.Windows.Media.Colors.DodgerBlue);
			patternFill.BackgroundColor =
				 new BackgroundColor { Rgb = patternFill.ForegroundColor.Rgb };
			fill.PatternFill = patternFill;
			fills.Append(fill);
			fills.Count = UInt32Value.FromUInt32((uint)fills.ChildElements.Count);
			return fills;
		}

		/*private static DocumentFormat.OpenXml.Spreadsheet.Fonts CreateFonts()
		{
			var fonts = new Fonts();
			var font = new DocumentFormat.OpenXml.Spreadsheet.Font();
			var fontName = new FontName { Val = StringValue.FromString("Calibri") };
			var fontSize = new FontSize { Val = DoubleValue.FromDouble(10) };
			font.FontName = fontName;
			font.FontSize = fontSize;
			fonts.Append(font);

			//Font Index 1
			font = new DocumentFormat.OpenXml.Spreadsheet.Font();
			fontName = new FontName { Val = StringValue.FromString("Calibri") };
			fontSize = new FontSize { Val = DoubleValue.FromDouble(10) };
			font.FontName = fontName;
			font.FontSize = fontSize;
			font.Color = new Color() { Rgb = new HexBinaryValue { Value = "FFA0A0A0" } };
			fonts.Append(font);

			fonts.Count = UInt32Value.FromUInt32((uint)fonts.ChildElements.Count);
			return fonts;
		}*/

		private static ForegroundColor TranslateForeground(System.Windows.Media.Color fillColor)
		{
			return new ForegroundColor()
			{
				Rgb = new HexBinaryValue()
				{
					Value = string.Format("{0:X2}{1:X2}{2:X2}{3:X2}",
					fillColor.A, fillColor.R, fillColor.G, fillColor.B)
				}
			};
		}
	}
}
