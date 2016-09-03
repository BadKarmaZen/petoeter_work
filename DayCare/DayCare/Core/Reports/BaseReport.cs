using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DayCare.Core.Reports
{
	public class BaseReport
	{
		public static string GetTempFilePathWithExtension(string extension)
		{
			var path = Path.GetTempPath();
			var fileName = string.Format("{0}.{1}", Guid.NewGuid(), extension);
			return Path.Combine(path, fileName);
		}

		public static void SetBorder(IXLStyle style, XLBorderStyleValues borderstyle = XLBorderStyleValues.Thin)
		{
			style.Border.LeftBorder = borderstyle;
			style.Border.RightBorder = borderstyle;
			style.Border.TopBorder = borderstyle;
			style.Border.BottomBorder = borderstyle;
		}
	}
}
